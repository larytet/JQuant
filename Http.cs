
using System;

// file system
using System.IO;

// text stream
using System.Text;


/// <summary>
/// I need an embedded HTTP server and this code should do the trick. This is very simple HTTP server. 
/// No HTTPS. Limited number of simultaneous connections. 
/// </summary>
namespace JQuantHttp
{
	public enum CtrlMsgEvent
	{
		START,
		PAUSE,
		STOP
	}

	public class ControlMessage
	{
		public CtrlMsgEvent msgEvent;
	}

	/// <summary>
	/// HTTP server will call such methods when a GET request arrives which contains question mark "?"
	/// After method HttpRequestHandler exits HTTP server closes networkStream and all related sockets
	/// 
	/// The method will set argument "stream" true if some other thread is going to close socket and HTTP server 
	/// should not drop the connection. In this case the TCP connection will survive exit of HttpRequestHandler
	/// 
	/// </summary>
	public delegate void HttpRequestHandler(string request, System.Net.Sockets.NetworkStream networkStream, out bool stream);

	public class Http : JQuant.IResourceStatistics
	{
		/// <summary>
		/// Create an HTTP server. Usually only one HTTP server will be created 
		/// </summary>
		/// <param name="port">
		/// A <see cref="System.Int32"/>
		/// TCP port, for example 80
		/// </param>
		/// <param name="maxConnections">
		/// A <see cref="System.Int32"/>
		/// Maximum number of requests served simultaneously, for example 5
		/// </param>
		/// <param name="rootPath">
		/// A <see cref="System.String"/>
		/// Full path to the files to be served by the server
		/// </param>
		public Http(int port, int maxConnections, string rootPath)
		{
			instance = this;

			this.Port = port;
			this.MaxConnections = maxConnections;
			this.RootPath = rootPath;

			// i need a thread waiting for incoming connection requests
			// and a pool of threads which serve the requests
			threadPool = new JQuant.ThreadPool("HTTPjob", MaxConnections);

			// i need a TCP socket where I wait for incoming connection requests
			tcpListener = default(System.Net.Sockets.TcpListener);
			try
			{
				tcpListener = new System.Net.Sockets.TcpListener(Port);
			}
			catch (Exception e)
			{
				System.Console.WriteLine("Failed to open port " + Port);
				System.Console.WriteLine(e.ToString());
			}
			if (tcpListener != default(System.Net.Sockets.TcpListener))
			{
			}

			mainThread = new MainThread(RootPath, tcpListener, threadPool);

		}

		public void Start()
		{
			if (tcpListener != default(System.Net.Sockets.TcpListener))
			{
				try
				{
					tcpListener.Start();
				}
				catch (Exception e)
				{
					System.Console.WriteLine("Failed to bind port " + Port);
					System.Console.WriteLine(e.ToString());
				}
			}
			mainThread.Start();
			System.Console.WriteLine("HTTP server started on port " + Port);
		}

		public void Stop()
		{
			mainThread.Stop();
			if (tcpListener != default(System.Net.Sockets.TcpListener))
			{
				tcpListener.Stop();
			}
		}

		public string RootPath
		{
			protected set;
			get;
		}

		public int MaxConnections
		{
			protected set;
			get;
		}

		public int Port
		{
			protected set;
			get;
		}




		/// <summary>
		/// Add a handler to the look up table of the HTTP server
		/// Every time a request comes in server looks in the hash table and if found there 
		/// calls a handler (delegate method)
		/// Will return false if the name is already in use
		/// </summary>
		/// <param name="name">
		/// A <see cref="System.String"/>
		/// Name of the handler to add
		/// </param>
		/// <param name="handler">
		/// A <see cref="HttpRequestHandler"/>
		/// HTTP request handler to be call when URL contains "name"
		/// </param>
		/// <returns>
		/// A <see cref="System.Boolean"/>
		/// Returns true if handler added
		/// </returns>
		public static bool AddRequestHandler(string name, HttpRequestHandler handler)
		{
			bool result;
			lock (handlers)
			{
				result = !(handlers.ContainsKey(name));
				if (result)
				{
					handlers.Add(name, handler);
				}
			}
			return result;
		}

		/// <summary>
		/// Remove specified HTTP request handler from the lookup table
		/// </summary>
		/// <param name="name">
		/// A <see cref="System.String"/>
		/// Name of the handler to remove
		/// </param>
		/// <returns>
		/// A <see cref="System.Boolean"/>
		/// Returns true if a handler removed
		/// </returns>
		public static bool RemoveRequestHandler(string name)
		{
			bool result;
			lock (handlers)
			{
				result = (handlers.ContainsKey(name));
				if (result)
				{
					handlers.Remove(name);
				}
			}
			return result;
		}

		public static HttpRequestHandler FindRequestHandler(string name)
		{
			HttpRequestHandler handler;
			bool result;

			lock (handlers)
			{
				result = handlers.TryGetValue((name), out handler);
			}

			if (!result)
			{
				handler = default(HttpRequestHandler);
			}
			return handler;
		}

		public void GetEventCounters(out System.Collections.ArrayList names, out System.Collections.ArrayList values)
		{
			names = new System.Collections.ArrayList(8);
			values = new System.Collections.ArrayList(8);

			names.Add("Connections"); values.Add(statistics.connections);
			names.Add("ConnPostpone"); values.Add(statistics.connectionPostpone);
			names.Add("jobsPlaced"); values.Add(statistics.jobsPlaced);
			names.Add("BytesIn"); values.Add(statistics.incomingBytes);
			names.Add("BytesOut"); values.Add(statistics.outgoingBytes);
			names.Add("Files"); values.Add(statistics.files);
			names.Add("NotFound"); values.Add(statistics.errorNotFound);
			names.Add("HttpRequests"); values.Add(statistics.httpRequests);
			names.Add("HttpRequestNotFound"); values.Add(statistics.errorHttpRequestNotFound);
		}

		public static JQuant.IResourceStatistics GetIResourceStatistics()
		{
			return instance;
		}

		public class Statistics
		{
			public int connections;
			public int connectionPostpone;
			public int jobsPlaced;
			public int incomingBytes;
			public int outgoingBytes;
			public int httpRequests;
			public int files;
			public int errorNotFound;
			public int errorHttpRequestNotFound;
		}


		/// <summary>
		/// The thread listens on the scoket and polls mailbox. When a connection request coming in the thread spawns 
		/// a job thread to handle HTTP request. There are two types of HTTP requests
		///  - get file
		///  - execute command
		/// All command execution GET requests will contain question mark "?"
		/// </summary>
		protected class MainThread : JQuant.MailboxThread<ControlMessage>
		{
			public MainThread(string rootPath, System.Net.Sockets.TcpListener tcpListener, JQuant.ThreadPool threadPool) :
				base("HTTP", 10, MBX_TIMEOUT_IDLE)
			{
				this.threadPool = threadPool;
				this.tcpListener = tcpListener;
				this.rootPath = rootPath;

				string patternGet = "^GET /(.*) HTTP/.+";
				this.regexPatternGet = new System.Text.RegularExpressions.Regex(patternGet);
			}

			protected override void HandleMessage(ControlMessage message)
			{
				// process incoming messages
				if (message != default(ControlMessage))
				{
					HandleMbxMessage(message);
				}

				// check if there is something in the socket
				HandleSocket();
			}

			/// <summary>
			/// accept incoming connection, spawn a separate job thread to handle HTTP request
			/// </summary>
			protected void HandleSocket()
			{
				// check if there is something in the socket
				bool clientPending = tcpListener.Pending();
				if ((threadPool.FreeThreads() > 0) && clientPending)
				{
					JQuant.Resources.systemEventCounters.httpChanged++;
					Http.statistics.connections++;

					Timeout = MBX_TIMEOUT_FAST;
					System.Net.Sockets.TcpClient tcpClient = default(System.Net.Sockets.TcpClient);

					// accept conneciton
					try
					{
						tcpClient = tcpListener.AcceptTcpClient();
					}
					catch (Exception e)
					{
						System.Console.WriteLine(e.ToString());
					}
					if (tcpClient != default(System.Net.Sockets.TcpClient))
					{
						Http.statistics.jobsPlaced++;
						threadPool.PlaceJob(HandleClient, null, tcpClient);
					}
				}
				else if (clientPending)
				{
					JQuant.Resources.systemEventCounters.httpChanged++;
					Http.statistics.connectionPostpone++;
				}

				if (!clientPending)
				{
					Timeout = MBX_TIMEOUT_IDLE;
				}
			}

			/// <summary>
			/// If there is a message waiting in the mailbox check the event. 
			/// </summary>
			protected void HandleMbxMessage(ControlMessage message)
			{
				switch (message.msgEvent)
				{
					case CtrlMsgEvent.START:
						state = State.RUN;
						Timeout = MBX_TIMEOUT_FAST;
						break;

					case CtrlMsgEvent.STOP:
						state = State.EXIT;
						break;

					case CtrlMsgEvent.PAUSE:
						state = State.PAUSE;
						Timeout = MBX_TIMEOUT_IDLE;
						break;
				}
			}

			/// <summary>
			/// Called from a job thread to handle HTTP request
			/// </summary>
			protected void HandleClient(ref object argument)
			{
				byte[] buffer = new byte[1024];
				System.Net.Sockets.TcpClient tcpClient = (System.Net.Sockets.TcpClient)argument;
				bool stream = false;
				try
				{
					System.Net.Sockets.NetworkStream networkStream = tcpClient.GetStream();
					int result = networkStream.Read(buffer, 0, buffer.Length);
					if (result > 0)
					{
						Http.statistics.incomingBytes += result;
						// convert bytes to string
						string request = Encoding.ASCII.GetString(buffer, 0, result);
						ProcessRequest(rootPath, networkStream, request, ref stream);
					}

					// close the client socket 
					tcpClient.Close();
				}
				catch (Exception e)
				{
					System.Console.WriteLine(e.Message);
					System.Console.WriteLine(e.StackTrace);
				}
			}

			/// <summary>
			/// Figire out type of the request - file or some service (CGI). Deliver the data to the HTTP client
			/// </summary>
			protected void ProcessRequest(string rootPath, System.Net.Sockets.NetworkStream networkStream, string clientRequest, ref bool stream)
			{
				System.Text.RegularExpressions.MatchCollection matches = regexPatternGet.Matches(clientRequest);
				int matchesCount = matches.Count;
				string filenameFull;
				bool useCache = true;

				if (matchesCount == 1)
				{
					// get groups
					System.Text.RegularExpressions.Match match = matches[0];
					System.Text.RegularExpressions.GroupCollection groups = match.Groups;

					string filename = groups[1].Captures[0].ToString(); // group[0] is reserved for the whole match
					filenameFull = filename;

					// server does not care about subdirectories. Send only files in the root directory 
					filename = System.IO.Path.GetFileName(filename);

					// no file means index.html file
					if (filename.Length == 0) 
					{
						filenameFull = "index_local.html";
						useCache = false;
						// else System.Console.WriteLine(filename);
					}


					// i have to figure out if this is a file or CGI script (management request)
					// management requests contain '?' (this is likely)
					int indexQuestionMark = filename.IndexOf('?');
					if (indexQuestionMark > 0)
					{
						Http.statistics.httpRequests++;
						string name = filename.Substring(0, indexQuestionMark);
						// look in the list of all CGI scripts and if found - call the delegate method
						HttpRequestHandler handler = Http.FindRequestHandler(name);
						// System.Console.WriteLine(name);
						if (handler != default(HttpRequestHandler))
						{
							handler(clientRequest, networkStream, out stream);
						}
						else
						{
							Http.statistics.errorHttpRequestNotFound++;
							System.Console.WriteLine("Unknown request:" + name);
							Http.SendErrorFileNotExist(networkStream, name);
						}
					}
					else // this is less likely - get actual file
					{
						Http.statistics.files++;

						// add root
						filename = rootPath + filenameFull;


						// send the file
						Http.SendFile(networkStream, filename, useCache);
					}
				}
			}


			protected enum State
			{
				RUN,
				PAUSE,
				EXIT
			}

			/// <summary>
			/// State of the HTTP main thread
			/// </summary>
			protected State state;
			protected static int MBX_TIMEOUT_IDLE = 1000;
			protected static int MBX_TIMEOUT_FAST = 50;
			protected System.Net.Sockets.TcpListener tcpListener;
			protected JQuant.ThreadPool threadPool;
			protected string rootPath;
			protected System.Text.RegularExpressions.Regex regexPatternGet;
		}

		public static void SendFile(System.Net.Sockets.NetworkStream networkStream, string filename, bool cache)
		{
			do
			{
				bool fileExists = (System.IO.File.Exists(filename));  // file exists ?
				if (!fileExists)
				{
					SendErrorFileNotExist(networkStream, filename);
					break;
				}

				// try to open the file for reading
				System.IO.FileStream fileStream = null;
				try
				{
					fileStream = new System.IO.FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
				}
				catch (Exception e)
				{
					SendErrorFileReadError(networkStream, filename);
					break;
				}

				FileInfo fi = new FileInfo(filename);
				DateTime fileModified = fi.LastWriteTime;
				long fileSize = fi.Length;
				string fileNameExtension = System.IO.Path.GetExtension(filename);
				string mimeType = Http.GetMimeType(fileNameExtension);

				// I can read from the file. Send first part of the server response - header
				SendHeader(networkStream, fileModified, cache, fileSize, mimeType);

				// send the data
				SendOctets(networkStream, fileStream, fileSize);

				// try to close the file
				try
				{
					fileStream.Close();
				}
				catch (Exception e)
				{
					System.Console.WriteLine(e.ToString());
				}
			}
			while (false);
		}

		/// <summary>
		/// send the data - read the file block by block and write to the socket
		/// i should send fileSize bytes in total
		/// </summary>
		public static bool SendOctets(System.Net.Sockets.NetworkStream networkStream, System.IO.FileStream fileStream, long fileSize)
		{
			byte[] buffer = new byte[1024];
			while (fileSize > 0)
			{
				int readBytes = fileStream.Read(buffer, 0, buffer.Length);
				if (readBytes > 0)
				{
					try
					{
						networkStream.Write(buffer, 0, readBytes);
						Http.statistics.outgoingBytes += readBytes;
					}
					catch (Exception e) { break; }
				}
				else break;
				fileSize -= readBytes;
			}

			bool result = (fileSize == 0);
			return result;
		}

		public static bool SendOctets(System.Net.Sockets.NetworkStream networkStream, byte[] data)
		{
			bool result = true;
			int size = data.Length;
			try
			{
				networkStream.Write(data, 0, size);
				Http.statistics.outgoingBytes += size;
			}
			catch (Exception e)
			{
				result = false;
			}

			return result;
		}

		public static bool SendHeader(System.Net.Sockets.NetworkStream networkStream, DateTime fileModified, long fileSize, string mime)
		{
			bool result = SendHeader(networkStream, fileModified, true, fileSize, mime);
			return result;
		}

		/// <summary>
		/// Mon, 23 May 2005 22:38:34 GMT
		/// </summary>
		protected const string DATE_FORMAT = "{0:ddd, dd MMM yyyy HH:mm:ss}";
		protected const string DATE_FORMAT_RFC1123 = "{0:r}";

		public static bool SendHeader(System.Net.Sockets.NetworkStream networkStream, DateTime modified, bool cache, long fileSize, string mime)
		{
			bool result = true;
			string header;
			string strModified = null;
			string strNow = null;

			try
			{
				strModified = String.Format(DATE_FORMAT_RFC1123, modified);
				strNow = String.Format(DATE_FORMAT_RFC1123, DateTime.Now);
			}
			catch (Exception e)
			{
				System.Console.WriteLine(e.ToString());
			}

			if (cache)
			{
				header = "HTTP/1.1 200 OK\r\n" + "Date: " + strNow + "\r\n" +
								"Server: JQuant\r\n" +
								"Last-Modified: " + strModified +
								"Accept-Ranges: bytes\r\n" +
								"Content-Length: " + fileSize + "\r\n" +
								"Connection: close\r\n" +
								"Content-Type: " + mime + "; charset=UTF-8\r\n\r\n";
			}
			else
			{
				header = "HTTP/1.1 200 OK\r\n" + "Date: " + strNow + "\r\n" +
								"Server: JQuant\r\n" +
								"Expires: " + strModified + "\r\n" +
								"Last-Modified: " + strModified + "\r\n" +
								"Cache-Control: no-cache, must-revalidate\r\n" +
								"Accept-Ranges: bytes\r\n" +
								"Content-Length: " + fileSize + "\r\n" +
								"Connection: close\r\n" +
								"Content-Type: " + mime + "; charset=UTF-8\r\n\r\n";
			}

			byte[] data = Encoding.ASCII.GetBytes(header);
			int size = data.Length;
			try
			{
				networkStream.Write(data, 0, size);
				Http.statistics.outgoingBytes += size;
			}
			catch (Exception e)
			{
				result = false;
			}

			return result;
		}


		public static void SendErrorFileNotExist(System.Net.Sockets.NetworkStream networkStream, string filename)
		{
			string s = "File " + filename + " not exists";
			byte[] data = Encoding.ASCII.GetBytes(s);
			try
			{
				networkStream.Write(data, 0, data.Length);
			}
			catch (Exception e)
			{
			}
		}

		public static void SendErrorFileReadError(System.Net.Sockets.NetworkStream networkStream, string filename)
		{
			string s = "Failed to read file " + filename;
			byte[] data = Encoding.ASCII.GetBytes(s);
			try
			{
				networkStream.Write(data, 0, data.Length);
			}
			catch (Exception e)
			{
			}
		}

		/// <summary>
		/// Lookup table for all supported MIME types. Maps file extension to MIME type
		/// See also method GetMimeType()
		/// </summary>
		protected class MIME_TYPES : System.Collections.Generic.Dictionary<string, string>
		{
			public MIME_TYPES()
				: base(50)
			{
				this.Add(".html", "text/html");
				this.Add(".js", "application/x-javascript");
				this.Add(".css", "text/css");
				this.Add(".png", "image/png");
				this.Add(".jpg", "image/jpeg");
				this.Add(".gif", "image/gif");
				this.Add(".ico", "image/x-icon");
				this.Add(".asp", "text/asp");
			}

			public static string DEFAULT = "application/octet-stream";
		}

		/// <summary>
		/// Get MIME type by file name extension
		/// If not fond in the lookup table return "application/octet-stream"
		/// </summary>
		public static string GetMimeType(string fileNameExtension)
		{
			string mimeType;
			bool res = mimeTypes.TryGetValue(fileNameExtension, out mimeType);
			if (!res)
			{
				mimeType = MIME_TYPES.DEFAULT;
			}

			return mimeType;
		}

		protected JQuant.ThreadPool threadPool;
		protected System.Net.Sockets.TcpListener tcpListener;
		JQuant.MailboxThread<ControlMessage> mainThread;

		/// <summary>
		/// lookup table for handlers of HTTP requests
		/// At this point I will keep all callbacks in one data base. Lookup time is not 
		/// a function of size of the table and there is no point to organize separate tables
		/// I use here "power up" initialization - I use this approachh rather rarely 
		/// </summary>
		static System.Collections.Generic.Dictionary<string, HttpRequestHandler> handlers
			= new System.Collections.Generic.Dictionary<string, HttpRequestHandler>(50);

		static MIME_TYPES mimeTypes = new MIME_TYPES();
		protected static Http instance;
		public static Statistics statistics = new Statistics();
	}
}

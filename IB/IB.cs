/// <summary>
/// Contains wrapper for the InteractiveBrokers API and simulation 
/// This is a rather large file
/// IB supportrs paper trading. I do not need to support paper trading option in the FSM
/// like I did in all algos so far.
/// </summary>


using System;

// file system
using System.IO;

// text stream
using System.Text;

namespace IB
{
	/// <summary>
	/// ProcessorMessage types to be sent to Processor
	/// </summary>
	public enum ProcessorMessageType
	{
		Connect,
		DataIn,
		Request
	}

	/// <summary>
	/// Possible states of the Processor
	/// </summary>
	public enum State
	{
		Idle,
		Connecting,
		Connected
	}
	
	/// <summary>
	/// Objects of this type can be sent to the Processot thread
	/// </summary>
	public class ProcessorMessage
	{
		
		public ProcessorMessage(ProcessorMessageType id, object data, object data1)
		{
			this.data = data;
			this.data1 = data1;
			this.id = id;
		}
		
		public ProcessorMessage(ProcessorMessageType id, object data)
		{
			this.data = data;
			this.data1 = null;
			this.id = id;
		}

		public ProcessorMessageType id;
		public object data;
		public object data1;
	}
	
	/// <summary>
	/// Do all Rx/Tx with IB. At this point I expect that only one connection should be
	/// handled at time. 
	/// </summary>
	public class Processor : JQuant.MailboxThread<ProcessorMessage>
	{
		public const int BUFFER_SIZE = 1 * 1024;

		/// <summary>
		/// Initialize the class. Use method CreateProcessor() to create an instance of the class
		/// </summary>
		protected Processor() 
			:base("Processor", 100)
		{
			state = State.Idle;
		 	rxHandler = new RxHandler(RxCallback, BUFFER_SIZE);
		}
		
		/// <summary>
		/// Create an instance of the processor
		/// </summary>
		/// <returns>
		/// A <see cref="Processor"/>
		/// </returns>
		public Processor CreateProcessor ()
		{
			return new Processor();
		}
		
		/// <summary>
		/// Connect to the server. This method will open a client socket and 
		/// start the FSM which attempts to connect to the specified server. 
		/// The actual connection is goingt to be established in the context of the 
		/// Processor FSM. Notification onConnect can be received asynchronously 
		/// by the application
		/// <param name="host">
		/// </summary>
		/// A <see cref="System.String"/>
		/// </param>
		/// <param name="port">
		/// A <see cref="System.Int32"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.Boolean"/>
		/// </returns>
		public bool Connect(string host, int port)
		{
			this.Send(new ProcessorMessage(ProcessorMessageType.Connect, host, port));
			return true;
		}
		
		
		protected static int requestId = 0;
		/// <summary>
		/// Application will call this method to get next unique request ID 
		/// </summary>
		public int GetUniqueRequestId()
		{
			lock(this)
			{
				return (requestId++);
			}
		}
		
		/// <summary>
		/// Application provides a callback to be called for all responses from the server
		/// </summary>
		public delegate void ResponseHandler(MessageIfc message);
		
		/// <summary>
		/// Send a request to the server. Application provides a callback which handles asynchronous 
		/// responses
		/// </summary>
		public bool SendRequest(RequestIfc request, ResponseHandler responseHandler)
		{
			bool res = false;
			do
			{
				this.Send(new ProcessorMessage(ProcessorMessageType.Request, request, responseHandler));
				res = true;
			}
			while (false);
			
			return res;
		}

		/// <summary>
		/// Remove request specified by the request ID. For example, application will call the method 
		/// after it tears the feed stream down
		/// </summary>
		public bool RemoveRequest(int id)
		{
			return true;
		}
		
		/// <summary>
		/// Returns true if connection is established with the server and 
		/// the initial handshake is alright
		/// </summary>
		/// <returns>
		/// A <see cref="System.Boolean"/>
		/// </returns>
		public bool isConnected()
		{
			return (state == State.Connected);
		}
		
		protected void RxCallback(MessageIfc messsage)
		{
			// new data in the connected state. Find the request handler by message ID
			// and forward the data
		}
		
		protected override void HandleMessage (ProcessorMessage message)
		{
			switch (state)
			{
			case State.Idle:
				HandleMessage_Idle (message);
				break;
			case State.Connecting:
				HandleMessage_Connecting (message);
				break;
			case State.Connected:
				HandleMessage_Connected (message);
				break;
			}
		}

		protected void HandleMessage_Idle (ProcessorMessage message)
		{
			switch (message.id)
			{
				case ProcessorMessageType.Connect:
				HandleMessage_IdleConnect (message);
				break;
			}
		
		}
		
		protected void HandleMessage_Connected (ProcessorMessage message)
		{
			switch (message.id)
			{
				case ProcessorMessageType.DataIn:
				rxHandler.HandleData((byte[])message.data, (int)message.data1);
				break;
				
				case ProcessorMessageType.Request:
				// add Request and application callback (response handler) to the hash table of pending requests 
				// and send request to the socket. At this point I send the request in the context of the 
				// Processor and  I let the OS to do the buffering. In the future I can run a separate WriteSocket
				// thread to avoid blocking call to the socket API.
				break;
			}
			
		}

		protected void HandleMessage_Connecting (ProcessorMessage message)
		{
			switch (message.id)
			{
				case ProcessorMessageType.DataIn:
				HandleMessage_ConnectingDataIn(message);
				break;
			}
			
		}
		
		protected void HandleMessage_IdleConnect (ProcessorMessage message)
		{
			string host = (string)message.data;
			int port = (int)(message.data1);
			clientSocket = null;
			try
			{
				clientSocket = new System.Net.Sockets.TcpClient (host, port);
			}
			catch (Exception e)
			{
				System.Console.WriteLine ("Failed to connect " + host + ", port "+port);
				System.Console.WriteLine (e.ToString ());
			}
			if (clientSocket != null)
			{
				socketReader = new SocketReader(this, clientSocket);
				socketReader.Start();
				bool res = WriteToSocket(clientSocket, CLIENT_VERSION);
				if (res)
				{
					this.state = State.Connecting;
				}
				else
				{
					SocketCleanup();
				}
			}
		
		}
		
		/// <summary>
		/// In the connecting state I am ready only for one message - server version
		/// </summary>
		/// <param name="message">
		/// A <see cref="ProcessorMessage"/>
		/// </param>
		protected void HandleMessage_ConnectingDataIn(ProcessorMessage message)
		{
			
		}
		
		protected void SocketCleanup()
		{
			clientSocket.Close();
			socketReader.Stop();
		}

		protected static bool WriteToSocket(System.Net.Sockets.TcpClient clientSocket, int data)
		{
			string s = data.ToString();
			bool res = WriteToSocket(clientSocket, s);

			return res;
		}
		
		protected static bool WriteToSocket(System.Net.Sockets.TcpClient clientSocket, string s)
		{
			System.Net.Sockets.NetworkStream networkStream;
			networkStream = clientSocket.GetStream();
			
			bool res;
			byte[] data = System.Text.Encoding.ASCII.GetBytes(s);
			try 
			{
				networkStream.Write(data, 0, data.Length);
				res = true;
			}	
			catch (Exception e)
			{
				res = false;
			}

			return res;
		}
		

		protected State state;
		protected System.Net.Sockets.TcpClient clientSocket;
		private static int CLIENT_VERSION = 46;
		private static int SERVER_VERSION = 38;
		SocketReader socketReader;
		RxHandler rxHandler;
		
	}
	
	class SocketReader
	{
		public SocketReader(Processor processor, System.Net.Sockets.TcpClient clientSocket)
		{
			this.networkStream = clientSocket.GetStream();
			this.processor = processor;
			exitFlag = false;
			thread = new System.Threading.Thread(this.Run);
			thread.Priority = System.Threading.ThreadPriority.Highest;
			rxHandler = new RxHandler(null, Processor.BUFFER_SIZE);
		}
		
		public void Start()
		{
			thread.Start();
			
		}
		
		public void Stop()
		{
			exitFlag = true;
		}
		
		public void Run()
		{
			byte[] buffer = new byte[Processor.BUFFER_SIZE];
			while (!exitFlag)
			{
				int bytes = 0;
				
				try
				{
					bytes = networkStream.Read(buffer, 0, buffer.Length);
				}
				catch (Exception e)
				{
				}
				if (bytes != 0)
				{
					HandleData(buffer, bytes);
				}
				else
				{
					System.Threading.Thread.Sleep(200);
				}
			}
		}
		
		protected void HandleData(byte[] data, int size)
		{
			processor.Send(new ProcessorMessage(ProcessorMessageType.DataIn, data, size));
		}
		
		bool exitFlag;
		System.Net.Sockets.NetworkStream networkStream;
		Processor processor;
		System.Threading.Thread thread;
		RxHandler rxHandler;
	}


} // namespace IB
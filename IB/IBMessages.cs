/// <summary>
/// Contains class handling all received messages from the IB
/// </summary>


using System;

// file system
using System.IO;

// text stream
using System.Text;

namespace IB
{
	public interface MessageIfc
	{
		/// <summary>
		/// In Java code this is tickerId. It can be called also request ID
		/// </summary>
		int GetId();
	}
	
	public enum TickType
	{
		BID_SIZE,
		ASK_SIZE,
		LAST_SIZE,
		UNKNOWN
	};

	public class Message_TickPrice : MessageIfc
	{
		public Message_TickPrice(int id, TickType tickType, double price, int size)
		{
			this.id = id;
			this.tickType = tickType;
			this.price = price;
			this.size = size;
		}
	
		public int GetId()
		{
			return id;
		}
		
		protected int id;
		public TickType tickType;
		public double price;
		public int size;
	}
	
	public enum ResponseType
	{
		NoResponse,
		SingleResponse, 
		Stream
	}
	
	/// <summary>
	/// Application provides a callback to be called for all responses from the server
	/// </summary>
	public delegate void ResponseHandler(MessageIfc message);
	
	public interface RequestIfc
	{
		int GetId();
		byte[] GetData();
		int GetSize();
		ResponseType getResponseType();
	}
	
	public class Request
	{
		// Outgoing messages
		public const int REQ_MKT_DATA = 1;
		public const int CANCEL_MKT_DATA = 2;
		public const int PLACE_ORDER = 3;
		public const int CANCEL_ORDER = 4;
		public const int REQ_OPEN_ORDERS = 5;
		public const int REQ_ACCOUNT_DATA = 6;
		public const int REQ_EXECUTIONS = 7;
		public const int REQ_IDS = 8;
		public const int REQ_CONTRACT_DATA = 9;
		public const int REQ_MKT_DEPTH = 10;
		public const int CANCEL_MKT_DEPTH = 11;
		public const int REQ_NEWS_BULLETINS = 12;
		public const int CANCEL_NEWS_BULLETINS = 13;
		public const int SET_SERVER_LOGLEVEL = 14;
		public const int REQ_AUTO_OPEN_ORDERS = 15;
		public const int REQ_ALL_OPEN_ORDERS = 16;
		public const int REQ_MANAGED_ACCTS = 17;
		public const int REQ_FA = 18;
		public const int REPLACE_FA = 19;
		public const int REQ_HISTORICAL_DATA = 20;
		public const int EXERCISE_OPTIONS = 21;
		public const int REQ_SCANNER_SUBSCRIPTION = 22;
		public const int CANCEL_SCANNER_SUBSCRIPTION = 23;
		public const int REQ_SCANNER_PARAMETERS = 24;
		public const int CANCEL_HISTORICAL_DATA = 25;
		public const int REQ_CURRENT_TIME = 49;
		public const int REQ_REAL_TIME_BARS = 50;
		public const int CANCEL_REAL_TIME_BARS = 51;
		public const int REQ_FUNDAMENTAL_DATA = 52;
		public const int CANCEL_FUNDAMENTAL_DATA = 53;
		
		public const int MIN_SERVER_VER_REAL_TIME_BARS = 34;
		public const int MIN_SERVER_VER_SCALE_ORDERS = 35;
		public const int MIN_SERVER_VER_SNAPSHOT_MKT_DATA = 35;
		public const int MIN_SERVER_VER_SSHORT_COMBO_LEGS = 35;
		public const int MIN_SERVER_VER_WHAT_IF_ORDERS = 36;
		public const int MIN_SERVER_VER_CONTRACT_CONID = 37;
		public const int MIN_SERVER_VER_PTA_ORDERS = 39;
		public const int MIN_SERVER_VER_FUNDAMENTAL_DATA = 40;
		public const int MIN_SERVER_VER_UNDER_COMP = 40;
		public const int MIN_SERVER_VER_CONTRACT_DATA_CHAIN = 40;
		public const int MIN_SERVER_VER_SCALE_ORDERS2 = 40;
		public const int MIN_SERVER_VER_ALGO_ORDERS = 41;
		public const int MIN_SERVER_VER_EXECUTION_DATA_CHAIN = 42;
		public const int MIN_SERVER_VER_NOT_HELD = 44;
		public const int MIN_SERVER_VER_SEC_ID_TYPE = 45;
	}
	

	public class MessageParser
	{
		// Incoming messages
		public const int TICK_PRICE = 1;
		public const int TICK_SIZE = 2;
		public const int ORDER_STATUS = 3;
		public const int ERR_MSG = 4;
		public const int OPEN_ORDER = 5;
		public const int ACCT_VALUE = 6;
		public const int PORTFOLIO_VALUE = 7;
		public const int ACCT_UPDATE_TIME = 8;
		public const int NEXT_VALID_ID = 9;
		public const int CONTRACT_DATA = 10;
		public const int EXECUTION_DATA = 11;
		public const int MARKET_DEPTH = 12;
		public const int MARKET_DEPTH_L2 = 13;
		public const int NEWS_BULLETINS = 14;
		public const int MANAGED_ACCTS = 15;
		public const int RECEIVE_FA = 16;
		public const int HISTORICAL_DATA = 17;
		public const int BOND_CONTRACT_DATA = 18;
		public const int SCANNER_PARAMETERS = 19;
		public const int SCANNER_DATA = 20;
		public const int TICK_OPTION_COMPUTATION = 21;
		public const int TICK_GENERIC = 45;
		public const int TICK_STRING = 46;
		public const int TICK_EFP = 47;
		public const int CURRENT_TIME = 49;
		public const int REAL_TIME_BARS = 50;
		public const int FUNDAMENTAL_DATA = 51;
		public const int CONTRACT_DATA_END = 52;
		public const int OPEN_ORDER_END = 53;
		public const int ACCT_DOWNLOAD_END = 54;
		public const int EXECUTION_DATA_END = 55;
		public const int DELTA_NEUTRAL_VALIDATION = 56;
		public const int TICK_SNAPSHOT_END = 57;
		
    

		
		/// <summary>
		/// Method will parse the array of bytes and generate an array of information elements
		/// Return true if success
		/// </summary>
		public delegate bool Parser(byte[] data, System.Collections.Generic.List<Utils.IEIndex> ieMap, out MessageIfc message, out int ieCount);
		
		public static readonly MessageParser[] list = new[] {
			new MessageParser(MessageParser.TICK_PRICE, 1, Parser_TICK_PRICE),
			new MessageParser(MessageParser.TICK_SIZE, 1, null),
			new MessageParser(MessageParser.ORDER_STATUS, 1, null),
			new MessageParser(MessageParser.ERR_MSG, 1, null),
			new MessageParser(MessageParser.OPEN_ORDER, 1, null),
			new MessageParser(MessageParser.ACCT_VALUE, 1, null),
			new MessageParser(MessageParser.PORTFOLIO_VALUE, 1, null),
			new MessageParser(MessageParser.ACCT_UPDATE_TIME, 1, null),
			new MessageParser(MessageParser.NEXT_VALID_ID, 1, null),
			new MessageParser(MessageParser.CONTRACT_DATA, 1, null),
			new MessageParser(MessageParser.EXECUTION_DATA, 1, null),
			new MessageParser(MessageParser.MARKET_DEPTH, 1, null),
			new MessageParser(MessageParser.MARKET_DEPTH_L2, 1, null),
			new MessageParser(MessageParser.NEWS_BULLETINS, 1, null),
			new MessageParser(MessageParser.MANAGED_ACCTS, 1, null),
			new MessageParser(MessageParser.RECEIVE_FA, 1, null),
			new MessageParser(MessageParser.HISTORICAL_DATA, 1, null),
			new MessageParser(MessageParser.BOND_CONTRACT_DATA, 1, null),
			new MessageParser(MessageParser.SCANNER_PARAMETERS, 1, null),
			new MessageParser(MessageParser.SCANNER_DATA, 1, null),
			new MessageParser(MessageParser.TICK_OPTION_COMPUTATION, 1, null),
			new MessageParser(MessageParser.TICK_GENERIC, 1, null),
			new MessageParser(MessageParser.TICK_STRING, 1, null),
			new MessageParser(MessageParser.TICK_EFP, 1, null),
			new MessageParser(MessageParser.CURRENT_TIME, 1, null),
			new MessageParser(MessageParser.REAL_TIME_BARS, 1, null),
			new MessageParser(MessageParser.FUNDAMENTAL_DATA, 1, null),
			new MessageParser(MessageParser.CONTRACT_DATA_END, 1, null),
			new MessageParser(MessageParser.OPEN_ORDER_END, 1, null),
			new MessageParser(MessageParser.ACCT_DOWNLOAD_END, 1, null),
			new MessageParser(MessageParser.EXECUTION_DATA_END, 1, null),
			new MessageParser(MessageParser.DELTA_NEUTRAL_VALIDATION, 1, null),
			new MessageParser(MessageParser.TICK_SNAPSHOT_END, 1, null)
		};
				
		public MessageParser(int id, int fields, Parser parser)
		{
			this.id = id;
			this.fields = fields;
			this.parser = parser;
		}
		
		public static MessageParser Find(int id)
		{
			foreach (MessageParser msg in list)
			{
				if (msg.id == id)
				{
					return msg;
				}
			}
			return null;
		}

		/// <summary>
		/// Methos assumes that array data contains message ID. The method will call 
		/// appropriate parser and return array of information elements
		/// </summary>
		public static bool Parse(byte[] data, System.Collections.Generic.List<Utils.IEIndex> ieMap, out MessageIfc message, out int ieCount)
		{
			bool res = true;
			message = null;
			ieCount = 0;
			do
			{
				// get first IE - message ID
				int ieMessageId;
				string ieMessageIdStr;
				Utils.IEIndex ieIndex = ieMap[0];
				res = Utils.GetIEValueInt (data, 0, ieIndex.firstByte, ieIndex.lastByte, out ieMessageId, out ieMessageIdStr);
				if (!res)
				{
					System.Console.Out.WriteLine("Failed to parse message ID ("+ieMessageIdStr+")");
					break;
				}
				
				MessageParser messageParser = MessageParser.Find(ieMessageId);
				res = (messageParser != null);
				if (!res)
				{
					System.Console.Out.WriteLine("Failed to find message ID ("+ieMessageId+")");
					break;
				}
				
				res = messageParser.parser(data, ieMap, out message, out ieCount);
				if (!res)
				{
					break;
				}
			}
			while (false);
			
			return res;
		}
/*		
		                int version = readInt();
                int tickerId = readInt();
                int tickType = readInt();
                double price = readDouble();
                int size = 0;
                if( version >= 2) {
                    size = readInt();
                }
                int canAutoExecute = 0;
                if (version >= 3) {
                    canAutoExecute = readInt();
                }
                eWrapper().tickPrice( tickerId, tickType, price, canAutoExecute);

                if( version >= 2) {
                    int sizeTickType = -1 ; // not a tick
                    switch (tickType) {
                        case 1: // BID
                            sizeTickType = 0 ; // BID_SIZE
                            break ;
                        case 2: // ASK
                            sizeTickType = 3 ; // ASK_SIZE
                            break ;
                        case 4: // LAST
                            sizeTickType = 5 ; // LAST_SIZE
                            break ;
                    }
                    if (sizeTickType != -1) {
                        eWrapper().tickSize( tickerId, sizeTickType, size);
                    }
                }
*/
		protected static TickType getTickType(int tickTypeValue)
		{
			switch (tickTypeValue)
			{
			case 1: return TickType.BID_SIZE;
			case 2: return TickType.ASK_SIZE;
			case 4: return TickType.LAST_SIZE;
			}
			return TickType.UNKNOWN;
		}
		
		/// <summary>
		/// ieMap[0] is TICK_PRICE (1)
		/// </summary>
		public static bool Parser_TICK_PRICE (byte[] data, System.Collections.Generic.List<Utils.IEIndex> ieMap, out MessageIfc message, out int ieCount)
		{
			bool res = false;
			message = null;
			Utils.IEIndex ieIndex;
			ieCount = 6;
			do
			{
				if (ieMap.Count < ieCount)
				{
					System.Console.Out.WriteLine ("Not enough ies. Expected "+ieCount+", actual "+ieMap.Count);
					break;
				}
				
				// get version
				string ieStr;
				int version; 
				int id; 
				TickType tickType;
				double price;
				int size;
				
				ieIndex = ieMap[1];
				res = Utils.GetIEValueInt (data, 0, ieIndex.firstByte, ieIndex.lastByte, out version, out ieStr);
				if (!res) {
					System.Console.Out.WriteLine ("Failed to parse version (" + ieStr + ")");
					break;
				}
				
				ieIndex = ieMap[2];
				res = Utils.GetIEValueInt (data, 0, ieIndex.firstByte, ieIndex.lastByte, out id, out ieStr);
				if (!res) {
					System.Console.Out.WriteLine ("Failed to parse id (" + ieStr + ")");
					break;
				}
				
				ieIndex = ieMap[3];
				int tickTypeValue;
				res = Utils.GetIEValueInt (data, 0, ieIndex.firstByte, ieIndex.lastByte, out tickTypeValue, out ieStr);
				if (!res) {
					System.Console.Out.WriteLine ("Failed to parse tickType (" + ieStr + ")");
					break;
				}
				tickType = getTickType(tickTypeValue);
				
				ieIndex = ieMap[4];
				res = Utils.GetIEValueDouble (data, 0, ieIndex.firstByte, ieIndex.lastByte, out price, out ieStr);
				if (!res) {
					System.Console.Out.WriteLine ("Failed to parse price (" + ieStr + ")");
					break;
				}
				
				ieIndex = ieMap[5];
				res = Utils.GetIEValueInt (data, 0, ieIndex.firstByte, ieIndex.lastByte, out size, out ieStr);
				if (!res) {
					System.Console.Out.WriteLine ("Failed to parse tickType (" + ieStr + ")");
					break;
				}
				
				message = new Message_TickPrice(id, tickType, price, size);
				
				res = true;
			}
			while (false);
			
			return res;
		}
		
		public readonly int id;
		public readonly int fields;		
		public readonly Parser parser;
	}
    
  
  
	/// <summary>
	/// RxHandler will call the method for all incoming messages
	/// </summary>
	public delegate void RxHandlerCallback(MessageIfc message);

	/// <summary>
	/// This class handles a parser of messages from the IB. The parser is a simple state machine. Application
	/// calls method HandleData() with the data received. Method HandleData() pulls the message(s) and calls 
	/// application callback. Applicaiton will need  one instance of this class for every active connection.
	/// </summary>
	public class RxHandler
	{
		protected enum State
		{
			Idle,
			Processing
		};
		
	
		/// <summary>
		/// Create a parser. 
		/// </summary>
		/// <param name="rxHandlerCallback">
		/// A <see cref="RxHandlerCallback"/>
		/// </param>
		/// <param name="bufferSize">
		/// A <see cref="System.Int32"/>
		/// Maximum buffer size in the call to HandleData()
		/// </param>
		public RxHandler(RxHandlerCallback rxHandlerCallback, int bufferSize)
		{
			this.rxHandlerCallback = rxHandlerCallback;
			state = RxHandler.State.Idle;
			shiftRegister = new byte[bufferSize];
			shiftRegisterSize = 0;
		}
	
		/// <summary>
		/// A message from IB is always starst from message identifier, for example REQ_MKT_DATA (1), followed by 
		/// version (always 1?) and message specific data (tickerId). The information elemnets are zero delimited 
		/// ASCII strings. Method copies the data locally and array data[] can be reused by the calling method
		/// </summary>
		/// <param name="data">
		/// A <see cref="System.Byte[]"/>
		/// </param>
		/// <param name="size">
		/// A <see cref="System.Int32"/>
		/// </param>
		public void HandleData (byte[] data, int size)
		{
			switch (state)
			{
				case State.Idle:
				HandleData_Idle(data, size);
				break;
				
				case State.Processing:
				HandleData_Processing(data, size);
				break;
				
			}
		}

		/// <summary>
		/// This is a new data - I expect to find a known message ID in the first information element
		/// </summary>
		/// <param name="data">
		/// A <see cref="System.Byte[]"/>
		/// </param>
		/// <param name="size">
		/// A <see cref="System.Int32"/>
		/// </param>
		protected void HandleData_Idle (byte[] data, int size)
		{
			int offset = 0;
			int firstByte;
			int lastByte;
			
			// copy the data to the shift register
			int copySize = size;
			Array.Copy(data, offset, shiftRegister, 0, copySize);
			size -= copySize;
			shiftRegisterSize = copySize;
			
			System.Collections.Generic.List<Utils.IEIndex> ieMap;
			int ieMapLength;
			do
			{
				bool res = Utils.SplitArray(shiftRegister, 0, size, out ieMap, out ieMapLength);
				if (!res)
				{
					break;
				}
				MessageIfc message;
				int ieCount;
				res = MessageParser.Parse(shiftRegister, ieMap, out message, out ieCount);
				if (!res)
				{
					break;
				}
				
				int charsToRemove = ieMap[ieCount-1].lastByte+1;
				// remove parsed elements
				Utils.RemoveLeadingBytes(shiftRegister, shiftRegisterSize, charsToRemove);
			}
			while (shiftRegister.Length > 0);
		}
		
		
		/// <summary>
		/// I am in the middle of processing of a message and this is new chunk of data. I 
		/// shall start the processing from the message ID I discovered in the previous chunk
		/// </summary>
		/// <param name="data">
		/// A <see cref="System.Byte[]"/>
		/// </param>
		/// <param name="size">
		/// A <see cref="System.Int32"/>
		/// </param>
		protected void HandleData_Processing (byte[] data, int size)
		{
		
		}
		

		
								
		protected RxHandlerCallback rxHandlerCallback;
		protected RxHandler.State state;
		protected byte[] shiftRegister;
		protected int shiftRegisterSize;
	}

	public class Utils
	{
		/// <summary>
		/// Returns first and last byte of the information element started at the specified offset
		/// </summary>
		public static bool GetIE (byte[] data, int size, int offset, out int firstByte, out int lastByte)
		{
			bool res;
			
			// this is a delimiter at offset. I am going to find position of the next delimiter
			if (data[offset] == 0) {
				firstByte = offset + 1;
			} else {
				firstByte = offset;
			}
			
			lastByte = 0;
			while (offset < size) {
				if (data[offset] == 0) {
					lastByte = offset - 1;
					break;
				}
				offset++;
			}
			
			res = (firstByte < size) && (lastByte > 0);
			
			return res;
		}

		/// <summary>
		/// Return integer value of the information element
		/// </summary>
		public static bool GetIEValueInt (byte[] data, int offset, int firstByte, int lastByte, out int ieValue, out string str)
		{
			bool res = true;
			ieValue = 0;
			
			str = Encoding.ASCII.GetString (data, offset, lastByte - firstByte + 1);
			try {
				ieValue = Int32.Parse (str);
			} catch (Exception e) {
				res = false;
			}
			
			return res;
		}
		
		public static bool GetIEValueDouble (byte[] data, int offset, int firstByte, int lastByte, out double ieValue, out string str)
		{
			bool res = true;
			ieValue = 0;
			
			str = Encoding.ASCII.GetString (data, offset, lastByte - firstByte + 1);
			try {
				ieValue = Double.Parse (str);
			} catch (Exception e) {
				res = false;
			}
			
			return res;
		}
		
		public class IEIndex
		{
			public IEIndex(int firstByte, int lastByte)
			{
				this.firstByte = firstByte;
				this.lastByte = lastByte;
			}
			
			public int firstByte;
			public int lastByte;
		};
		
		/// <summary>
		/// Splits zero delimitered array of bytes into map of information elements. 
		/// An information element is described by first and last byte in the array
		/// </summary>
		public static bool SplitArray(byte[] data, int offset, int size, out System.Collections.Generic.List<Utils.IEIndex> list, out int length)
		{
			list = new System.Collections.Generic.List<Utils.IEIndex>(10);
			length = 0;
			
			if (data[offset] == 0)
			{
				offset++;
			}
			int firstByte = 0;
			int lastByte;
			while (offset < size)
			{
				if (data[offset] == 0)
				{
					lastByte = offset;
					length = offset+1;
					IEIndex ieIndex = new IEIndex(firstByte, lastByte);
					list.Add(ieIndex);
					firstByte = lastByte + 1;
				}
				offset++;
			}
			
			
			return (length != 0);
		}
	
		public static void RemoveIEValue(byte[] data, int size, int firstByte, int lastByte)
		{
			int ieSize = lastByte - firstByte + 1;
			size -= ieSize;
			Array.Copy(data, lastByte+1, data, 0, size);
		}
		
		public static void RemoveLeadingBytes(byte[] data, int size, int firstByte)
		{
			Array.Copy(data, firstByte, data, 0, size-firstByte);
		}
	}

} // namespace IB

using System;
using System.ComponentModel;

namespace TaskBarLibSim
{
	/// <summary>
	/// MOFINQType can be used to parse the records returned from User.GetOrdersMaof
	/// </summary>
	public struct MOFINQType
	{
		public string ERR;          //שגוי
		public string SYS_TYPE;     //סיסטם מקור הפקודה
		public string SND_RCV; 	    // -מטרת השדר
		public string ORDR_TYPE; 	//MANA/ORDR  שיטת הפקודה
		public string OPR_NAME; 	// סיסמאת המשתמש
		public string SUG_INQ; 	    //0=DETAIL     סוג פירוט
		public string SEQ_PIC; 	    // FMR  -    ספרור שוטף
		public string MANA_PIC; 	//מספר מנה/שעה אחרונה
		public string BNO_PIC; 	    //מספר נייר ערך
		public string BNO_NAME; 	//שם נייר
		public string OP;           //קוד קניה / מכירה
		public string BRANCH_PIC; 	//סניף
		public string TIK_PIC; 	    //תיק
		public string SUG_MAVR_PIC; // סוג  חשבון מפצל/מעבר
		public string ID_MAVR_PIC;  //מספר חשבון מפצל/מעבר
		public string SUG_PIC; 	    // סוג חשבון
		public string ID_PIC; 	    //חשבון
		public string ID_NAME; 	    //שם החשבון
		public string SUG_ID_PIC; 	//סוג חשבון
		public string NOSTRO; 	    //קוד נוסטרו
		public string ORDR_NV_PIC;  //כמות מבוקשת
		public string ORDR_SUG; 	//סוג פקודה
		public string ORDR_PRC_PIC; //מחיר הזמנה
		public string ORDR_TIME; 	//זמן קליטת ההזמנה
		public string DIL_NV_PIC; 	//כמות בעסקה
		public string DIL_PRC_PIC;  //מחיר עסקה
		public string DIL_TIME_PIC; //שעת העסקה
		public string MBR_SEQ_PIC;  //מספר הודעת חבר
		public string RZF_SEQ_PIC;  //מספר הודעת רצף
		public string RZF_ORD_PIC;  //מספר פקודה ברצף
		public string ORDER_NO_PIC; //HOST - ספרור מיוחד
		public string DIL_PIC; 	    //מספר אישור עסקה
		public string COD_UPD; 	    //סטטוס הפקודה
		public string STS; 	        //סטטוס הפקודה
		public string ERR_DATA; 	//נתונים חסרים/שגויים
		public string ERR_INQ; 	    //שגוי
		public string ERR_UPD; 	    //שגוי
		public string SUG_INFO; 	//0=ORDR  1=DIL  2=SUM  
		public string DSP_FMR; 	    //מצב הפקודה
	}

	/// <summary>
	/// RZFINQType can be used to parse the records returned from User.GetOrdersRezef
	/// </summary>
	public struct RZFINQType
	{
		public string ERR; 	        //שגוי
		public string SYS_TYPE; 	//סיסטם מקור הפקודה
		public string SND_RCV; 	    // -מטרת השדר
		public string ORDR_TYPE;    //MANA/ORDR  שיטת הפקודה
		public string OPR_NAME; 	// סיסמאת המשתמש
		public string SUG_INQ; 	    //0=DETAIL     סוג פירוט
		public string SEQ_N; 	    // FMR  -    ספרור שוטף
		public string MANA_N; 	    //מספר מנה/שעה אחרונה
		public string BNO_N; 	    //מספר נייר ערך
		public string BNO_NAME; 	//שם נייר
		public string OP; 	        //קוד קניה / מכירה
		public string BRANCH_N; 	//סניף
		public string TIK_N; 	    //תיק
		public string SUG_MAVR_N;   // סוג  חשבון מפצל/מעבר
		public string ID_MAVR_N;    //מספר חשבון מפצל/מעבר
		public string SUG_N; 	    // סוג חשבון
		public string ID_N; 	    //חשבון
		public string ID_NAME; 	    //שם החשבון
		public string SUG_ID_N; 	//סוג חשבון
		public string NOSTRO; 	    //קוד נוסטרו
		public string ORDR_NV_N;    //כמות מבוקשת
		public string ORDR_SUG; 	//סוג פקודה
		public string ORDR_PRC_N;   //מחיר הזמנה
		public string ORDR_TIME;    //זמן קליטת ההזמנה
		public string DIL_NV_N; 	//כמות בעסקה
		public string DIL_PRC_N;    //מחיר עסקה
		public string DIL_TIME_N;   //שעת העסקה
		public string MBR_SEQ_N;    //מספר הודעת חבר
		public string RZF_SEQ_N;    //מספר הודעת רצף
		public string RZF_ORD_N;    //מספר פקודה ברצף
		public string ORDER_NO_N;   //HOST - ספרור מיוחד
		public string DIL_N; 	    //מספר אישור עסקה
		public string COD_UPD; 	    //סטטוס הפקודה
		public string STS; 	        //סטטוס הפקודה
		public string ERR_DATA; 	//נתונים חסרים/שגויים
		public string ERR_INQ;  	//שגוי
		public string ERR_UPD; 	    //שגוי
		public string SUG_INFO; 	//0=ORDR  1=DIL  2=SUM  
		public string MSG1;	        // הודעה כללית
		public string STS_NAME; 	//מצב הפקודה
		public string DSP_FMR; 	    //מצב הפקודה                              
	}

	/// <summary>
	/// Numerical definition of the error type returned by SendRezefOrder and SendMaofOrder processes.
	/// </summary>
	public enum OrdersErrorTypes
	{
		Fatal,
		Confirmation,
		ReEnter,
		PasswordReq,
		Alert,
		NoError
	}

	/// <summary>
	/// Numerical definition of the Trading type for which Holdings will be retrieved.
	/// </summary>
	public enum TradeType
	{
		ALLTradeType = -1,
		MF = 0,
		RZ = 1
	}

	/// <summary>
	/// Type of calculation type for calculating account securities (Margin requirements).
	/// It is ususally set to RM_Dill_ShortOrders. 
	/// </summary>
	public enum SecurityCalcType
	{
		RMOnly = 0,
		RM_Dill = 1,
		RM_Dill_Orders = 2,
		RM_Dill_ShortOrders = 3,
	}

	/// <summary>
	/// Maof Basic Order Structure
	/// </summary>
	public struct MaofOrderType
	{
		public string Branch;       //סניף
		public string Account;      //חשבון
		public string Option;       //מספר אופציה
		public string operation;    //פעולה
		public string ammount;      //כמות
		public string price;        //מחיר
		public string Sug_Pkuda;    //סוג פקודה
		public string Asmachta;     //אסמכתא
		public string AsmachtaFmr;  //אסמכתא פ.מ.ר
		public int Pass;            //נקלט
		public int OrderID;         //זיהוי פקודה פנימי
	}
	/// <summary>
	/// Basic Rezef Order Structure
	/// </summary>
	public struct RezefBasicOrder
	{
		public string operation;
		public string asmachta_fmr;
		public string ammount;
		public string price;
		public string Stock_Number;
		public string OP;
		public string Branch;
		public string Account;
		public string order_type;
		public string asmachta_rezef;
		public string price_percent;
		public string shlav;
		public string Nv_Del;
		public string ORDR_TYPE;
		public string Mana;
		public string Zira;
		public string Nv_Min;
		public string Strat_Date;
		public string end_date;
	}

	/// <summary>
	/// מבנה רשומת הוראה חדשה
	/// </summary>
	public struct RezefSimpleOrder
	{
		public int BNO;     //מספר נייר ערך
		public int Amount;  //כמות
		public double price;//מחיר
		public int Branch;  //סניף
		public int Account; //חשבון
		public OrderOperation operation;    //סוג פעולה
		public RezefOrderKind OrderKind;    //סוג הוראה
		public long Query;  //הוראת שאילתא
	}

	public struct RezefContinuousOrder
	{
		public int BNO;
		public int Amount;
		public double price;
		public int Branch;
		public int Account;
		public OrderOperation operation;
		public RezefOrderKind OrderKind;
		public int StartDate;
		public int EndDate;
		public int MinAmount;
		public int MaxAmount;
		public int ImmediateExecution;
	}



	/// <summary>
	/// פעולות קניה ומכירה
	/// </summary>
	public enum OrderOperation
	{
		OrderOperationNewBuy,
		OrderOperationNewSell,
		OrderOperationUpdBuy,
		OrderOperationUpdSell,
		OrderOperationDelete
	}

	/// <summary>
	/// סוגי פקודות ברצף
	/// </summary>
	public enum RezefOrderKind
	{
		RezefOrderKindLMT = 0,  //Lmt order - this one is probably we're going to use, if any
		RezefOrderKindMKT = 1,  //MKT
		RezefOrderKindATC = 2,  //At Close auction
		RezefOrderKindLMO = 3,  //Limit at opening auction
		RezefOrderKindKRN = 4,  //??? - need to check it
		RezefOrderKindJMB = 5   //Jumbo - need special permissions (it's for instituational guys)
	}

	/// <summary>
	/// Base Asset Type Enumaration.
	/// (FMR mean 'underlying' by 'base')
	/// We're intersted exclusively in BaseAssetMaof(=1), at the moment
	/// </summary>
	public enum BaseAssetTypes
	{
		BaseAssetAll = -1,
		BaseAssetBanks = 4,
		BaseAssetDollar = 2,
		BaseAssetEuro = 5,
		BaseAssetInterest = 3,
		BaseAssetMaof = 1,          //this one we're going to use
		BaseAssetShacharAroch = 7,
		BaseAssetShacharBenoni = 6
	}

	/// <summary>
	/// Underlying asset info
	/// *base assets info*
	/// </summary>
	public struct BaseAssetType
	{
		public int BaseAssetCode;   //base asset code
		public int BNO;             //base asset bno
		public int nCurr;           //currency code  
		public string BaseAssetKind;//base asset type 
		public string TradeMethod;  //trade method
		public double Value;        //morning value  
		public double Interest;     //interest
		public double Dividend;     //dividend
		public double StdIv;        //standard deviation   
		public double ValueChange;  //value change 
		public double StdIdChange;  //standard dev change
		public double Mult;         //value multiplier
		public int ExpiresToday;    //any derivative expiration
		public int ExpDate;         //nearest expiration date
		public int ExpDays;         //days to expiration date
		public string NameHeb;      //hebrew name
		public string NameEng;      //english name
	}

	/// <summary>
	/// drop here everything related to orders sending
	/// </summary>
	public partial class UserClass
	{
		// the following functions are needed to get the info regarding our orders and executions, 
		// in order to place orders we need another set of methods - see StartMaofSession/StartrezefSession below.
		// there is no event mechanism to get orders executions notifications, polling is used instead.
		// so we need to guess if our orders were filled, or put a timer to poll the server periodically
		// while it's in 'Passed' (meaning waiting for execution) state. That's why FMR is an asshole, fuck it!

		/// <summary>
		/// Starts a data stream of orders and executions into the Taskbar from the orders server or AS/400, 
		/// for the specifed branch and account. 
		/// Should be called before attempting to recieve information using GetOrdersMF/GetOrdersRZ.
		/// There is no event mechanism, polling used.
		/// </summary>
		/// <param name="sessionId">Unique Session Identification number that identifies the session 
		/// for which the function is called. Use FMRShell.Connection.GetSessionId() to get the number.
		/// Of course u need to be logged in first and have an instance of FMRShell.Connection kicking.</param>
		/// <param name="Account">It's a filter for multi-account guys.
		/// Streaming will only take place for records pertaining to this account number. Grab it from 
		/// FMRShell.Connection.Parameters.Account property.</param>
		/// <param name="Branch">Branch no. We use '000' as we don't have a branch, 
		/// FMRShell.Connection.Parameters.Branch property will do the magic in any case. </param>
		/// <param name="streamType">A filter. Choose if you want to see maof orders only, rezef orders only or both. 
		/// Specify 'MaofTradeType','RezefTradeType'or 'AllTradeType' accordingly.</param>
		/// <returns> Any value but 0 is an error:
		/// 0 : Orders Streaming started successfully.
		/// -1 : General function failure.
		/// -2 : Orders Data source initialization failure.
		/// -3 : Illegal streamType parameter.
		/// -4 : Short Account not found.
		/// -5 : Inedequate authorization.</returns>
		public virtual int OrdersStreamStart(int sessionId, string Account, string Branch, TradeType streamType)
		{
			if (marketSimulationRezef != default(JQuant.MarketSimulationRezef))
			{
				// connect market simulation to the data generator
				return 0;
			}
			//return -1;
			else return 0;
		}

		/// <summary>
		/// stops order stream upon finishing using GetOrdersMF/GetOrdersRZ probably at the end of trading session.
		/// </summary>
		/// <param name="sessionId">Grab it using FMRShell.Connection.GetSessionId().</param>
		/// <param name="Account">A filter, use FMRShell.Connection.Parameters.Account property</param>
		/// <param name="Branch">A filter, use FMRShell.Connection.Parameters.Branch property</param>
		/// <param name="streamType">'MaofTradeType','RezefTradeType'or 'AllTradeType'</param>
		/// <returns> Any value but 0 is an error:
		///  0 : Orders Streaming stopped successfully.
		///  -1 : General function failure.
		///  -2 : Orders Data source initialization failure.
		///  -3 : Unable to stop Streaming.
		///  -5 : Inedequate authorization</returns>
		public virtual int OrdersStreamStop(int sessionId, string Account, string Branch, TradeType streamType)
		{
			if (marketSimulationRezef != default(JQuant.MarketSimulationRezef))
			{
				// disconnect market simulation from the data generator
				return 0;
			}
			//else return -1;
			else return 0;
		}


		// The next pair of functions retrieve a detailed list of Maof / Rezef orders 
		// and executions for the specifed branch and account.
		// There is another pair of functions called GetOrdersMaof and GetOrdersRezef 
		// which are executed vs. AS/400. Since we are configured to work vs. dedicated orders server
		// no need to use them, maybe just in case the dedicated server is down, and use AS/400 as a backup.
		// still, this solution is a mess as we need to call the broker's IT to change the configuration.

		/// <summary>
		/// Retrieves a detailed list of Maof orders and executions for the specifed branch and account.
		/// GetOrdersRZ receives Order records either directly from the Order's Server or from an AS/400 server, 
		/// depending on the user's configuration. To enable the records to be drawn from the server, 
		/// OrdersStreamStart must first be executed. Records can be parsed using the MOFINQType structure. 
		/// </summary>
		/// <param name="sessionId">Use FMRShell.Connection.GetSessionId()</param>
		/// <param name="vecRecords">A String array into which the rezef order records will be inserted. 
		/// If strLastTime is specified and is other than "00000000", the vector will represent any orders
		/// received after that time. </param>
		/// <param name="Account">A filter, use FMRShell.Connection.Parameters.Account property</param>
		/// <param name="Branch">A filter, use FMRShell.Connection.Parameters.Branch property</param>
		/// <param name="LastTime">This is the 'Retrieve & Refresh' last time parameter. 
		/// Only records which have been updated past this time will be retrieved. 
		/// If this parameter is omitted or if "0" is specified all records are retrieved regardless of update time. </param>
		/// <returns>Upon success the function returns the total number of records retrieved into vecRecords. 
		/// -1 : General function failure. 
		/// -2 : Orders Data source initialization failure.
		/// -4 : Short Account not found.
		/// -5 : Inedequate authorization.</returns>
		public int GetOrdersMF(int sessionId, out System.Array vecRecords, string Account, string Branch, ref string LastTime)
		{
			vecRecords = null;
			return 0;
		}

		/// <summary>
		/// Retrieves a detailed list of Rezef orders and executions for the specifed branch and account.
		/// GetOrdersRZ receives Order records either directly from the Order's Server or from an AS/400 server, 
		/// depending on the user's configuration. To enable the records to be drawn from the server, 
		/// OrdersStreamStart must first be executed. Records can be parsed using the RZFINQType structure. 
		/// </summary>
		/// <param name="sessionId">Use FMRShell.Connection.GetSessionId()</param>
		/// <param name="vecRecords">A String array into which the rezef order records will be inserted. 
		/// If strLastTime is specified and is other than "00000000", the vector will represent any orders
		/// received after that time. </param>
		/// <param name="Account">A filter, use FMRShell.Connection.Parameters.Account property</param>
		/// <param name="Branch">A filter, use FMRShell.Connection.Parameters.Branch property</param>
		/// <param name="LastTime">This is the 'Retrieve & Refresh' last time parameter. 
		/// Only records which have been updated past this time will be retrieved. 
		/// If this parameter is omitted or if "0" is specified all records are retrieved regardless of update time. </param>
		/// <returns>Upon success the function returns the total number of records retrieved into vecRecords. 
		/// -1 : General function failure. 
		/// -2 : Orders Data source initialization failure.
		/// -4 : Short Account not found.
		/// -5 : Inedequate authorization.</returns>
		public virtual int GetOrdersRZ(int sessionId, out Array vecRecords, string Account, string Branch, ref string LastTime)
		{
			//respond only if there is not null order instance in waiting state
			if (this._rezefSimpleOrder.Equals(default(RezefSimpleOrder)))
			{
				vecRecords = new object[0];
			}
			else
			{
				//give it some time to respond - always get back after 4 polls:
				if (this._pollsCounter < 4)
				{
					_pollsCounter++;
					vecRecords = new object[0];
				}
				else
				{
					//reset polls counter
					this._pollsCounter = 0;

					// fill the vecRecords 
					RZFINQType data = new RZFINQType();

					Random rand = new Random();

					data.BNO_N = _rezefSimpleOrder.BNO.ToString();
					data.BNO_NAME = rand.Next().ToString();
					data.BRANCH_N = "000";
					data.COD_UPD = rand.Next().ToString();
					data.DIL_N = rand.Next().ToString(); ;
					data.DIL_NV_N = rand.Next().ToString();
					data.DIL_PRC_N = rand.Next().ToString();
					data.DIL_TIME_N = rand.Next().ToString();
					data.DSP_FMR = rand.Next().ToString();
					data.ERR = rand.Next().ToString();
					data.ERR_DATA = rand.Next().ToString();
					data.ERR_INQ = rand.Next().ToString();
					data.ERR_UPD = rand.Next().ToString();
					data.ID_MAVR_N = rand.Next().ToString();
					data.ID_N = rand.Next().ToString();
					data.ID_NAME = rand.Next().ToString();
					data.MANA_N = rand.Next().ToString();
					data.MBR_SEQ_N = rand.Next().ToString();
					data.MSG1 = rand.Next().ToString();
					data.NOSTRO = rand.Next().ToString();
					data.OP = rand.Next().ToString();
					data.OPR_NAME = rand.Next().ToString();
					data.ORDER_NO_N = rand.Next().ToString();
					data.ORDR_NV_N = rand.Next().ToString();
					data.ORDR_PRC_N = rand.Next().ToString();
					data.ORDR_SUG = rand.Next().ToString();
					data.ORDR_TIME = rand.Next().ToString();
					data.ORDR_TYPE = rand.Next().ToString();
					data.RZF_ORD_N = rand.Next().ToString();
					data.RZF_SEQ_N = rand.Next().ToString();
					data.SEQ_N = this._orderId.ToString(); //this is our AsmachtaFMR - order's unique id
					data.SND_RCV = rand.Next().ToString();
					data.STS_NAME = rand.Next().ToString();
					data.SUG_ID_N = rand.Next().ToString();
					data.SUG_INFO = rand.Next().ToString();
					data.SUG_INQ = rand.Next().ToString();
					data.SUG_MAVR_N = rand.Next().ToString();
					data.SUG_N = rand.Next().ToString();
					data.SYS_TYPE = rand.Next().ToString();
					data.TIK_N = rand.Next().ToString();

					switch (_orderState)
					{
						case rzOrdersStates.WaitingApprove:
						case rzOrdersStates.WaitingUpdate:
							data.STS = "4"; //"נקלט" - approved TASE  - just change its value in order to test various events in orders FSM
							this._orderState = rzOrdersStates.WaitingFill;
							break;
						case rzOrdersStates.WaitingFill:
							data.STS = "6"; //"בצוע מלא" - complete fill TODO: add support for partial fill
							//cleanup after the fill
							/*
							this._orderState = rzOrdersStates.Idle;
							this._rezefSimpleOrder = default(RezefSimpleOrder);
							*/
							break;
						case rzOrdersStates.WaitingCancel:
							data.STS = "7"; //"בוטל"
							//cleanup after cancel
							this._orderState = rzOrdersStates.Idle;
							this._rezefSimpleOrder = default(RezefSimpleOrder);
							break;
						default:
							data.STS = "A"; //"שגוי"
							//cleanup - the order is not valid anymore
							this._orderState = rzOrdersStates.Idle;
							this._rezefSimpleOrder = default(RezefSimpleOrder);
							break;
					}

					if (data.STS == "6")
					{
						vecRecords = new object[0];
					}
					else
					{
						vecRecords = new object[1];
						vecRecords.SetValue(data, 0);
					}

					//fill LastTime with the value
					LastTime =
						DateTime.Now.Hour.ToString() +
						DateTime.Now.Minute.ToString() +
						DateTime.Now.Second.ToString() +
						DateTime.Now.Millisecond.ToString()
						;

					this._pollsCounter = 0;

				}
			}
			return vecRecords.Length;
		}



		/// <summary>
		/// Begins Maof trading session.
		/// </summary>
		/// <param name="sessionId">A <see cref="System.Int32"/>
		/// Unique Session Identification number that 
		/// identifies the session for which the function is called.</param>
		/// <param name="Account">A<see cref="System.String"/>
		/// The account for which the maof session will be initialized.</param>
		/// <param name="Branch">A<see cref="System.String"/>
		/// The branch for which the maof session will be initialized.</param>
		/// <param name="calcType">A<see cref="TaskBarLibSim.SecurityCalcType"/>
		/// Type of calculation type for calculating account securities.
		/// It is ususally set to RM_Dill_ShortOrders.</param>
		/// <returns>A <see cref="System.Int32"/>  
		/// 1 : Unable to begin Maof session.
		/// 0 : Maof session started successfully.
		/// -1 : General function failure.
		/// -2 : Account or branch parameters not relayed to function.
		/// -3 : DataBase failure.
		/// -4 : Account not found.
		/// -5 : Short Account not found.
		/// -6 : Inadequate "Generator" authorization.
		/// -7 : Inadequate User authorization.</returns>
		public virtual int StartMaofSession(int sessionId, string Account, string Branch, SecurityCalcType calcType)
		{
			return 0;
		}

		/// <summary>
		/// Terminates Maof trading session.
		/// </summary>
		/// <param name="sessionId"><param name="sessionId">A <see cref="System.Int32"/>
		/// Unique Session Identification number that 
		/// identifies the session for which the function is called.</param>
		/// <param name="Account">A<see cref="System.String"/>
		/// The account for which the maof session will be initialized.</param>
		/// <param name="Branch">A<see cref="System.String"/>
		/// The branch for which the maof session will be initialized.</param>
		/// <returns>A <see cref="System.Int32"/> 
		///  1 : Unable to terminate Maof session.
		///  0 : Maof session terminated successfully.
		///  -1 : General function failure.
		///  -5 : Inadequate user permissions.</returns>
		public virtual int StopMaofSession(int sessionId, string Account, string Branch)
		{
			return 0;
		}

		JQuant.MarketSimulationRezef marketSimulationRezef = default(JQuant.MarketSimulationRezef);

		/// <summary>
		/// Begins Rezef trading session.
		/// This is simulation code. TaskBarLibSim.K300Class.InitStreamSimulation() should be called before call to this method
		/// </summary>
		/// <param name="sessionId">A <see cref="System.Int32"/>
		/// Unique Session Identification number that 
		/// identifies the session for which the function is called.</param>
		/// <param name="Account">A<see cref="System.String"/>
		/// The account for which the rezef session will be initialized.</param>
		/// <param name="Branch">A<see cref="System.String"/>
		/// The branch for which the rezef session will be initialized.</param>
		/// <param name="calcType">A<see cref="TaskBarLibSim.SecurityCalcType"/>
		/// Type of calculation type for calculating account securities.
		/// It is ususally set to RM_Dill_ShortOrders.</param>
		/// <returns>A <see cref="System.Int32"/>  
		/// 1 : Unable to begin Rezef session.
		/// 0 : Rezef session started successfully.
		/// -1 : General function failure.
		/// -2 : Account or branch parameters not relayed to function.
		/// -3 : DataBase failure.
		/// -4 : Account not found.
		/// -5 : Short Account not found.
		/// -6 : Inadequate User authorization.</returns>
		public virtual int StartRezefSession(int sessionId, string Account, string Branch)
		{
			// create market simulation here 
			// Start market simulation 

			if (marketSimulationRezef == default(JQuant.MarketSimulationRezef))
			{
				//TODO: this one causes freeze on calling 'exit'
				//marketSimulationRezef = new JQuant.MarketSimulationRezef();

			}
			// connect data generators to the market simulation (somebody should call InitStreamSimulation) before i reach this line
			// it is done in the OrdersStreamStart


			// enable some debug trace to see trading on the screen. some signs of life
			/*
			marketSimulationRezef.EnableTrace(80608128);

			marketSimulationRezef.EnableTrace(80616808);
			*/
			return 0;
		}

		/// <summary>
		/// Terminates Rezef trading session
		/// </summary>
		/// <param name="sessionId">session id</param>
		/// <param name="Account">account no.</param>
		/// <param name="Branch">branch no.</param>
		/// <returns>A <see cref="System.Int32"/>  
		///  0 : Rezef session terminated successfully.
		/// -1 : General function failure.
		/// -2 : Inadequate user permissions.</returns>
		public virtual int StopRezefSession(int sessionId, string Account, string Branch)
		{
			// i need an accurate cleanup - market simulation is far from trivial
			if (marketSimulationRezef != default(JQuant.MarketSimulationRezef))
			{
				// disconnect market simulation from the data generator in 
				// it is done in the OrdersStreamStop

				marketSimulationRezef.Dispose();
				marketSimulationRezef = default(JQuant.MarketSimulationRezef);
			}

			return 0;
		}

		/// <summary>
		/// Outdated. Use SendOrderRZ instead.
		/// </summary>
		public virtual int SendRezefOrder(int sessionId, ref RezefBasicOrder Order, string Location, string Trade_Type, ref string VBMsg, ref int ErrNO,
				out OrdersErrorTypes ErrorType, ref int OrderID, string AuthUserName, string AuthPassword, string ReEnteredValue)
		{
			ErrorType = OrdersErrorTypes.NoError;
			// i need an accurate cleanup - market simulation is far from trivial
			if (marketSimulationRezef == default(JQuant.MarketSimulationRezef))
			{
				return -1;
			}

			// place order in the MarketSimulation, get OrderId
			// return Ok
			return 0;
		}

		public virtual int SendContOrderRZ(int sessionId, ref RezefContinuousOrder Order, ref int AsmachtaFmr, int AsmachtaRezef, out string VBMsg, out int ErrNO,
				out OrdersErrorTypes ErrorType, ref int OrderID, string AuthUserName, string AuthPassword, string ReEnteredValue)
		{
			ErrorType = OrdersErrorTypes.NoError;
			ErrNO = 0;
			VBMsg = "";
			return 0;
		}

		public virtual int SendMaofOrder(int sessionId, ref MaofOrderType Order, ref string VBMsg, ref int ErrNO, out OrdersErrorTypes ErrorType,
				ref int OrderID, string AuthUserName, string AuthPassword, string ReEnteredValue, int SPCOrder)
		{
			ErrorType = OrdersErrorTypes.NoError;
			return 0;
		}


		/// <summary>
		/// i am using MarketSimulation to handle the orders. I do not need all arguments of the FMR API. 
		/// Only RezefBasicOrder and OrderID are in this phase. 
		/// This one is going to be our workhorse at the moment.
		/// Sends Single Rezef Order. Replaces SendRezefOrder function.
		/// </summary>
		/// <param name="sessionId">Use FMRShell.Connection.GetSessionId()</param>
		/// <param name="Order">Structure of RezefSimpleOrder Type containing all details regarding the order that is being sent.</param>
		/// <param name="AsmachtaFmr">Unique order reference number provided by FMR systems. 
		/// This reference is returned as an out parameter, and should be provided when updating or canceling the order.</param>
		/// <param name="AsmachtaRezef">Unique order reference number provided by the stock exchange. 
		/// The reference should be retrieved using order retrieval function. Must be provided when updating/canceling.</param>
		/// <param name="VBMsg">If the sending order operation is unsuccessful, a message is relayed back to the client application 
		/// with the reason for the failure. If the operation is successful, VBMsg remains empty. </param>
		/// <param name="ErrNO">If an error occurs during the operation, an error number will be relayed back to the client application.
		/// see wiki/ fmr's help for details.</param>
		/// <param name="ErrorType">Type of error returned by function call. 
		/// Error type is defined by OrdersErrorTypes enumeration. </param>
		/// <param name="OrderID">Unique order id allocated by taskbar returned by function for order transaction identification. 
		/// Must be provided when resending order due to user confirmation requirement. </param>
		/// <param name="AuthUserName">In the event that the operation requires password authorization, 
		/// this field will contain the authorizing user name. </param>
		/// <param name="AuthPassword">In the event that the operation requires password authorization, 
		/// this field will contain the password for the authorizing user. </param>
		/// <param name="ReEnteredValue">In case that the operation requires confirmation, 
		/// the field will contain the value re-entered by the user. </param>
		/// <returns>The function returns 0 upon success and -1 upon failure. </returns>
		public virtual int SendOrderRZ(int sessionId, ref RezefSimpleOrder Order, ref int AsmachtaFmr, int AsmachtaRezef, out string VBMsg, out int ErrNO,
				 out OrdersErrorTypes ErrorType, ref int OrderID, string AuthUserName, string AuthPassword, string ReEnteredValue)
		{
			//initialize data
			int rc = 0;
			VBMsg = "";
			ErrNO = 0;
			ErrorType = OrdersErrorTypes.NoError;

			//continue flag
			bool _continue = false;

			//check the arguments, internal error if something is wrong
			if (sessionId != this._sessionId)
			{
				VBMsg = "Sending failure";
				ErrNO = -2;
				ErrorType = OrdersErrorTypes.Fatal;
				rc = -1;
			}

			else
			{
				if (
				Order.operation == OrderOperation.OrderOperationDelete ||
				Order.operation == OrderOperation.OrderOperationUpdBuy ||
				Order.operation == OrderOperation.OrderOperationUpdSell)
				{
					if (AsmachtaFmr == 0 || AsmachtaRezef == 0)
					{
						VBMsg = "Missing parameters in RezefBasicOrder Type.";
						ErrNO = 91;
						ErrorType = OrdersErrorTypes.Fatal;
						rc = -1;
					}
				}

				if (Order.Amount >= 99999)
				{
					if (ReEnteredValue == "YES")
					{
						_continue = true;
					}
					else
					{
						VBMsg = " Order value above permitted limit.";
						ErrNO = 9;
						ErrorType = OrdersErrorTypes.Confirmation;
						rc = -1;
					}
				}

				else if (Order.price <= 0)
				{
					if (ReEnteredValue == default(string))
					{
						VBMsg = "LMT Order value above permitted limit.";
						ErrNO = 3;
						ErrorType = OrdersErrorTypes.ReEnter;
						rc = -1;
					}
					else
					{
						Order.price = JQuant.Convert.StrToDouble(ReEnteredValue);
						_continue = true;
					}
				}

				else if (Order.price > 50000)
				{
					if (AuthPassword == default(string) ||
						AuthUserName == default(string))
					{
						VBMsg = "Illegal change from base rate.";
						ErrNO = 25;
						ErrorType = OrdersErrorTypes.PasswordReq;
						rc = -1;
					}
					else
						_continue = true;
				}

				else if (Order.price == 1)
				{
					VBMsg = "Illegal difference between Closing price and Base price.";
					ErrNO = 22;
					ErrorType = OrdersErrorTypes.Alert;
					_continue = true;
				}
				else
				{
					_continue = true;
				}

				if (_continue)
				{
					//only in case of success the order is passed to TASE:

					switch (Order.operation)
					{
						//a new order
						case OrderOperation.OrderOperationNewBuy:
						case OrderOperation.OrderOperationNewSell:
							this._rezefSimpleOrder = Order;
							this._orderId++;            //generate unique order id
							AsmachtaFmr = _orderId;     //initialize ref no
							this._pollsCounter = 0;
							this._orderState = rzOrdersStates.WaitingApprove;
							break;
						//cancel
						case OrderOperation.OrderOperationDelete:
							this._pollsCounter = 0;
							this._orderState = rzOrdersStates.WaitingCancel;
							break;
						//update
						case OrderOperation.OrderOperationUpdBuy:
						case OrderOperation.OrderOperationUpdSell:
							this._rezefSimpleOrder = Order; //just copy updated order
							this._pollsCounter = 0;
							this._orderState = rzOrdersStates.WaitingUpdate;
							break;
					}
				}
			}

			return rc;
		}

		/// <summary>
		/// Sends Multiple Rezef Orders, up to 90 different orders.
		/// </summary>
		/// <param name="sessionId">Unique Session Identification number that identifies the session for which the function is called.</param>
		/// <param name="Orders">Array of RezefBasicOrder structures containing all details regarding the orders being sent.</param>
		/// <param name="Location">Should always receive the value "CNT". It was previously possible to trade at other locations.</param>
		/// <param name="Trade_Type">Should always receive the value "REZEF". It was previously possible to trade using different trading types.</param>
		/// <param name="ErrNO">If an error occurs during the operation, an error number will be relayed back to the client application.</param>
		/// <returns>The function returns 0 upon success and -1 upon failure.</returns>
		public virtual int SendOrderSpeedRZ(int sessionId, ref System.Array Orders, string Location, string Trade_Type, ref int ErrNO)
		{
			return 0;
		}

		public virtual int UserAuthentication(int sessionId, string Password)
		{
			return 0;
		}

		public virtual int UpdateUserPassword(int sessionId, string strOldPassword, string strNewPassword, string strUserNumber)
		{
			return 0;
		}

		// NOT a part of real TaskBar - I need it for testing purposes
		// this is simulation object which implements some basic logic of orders processing 
		// by TaskBar and generates events in response for orders activity by RezefOrderFSM
		// generally is a placeholder and a simple state machine
		protected enum rzOrdersStates
		{
			[Description("No active order in the system")]
			Idle,
			[Description("A new order - SendOrderRZ called, waiting for response")]
			WaitingApprove,
			[Description("Existing order update - SendOrderRZ called, waiting for response")]
			WaitingUpdate,
			[Description("Existing order cancel -  - SendOrderRZ called, waiting for response")]
			WaitingCancel,
			[Description("Existing order is on the limit book, waiting for fill")]
			WaitingFill,
		}

		protected RezefSimpleOrder _rezefSimpleOrder;
		//counts orders, unique id ( = AsmachtaFMR )
		protected int _orderId;
		//order state
		protected rzOrdersStates _orderState;

		//count polls, initialized only if Order is initialized
		protected int _pollsCounter;

	}//partial class UserClass
}//namespace TaskBarLibSim

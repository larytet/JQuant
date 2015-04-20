
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.IO;


/// <summary>
/// I want to run TaskBarLib simulation. I am going to implement API using prerecorded
/// or generated in some other way log files as a data feed
/// Only small part of the API is implemented
/// This class is used by the FMRShell and allows to simulate different scenarios
/// and play back previously recorded log files
/// In the future this class will contain engines which simulate behaviour of the
/// real system
///
/// Side note. Another approach to the simulation is to install a real server running
/// simulator and let the rest of the application including 3rd party code (DLLs from
/// FMR) use the simulation server. At this point it looks like an overkill. Thanks to
/// the time stamps i can simulate the system behaviour fairly close.
/// </summary>
namespace TaskBarLibSim
{
    public enum MonthType
    {
        AllMonths = -1,
        April = 4,
        August = 8,
        December = 12,
        February = 2,
        January = 1,
        July = 7,
        June = 6,
        March = 3,
        May = 5,
        November = 11,
        October = 10,
        September = 9
    }

    public enum MadadTypes
    {
        AllMadad = -1,
        BANK = 4,
        BINUI = 6,
        CURRENCYLNKD = 0x17,
        DOLLAR = 5,
        GOV_CHG0 = 20,
        GOV_CHG5 = 0x15,
        GOV_FIXED0 = 0x11,
        GOV_FIXED2 = 0x12,
        GOV_FIXED5 = 0x13,
        MAALE = 14,
        MAKAM = 0x16,
        TELBOND = 11,
        TELBOND40 = 15,
        TELBOND60 = 0x10,
        TELDIV20 = 10,
        TLTK = 2,
        TLV100 = 3,
        TLV25 = 0,
        TLV75 = 1,
        TLVFIN = 7,
        TLVNADLAN15 = 8,
        YETER120 = 12,
        YETER30 = 9,
        YETER50 = 13
    }

    public enum K300StreamType
    {
        IndexStream = 0x34,
        MaofCNTStream = 50,  //out of date, not used anymore
        MaofStream = 0x30,
        RezefCNTStream = 0x33,  //out of date, not used anymore
        RezefStream = 0x31
    }

    public enum StockKind
    {
        StockKindAgach = 1,
        StockKindAll = -1,
        StockKindKeren = 3,
        StockKindMakam = 2,
        StockKindMenaya = 0
    }

    public enum LoginStatus
    {
        LoginSessionActive,
        LoginSessionInProgress,
        LoginSessionInactive,
        LoginSessionDBInitFailure,
        LoginSessionAS400Failure,
        LoginSessionPasswordExpired,
        LoginSessionPasswordChangeFailure,
        LoginSessionPasswordChangedToday,
        LoginSessionWrongUserOrPassword,
        LoginSessionMaxUsersLimit,
        LoginSessionReLogin,
        LoginSessionTaskBarBlocked
    }

    public enum OnlineSessionType
    {
        OnlineSessionTypeAccounts,
        OnlineSessionTypeAll,
        OnlineSessionTypeCustodian,
        OnlineSessionTypeKranot,
        OnlineSessionTypeKupa
    }

    public enum QueryType
    {
        qtDetailed = 0x30,
        qtSummary = 0x31
    }

    public enum YieldDataType
    {
        YieldData5YearsbyYear = 4,
        YieldDataMonthbyDay = 1,
        YieldDataTwelveMonths = 3,
        YieldDataYearbyMonth = 2,
        YieldDataYearbyQuater = 5
    }

    /// <summary>
    /// Query types enumaration
    /// </summary>
    public enum ConnectionState
    {
        csOpen,
        csProcessing,
        csClosed
    }

    public enum LoginLevel
    {
        LoginLevelAccounts = 3,
        LoginLevelMax = 50,
        LoginLevelOptionsStocks = 2,
        LoginLevelPermissions = 1
    }

    /// <summary>
    /// This type is represents the timestamps from AS400 servers
    /// </summary>
    public struct AS400DateTime
    {
        public int year;
        public int Month;
        public int day;
        public int hour;
        public int minute;
        public int second;
        public int ms;
    }

    public struct K300MaofType
    {
        public string SUG_REC;
        public string TRADE_METH;
        public string BNO_Num;
        public string LAST_REC;
        public string SIDURI_Num;
        public string SYMBOL_E;
        public string Symbol;
        public string BNO_NAME_E;
        public string BNO_NAME;
        public string BRANCH_NO;
        public string BRANCH_U;
        public string SUG_BNO;
        public string MIN_UNIT;
        public string HARIG_NV;
        public string MIN_PR;
        public string MAX_PR;
        public string BASIS_PRC;
        public string BASIS_COD;
        public string STATUS_COD;
        public string EX_DATE;
        public string EX_PRC;
        public string VL_MULT;
        public string VL_COD;
        public string ZERO_COD;
        public string shlav;
        public string STATUS;
        public string TRD_STP_CD;
        public string TRD_STP_N;
        public string STP_OPN_TM;
        public string LMT_BY1;
        public string LMT_BY2;
        public string LMT_BY3;
        public string LMY_BY1_NV;
        public string LMY_BY2_NV;
        public string LMY_BY3_NV;
        public string RWR_FE;
        public string LMT_SL1;
        public string LMT_SL2;
        public string LMT_SL3;
        public string LMY_SL1_NV;
        public string LMY_SL2_NV;
        public string LMY_SL3_NV;
        public string RWR_FF;
        public string PRC;
        public string COD_PRC;
        public string SUG_PRC;
        public string LST_DF_BS;
        public string RWR_FG;
        public string LST_DL_PR;
        public string LST_DL_TM;
        public string LST_DL_VL;
        public string DAY_VL;
        public string DAY_VL_NIS;
        public string DAY_DIL_NO;
        public string RWR_FH;
        public string DAY_MAX_PR;
        public string DAY_MIN_PR;
        public string POS_OPN;
        public string POS_OPN_DF;
        public string STS_NXT_DY;
        public string UPD_DAT;
        public string UPD_TIME;
        public string FILER;

        // This is a special ingredient for the data playback. When running playback I have internal timestamps 
        // in the log.
        // When I play the data log file back I want the system to use the original time stamps. 
        public DateTime TimeStamp;
        public long Ticks;
    }

    public struct K300RzfType
    {
        public string SUG_REC;
        public string BNO_Num;
        public string BNO_NAME;
        public string Symbol;
        public string TRADE_METH;
        public string SIDURI_Num;
        public string RWR_VA;
        public string MIN_UNIT;
        public string HARIG_NV;
        public string MIN_PR_OPN;
        public string MAX_PR_OPN;
        public string MIN_PR_CNT;
        public string MAX_PR_CNT;
        public string BASIS_PRC;
        public string STATUS;
        public string EX_COD;
        public string EX_DETAIL;
        public string RWR_VB;
        public string shlav;
        public string LAST_PRC;
        public string TRD_STP_N;
        public string STP_OPN_TM;
        public string RWR_VD;
        public string LMT_BY1;
        public string LMT_BY2;
        public string LMT_BY3;
        public string LMY_BY1_NV;
        public string LMY_BY2_NV;
        public string LMY_BY3_NV;
        public string MKT_NV_BY;
        public string MKT_NV_BY_NUM;
        public string RWR_VE;
        public string LMT_SL1;
        public string LMT_SL2;
        public string LMT_SL3;
        public string LMY_SL1_NV;
        public string LMY_SL2_NV;
        public string LMY_SL3_NV;
        public string MKT_NV_SL;
        public string MKT_NV_SL_NUM;
        public string RWR_VF;
        public string THEOR_PR;
        public string THEOR_VL;
        public string RWR_VG;
        public string LST_DL_PR;
        public string LST_DL_TM;
        public string LST_DF_BS;
        public string LST_DF_OPN;
        public string LST_DL_VL;
        public string DAY_VL;
        public string DAY_VL_NIS;
        public string DAY_DIL_NO;
        public string DAY_MAX_PR;
        public string DAY_MIN_PR;
        public string BNO_NAME_E;
        public string SYMBOL_E;
        public string STP_COD;
        public string COD_SHAAR;
        public string UPD_DAT;
        public string UPD_TIME;

        // This is a special ingredient for the data playback. When running playback I have internal timestamps 
        // in the log.
        // When I play the data log file back I want the system to use the original time stamps. 
        public DateTime TimeStamp;
        public long Ticks;
    }

    public struct K300MadadType
    {
        public string SUG_RC;
        public string BNO_N;
        public string FIL1_VK;
        public string MDD_COD;
        public string MDD_SUG;
        public string MDD_N;
        public string FIL2_VK;
        public string MDD_NAME;
        public string Madad;
        public string FIL3_VK;
        public string MDD_DF;
        public string CALC_TIME;
        public string FIL6_VK;
        public string UPD_DAT;
        public string UPD_TIME;
    }

    /// <summary>
    /// Defines securitys' weights in different TASE indices
    /// (the weights change daily)
    /// </summary>
    public struct SH161Type
    {
        public long BNO;               // Security's TASE Id
        public string BNO_NAME;        // security's Hebrew name
        public double PRC;             // Base price (usually it's previous close adjusted for splits and dividends
        public double HON_RASHUM;      // registered capital
        public double PCNT;            // security's weight in the index
        public long MIN_NV;            // 
        public double BNO_IN_MDD;      // 
        public double PUBLIC_PRCNT;    // 
        public double NV_TZAFA;        // 
    }

    /// <summary>
    /// used to parse the records returned from User.GetOnlineRM
    /// this one is for cash balance
    /// </summary>
    public struct RmOnlineTotalInfoType
    {
        string sBaseCash;	//����� ���� = ����� ������
        string sLastCash;	//����� ����� = ����� �����
        string sCashChange;	//����� �����
        string sBaseVL;		//���� ���� =  ���� ������
        string sLastVL;		//���� ����� - ���� �����
        string sProfitLoss;	//����/���� = ���� ����� ����
        string sDate;		//����� + ���
        string sCngCost;	//����� ����
    }

    /// <summary>
    /// used to parse the records returned from User.GetOnlineRM
    /// this one is for any security except cash
    /// </summary>
    public struct RmOnlineRecordType
    {
        string sBnoName;	//�� ����
        string sBnoNum;		//���� ����
        string sSugBno;		//��� ����
        string sBasePrice;	//��� ���� = ���� ������
        string sBaseNV;		//���� ���� = ���� ������
        string sBaseVL;		//���� ���� = ��� ������
        string sLastPrice;	//��� �����
        string sLastNV;		//���� ������ = ���� ������
        string sLastVL;		//���� ����� = ��� �����
        string sVLChange;	//����� ����� = ����� ���
        string sMoneyMoved;	//�����  = ����� �����
        string sProfitLoss;	//����/���� = ���� ����� ����
        string sTotalCost;	//����
        string sCngCost;	//����/����� �����
        string sAverageCost;//���� ������
        string sCngCostPcnt;//����� ����� �������
        string lvl1;		//��.����� ��� ���
        string lvl2;		//��.����� ��� ��''�
        string lvl3;		//��.����� �.����                              
        string lvl4;		//��.����� �������                              
        string lvl5;		//��.����� �������                              
        string lvl6;		//��.����� �����  
    }

    /// <summary>
    /// User Password Structure
    /// </summary>
    public struct UserPasswordType
    {
        //��� �����      
        public string SUG_RC;
        //���� �����        
        public string USR_NAME;
        //�����        
        public string USR_PW;
        //����        
        public string Branch;
        //���        
        public string Sug;
        //����� ����        
        public string MFZL_ID;
        //���� ���        
        public string TIK_N;
        //�� �����        
        public string ID_N;
        //�� ��������        
        public string APPL_N;
        //Device Name        
        public string DEVICE_N;
        //��� �����        
        public string AUTH_LVL;
        //Field        
        public string FIELD;
        //Operator        
        public string OPERATOR;
        //Value        
        public string val;
        //����� ����� �����        
        public string UPD_DAT;
        //����� ����        
        public string END_DAT;
        //�� ����� �����        
        public string UPD_USR_N;
        //���� ����� �����        
        public string FROM_USR_N;
        //FIL        
        public string FIL;
    }


    /// <summary>
    /// Account Structure
    /// </summary>
    struct AccountType
    {
        //����        
        public string BRANCH_NO;
        //���� �����

        public string ID_NO;

        public string ID_FIL;
        //���� ���        

        public string TIK_NO;
        //�� �����        

        public string ID_NAME;

        public string REQ_MRGN;

        public string RCRDIT_MAOF;
        //���� ���        

        public string ID_TOWN;
        //���� �����        

        public string ID_PHONE_NO;

        public string CRDIT_LN;

        public string CRDIT_MAOF;

        public string MNIOT_VL;

        public string ID_TOT_VL;

        public string MIN_COM;

        public string MIN_C_MAOF;

        public string MAX_C_MAOF;

        public string MIN_C_FUTR;

        public string MAX_C_FUTR;
        //�����        

        public string SIVUG;

        public string MAOF_COD;

        public string FREEZ_COD;

        public string FREEZ_TEXT;

        public string IMM_DRAFT;

        public string COD_H_BNO;

        public string COD_H_MAOF;

        public string COD_H_KUPA;

        public string COD_H_FRGN;

        public string COD_H_FUTR;

        public string COD_H_CURR;

        public string ID_PCNT;
        //���        

        public string SUG_ID;

        public string TAT_SIVUG;

        public string EXT_MARGIN;

        public string ITRA_CASH;

        public string ID_STREET;

        public string PRE_MAOF1;

        public string PRE_MAOF2;

        public string PRE_MAOF3;

        public string PRE_MAOF4;

        public string PRE_MAOF5;

        public string MDL;
        //��� ����� ������        

        public string TEL_PW;

        public string FIL;
    }


    public delegate void IK300Event_FireMaofCNTEventHandler(ref Array psaStrRecords, ref int nRecords);
    public delegate void IK300Event_FireMaofEventHandler(ref Array psaStrRecords, ref int nRecords);
    public delegate void IK300Event_FireRezefCNTEventHandler(ref Array psaStrRecords, ref int nRecords);
    public delegate void IK300Event_FireRezefEventHandler(ref Array psaStrRecords, ref int nRecords);

    public interface IK300
    {
        int GetMAOF(ref Array vecRecords, ref string strLastTime, string strOptionNumber, MadadTypes strMadad);
        int GetMAOFRaw(ref Array vecRecords, ref string strLastTime, string strOptionNumber, MadadTypes strMadad);
        void StopUpdate(int pnID);
        int StartStream(K300StreamType streamType, string strStockNumber, MadadTypes strMadad, int withEvents);
        int K300StartStream(K300StreamType streamType);
        int K300StopStream(K300StreamType streamType);
    }

    public interface K300 : IK300, IK300Event_Event
    {
    }

    public interface IK300Event_Event
    {
    }


    public interface IK300Events
    {
    }

    public interface K300Events : IK300Events, _IK300EventsEvents_Event
    {
    }

    public delegate void _IK300EventsEvents_OnMaofEventHandler(ref K300MaofType data);
    public delegate void _IK300EventsEvents_OnRezefEventHandler(ref K300RzfType data);
    public delegate void _IK300EventsEvents_OnMadadEventHandler(ref K300MadadType data);

    public interface _IK300EventsEvents_Event
    {
        // Events
        event _IK300EventsEvents_OnMaofEventHandler OnMaof;
        event _IK300EventsEvents_OnRezefEventHandler OnRezef;
    }

    public class K300Class : IK300, K300, IK300Event_Event
    {
        public K300Class()
        {
            maofStreamStarted = false;
            rezefStreamStarted = false;
            SimulationTop.k300Class = this;
        }

        // Methods
        public virtual int GetMAOF(ref Array vecRecords, ref string strLastTime, string strOptionNumber, MadadTypes strMadad)
        {
            return 0;
        }

        public virtual int GetMAOFRaw(ref Array vecRecords, ref string strLastTime, string strOptionNumber, MadadTypes strMadad)
        {
            return 0;
        }

        public virtual int GetBaseAssets2(out System.Array psaRecords, int BaseAssetCode)
        {
            int result = -1;

            if (SimulationTop.PlayBackLogNum != 0)
            {
                double[] TA25Basevalues = {1119.68,1120.6,1105.34,1115.17,1117.38,1130.1,1146.28,1135.99,1142.73,1138.37,1140.69,1145.06,1157.9,1163.11,1168.65,1169.75,1166.23,1179.12,1169.71,1145.85,1151.9,1146.95,1159.23,1156.38,1151.92,1128.98,1136.36,1128.47,1125.04,1135.79,1130.8,1135.17,1127.39,1129.03,1136.46,1151.13,1166.53,1168.21,1165.96,1172.94,1176.47,1169.46,1172.34,1159.61,1187.08,1195.72,1195.03,1207.63,1215.33,1207.01,1198.81,1208.09,1209.4,1207.92,1210.49,1221.04,1202.28,1216.24,1225.44,1231.3,1232.66,1229.07,1237.85,1230.18,1227.74,1220.46,1220.62,1231.18,1215.01,1203.84,1207.64,1199.63,1208.54,1204.1,1199.44,1185.99,1164.23,1166.69,1173.75,1153.8,1133.44,1140.61,1128.14,1165.06,1145.04,1163.97,1154.62,1124.83,1131.26,1101.45,1127.37,1111.98,1088.16,1107.12,1103.84,1097.38,1076.98,1080.5,1075.92,1089.93,1090.8,1103.36,1110.64,1108.13,1098.96,1106.72,1126.09,1132.43,1126.28,1117.65,1093.65,1086.49,1093.49,1070.67,1062.45};
                
                psaRecords = System.Array.CreateInstance(typeof(double), 1);
                psaRecords.SetValue(TA25Basevalues[SimulationTop.PlayBackLogNum - 1],0);
                result = 1;
                //debug print
                Console.WriteLine("The base TA25 index is " + psaRecords.GetValue(0) + " for log no. " + SimulationTop.PlayBackLogNum);
            }
            else
            {
                psaRecords = null;
            }

            return result;
        }

        public virtual int GetBaseAssets(out System.Array psaRecords, int BaseAssetCode)
        {
            psaRecords = null;
            return -1;
        }

        /// <summary>
        /// retreive weight of securities in indexes.
        /// </summary>
        /// <param name="vecRecords">Array in which <see cref="TaskBarLibSim.SH161Type"/>
        /// data is kept. Containing all the records for the index specified in parameter madadSymbol </param>
        /// <param name="madadSymbol">Only stocks included in this MadadType index will be retrieved. </param>
        /// <returns>Upon success the function returns the total 
        /// number of SH161Type records retrieved into the vecRecords array. 0 if no records were found. </returns>
        public virtual int GetSH161(ref System.Array vecRecords, MadadTypes madadSymbol)
        {
            int result = -1;
            vecRecords = System.Array.CreateInstance(typeof(SH161Type), 25);

            //this is a patch - will need to fix it some day
            string filename;
            if (SH161LogFileName == null)
                filename = Environment.GetEnvironmentVariable("JQUANT_ROOT") + "sh161.csv";
            else
                filename = SH161LogFileName;

            FileStream fileStream;
            StreamReader streamReader;

            try
            {
                fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
                streamReader = new StreamReader(fileStream, System.Text.Encoding.UTF7, false, 1024);

                string str;

                do
                {
                    try
                    {
                        str = streamReader.ReadLine();

                        if (result >= 0)
                        {
                            SH161Type data = new SH161Type();

                            int from = 0;
                            int to = str.IndexOf(",", from);

                            data.BNO = System.Convert.ToInt64(str.Substring(from, (to - from)));
                            from = to + 1; to = str.IndexOf(",", from);

                            data.BNO_NAME = str.Substring(from, (to - from));
                            from = to + 1; to = str.IndexOf(",", from);

                            data.PRC = System.Convert.ToDouble(str.Substring(from, (to - from)));
                            from = to + 1; to = str.IndexOf(",", from);

                            data.HON_RASHUM = System.Convert.ToDouble(str.Substring(from, (to - from)));
                            from = to + 1; to = str.IndexOf(",", from);

                            data.PCNT = System.Convert.ToDouble(str.Substring(from, (to - from)));
                            from = to + 1; to = str.IndexOf(",", from);

                            data.MIN_NV = System.Convert.ToInt64(str.Substring(from, (to - from)));
                            from = to + 1; to = str.IndexOf(",", from);

                            data.BNO_IN_MDD = System.Convert.ToDouble(str.Substring(from, (to - from)));
                            from = to + 1; to = str.IndexOf(",", from);

                            data.PUBLIC_PRCNT = System.Convert.ToDouble(str.Substring(from, (to - from)));
                            from = to + 1; to = str.Length;

                            data.NV_TZAFA = System.Convert.ToDouble(str.Substring(from, (to - from)));

                            vecRecords.SetValue(data, result);
                        }
                        result++;
                    }
                    catch (IOException e)
                    {
                        System.Console.WriteLine("Failed to read file " + filename);
                        System.Console.WriteLine(e.ToString());
                        break;
                    }

                }
                while (result < 25);

                fileStream.Close();
                fileStream = default(FileStream);
                streamReader = default(StreamReader);
            }
            catch (IOException e)
            {
                System.Console.WriteLine("Failed to open file " + filename);
                System.Console.WriteLine(e.ToString());
            }

            return result;
        }

        public static string SH161LogFileName;

        public virtual int K300StartStream(K300StreamType streamType)
        {
            switch (streamType)
            {
                case K300StreamType.MaofStream:
                    {
                        // start data generation thread
                        maofGenerator.Start();

                        // set flag to keep track of the started streams
                        maofStreamStarted = true;
                        break;
                    }

                case K300StreamType.RezefStream:
                    {
                        rezefGenerator.Start();
                        rezefStreamStarted = true;
                        break;
                    }

                case K300StreamType.IndexStream:
                    {
                        madadGenerator.Start();
                        madadStreamStarted = true;
                        break;
                    }

                default:
                    break;
            }
            return 0;
        }

        /// <summary>
        /// initializes the simulation
        /// actual simulation stream will start by the K300StartStream
        /// </summary>
        public static void InitStreamSimulation(ISimulationDataGenerator<K300MaofType> maofGenerator)
        {
            K300Class.maofGenerator = maofGenerator;
        }

        public static void InitStreamSimulation(ISimulationDataGenerator<K300RzfType> rzfGenerator)
        {
            K300Class.rezefGenerator = rzfGenerator;
        }

        public static void InitStreamSimulation(ISimulationDataGenerator<K300MadadType> mddGenerator)
        {
            K300Class.madadGenerator = mddGenerator;
        }

        public virtual int K300StopStream(K300StreamType streamType)
        {
            switch (streamType)
            {
                case K300StreamType.MaofStream:
                    maofGenerator.Stop();
                    break;

                case K300StreamType.RezefStream:
                    System.Console.WriteLine("Stop Rezef generator");
                    rezefGenerator.Stop();
                    break;

                case K300StreamType.IndexStream:
                    madadGenerator.Stop();
                    break;

                default:
                    break;
            }
            return 0;
        }

        public virtual int StartStream(K300StreamType streamType, string strStockNumber, MadadTypes strMadad, int withEvents)
        {
            maofGenerator.Start();
            return 0;
        }

        public virtual void StopUpdate(int pnID)
        {
        }

        public int K300SessionId
        {
            set;
            get;
        }

        protected bool maofStreamStarted;
        protected bool rezefStreamStarted;
        protected bool madadStreamStarted;
        protected static ISimulationDataGenerator<K300MaofType> maofGenerator;
        protected static ISimulationDataGenerator<K300RzfType> rezefGenerator;
        protected static ISimulationDataGenerator<K300MadadType> madadGenerator;
    }

    public class K300EventsClass : IK300Events, K300Events, _IK300EventsEvents_Event
    {

        public K300EventsClass()
        {
            SimulationTop.k300EventsClass = this;
        }

        // Events
        public event _IK300EventsEvents_OnMaofEventHandler OnMaof;
        public event _IK300EventsEvents_OnRezefEventHandler OnRezef;
        public event _IK300EventsEvents_OnMadadEventHandler OnMadad;

        /// <summary>
        /// part of the simulation
        /// send event to all registered users 
        /// </summary>
        public void SendEventMaof(ref K300MaofType data)
        {
            OnMaof(ref data);
        }

        /// <summary>
        /// Send simulated Rezef data to all registered users
        /// </summary>
        public void SendEventRzf(ref K300RzfType data)
        {
            OnRezef(ref data);
        }

        /// <summary>
        /// Send simulated Rezef data to all registered users
        /// </summary>
        public void SendEventMdd(ref K300MadadType data)
        {
            OnMadad(ref data);
        }

        // Properties - are used to filter the events data 
        public BaseAssetTypes EventsFilterBaseAsset { set; get; }
        public int EventsFilterBno { set; get; }
        public int EventsFilterMadad { set; get; }
        public int EventsFilterMaof { set; get; }
        public MonthType EventsFilterMonth { set; get; }
        public int EventsFilterRezef { set; get; }
        public StockKind EventsFilterStockKind { set; get; }
        public MadadTypes EventsFilterStockMadad { set; get; }
    }


    public partial class UserClass
    {
        public UserClass()
        {
            _loginProgress = 0;
            _loginStatus = LoginStatus.LoginSessionInactive;

            //init order simulation
            _orderId = 0;
            _rezefSimpleOrder = default(RezefSimpleOrder);
            _orderState = rzOrdersStates.Idle;
        }

        public int GetUserPermissions(int SessionId, out System.Array psaStrRecords)
        {
            psaStrRecords = default(System.Array);
            return 0;
        }

        public int GetAccounts(int SessionId, string strQuery, ref System.Array psaStrRecords, ref string ErrMsg)
        {
            return 0;
        }
        public int Login(string username, string AS400Password, string AppPassword, out string message, out int sessionId)
        {
            message = "";
            _sessionId = 1;
            sessionId = _sessionId;
            _loginStatus = LoginStatus.LoginSessionInProgress;
            _loginStarted = DateTime.Now;

            return _sessionId;
        }

        public void GetLoginActivity(ref int sessionId, out int percent, out string description)
        {
            // simulation - if in progress move things                      
            switch (_loginStatus)
            {
                case LoginStatus.LoginSessionInactive:
                    // do nothing until Login() is not being called
                    break;
                case LoginStatus.LoginSessionActive:
                    // login done - nothing more is required
                    break;

                default:
                    TimeSpan ts = TimeSpan.FromMilliseconds(100);
                    DateTime current = DateTime.Now;
                    if ((_loginStarted + ts) <= current)
                    {
                        _loginProgress += 100;
                        _loginStarted = current;
                    }

                    if (_loginProgress >= 100)
                    {
                        _loginStatus = LoginStatus.LoginSessionActive;
                    }

                    break;
            }

            percent = _loginProgress;
            description = "";
            sessionId = _sessionId;
        }


        public LoginStatus get_LoginState(ref int sessionId)
        {
            return _loginStatus;
        }

        public string get_LoginErrorDesc(ref int sessionId)
        {
            return _loginErrorDesc;
        }

        /// <summary>
        /// Carries out the (simulated) AS400 logout process.
        /// </summary>
        /// <param name="SessionId">A <see cref="System.Int32"/>
        /// The logout process requires a 
        /// unique Session Identification number that identifies 
        /// the session to be closed.</param>
        /// <returns>A <see cref="System.Int32"/>
        /// In the event of failure -1 is returned from the function.
        /// In the event of success 0 is returned from the function.</returns>
        public int Logout(int SessionId)
        {
            bool success = true;
            if (success) return 0;
            else return -1;
        }

        //public properties:
        /// <summary>
        /// Relays the customer number for the current taskbar configuration.
        /// String representing the customer number in current taskbar configuration.
        /// </summary>
        public virtual string Cust { get { return this._cust; } }

        /// <summary>
        /// Query type - open/closed/processing.
        /// </summary>
        public virtual ConnectionState State { get { return this._cs; } }

        /// <summary>
        /// String representing the System number in current taskbar configuration. 
        /// </summary>
        public virtual string System { get { return this._system; } }

        /// <summary>
        /// String representing the Stock exchange member's (=broker) name for current taskbar configuration. 
        /// </summary>
        public virtual string SystemName { get { return this._sysName; } }


        protected int _loginProgress;
        protected int _sessionId;
        protected string _loginErrorDesc;
        protected LoginStatus _loginStatus;
        protected DateTime _loginStarted;

        protected string _cust = "aryeh";
        protected ConnectionState _cs;
        protected string _system = "TBSim";
        protected string _sysName = "TaskBarLibSim";

    }

    public class ConfigClass
    {
        /// <summary>
        /// This function is used to get timestamp from AS400 server in order to compute
        /// roundrip times and latencies
        /// </summary>
        /// <param name="dt">A <see cref="TaskBarLibSim.AS400DateTime"/></param>
        /// <param name="latency">A <see cref="System.Int32"/></param>
        /// <returns>0 if success, -1 if failure</returns>
        public int GetAS400DateTime(out AS400DateTime dt, out int latency)
        {
            DateTime now = DateTime.Now;

            dt = new AS400DateTime();

            //fill the AS400DateTime struct with updated values
            dt.year = now.Year;
            dt.Month = now.Month;
            dt.day = now.Day;
            dt.hour = now.Hour;
            dt.minute = now.Minute;
            dt.second = now.Second;
            dt.ms = now.Millisecond;

            //An arbitrary value for latency
            latency = random.Next(15, 2 * 1000);

            countGetAS400DateTime++;

            // produce failure from time to time
            bool success = (random.Next(0, 2) == 0);
            if (success) return 0;
            else return -1;
        }

        protected static int countGetAS400DateTime;
        protected static Random random = new Random();
    }

    #region simulation;
    // From this line down - simulation-related engine, which isn't part of the TaskBarLib API

    /// <summary>
    /// this is not part of the FMR's TaskBarLib. Part of the simulation engine
    /// </summary>
    /// <param name="generator">
    /// A <see cref="ISimulationStreamGenerator"/>
    /// </param>
    public interface ISimulationDataGenerator<DataType>
    {
        void Start();

        void Stop();
    }

    /// <summary>
    /// a thread firing specified event
    /// </summary>
    public abstract class EventGenerator<DataType> : ISimulationDataGenerator<DataType>
    {
        public EventGenerator()
        {
            notStopped = true;
        }

        public virtual void Start()
        {
            notStopped = true;
            Thread t = new Thread(Run);
            t.Priority = ThreadPriority.Highest;
            t.Start();
            return;
        }

        public virtual void Stop()
        {
            notStopped = false;
            return;
        }

        /// <summary>
        /// thread main loop 
        /// </summary>
        protected void Run()
        {
            while (notStopped)
            {
                DataType data;
                bool result = GetData(out data);

                if (!result)
                {
                    break;
                }
                SendEvents(ref data);
            }
        }

        /// <summary>
        /// Returns next chunk of data of type. If Ok return true 
        /// The method should block the calling thread until the next chunk
        /// of data is available or there is no more data will ever be available
        /// in the later case the method will return false
        /// </summary>
        /// <returns>
        /// A <see cref="System.Object"/>
        /// Data will be set to the next generated chunk
        /// </returns>
        protected abstract bool GetData(out DataType data);

        protected abstract void SendEvents(ref DataType data);

        private bool notStopped;
    }

    /// <summary>
    /// Use childs of this class, like MaofDataGeneratorLogFile and RezefDataGeneratorLogFile
    /// This class provides OpenFile-ReadLinesFromFile-CallAbstractParser-SendData services
    /// </summary>
    public abstract class EventGeneratorPlayback<DataType> : EventGenerator<DataType>
    {
        protected EventGeneratorPlayback(string delimiter, string filename)
        {
            this.delimiter = delimiter;
            this.filename = filename;
            ReadyToGo = false;

            baseTimeSpanLog = default(TimeSpan);
            baseTimePb = default(DateTime);

            System.Console.WriteLine("Simulation playback data from " + filename);

            fileStream = default(FileStream);
            this.filename = filename;
            streamReader = default(StreamReader);

            Type t = typeof(DataType);
            fields = t.GetFields();

            do
            {
                try
                {
                    fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
                    streamReader = new StreamReader(fileStream, System.Text.Encoding.UTF7, false, 1024 * 5);
                }
                catch (IOException e)
                {
                    System.Console.WriteLine("Failed to open file " + filename);
                    if (fileStream != default(FileStream))
                    {
                        fileStream.Close();
                        // help Garbage collector
                        streamReader = default(StreamReader);
                        fileStream = default(FileStream);
                        break;
                    }
                    System.Console.WriteLine(e.ToString());
                }

                bool res = CheckFile(fileStream);
                if (!res) break;

                ReadyToGo = true;
            }
            while (false);
        }

        ~EventGeneratorPlayback()
        {
            // close the file I read from 
            if (fileStream != default(FileStream))
            {
                fileStream.Close();
                // help Garbage collector
                streamReader = default(StreamReader);
                fileStream = default(FileStream);
            }
        }

        /// <summary>
        /// Generic part of the log file parser - find next field in the line 
        /// </summary>
        /// <param name="src">
        /// A <see cref="System.String"/>
        /// Where to look for the field
        /// </param>
        /// <param name="from">
        /// A <see cref="System.Int32"/>
        /// Character to start to look from
        /// </param>
        /// <param name="field">
        /// A <see cref="System.String"/>
        /// Found field
        /// </param>
        /// <returns>
        /// A <see cref="System.Boolean"/>
        /// True if a field was found
        /// </returns>
        protected bool getNextField(string src, ref int from, out string field)
        {

            bool res = false;
            field = "";

            do
            {
                if (from < 0)
                {
                    System.Console.WriteLine("EventGeneratorPlayback::getNextField from=" + from);
                    System.Console.WriteLine("1.Failed to find delimiter " + delimiter + " position " + from + " line " + count);
                    //System.Console.WriteLine("Line=" + src);
                    break;
                }


                if (from > src.Length)
                {
                    System.Console.WriteLine("EventGeneratorPlayback::getNextField from=" + from + " src.Length=" + src.Length);
                    System.Console.WriteLine("2.Failed to find delimiter " + delimiter + " position " + from + " line " + count);
                    //System.Console.WriteLine("Line=" + src);
                    break;
                }

                if (from == src.Length)
                {
                    break;
                }

                int to = src.IndexOf(delimiter, from);

                if (to < 0) // may be end of line ?
                {
                    // fix the value - reference the char after end of line
                    to = src.Length;
                }

                if ((to >= from) && (from >= 0))
                {
                    field = src.Substring(from, (to - from));
                    //Console.WriteLine("'" + field + "'");
                }
                else
                {
                    System.Console.WriteLine("3.Failed to find delimiter " + delimiter + " position " + from + " line " + count);
                    System.Console.WriteLine("Line=" + src);
                    break;
                }

                from = to;
                if (from >= src.Length) from = src.Length;

                res = true;
            }
            while (false);


            return res;
        }

        protected override bool GetData(out DataType data)
        {
            bool res;
            data = default(DataType);
            TimeSpan timeSpan = default(TimeSpan);

            // sanity check - i opened the file and everything is Ok
            // i can try to read the data
            if (!ReadyToGo) return false;


            // in the very first call to the GetData i am going to sleep
            // for initialDelay - first data arrives to the data collector with delay
            // simulation of the network delay
            // there are going to be two playbacks of the same log file running concurently
            // one (with zero initial delay) for the market simulation and another for the
            // trading algorithm
            if (initialDelay > 0)
            {
                Thread.Sleep(initialDelay);
                initialDelay = 0;
            }


            bool parseRes = false;

            do
            {
                res = false;

                if (streamReader.EndOfStream)
                {
                    res = false;
                    break;
                }

                // let's try to read
                string str;
                try
                {
                    str = streamReader.ReadLine();
                    res = true;
                }
                catch (IOException e)
                {
                    System.Console.WriteLine(e.ToString());
                    res = false;
                    break;
                }
                //				Console.WriteLine(str);
                //				Console.WriteLine("-----------");

                // parse the string
                // if i failed to parse read next line until eof or read error
                // i just skip the bad line
                parseRes = ParseLogString(str, out data, out timeSpan);

                res = true;
            }
            while (!parseRes); // if parsing failed go to next line

            // i calculate the delay and wait
            if (res)
            {
                DoDelay(timeSpan);
            }

            count += 1;

            return res;
        }

        /// <summary>
        /// Look the two consecutive time stamps. Calculate time span - time elapsed from the previous
        /// log entry. Sleep for some time if i am playing the log too fast
        /// </summary>
        private void DoDelay(TimeSpan timeSpan)
        {
            int delayLog = 0, delayPb = 0;

            // this not the first time the method is called
            // calculate how much time went locally and according to the log
            if (baseTimeSpanLog != default(TimeSpan))
            {
                DateTime dtPb = DateTime.Now;

                // how much time elapsed from the base time - time when I started playback
                delayPb = (int)((dtPb - baseTimePb).TotalMilliseconds);

                // how much time elapsed according to the log file
                delayLog = (int)((timeSpan - baseTimeSpanLog).TotalMilliseconds);
            }
            else
            {
                baseTimeSpanLog = timeSpan;
                baseTimePb = DateTime.Now;
            }

            // i have to take care of speedup
            // because of speedup my delays are going to be shorter - i am running
            // through the log faster
            // local (playback) time runs faster
            // delayPb is time elapsed from the last call to the method
			int dpb = delayPb;
            delayPb = (int)(delayPb * this.speedup);

            // if i am running faster than log i'll do delay. if i am slower there is nothing to do.
            // calculate next sleep. the shortest possible sleep can be limited
            // In Windows i can't sleep for shorter period than 15ms, but i am not worry about that
            // i always try to run not faster than the log i am playing            
#if WINDOWS
			if (delayLog > (delayPb+50))
#else
            if (delayLog > (delayPb + 0))
#endif
            {
                int delay = delayLog - delayPb;
				delay = (int)(delay / speedup);
				Thread.Sleep(delay);
            }

        }

        public override void Start()
        {
            if (speedup != 1.0)
            {
                System.Console.WriteLine("Simulation playback speedup " + speedup);
            }
            base.Start();
        }

        protected void SetDelay(int delay)
        {
            this.initialDelay = delay;
        }

        protected void SetSpeedup(double speedup)
        {
            this.speedup = speedup;
        }

        public int GetCount()
        {
            return count;
        }

        protected abstract bool ParseLogString(string str, out DataType data, out TimeSpan timeSpan);
        protected abstract bool CheckFile(FileStream fileStream);

        /// <summary>
        /// for example a comma
        /// used in the getNextField() 
        /// </summary>
        private string delimiter;

        protected FieldInfo[] fields;

        /// <summary>
        /// Delay in milliseconds. This is for simulation of the roundtrip delay
        /// in the real network connection
        /// </summary>
        protected int initialDelay;

        protected int count;
        protected FileStream fileStream;
        protected StreamReader streamReader;
        protected string filename;

        // if 2.0 then 1s of the log will be 500ms of real time
        // i will run the log at double speed
        double speedup;

        // time span in the log file - very first entry
        private TimeSpan baseTimeSpanLog;

        // local system (playback) very first call
        private DateTime baseTimePb;

        protected bool ReadyToGo;
    }

    /// <summary>
    /// this is a thread generating event based on the Maof log file
    /// </summary>
    public class MaofDataGeneratorLogFile : EventGeneratorPlayback<K300MaofType>, ISimulationDataGenerator<K300MaofType>, JQuant.IDataGenerator
    {
        /// Log file to read the data from
        /// <param name="filename">
        /// A <see cref="System.String"/>
        /// </param>
        /// <param name="speedup">
        /// Number of times to accelerate the time. For example, if speedup is 2
        /// then events which took 1s in the log file will be sent in 500ms
        /// if speedup is 0.1 the play back will be slower by 10 times 
        /// A <see cref="System.Double"/>
        /// </param>
        /// <param name="delay">
        /// A <see cref="System.Int32"/>
        /// Initial delay (milliseconds) before I start to send events 
        /// This is simulation of the network delay
        /// There are going to be two playbacks of the same log file running concurently -
        /// one (with zero initial delay) for the market simulation and another (delayed) for the
        /// trading algorithm
        /// </param>
        public MaofDataGeneratorLogFile(string filename, double speedup, int delay)
            : base(",", filename)
        {
            // set initial delay
            // first event will be sent only after the delay expires
            base.SetDelay(delay);
            base.SetSpeedup(speedup);
			lastCallToParseLogString = DateTime.Now;
        }

        ~MaofDataGeneratorLogFile()
        {
        }

        protected override bool CheckFile(FileStream fileStream)
        {
            //Usual header (legend) for maof log:
            const string HEADER = "SUG_REC,TRADE_METH,BNO_Num,LAST_REC,SIDURI_Num,SYMBOL_E,Symbol,BNO_NAME_E,BNO_NAME,BRANCH_NO,BRANCH_U,SUG_BNO,MIN_UNIT,HARIG_NV,MIN_PR,MAX_PR,BASIS_PRC,BASIS_COD,STATUS_COD,EX_DATE,EX_PRC,VL_MULT,VL_COD,ZERO_COD,shlav,STATUS,TRD_STP_CD,TRD_STP_N,STP_OPN_TM,LMT_BY1,LMT_BY2,LMT_BY3,LMY_BY1_NV,LMY_BY2_NV,LMY_BY3_NV,RWR_FE,LMT_SL1,LMT_SL2,LMT_SL3,LMY_SL1_NV,LMY_SL2_NV,LMY_SL3_NV,RWR_FF,PRC,COD_PRC,SUG_PRC,LST_DF_BS,RWR_FG,LST_DL_PR,LST_DL_TM,LST_DL_VL,DAY_VL,DAY_VL_NIS,DAY_DIL_NO,RWR_FH,DAY_MAX_PR,DAY_MIN_PR,POS_OPN,POS_OPN_DF,STS_NXT_DY,UPD_DAT,UPD_TIME,FILER,TimeStamp,Ticks";

            bool res = false;
            string str;
            do
            {

                // let's try to read
                try
                {
                    str = streamReader.ReadLine();
                }
                catch (IOException e)
                {
                    System.Console.WriteLine("Failed to read file " + filename);
                    System.Console.WriteLine(e.ToString());
                    break;
                }

                // first line is legend
                if (str.IndexOf(HEADER) != 0)
                {
                    System.Console.WriteLine("First line match failed in the file " + filename);
                    System.Console.WriteLine("Expected " + HEADER);
                    System.Console.WriteLine("Read " + str);
                    //break;
                }

                res = true;
            }
            while (false);

            return res;
        }

        protected override bool ParseLogString(string str, out K300MaofType data, out TimeSpan timeSpan)
        {
            bool res = false;
            int commaIndex = 0;
            timeSpan = TimeSpan.FromTicks(0);
			
			

            // create a new object
            data = new K300MaofType();

            // boxing of the structure
            object o = (object)data;

            do
            {
                // i handle last two fields of the K300RzfType separately
                int fieldsCount = fields.Length - 2;
	            string timeStampStr = null, ticksStr = null;
                // set all fields in the object
                for (int fieldIdx = 0; fieldIdx < fieldsCount; fieldIdx++)
                {
                    FieldInfo fi = fields[fieldIdx];
                    string fieldValue;

                    // getNextField() is in the parent class
                    // the method fetches next field in the line
                    // this is where delimiter (in our case a comma) is important
                    res = getNextField(str, ref commaIndex, out fieldValue);

                    if (!res)
                    {
                        // System.Console.WriteLine("Failed to get field str=" + str + ", commaIndex=" + commaIndex);
                        break;
                    }

                    // commaIndex_1 points to comma
                    commaIndex++;

                    fi.SetValue(o, fieldValue);
                }

                if (!res) break;

                // unboxing of the structure
                data = (K300MaofType)o;

                // the tricky part
                // last two fields in the record - TimeStamp and Ticks were not parsed
                // parse them now and convert to variable of type DateTime 
                res = getNextField(str, ref commaIndex, out timeStampStr);
                commaIndex++;
                if (!res)
                {
                    System.Console.WriteLine("Failed to fectch time stamp from " + str);
                    System.Console.WriteLine("Expected at " + commaIndex);
                    break;
                }
                res = getNextField(str, ref commaIndex, out ticksStr);
                commaIndex++;
                if (!res)
                {
                    System.Console.WriteLine("Failed to fectch ticks from " + str);
                    System.Console.WriteLine("Expected at " + commaIndex);
                    break;
                }

                timeSpan = TimeSpan.Parse(timeStampStr);

                // Application expects that the data log contains generated internally timestamp
                // Parse the fields and add to the data strcuture
                try
                {
                    data.Ticks = JQuant.Convert.StrToLong(ticksStr);
                }
                catch (Exception e)
                {
                    System.Console.WriteLine("Failed to parse ticks" + ticksStr);
                    res = false;
                    break;
                }
                try
                {
                    data.TimeStamp = DateTime.Parse(timeStampStr);
                }
                catch (Exception e)
                {
                    System.Console.WriteLine("Failed to parse time stamp:" + timeStampStr);
                    res = false;
                    break;
                }

                res = true;
            }
            while (false);
	
            return res;
        }

        protected override void SendEvents(ref K300MaofType data)
        {
            eventCounter++;
            SimulationTop.k300EventsClass.SendEventMaof(ref data);
        }

        public string GetName()
        {
            return "Maof playback generator";
        }


        public string Name
        {
            get;
            set;
        }

        public void GetEventCounters(out System.Collections.ArrayList names, out System.Collections.ArrayList values)
        {
            names = new System.Collections.ArrayList(0);
            values = new System.Collections.ArrayList(0);

        }

        protected int eventCounter = 0;
		protected DateTime lastCallToParseLogString;
		protected TimeSpan lastTimeSpan;
    }

    public class RezefDataGeneratorLogFile : EventGeneratorPlayback<K300RzfType>, ISimulationDataGenerator<K300RzfType>, JQuant.IDataGenerator
    {
        public RezefDataGeneratorLogFile(string filename, double speedup, int delay)
            : base(",", filename)
        {
            // set initial delay
            // first event will be sent only after the delay expires
            base.SetDelay(delay);
            base.SetSpeedup(speedup);
        }

        ~RezefDataGeneratorLogFile()
        {
        }

        protected override bool CheckFile(FileStream fileStream)
        {
            //Usual header (legend) for rezef log:
            const string HEADER = "SUG_REC,BNO_Num,BNO_NAME,Symbol,TRADE_METH,SIDURI_Num,RWR_VA,MIN_UNIT,HARIG_NV,MIN_PR_OPN,MAX_PR_OPN,MIN_PR_CNT,MAX_PR_CNT,BASIS_PRC,STATUS,EX_COD,EX_DETAIL,RWR_VB,shlav,LAST_PRC,TRD_STP_N,STP_OPN_TM,RWR_VD,LMT_BY1,LMT_BY2,LMT_BY3,LMY_BY1_NV,LMY_BY2_NV,LMY_BY3_NV,MKT_NV_BY,MKT_NV_BY_NUM,RWR_VE,LMT_SL1,LMT_SL2,LMT_SL3,LMY_SL1_NV,LMY_SL2_NV,LMY_SL3_NV,MKT_NV_SL,MKT_NV_SL_NUM,RWR_VF,THEOR_PR,THEOR_VL,RWR_VG,LST_DL_PR,LST_DL_TM,LST_DF_BS,LST_DF_OPN,LST_DL_VL,DAY_VL,DAY_VL_NIS,DAY_DIL_NO,DAY_MAX_PR,DAY_MIN_PR,BNO_NAME_E,SYMBOL_E,STP_COD,COD_SHAAR,UPD_DAT,UPD_TIME,TimeStamp,Ticks";
            bool res = false;
            string str;
            do
            {

                // let's try to read
                try
                {
                    str = streamReader.ReadLine();
                }
                catch (IOException e)
                {
                    System.Console.WriteLine("Failed to read file " + filename);
                    System.Console.WriteLine(e.ToString());
                    break;
                }

                // first line is legend
                if (str.IndexOf(HEADER) != 0)
                {
                    System.Console.WriteLine("First line match failed in the file " + filename);
                    System.Console.WriteLine("Expected " + HEADER);
                    System.Console.WriteLine("Read " + str);
                    //break;
                }

                res = true;
            }
            while (false);


            return res;
        }

        protected override bool ParseLogString(string str, out K300RzfType data, out TimeSpan timeSpan)
        {
            bool res = false;
            int commaIndex = 0;
            timeSpan = TimeSpan.FromTicks(0);

            // create a new object
            data = new K300RzfType();

            // boxing of the structure
            object o = (object)data;

            do
            {
                // i handle last two fields of the K300RzfType separately
                int fieldsCount = fields.Length - 2;
                // set all fields in the object
                for (int fieldIdx = 0; fieldIdx < fieldsCount; fieldIdx++)
                {
                    FieldInfo fi = fields[fieldIdx];
                    string fieldValue;

                    // getNextField() is in the parent class
                    // the method fetches next field in the line
                    // this is where delimiter (in our case a comma) is important
                    res = getNextField(str, ref commaIndex, out fieldValue);

                    if (!res)
                    {
                        // System.Console.WriteLine("Failed to get field str=" + str + ", commaIndex=" + commaIndex);
                        break;
                    }

                    // commaIndex_1 points to comma
                    commaIndex++;

                    fi.SetValue(o, fieldValue);
                }

                if (!res) break;

                // unboxing of the structure
                data = (K300RzfType)o;

                // the tricky part
                // last two fields in the record - TimeStamp and Ticks were not parsed
                // parse them now and convert to variable of type DateTime 
                string timeStampStr, ticksStr;
                res = getNextField(str, ref commaIndex, out timeStampStr);
                commaIndex++;
                if (!res)
                {
                    System.Console.WriteLine("Failed to fectch time stamp from " + str);
                    System.Console.WriteLine("Expected at " + commaIndex);
                    break;
                }
                res = getNextField(str, ref commaIndex, out ticksStr);
                commaIndex++;
                if (!res)
                {
                    System.Console.WriteLine("Failed to fectch ticks from " + str);
                    System.Console.WriteLine("Expected at " + commaIndex);
                    break;
                }

                // I need time span to do the correct delays in the playing the data log back 
                timeSpan = TimeSpan.Parse(timeStampStr);

                // Application expects that the data log contains generated internally timestamp
                // Parse the fields and add to the data strcuture
                try
                {
                    data.Ticks = JQuant.Convert.StrToLong(ticksStr);
                }
                catch (Exception e)
                {
                    System.Console.WriteLine("Failed to parse ticks" + ticksStr);
                    res = false;
                    break;
                }
                try
                {
                    data.TimeStamp = DateTime.Parse(timeStampStr);
                }
                catch (Exception e)
                {
                    System.Console.WriteLine("Failed to parse time stamp:" + timeStampStr);
                    res = false;
                    break;
                }


                res = true;
            }
            while (false);

            return res;
        }

        protected override void SendEvents(ref K300RzfType data)
        {
            eventCounter++;
            SimulationTop.k300EventsClass.SendEventRzf(ref data);
        }

        public string GetName()
        {
            return "Rezef playback generator";
        }


        public string Name
        {
            get;
            set;
        }

        public void GetEventCounters(out System.Collections.ArrayList names, out System.Collections.ArrayList values)
        {
            names = new System.Collections.ArrayList(0);
            values = new System.Collections.ArrayList(0);

        }

        protected int eventCounter = 0;
    }

    /// <summary>
    /// this is a thread generating Maof data
    /// very simple all fields are random 
    /// objects of this type used as an argument to the InitStreamSimulation
    /// </summary>
    /// <param name="maofGenerator">
    /// A <see cref="ISimulationStreamGenerator"/>
    /// </param>
    public class MaofDataGeneratorRandom : EventGenerator<K300MaofType>, ISimulationDataGenerator<K300MaofType>, JQuant.IDataGenerator
    {
        public MaofDataGeneratorRandom()
        {
            randomString = new JQuant.RandomNumericalString(21, 80155);

            Type t = typeof(K300MaofType);
            fields = t.GetFields();
            count = 0;

            return;
        }

        protected override bool GetData(out K300MaofType data)
        {
            // delay - usually delay will be in the GetData
            // GetData reads log, pulls the time stamps and simulates
            // timing of the real data stream
            // Thread.Sleep(50);

            // create a new object
            data = new K300MaofType();

            // box the structure
            object o = (object)data;
            // set all fields in the object
            foreach (FieldInfo fi in fields)
            {
                string fieldValue = randomString.Next();
                fi.SetValue(o, fieldValue);
            }
            // unboxing of the structure
            data = (K300MaofType)o;
            // i want to mark first and last field
            data.SUG_REC = "SUG_REC";
            data.FILER = "FILER";
            data.BNO_NAME = "Maof";


            count += 1;


            return true;
        }

        protected override void SendEvents(ref K300MaofType data)
        {
            SimulationTop.k300EventsClass.SendEventMaof(ref data);
            // avoid tight loops in the system
            // Thread.Sleep(10);
        }

        public int GetCount()
        {
            return count;
        }

        public string GetName()
        {
            return "Maof data random generator";
        }


        JQuant.IRandomString randomString;
        protected FieldInfo[] fields;
        int count;
    }



    /// <summary>
    /// this is a thread generating 
    /// very siimple all fields are random Rezef data generator
    /// objects of this type used as an argument to the InitStreamSimulation
    /// </summary>
    /// <param name="maofGenerator">
    /// A <see cref="ISimulationStreamGenerator"/>
    /// </param>
    public class RezefDataGeneratorRandom : EventGenerator<K300RzfType>, ISimulationDataGenerator<K300RzfType>, JQuant.IDataGenerator
    {
        public RezefDataGeneratorRandom()
        {
            randomString = new JQuant.RandomNumericalString(21, 80155);

            Type t = typeof(K300RzfType);
            fields = t.GetFields();
            count = 0;

            return;
        }

        protected override bool GetData(out K300RzfType data)
        {
            // delay - usually delay will be in the GetData
            // GetData reads log, pulls the time stamps and simulates
            // timing of the real data stream
            Thread.Sleep(50);

            // create a new object
            data = new K300RzfType();

            // box the structure
            object o = (object)data;
            // set all fields in the object
            foreach (FieldInfo fi in fields)
            {
                string fieldValue = randomString.Next();
                fi.SetValue(o, fieldValue);
            }
            // unboxing of the structure
            data = (K300RzfType)o;
            data.BNO_NAME = "Rezef";


            count += 1;


            return true;
        }

        protected override void SendEvents(ref K300RzfType data)
        {
            SimulationTop.k300EventsClass.SendEventRzf(ref data);
            // avoid tight loops in the system
            Thread.Sleep(50);
        }

        public int GetCount()
        {
            return count;
        }

        public string GetName()
        {
            return "Rezef data random generator";
        }


        JQuant.IRandomString randomString;
        protected FieldInfo[] fields;
        int count;
    }


    /// <summary>
    /// this is a thread generating 
    /// very siimple all fields are random Madad data generator
    /// objects of this type used as an argument to the InitStreamSimulation
    /// </summary>
    /// <param name="maofGenerator">
    /// A <see cref="ISimulationStreamGenerator"/>
    /// </param>
    public class MadadDataGeneratorRandom : EventGenerator<K300MadadType>, ISimulationDataGenerator<K300MadadType>, JQuant.IDataGenerator
    {
        public MadadDataGeneratorRandom()
        {
            randomString = new JQuant.RandomNumericalString(21, 80155);

            Type t = typeof(K300MadadType);
            fields = t.GetFields();
            count = 0;


            return;
        }

        protected override bool GetData(out K300MadadType data)
        {
            // delay - usually delay will be in the GetData
            // GetData reads log, pulls the time stamps and simulates
            // timing of the real data stream
            Thread.Sleep(50);

            // create a new object
            data = new K300MadadType();

            // box the structure
            object o = (object)data;
            // set all fields in the object
            foreach (FieldInfo fi in fields)
            {
                string fieldValue = randomString.Next();
                fi.SetValue(o, fieldValue);
            }
            // unboxing of the structure
            data = (K300MadadType)o;
            data.BNO_N = "Madad";

            count += 1;

            return true;
        }

        protected override void SendEvents(ref K300MadadType data)
        {
            SimulationTop.k300EventsClass.SendEventMdd(ref data);
            // avoid tight loops in the system
            Thread.Sleep(50);
        }

        public int GetCount()
        {
            return count;
        }

        public string GetName()
        {
            return "Madad data random generator";
        }


        JQuant.IRandomString randomString;
        protected FieldInfo[] fields;
        int count;
    }


    public class MarketSimulationOrder
    {
        public MarketSimulationOrder()
        {

        }

        //returns BNO_Num
        public int GetSecurity()
        {
            return 0;
        }

        //Order's limit price 
        //All of Maof orders are of type limit
        public int GetPrice()
        {
            return 0;
        }

        //I prefer to call this indicator 'IsBuy', not 'IsSell'
        //positive (true) is security accumulation negative is the opposite
        public bool IsBuy
        {
            get;
            protected set;
        }

        //reference to the TaskBar.MaofOrderType instance
        object data;
        //this one is used to track order's current position in the queue
        int placeInOrderQueue;

    }//class MarketSimulationOrder


    /// <summary>
    /// I need some class where all apparently disconnected classes are connected
    /// For example K300Class and K300EventsClass
    /// User application registers event listeners in the EventClass and calls StartStream
    /// in the K300Class
    /// </summary>
    class SimulationTop
    {
        public static K300EventsClass k300EventsClass;
        public static K300Class k300Class;

        //in case the playback is from an numbered log - I put the number here
        //by default it's zero
        public static int PlayBackLogNum = 0;
    }
    #endregion;


}//namespace TaskBarLibSim

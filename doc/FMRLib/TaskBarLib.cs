namespace TaskBarLib
{
    [ComImport, InterfaceType((short) 2), TypeLibType((short) 0x1000), Guid("4EE19F38-3979-42C1-BE3A-968323DB1A70")]
    public interface _IK300EventsEvents
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(1)]
        void OnMaof([In] ref K300MaofType data);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(2)]
        void OnRezef([In] ref K300RzfType data);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(3)]
        void OnMadad([In] ref K300MadadType data);
    }

    [ComEventInterface(typeof(_IK300EventsEvents), typeof(_IK300EventsEvents_EventProvider)), TypeLibType((short) 0x10), ComVisible(false)]
    public interface _IK300EventsEvents_Event
    {
        // Events
        event _IK300EventsEvents_OnMadadEventHandler OnMadad;
        event _IK300EventsEvents_OnMaofEventHandler OnMaof;
        event _IK300EventsEvents_OnRezefEventHandler OnRezef;
    }

    internal sealed class _IK300EventsEvents_EventProvider : _IK300EventsEvents_Event, IDisposable
    {
        // Fields
        private ArrayList m_aEventSinkHelpers;
        private UCOMIConnectionPoint m_ConnectionPoint;
        private UCOMIConnectionPointContainer m_ConnectionPointContainer;

        // Methods
        public _IK300EventsEvents_EventProvider(object);
        public override void add_OnMadad(_IK300EventsEvents_OnMadadEventHandler);
        public override void add_OnMaof(_IK300EventsEvents_OnMaofEventHandler);
        public override void add_OnRezef(_IK300EventsEvents_OnRezefEventHandler);
        public override void Dispose();
        public override void Finalize();
        private void Init();
        public override void remove_OnMadad(_IK300EventsEvents_OnMadadEventHandler);
        public override void remove_OnMaof(_IK300EventsEvents_OnMaofEventHandler);
        public override void remove_OnRezef(_IK300EventsEvents_OnRezefEventHandler);
    }

    [ComVisible(false), TypeLibType((short) 0x10)]
    public delegate void _IK300EventsEvents_OnMadadEventHandler([In] ref K300MadadType data);

    [TypeLibType((short) 0x10), ComVisible(false)]
    public delegate void _IK300EventsEvents_OnMaofEventHandler([In] ref K300MaofType data);

    [ComVisible(false), TypeLibType((short) 0x10)]
    public delegate void _IK300EventsEvents_OnRezefEventHandler([In] ref K300RzfType data);

    [ClassInterface(ClassInterfaceType.None)]
    public sealed class _IK300EventsEvents_SinkHelper : _IK300EventsEvents
    {
        // Fields
        public int m_dwCookie;
        public _IK300EventsEvents_OnMadadEventHandler m_OnMadadDelegate;
        public _IK300EventsEvents_OnMaofEventHandler m_OnMaofDelegate;
        public _IK300EventsEvents_OnRezefEventHandler m_OnRezefDelegate;

        // Methods
        internal _IK300EventsEvents_SinkHelper();
        public override void OnMadad(ref K300MadadType);
        public override void OnMaof(ref K300MaofType);
        public override void OnRezef(ref K300RzfType);
    }

    [StructLayout(LayoutKind.Sequential, Pack=4), Guid("D717A438-4447-4506-95AD-992A486B6003")]
    public struct AccountType
    {
        [MarshalAs(UnmanagedType.BStr)]
        public string BRANCH_NO;
        [MarshalAs(UnmanagedType.BStr)]
        public string ID_NO;
        [MarshalAs(UnmanagedType.BStr)]
        public string ID_FIL;
        [MarshalAs(UnmanagedType.BStr)]
        public string TIK_NO;
        [MarshalAs(UnmanagedType.BStr)]
        public string ID_NAME;
        [MarshalAs(UnmanagedType.BStr)]
        public string REQ_MRGN;
        [MarshalAs(UnmanagedType.BStr)]
        public string RCRDIT_MAOF;
        [MarshalAs(UnmanagedType.BStr)]
        public string ID_TOWN;
        [MarshalAs(UnmanagedType.BStr)]
        public string ID_PHONE_NO;
        [MarshalAs(UnmanagedType.BStr)]
        public string CRDIT_LN;
        [MarshalAs(UnmanagedType.BStr)]
        public string CRDIT_MAOF;
        [MarshalAs(UnmanagedType.BStr)]
        public string MNIOT_VL;
        [MarshalAs(UnmanagedType.BStr)]
        public string ID_TOT_VL;
        [MarshalAs(UnmanagedType.BStr)]
        public string MIN_COM;
        [MarshalAs(UnmanagedType.BStr)]
        public string MIN_C_MAOF;
        [MarshalAs(UnmanagedType.BStr)]
        public string MAX_C_MAOF;
        [MarshalAs(UnmanagedType.BStr)]
        public string MIN_C_FUTR;
        [MarshalAs(UnmanagedType.BStr)]
        public string MAX_C_FUTR;
        [MarshalAs(UnmanagedType.BStr)]
        public string SIVUG;
        [MarshalAs(UnmanagedType.BStr)]
        public string MAOF_COD;
        [MarshalAs(UnmanagedType.BStr)]
        public string FREEZ_COD;
        [MarshalAs(UnmanagedType.BStr)]
        public string FREEZ_TEXT;
        [MarshalAs(UnmanagedType.BStr)]
        public string IMM_DRAFT;
        [MarshalAs(UnmanagedType.BStr)]
        public string COD_H_BNO;
        [MarshalAs(UnmanagedType.BStr)]
        public string COD_H_MAOF;
        [MarshalAs(UnmanagedType.BStr)]
        public string COD_H_KUPA;
        [MarshalAs(UnmanagedType.BStr)]
        public string COD_H_FRGN;
        [MarshalAs(UnmanagedType.BStr)]
        public string COD_H_FUTR;
        [MarshalAs(UnmanagedType.BStr)]
        public string COD_H_CURR;
        [MarshalAs(UnmanagedType.BStr)]
        public string ID_PCNT;
        [MarshalAs(UnmanagedType.BStr)]
        public string SUG_ID;
        [MarshalAs(UnmanagedType.BStr)]
        public string TAT_SIVUG;
        [MarshalAs(UnmanagedType.BStr)]
        public string EXT_MARGIN;
        [MarshalAs(UnmanagedType.BStr)]
        public string ITRA_CASH;
        [MarshalAs(UnmanagedType.BStr)]
        public string ID_STREET;
        [MarshalAs(UnmanagedType.BStr)]
        public string PRE_MAOF1;
        [MarshalAs(UnmanagedType.BStr)]
        public string PRE_MAOF2;
        [MarshalAs(UnmanagedType.BStr)]
        public string PRE_MAOF3;
        [MarshalAs(UnmanagedType.BStr)]
        public string PRE_MAOF4;
        [MarshalAs(UnmanagedType.BStr)]
        public string PRE_MAOF5;
        [MarshalAs(UnmanagedType.BStr)]
        public string MDL;
        [MarshalAs(UnmanagedType.BStr)]
        public string TEL_PW;
        [MarshalAs(UnmanagedType.BStr)]
        public string FIL;
    }

    [StructLayout(LayoutKind.Sequential, Pack=8), Guid("4D1CADC3-173E-4DF9-90A9-F9883E2AB861")]
    public struct AccountYieldByRequirement
    {
        public int Yld_sug;
        public int Yld_id;
        public int Yld_bno;
        public int Yld_count;
        public int Yld_date;
        public double Yld_vl_dd;
        public double Yld_yld_dd;
    }

    [StructLayout(LayoutKind.Sequential, Pack=8), Guid("C9BB5DB8-D080-4CB3-9AAC-7CF20F45283C")]
    public struct AccountYieldDetailed
    {
        public int Yld_sug;
        public int Yld_id;
        public int Yld_bno;
        public int Yld_date;
        public double Yld_vl_dd;
        public double Yld_yld_dd;
    }

    [StructLayout(LayoutKind.Sequential, Pack=8), Guid("887F00A8-8B7E-4E2F-A7E4-486728E228B1")]
    public struct AccountYieldInitial
    {
        public int Yld_sug;
        public int Yld_id;
        public int Yld_bno;
        public int Yld_date;
        public double Yld_vl_dd;
        public double Yld_yld_dd;
        public double Yld_vl_mm;
        public double Yld_yld_mm;
        public double Yld_vl_12;
        public double Yld_yld_12;
        public double Yld_vl_3m;
        public double Yld_yld_3m;
        public double Yld_vl_yy;
        public double Yld_yld_yy;
    }

    [StructLayout(LayoutKind.Sequential, Pack=4), Guid("C92AC032-EAF1-472C-8CAE-4BA5A648E6E8")]
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

    [StructLayout(LayoutKind.Sequential, Pack=8), Guid("48540B22-2B02-4F4A-96EC-C76784BB7255")]
    public struct AssetSecuritiesType
    {
        public BaseAssetTypes nBaseAsset;
        public double IV;
        public double Madad;
        public double Dividend;
        public double Interest;
        public int SenarioNumber;
        public double ExistingSec;
        public double NeededSec;
    }

    [StructLayout(LayoutKind.Sequential, Pack=8), Guid("24508459-EFB3-4C73-A616-D008A5422DE7")]
    public struct BaseAssetInfo
    {
        public int BaseAssetCode;
        public int DaysToExp;
        public int ExpDate;
        public double Interest;
        public double IV;
        public double Dividend;
        public double MadadChange;
        public double IVChange;
        public double Madad;
    }

    [StructLayout(LayoutKind.Sequential, Pack=8), Guid("24508459-8615-45AA-818C-8BA7021F580A")]
    public struct BaseAssetType
    {
        public int BaseAssetCode;
        public int BNO;
        public int nCurr;
        [MarshalAs(UnmanagedType.BStr)]
        public string BaseAssetKind;
        [MarshalAs(UnmanagedType.BStr)]
        public string TradeMethod;
        public double Value;
        public double Interest;
        public double Dividend;
        public double StdIv;
        public double ValueChange;
        public double StdIdChange;
        public double Mult;
        public int ExpiresToday;
        public int ExpDate;
        public int ExpDays;
        [MarshalAs(UnmanagedType.BStr)]
        public string NameHeb;
        [MarshalAs(UnmanagedType.BStr)]
        public string NameEng;
    }

    [Guid("8B457C76-768D-42BE-AAB4-048D172B960B")]
    public enum BaseAssetTypes
    {
        BaseAssetAll = -1,
        BaseAssetBanks = 4,
        BaseAssetDollar = 2,
        BaseAssetEuro = 5,
        BaseAssetInterest = 3,
        BaseAssetMaof = 1,
        BaseAssetShacharAroch = 7,
        BaseAssetShacharBenoni = 6
    }

    [StructLayout(LayoutKind.Sequential, Pack=8), Guid("9EC172DF-5716-457D-B3AD-665558954DE9")]
    public struct BSDerivativesType
    {
        public double dDeltaCall;
        public double dThetaCall;
        public double dVegaCall;
        public double dGammaCall;
        public double dPriceCall;
        public double dDeltaPut;
        public double dThetaPut;
        public double dVegaPut;
        public double dGammaPut;
        public double dPricePut;
    }

    [ComImport, Guid("24508459-ACFE-43F0-847B-03DC1A24AC28"), CoClass(typeof(ConfigClass))]
    public interface Config : IConfig
    {
    }

    [ComImport, ClassInterface((short) 0), TypeLibType((short) 2), Guid("24508459-6F88-45DA-A56A-C13A3224EC86")]
    public class ConfigClass : IConfig, Config
    {
        // Methods
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(6)]
        public virtual extern int ConfigFieldAll([In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array pConfigRecords);
        [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(5)]
        public virtual extern string ConfigFieldQuery([In] ConfigField FIELD);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(7)]
        public virtual extern int GetAS400DateTime(out AS400DateTime dateTimeStruct, out int latency);

        // Properties
        [DispId(1)]
        public virtual string Cust { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(1)] get; }
        [DispId(4)]
        public virtual int sessionId { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(4)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(4)] set; }
        [DispId(2)]
        public virtual string System { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(2)] get; }
        [DispId(1)]
        public virtual string TaskBarLib.IConfig.Cust { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(1)] get; }
        [DispId(4)]
        public virtual int TaskBarLib.IConfig.sessionId { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(4)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(4)] set; }
        [DispId(2)]
        public virtual string TaskBarLib.IConfig.System { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(2)] get; }
        [DispId(3)]
        public virtual int TaskBarLib.IConfig.UsesEncryption { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(3)] get; }
        [DispId(3)]
        public virtual int UsesEncryption { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(3)] get; }
    }

    [Guid("5F318EAA-968C-427B-8B09-50EA7FC572FE")]
    public enum ConfigField
    {
        ConfigFieldAS400IP1 = 10,
        ConfigFieldAS400IP2 = 11,
        ConfigFieldConnectionTimeout = 12,
        ConfigFieldCust = 1,
        ConfigFieldDataSource = 5,
        ConfigFieldK300Ip = 6,
        ConfigFieldOrdersIp = 7,
        ConfigFieldSystem = 2,
        ConfigFieldTBLogDir = 8,
        ConfigFieldTBLogFile = 9,
        ConfigFieldUsesEncryption = 3,
        ConfigFieldVersion = 4
    }

    [StructLayout(LayoutKind.Sequential, Pack=4), Guid("2F0CE1AB-0793-4EB9-8B20-ED2195268681")]
    public struct ConfigType
    {
        public ConfigField FIELD;
        [MarshalAs(UnmanagedType.BStr)]
        public string TBConfigReply;
    }

    [Guid("083AE28E-D225-41FA-9E09-2E91DF8CFFE0")]
    public enum ConnectionState
    {
        csOpen,
        csProcessing,
        csClosed
    }

    [StructLayout(LayoutKind.Sequential, Pack=4), Guid("D717A434-4447-4506-95AD-992A486B6003")]
    public struct ConsStockType
    {
        [MarshalAs(UnmanagedType.BStr)]
        public string TRD_DAT;
        [MarshalAs(UnmanagedType.BStr)]
        public string BNO_NO;
        [MarshalAs(UnmanagedType.BStr)]
        public string BNO_NAME;
        [MarshalAs(UnmanagedType.BStr)]
        public string Symbol;
        [MarshalAs(UnmanagedType.BStr)]
        public string SUG_BNO;
        [MarshalAs(UnmanagedType.BStr)]
        public string ANAF;
        [MarshalAs(UnmanagedType.BStr)]
        public string TRADE_METH;
        [MarshalAs(UnmanagedType.BStr)]
        public string UNIT_OPEN;
        [MarshalAs(UnmanagedType.BStr)]
        public string UNIT_CONT;
        [MarshalAs(UnmanagedType.BStr)]
        public string UNIT_CLOS;
        [MarshalAs(UnmanagedType.BStr)]
        public string LMT_UP_OPN;
        [MarshalAs(UnmanagedType.BStr)]
        public string LMT_DN_OPN;
        [MarshalAs(UnmanagedType.BStr)]
        public string LMT_UP_CNT;
        [MarshalAs(UnmanagedType.BStr)]
        public string LMT_DN_CNT;
        [MarshalAs(UnmanagedType.BStr)]
        public string NV_LMT_ERR;
        [MarshalAs(UnmanagedType.BStr)]
        public string NV_LMT_FTL;
        [MarshalAs(UnmanagedType.BStr)]
        public string HON_RASHUM;
        [MarshalAs(UnmanagedType.BStr)]
        public string LVL1;
        [MarshalAs(UnmanagedType.BStr)]
        public string LVL2;
        [MarshalAs(UnmanagedType.BStr)]
        public string LVL3;
        [MarshalAs(UnmanagedType.BStr)]
        public string LVL4;
        [MarshalAs(UnmanagedType.BStr)]
        public string LVL5;
        [MarshalAs(UnmanagedType.BStr)]
        public string LVL6;
        [MarshalAs(UnmanagedType.BStr)]
        public string ENG_NAME;
        [MarshalAs(UnmanagedType.BStr)]
        public string RATING_MRG;
        [MarshalAs(UnmanagedType.BStr)]
        public string RATING_CRT;
        [MarshalAs(UnmanagedType.BStr)]
        public string RISK_COD;
        [MarshalAs(UnmanagedType.BStr)]
        public string DIRUG_AG;
        [MarshalAs(UnmanagedType.BStr)]
        public string BASIS_PRC;
        [MarshalAs(UnmanagedType.BStr)]
        public string BASIS_YLD;
        [MarshalAs(UnmanagedType.BStr)]
        public string MDD_MAOF;
        [MarshalAs(UnmanagedType.BStr)]
        public string MDD_TLV100;
        [MarshalAs(UnmanagedType.BStr)]
        public string UNIT_CONT_8;
        [MarshalAs(UnmanagedType.BStr)]
        public string RDM_DAYS;
        [MarshalAs(UnmanagedType.BStr)]
        public string MDD_TLV75;
        [MarshalAs(UnmanagedType.BStr)]
        public string MDD_TL_TK;
        [MarshalAs(UnmanagedType.BStr)]
        public string MDD_BANKS;
    }

    [ComImport, Guid("30F0C672-1CE7-4052-B913-A7BD947A4C8C"), CoClass(typeof(DirectDlvClass))]
    public interface DirectDlv : IDirectDlv
    {
    }

    [ComImport, ClassInterface((short) 0), Guid("892C585B-5EA6-4EA0-A14A-E77588D75BDE"), TypeLibType((short) 2)]
    public class DirectDlvClass : IDirectDlv, DirectDlv
    {
        // Methods
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(1)]
        public virtual extern int ExecDirect([In, MarshalAs(UnmanagedType.BStr)] string request, [MarshalAs(UnmanagedType.BStr)] out string response, [In, Optional, DefaultParameterValue(""), MarshalAs(UnmanagedType.BStr)] string separator);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(2)]
        public virtual extern int ExecDirect4K([In, MarshalAs(UnmanagedType.BStr)] string request, [MarshalAs(UnmanagedType.BStr)] out string response, [In, Optional, DefaultParameterValue(""), MarshalAs(UnmanagedType.BStr)] string separator);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(3)]
        public virtual extern int SafeExecDirect([In, MarshalAs(UnmanagedType.BStr)] string request, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_BSTR)] out Array response, out int errorCode, [MarshalAs(UnmanagedType.BStr)] out string ErrorMessage, [In, Optional, DefaultParameterValue(0)] int Use4KRequest);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(4)]
        public virtual extern int SafeExecDirect2([In] int sessionId, [In, MarshalAs(UnmanagedType.BStr)] string request, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_BSTR)] out Array response, out int errorCode, [MarshalAs(UnmanagedType.BStr)] out string ErrorMessage, [In, Optional, DefaultParameterValue(0)] int Use4KRequest);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(5)]
        public virtual extern int SafeExecDirectJ([In] int sessionId, [In, MarshalAs(UnmanagedType.BStr)] string request, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_BSTR)] out Array response, out int errorCode, [MarshalAs(UnmanagedType.BStr)] out string ErrorMessage, [In, Optional, DefaultParameterValue(0)] int Use4KRequest);
    }

    [StructLayout(LayoutKind.Sequential, Pack=8), Guid("49B9AB93-F2B2-4E2B-996E-FB1FFEF2F44D")]
    public struct ExtOptRecord
    {
        public int BNO;
        [MarshalAs(UnmanagedType.BStr)]
        public string Isin;
        public int BaseAsset;
        public double Strike;
        public int DaysToExpiry;
        public int ExpMonth;
        [MarshalAs(UnmanagedType.BStr)]
        public string ExpDate;
        public int OptionType;
        public double Mult;
        public double IndexMult;
        public int NumberFit;
    }

    [StructLayout(LayoutKind.Sequential, Pack=8), Guid("C9B70EF7-3769-4AF4-8A2B-D8A437B47B8F")]
    public struct FundExecutionType
    {
        public int BNO;
        public int MemberNo;
        public int NvBuy;
        public double VolBuy;
        public int NvSell;
        public double VolSell;
        [MarshalAs(UnmanagedType.BStr)]
        public string TrdStage;
        public int DateTime;
        [MarshalAs(UnmanagedType.BStr)]
        public string MemberName;
        [MarshalAs(UnmanagedType.BStr)]
        public string BnoName;
        public double PricePDN;
        public double PriceIZR;
        public int NvTotal;
        public double VolTotal;
    }

    [ComImport, CoClass(typeof(FundsClass)), Guid("C9B4F1EB-D07A-4735-BA08-BC5F0230ECAD")]
    public interface Funds : IFunds
    {
    }

    [ComImport, ClassInterface((short) 0), Guid("24C6F384-7154-4801-BE39-BE0C6874FEA1"), TypeLibType((short) 2)]
    public class FundsClass : IFunds, Funds
    {
        // Methods
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(3)]
        public virtual extern int GetFundExecution([In] int sessionId, [In] int DateFrom, [In] int DateTo, [In] int BNO, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] out Array RecordSet);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(2)]
        public virtual extern int GetKranot([In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array psaStrRecords, [In, MarshalAs(UnmanagedType.BStr)] string strBNO_NO, [MarshalAs(UnmanagedType.BStr)] out string ERR_MSG);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(1)]
        public virtual extern int GetKrnBizuim([In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array psaStrRecords, [In, MarshalAs(UnmanagedType.BStr)] string strDateFrom, [In, MarshalAs(UnmanagedType.BStr)] string strDateTo, [In, MarshalAs(UnmanagedType.BStr)] string strBNO_NO, [In, MarshalAs(UnmanagedType.BStr)] string strMember);
    }

    [ComImport, Guid("24508459-ACFE-43F0-847B-03DC1A24AC28"), TypeLibType((short) 0x10c0)]
    public interface IConfig
    {
        [DispId(1)]
        string Cust { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(1)] get; }
        [DispId(2)]
        string System { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(2)] get; }
        [DispId(3)]
        int UsesEncryption { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(3)] get; }
        [DispId(4)]
        int sessionId { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(4)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(4)] set; }
        [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(5)]
        string ConfigFieldQuery([In] ConfigField FIELD);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(6)]
        int ConfigFieldAll([In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array pConfigRecords);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(7)]
        int GetAS400DateTime(out AS400DateTime dateTimeStruct, out int latency);
    }

    [ComImport, Guid("30F0C672-1CE7-4052-B913-A7BD947A4C8C"), TypeLibType((short) 0x10c0)]
    public interface IDirectDlv
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(1)]
        int ExecDirect([In, MarshalAs(UnmanagedType.BStr)] string request, [MarshalAs(UnmanagedType.BStr)] out string response, [In, Optional, DefaultParameterValue(""), MarshalAs(UnmanagedType.BStr)] string separator);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(2)]
        int ExecDirect4K([In, MarshalAs(UnmanagedType.BStr)] string request, [MarshalAs(UnmanagedType.BStr)] out string response, [In, Optional, DefaultParameterValue(""), MarshalAs(UnmanagedType.BStr)] string separator);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(3)]
        int SafeExecDirect([In, MarshalAs(UnmanagedType.BStr)] string request, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_BSTR)] out Array response, out int errorCode, [MarshalAs(UnmanagedType.BStr)] out string ErrorMessage, [In, Optional, DefaultParameterValue(0)] int Use4KRequest);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(4)]
        int SafeExecDirect2([In] int sessionId, [In, MarshalAs(UnmanagedType.BStr)] string request, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_BSTR)] out Array response, out int errorCode, [MarshalAs(UnmanagedType.BStr)] out string ErrorMessage, [In, Optional, DefaultParameterValue(0)] int Use4KRequest);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(5)]
        int SafeExecDirectJ([In] int sessionId, [In, MarshalAs(UnmanagedType.BStr)] string request, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_BSTR)] out Array response, out int errorCode, [MarshalAs(UnmanagedType.BStr)] out string ErrorMessage, [In, Optional, DefaultParameterValue(0)] int Use4KRequest);
    }

    [ComImport, Guid("C9B4F1EB-D07A-4735-BA08-BC5F0230ECAD"), TypeLibType((short) 0x10c0)]
    public interface IFunds
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(1)]
        int GetKrnBizuim([In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array psaStrRecords, [In, MarshalAs(UnmanagedType.BStr)] string strDateFrom, [In, MarshalAs(UnmanagedType.BStr)] string strDateTo, [In, MarshalAs(UnmanagedType.BStr)] string strBNO_NO, [In, MarshalAs(UnmanagedType.BStr)] string strMember);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(2)]
        int GetKranot([In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array psaStrRecords, [In, MarshalAs(UnmanagedType.BStr)] string strBNO_NO, [MarshalAs(UnmanagedType.BStr)] out string ERR_MSG);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(3)]
        int GetFundExecution([In] int sessionId, [In] int DateFrom, [In] int DateTo, [In] int BNO, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] out Array RecordSet);
    }

    [ComImport, Guid("734AD6E7-C1D5-4A59-B3DF-9ACA7C28C5AA"), TypeLibType((short) 0x10c0)]
    public interface IIShortStocks
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(1)]
        int GetSafeStock([In, MarshalAs(UnmanagedType.BStr)] string StockID, [In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array StockDetails);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(2)]
        int GetAllSafeShortStocks([In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array StockDetails, [In, Optional, MarshalAs(UnmanagedType.Struct)] ref object SortBy, [In, Optional, MarshalAs(UnmanagedType.Struct)] ref object SortDirecton);
    }

    [ComImport, Guid("259FD37B-C3D0-4933-BF7C-DB646A625274"), TypeLibType((short) 0x10c0)]
    public interface IK300
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(1)]
        int GetMAOF([In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array vecRecords, [In, Out, MarshalAs(UnmanagedType.BStr)] ref string strLastTime, [In, MarshalAs(UnmanagedType.BStr)] string strOptionNumber, [In, Optional, DefaultParameterValue(-1)] MadadTypes strMadad);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(30)]
        int GetMAOFRaw([In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_BSTR)] ref Array vecRecords, [In, Out, MarshalAs(UnmanagedType.BStr)] ref string strLastTime, [In, MarshalAs(UnmanagedType.BStr)] string strOptionNumber, [In, Optional, DefaultParameterValue(-1)] MadadTypes strMadad);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(2)]
        int GetBaseAssets([MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] out Array psaRecords, [In, Optional, DefaultParameterValue(-1)] int BaseAssetCode);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(3)]
        void GetTradeOptions([In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_BSTR)] ref Array psaStrRecords, [In, Out] ref int retVal);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(4)]
        bool CalculateScenarios();
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(5)]
        int GetRezef([In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array vecStrRecords, [In, Out, MarshalAs(UnmanagedType.BStr)] ref string strLastTime, [In, Optional, DefaultParameterValue("00000000"), MarshalAs(UnmanagedType.BStr)] string strStockNumber, [In, Optional, DefaultParameterValue(-1)] MadadTypes strMadad);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x1d)]
        int GetRezefRaw([In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_BSTR)] ref Array vecStrRawRecords, [In, Out, MarshalAs(UnmanagedType.BStr)] ref string strLastTime, [In, Optional, DefaultParameterValue("00000000"), MarshalAs(UnmanagedType.BStr)] string strStockNumber, [In, Optional, DefaultParameterValue(-1)] MadadTypes strMadad);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(6)]
        int GetIndexes([In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array psaStrRecords);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(7)]
        int GetIndexStructure([In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_BSTR)] ref Array psaStrRecords);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(8)]
        int GetStocksRZF([In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_BSTR)] ref Array psaStrRecords);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(9)]
        void StopUpdate(int pnID);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(10)]
        int StartStream([In, Optional, DefaultParameterValue(0)] K300StreamType streamType, [In, Optional, DefaultParameterValue("00000000"), MarshalAs(UnmanagedType.BStr)] string strStockNumber, [In, Optional, DefaultParameterValue(-1)] MadadTypes strMadad, [In, Optional, DefaultParameterValue(1)] int withEvents);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(11)]
        int GetStockHistory([In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_BSTR)] ref Array psaStrRecords, [In, MarshalAs(UnmanagedType.BStr)] string StockNumber);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(12)]
        int GetStockValue([In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_BSTR)] ref Array psaStrRecords, [In, Optional, DefaultParameterValue("00000000"), MarshalAs(UnmanagedType.BStr)] string strLastTime);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(13)]
        int GetStatistics([In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array psaStrRecords);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(14)]
        int GetMadadHistory([In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array psaStrRecords, [In, Optional, DefaultParameterValue("000"), MarshalAs(UnmanagedType.BStr)] string MadadCode, [In, Optional, DefaultParameterValue("-1"), MarshalAs(UnmanagedType.BStr)] string strLastTime);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(15)]
        int GetMaofStocks([In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array psaStrRecords, [In, Optional, DefaultParameterValue(0)] MadadTypes MadadSymbol);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x10)]
        int GetStockStage([In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array psaStrRecords, [In, MarshalAs(UnmanagedType.BStr)] string StockNumber);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x11)]
        int GetConstStock([In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array psaStrRecords);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x12)]
        int GetMaofCnt([In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_BSTR)] ref Array psaStrRecords, [In, MarshalAs(UnmanagedType.BStr)] string strBranch, [In, MarshalAs(UnmanagedType.BStr)] string strHelpAccount, [In, MarshalAs(UnmanagedType.BStr)] string strTimeOrders, [In, MarshalAs(UnmanagedType.BStr)] string strTimeK300, [In, MarshalAs(UnmanagedType.BStr)] string strBaseAsset, [In, MarshalAs(UnmanagedType.BStr)] string strOnlyK300, [In, MarshalAs(UnmanagedType.BStr)] string strOnlyOrders, [In, MarshalAs(UnmanagedType.BStr)] string strOptionMonth);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x13)]
        int GetRzfCNT([In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_BSTR)] ref Array psaStrRecords, [In, MarshalAs(UnmanagedType.BStr)] string strOPName, [In, MarshalAs(UnmanagedType.BStr)] string strBranch, [In, MarshalAs(UnmanagedType.BStr)] string strHelpAccount, [In, MarshalAs(UnmanagedType.BStr)] string strTimeOrders, [In, MarshalAs(UnmanagedType.BStr)] string strTimeK300, [In, MarshalAs(UnmanagedType.BStr)] string strBaseAsset, [In, MarshalAs(UnmanagedType.BStr)] string strBaseAssetIndication);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x1c)]
        int GetMaofScenarios([In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array vecRecords, [In, MarshalAs(UnmanagedType.BStr)] string strOptionNumber, [In, Optional, DefaultParameterValue(-1)] MadadTypes strMadad);
        [DispId(20)]
        string CNTOPName { [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(20)] set; }
        [DispId(0x15)]
        string CNTBranch { [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x15)] set; }
        [DispId(0x16)]
        string CNTHelpAccount { [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x16)] set; }
        [DispId(0x17)]
        string CNTBaseAsset { [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x17)] set; }
        [DispId(0x18)]
        string CNTBaseAssetIndication { [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x18)] set; }
        [DispId(0x19)]
        string CNTOnlyK300 { [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x19)] set; }
        [DispId(0x1a)]
        string CNTOnlyOrders { [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x1a)] set; }
        [DispId(0x1b)]
        string CNTOptionMonth { [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x1b)] set; }
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x1f)]
        int GetShortTradeOptions([In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array psaStrRecords);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x20)]
        int GetK300MF([MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] out Array vecRecords, [In, Out, MarshalAs(UnmanagedType.BStr)] ref string LastTime, [In, MarshalAs(UnmanagedType.BStr)] string BNO, [In, Optional, DefaultParameterValue(-1)] BaseAssetTypes strMadad, [In, Optional, DefaultParameterValue(-1)] MonthType Month);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x24)]
        int GetK300RZ([MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] out Array vecRecords, [In, Out, MarshalAs(UnmanagedType.BStr)] ref string LastTime, [In, Optional, DefaultParameterValue("00000000"), MarshalAs(UnmanagedType.BStr)] string BNO, [In, Optional, DefaultParameterValue(-1)] StockKind kind, [In, Optional, DefaultParameterValue(-1)] MadadTypes madadType);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x23)]
        int GetK300Madad([MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] out Array vecRecords, [In, Optional, DefaultParameterValue("0"), MarshalAs(UnmanagedType.BStr)] string BNO);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x21)]
        int K300StartStream([In] K300StreamType streamType);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x22)]
        int K300StopStream([In] K300StreamType streamType);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x25)]
        int CreateK300EventsFile([In, MarshalAs(UnmanagedType.BStr)] string CacheFolder, [In, MarshalAs(UnmanagedType.BStr)] string DataFileName, [In] int EventsPerFile, [In] K300StreamType streamType, [MarshalAs(UnmanagedType.BStr)] out string ErrorMessage);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x26)]
        int GetShacharBondsInFuture([In, MarshalAs(UnmanagedType.BStr)] string BNO, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] out Array vecRecords);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x27)]
        int GetSH500([In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array psaStrRecords, [In, MarshalAs(UnmanagedType.BStr)] string Query);
        [DispId(40)]
        int K300SessionId { [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(40)] set; }
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x29)]
        int GetSH161([In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array vecRecords, [In] MadadTypes MadadSymbol);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x2a)]
        int GetBaseAssets2([MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] out Array psaRecords, [In, Optional, DefaultParameterValue(-1)] int BaseAssetCode);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x2b)]
        int GetMAOFByBaseAsset([In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array vecRecords, [In, Optional, DefaultParameterValue(-1)] int baseAssetBno);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x2c)]
        int GetRezefByIndex([In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array vecStrRecords, [In, Optional, DefaultParameterValue(-1)] int indexBno);
    }

    [ComImport, TypeLibType((short) 0x1100), Guid("3078E250-D427-454F-800D-71A105D64A18")]
    public interface IK300Event
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void FireMaof([MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array psaStrRecords, ref int nRecords);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void FireRezef([MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array psaStrRecords, ref int nRecords);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void FireMaofCNT([MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_BSTR)] ref Array psaStrRecords, ref int nRecords);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void FireRezefCNT([MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_BSTR)] ref Array psaStrRecords, ref int nRecords);
    }

    [ComEventInterface(typeof(IK300Event), typeof(IK300Event_EventProvider)), ComVisible(false), TypeLibType((short) 0x10)]
    public interface IK300Event_Event
    {
        // Events
        event IK300Event_FireMaofEventHandler FireMaof;
        event IK300Event_FireMaofCNTEventHandler FireMaofCNT;
        event IK300Event_FireRezefEventHandler FireRezef;
        event IK300Event_FireRezefCNTEventHandler FireRezefCNT;
    }

    internal sealed class IK300Event_EventProvider : IK300Event_Event, IDisposable
    {
        // Fields
        private ArrayList m_aEventSinkHelpers;
        private UCOMIConnectionPoint m_ConnectionPoint;
        private UCOMIConnectionPointContainer m_ConnectionPointContainer;

        // Methods
        public IK300Event_EventProvider(object);
        public override void add_FireMaof(IK300Event_FireMaofEventHandler);
        public override void add_FireMaofCNT(IK300Event_FireMaofCNTEventHandler);
        public override void add_FireRezef(IK300Event_FireRezefEventHandler);
        public override void add_FireRezefCNT(IK300Event_FireRezefCNTEventHandler);
        public override void Dispose();
        public override void Finalize();
        private void Init();
        public override void remove_FireMaof(IK300Event_FireMaofEventHandler);
        public override void remove_FireMaofCNT(IK300Event_FireMaofCNTEventHandler);
        public override void remove_FireRezef(IK300Event_FireRezefEventHandler);
        public override void remove_FireRezefCNT(IK300Event_FireRezefCNTEventHandler);
    }

    [ComVisible(false), TypeLibType((short) 0x10)]
    public delegate void IK300Event_FireMaofCNTEventHandler([MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_BSTR)] ref Array psaStrRecords, ref int nRecords);

    [ComVisible(false), TypeLibType((short) 0x10)]
    public delegate void IK300Event_FireMaofEventHandler([MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array psaStrRecords, ref int nRecords);

    [TypeLibType((short) 0x10), ComVisible(false)]
    public delegate void IK300Event_FireRezefCNTEventHandler([MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_BSTR)] ref Array psaStrRecords, ref int nRecords);

    [TypeLibType((short) 0x10), ComVisible(false)]
    public delegate void IK300Event_FireRezefEventHandler([MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array psaStrRecords, ref int nRecords);

    [ClassInterface(ClassInterfaceType.None)]
    public sealed class IK300Event_SinkHelper : IK300Event
    {
        // Fields
        public int m_dwCookie;
        public IK300Event_FireMaofCNTEventHandler m_FireMaofCNTDelegate;
        public IK300Event_FireMaofEventHandler m_FireMaofDelegate;
        public IK300Event_FireRezefCNTEventHandler m_FireRezefCNTDelegate;
        public IK300Event_FireRezefEventHandler m_FireRezefDelegate;

        // Methods
        internal IK300Event_SinkHelper();
        public override void FireMaof(ref Array, ref int);
        public override void FireMaofCNT(ref Array, ref int);
        public override void FireRezef(ref Array, ref int);
        public override void FireRezefCNT(ref Array, ref int);
    }

    [ComImport, Guid("A271F002-AD1E-4F27-B86F-8A4F49B92A8A"), TypeLibType((short) 0x10c0)]
    public interface IK300Events
    {
        [DispId(1)]
        BaseAssetTypes EventsFilterBaseAsset { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(1)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(1)] set; }
        [DispId(2)]
        MonthType EventsFilterMonth { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(2)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(2)] set; }
        [DispId(3)]
        int EventsFilterBno { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(3)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(3)] set; }
        [DispId(4)]
        int EventsFilterMaof { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(4)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(4)] set; }
        [DispId(5)]
        int EventsFilterRezef { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(5)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(5)] set; }
        [DispId(6)]
        int EventsFilterMadad { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(6)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(6)] set; }
        [DispId(7)]
        StockKind EventsFilterStockKind { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(7)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(7)] set; }
        [DispId(8)]
        MadadTypes EventsFilterStockMadad { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(8)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(8)] set; }
    }

    [StructLayout(LayoutKind.Sequential, Pack=4), Guid("A479FFA8-4B4C-4042-8D03-6530FA4ECC26")]
    public struct IndexInfoType
    {
        [MarshalAs(UnmanagedType.BStr)]
        public string SUG_REC;
        [MarshalAs(UnmanagedType.BStr)]
        public string TBL_NO;
        [MarshalAs(UnmanagedType.BStr)]
        public string LINE_NO;
        [MarshalAs(UnmanagedType.BStr)]
        public string COL_NO;
        [MarshalAs(UnmanagedType.BStr)]
        public string COD_MDD;
        [MarshalAs(UnmanagedType.BStr)]
        public string NAME_MDD;
        [MarshalAs(UnmanagedType.BStr)]
        public string VAL_MDD;
        [MarshalAs(UnmanagedType.BStr)]
        public string CHG_PCNT;
        [MarshalAs(UnmanagedType.BStr)]
        public string CHG_DIRCT;
        [MarshalAs(UnmanagedType.BStr)]
        public string FIL1;
    }

    [ComImport, Guid("622ECF94-4A34-48A3-8B05-C1120ECD7144"), TypeLibType((short) 0x10c0)]
    public interface IOnlineEvents
    {
        [DispId(1)]
        int EventsFilterKrn { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(1)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(1)] set; }
        [DispId(2)]
        int EventsFilterAccount { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(2)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(2)] set; }
        [DispId(3)]
        int EventsFilterBranch { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(3)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(3)] set; }
        [DispId(4)]
        int EventsFilterCustodian { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(4)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(4)] set; }
        [DispId(5)]
        int EventsFilterKupa { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(5)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(5)] set; }
        [DispId(6)]
        int EventsFilterRegAccount { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(6)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(6)] set; }
    }

    [ComImport, InterfaceType((short) 2), TypeLibType((short) 0x1000), Guid("B7E9E5CD-CBF0-4D9C-8C48-8C8FFB6466E8")]
    public interface IOnlineEventsEvents
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(1)]
        void OnOnlineKeren([In, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array psaStrRecords);
    }

    [TypeLibType((short) 0x10), ComVisible(false), ComEventInterface(typeof(IOnlineEventsEvents), typeof(IOnlineEventsEvents_EventProvider))]
    public interface IOnlineEventsEvents_Event
    {
        // Events
        event IOnlineEventsEvents_OnOnlineKerenEventHandler OnOnlineKeren;
    }

    internal sealed class IOnlineEventsEvents_EventProvider : IOnlineEventsEvents_Event, IDisposable
    {
        // Fields
        private ArrayList m_aEventSinkHelpers;
        private UCOMIConnectionPoint m_ConnectionPoint;
        private UCOMIConnectionPointContainer m_ConnectionPointContainer;

        // Methods
        public IOnlineEventsEvents_EventProvider(object);
        public override void add_OnOnlineKeren(IOnlineEventsEvents_OnOnlineKerenEventHandler);
        public override void Dispose();
        public override void Finalize();
        private void Init();
        public override void remove_OnOnlineKeren(IOnlineEventsEvents_OnOnlineKerenEventHandler);
    }

    [TypeLibType((short) 0x10), ComVisible(false)]
    public delegate void IOnlineEventsEvents_OnOnlineKerenEventHandler([In, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array psaStrRecords);

    [ClassInterface(ClassInterfaceType.None)]
    public sealed class IOnlineEventsEvents_SinkHelper : IOnlineEventsEvents
    {
        // Fields
        public int m_dwCookie;
        public IOnlineEventsEvents_OnOnlineKerenEventHandler m_OnOnlineKerenDelegate;

        // Methods
        internal IOnlineEventsEvents_SinkHelper();
        public override void OnOnlineKeren(ref Array);
    }

    [ComImport, Guid("24508459-7E53-43F2-B7AD-199DAFFE5163"), TypeLibType((short) 0x10c0)]
    public interface IPermissions
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(1)]
        int GetUserPermissions([In] int sessionId, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] out Array psaStrRecords, [In, Optional, DefaultParameterValue(0)] int ReloadPermissions);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(2)]
        int GetUserApp([In] int sessionId, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_BSTR)] out Array psaStrRecords, [In, Optional, DefaultParameterValue(""), MarshalAs(UnmanagedType.BStr)] string AppName);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(3)]
        int RefreshUserPermissions([In] int sessionId);
    }

    [ComImport, Guid("734AD6E7-C1D5-4A59-B3DF-9ACA7C28C5AA"), CoClass(typeof(IShortStocksClass))]
    public interface IShortStocks : IIShortStocks
    {
    }

    [ComImport, TypeLibType((short) 2), ClassInterface((short) 0), Guid("0DFE954C-5343-4717-8B93-F84E4F0F59E6")]
    public class IShortStocksClass : IIShortStocks, IShortStocks
    {
        // Methods
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(2)]
        public virtual extern int GetAllSafeShortStocks([In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array StockDetails, [In, Optional, MarshalAs(UnmanagedType.Struct)] ref object SortBy, [In, Optional, MarshalAs(UnmanagedType.Struct)] ref object SortDirecton);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(1)]
        public virtual extern int GetSafeStock([In, MarshalAs(UnmanagedType.BStr)] string StockID, [In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array StockDetails);
    }

    [ComImport, TypeLibType((short) 0x10c0), Guid("14F44D18-F88D-4260-B843-22084A594345")]
    public interface ITradeOptions
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(1)]
        int BlackAndScholesEx([In] int CallType, [In] int nOptionsToExcersize, [In] int nOptionsToBuy, [In] double dIV, [In] double dDividend, [In] double dInterest, [In] double dAssetPrice, [In] double dStrikePrice, [In] int nDaysToExpire, out BSDerivativesType Derivatives);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(2)]
        int GetOptionByBno([In] int BNO, out ExtOptRecord record);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(3)]
        int GetOptionByBaseAsset([In] BaseAssetTypes BaseAsset, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] out Array records, [In, Optional, DefaultParameterValue(-1)] MonthType Month);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(4)]
        int GetOptionByStrike([In] double dStrike, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] out Array records, [In, Optional, DefaultParameterValue(-1)] BaseAssetTypes BaseAsset, [In, Optional, DefaultParameterValue(-1)] MonthType Month);
    }

    [ComImport, Guid("1F6A77E6-EAFE-47ED-9DE5-6E96C67E632F"), TypeLibType((short) 0x10c0)]
    public interface IUser
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(1)]
        int Login([In, MarshalAs(UnmanagedType.BStr)] string username, [In, MarshalAs(UnmanagedType.BStr)] string AS400Password, [In, MarshalAs(UnmanagedType.BStr)] string AppPassword, [MarshalAs(UnmanagedType.BStr)] out string message, out int sessionId);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x41)]
        int LoginByLevel([In, MarshalAs(UnmanagedType.BStr)] string username, [In, MarshalAs(UnmanagedType.BStr)] string AS400Password, [In, MarshalAs(UnmanagedType.BStr)] string AppPassword, [In, MarshalAs(UnmanagedType.BStr)] string sysName, [In] int customer, [In] LoginLevel level, [MarshalAs(UnmanagedType.BStr)] out string message, out int sessionId);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(2)]
        int LoginAndChangePassword([In, MarshalAs(UnmanagedType.BStr)] string username, [In, MarshalAs(UnmanagedType.BStr)] string oldAS400Password, [In, MarshalAs(UnmanagedType.BStr)] string newAS400Password, [In, MarshalAs(UnmanagedType.BStr)] string AppPassword, [MarshalAs(UnmanagedType.BStr)] out string message, out int sessionId);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x42)]
        int LoginByLevelAndChangePassword([In, MarshalAs(UnmanagedType.BStr)] string username, [In, MarshalAs(UnmanagedType.BStr)] string oldAS400Password, [In, MarshalAs(UnmanagedType.BStr)] string newAS400Password, [In, MarshalAs(UnmanagedType.BStr)] string AppPassword, [In, MarshalAs(UnmanagedType.BStr)] string sysName, [In] int customer, [In] LoginLevel req_level, [MarshalAs(UnmanagedType.BStr)] out string message, out int sessionId);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x31)]
        int Logout([In] int sessionId);
        [DispId(0x21)]
        LoginStatus this[ref int sessionId] { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x21)] get; }
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x43)]
        LoginStatus LoginStateByLevel([In] ref int sessionId, [In] LoginLevel req_level);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x22)]
        void GetLoginActivity(ref int sessionId, out int percent, [MarshalAs(UnmanagedType.BStr)] out string description);
        [DispId(0x29)]
        string this[ref int sessionId] { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x29)] get; }
        [DispId(0x2a)]
        int this[ref int sessionId] { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x2a)] get; }
        [DispId(0x2c)]
        string this[ref int sessionId] { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x2c)] get; }
        [DispId(0x2d)]
        string this[ref int sessionId] { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x2d)] get; }
        [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x44)]
        string LoginErrorDescByLevel([In] ref int sessionId, [In] LoginLevel req_level);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(4)]
        int GetOrdersRezef([In] int sessionId, [In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array psaStrRecords, [In, Out, Optional, DefaultParameterValue("00000000"), MarshalAs(UnmanagedType.BStr)] ref string strLastTime, [In, Optional, DefaultParameterValue(0)] QueryType QueryType, [In, Optional, DefaultParameterValue("000000"), MarshalAs(UnmanagedType.BStr)] string strAccount, [In, Optional, DefaultParameterValue("000"), MarshalAs(UnmanagedType.BStr)] string strBranch, [In, Optional, DefaultParameterValue("00000000"), MarshalAs(UnmanagedType.BStr)] string strStockNumber);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(5)]
        int GetOrdersMaof([In] int sessionId, [In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array vecStrRecords, [In, Out, MarshalAs(UnmanagedType.BStr)] ref string strLastTime, [In, Optional, DefaultParameterValue(0)] QueryType qType, [In, Optional, DefaultParameterValue("000000"), MarshalAs(UnmanagedType.BStr)] string strAccount, [In, Optional, DefaultParameterValue("000"), MarshalAs(UnmanagedType.BStr)] string strBranch, [In, Optional, DefaultParameterValue("00000000"), MarshalAs(UnmanagedType.BStr)] string strOptionNumber);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x24)]
        int GetOrdersMF([In] int sessionId, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] out Array vecRecords, [In, MarshalAs(UnmanagedType.BStr)] string Account, [In, MarshalAs(UnmanagedType.BStr)] string Branch, [In, Out, Optional, DefaultParameterValue("0"), MarshalAs(UnmanagedType.BStr)] ref string LastTime);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x27)]
        int GetOrdersRZ([In] int sessionId, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] out Array vecRecords, [In, MarshalAs(UnmanagedType.BStr)] string Account, [In, MarshalAs(UnmanagedType.BStr)] string Branch, [In, Out, Optional, DefaultParameterValue("0"), MarshalAs(UnmanagedType.BStr)] ref string LastTime);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x25)]
        int OrdersStreamStart([In] int sessionId, [In, MarshalAs(UnmanagedType.BStr)] string Account, [In, MarshalAs(UnmanagedType.BStr)] string Branch, [In] TradeType streamType);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x26)]
        int OrdersStreamStop([In] int sessionId, [In, MarshalAs(UnmanagedType.BStr)] string Account, [In, MarshalAs(UnmanagedType.BStr)] string Branch, [In] TradeType streamType);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(6)]
        int GetHoldingsEx([In] int sessionId, [In, MarshalAs(UnmanagedType.BStr)] string strTik, [In, MarshalAs(UnmanagedType.BStr)] string strAccount, [In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array vecStrRecords, [In] TradeType type, [In, Optional, DefaultParameterValue("000"), MarshalAs(UnmanagedType.BStr)] string strBranch, [In, Optional, DefaultParameterValue("00000000"), MarshalAs(UnmanagedType.BStr)] string strStock, [In, Optional, DefaultParameterValue("000000"), MarshalAs(UnmanagedType.BStr)] string strMefazel);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(7)]
        int GetHoldings([In] int sessionId, [In, MarshalAs(UnmanagedType.BStr)] string strTik, [In, MarshalAs(UnmanagedType.BStr)] string strAccount, [In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array vecStrRecords, [In, Optional, DefaultParameterValue("000"), MarshalAs(UnmanagedType.BStr)] string strBranch, [In, Optional, DefaultParameterValue("00000000"), MarshalAs(UnmanagedType.BStr)] string strStock);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(13)]
        int GetAccounts([In] int sessionId, [In, MarshalAs(UnmanagedType.BStr)] string strQuery, [In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array psaStrRecords, [In, Out, MarshalAs(UnmanagedType.BStr)] ref string ErrMsg);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(14)]
        int GetUserPermissions([In] int sessionId, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] out Array psaStrRecords);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(15)]
        int UpdateUserPassword([In] int sessionId, [In, MarshalAs(UnmanagedType.BStr)] string strOldPassword, [In, MarshalAs(UnmanagedType.BStr)] string strNewPassword, [In, MarshalAs(UnmanagedType.BStr)] string strUserNumber);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x2e)]
        int UserAuthentication([In] int sessionId, [In, MarshalAs(UnmanagedType.BStr)] string Password);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x10)]
        int SendRezefOrder([In] int sessionId, [In, Out] ref RezefBasicOrder Order, [In, MarshalAs(UnmanagedType.BStr)] string Location, [In, MarshalAs(UnmanagedType.BStr)] string Trade_Type, [In, Out, MarshalAs(UnmanagedType.BStr)] ref string VBMsg, [In, Out] ref int ErrNO, out OrdersErrorTypes ErrorType, [In, Out] ref int OrderID, [In, MarshalAs(UnmanagedType.BStr)] string AuthUserName, [In, MarshalAs(UnmanagedType.BStr)] string AuthPassword, [In, MarshalAs(UnmanagedType.BStr)] string ReEnteredValue);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(20)]
        int SendMaofOrder([In] int sessionId, [In, Out] ref MaofOrderType Order, [In, Out, MarshalAs(UnmanagedType.BStr)] ref string VBMsg, [In, Out] ref int ErrNO, out OrdersErrorTypes ErrorType, [In, Out] ref int OrderID, [In, MarshalAs(UnmanagedType.BStr)] string AuthUserName, [In, MarshalAs(UnmanagedType.BStr)] string AuthPassword, [In, MarshalAs(UnmanagedType.BStr)] string ReEnteredValue, [In, Optional, DefaultParameterValue(0)] int SPCOrder);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x15)]
        int SendMaofOrderPele([In] int sessionId, [In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array Orders, [In, Out, MarshalAs(UnmanagedType.BStr)] ref string VBMsg, [In, Out] ref int ErrNO, out OrdersErrorTypes ErrorType, [In, Out] ref int OrderID, [In, MarshalAs(UnmanagedType.BStr)] string AuthUserName, [In, MarshalAs(UnmanagedType.BStr)] string AuthPassword, [In, MarshalAs(UnmanagedType.BStr)] string ReEnteredValue, [In, Optional, DefaultParameterValue(0)] int SPCOrder);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x20)]
        int SendMaofSpeedOrderPele([In] int sessionId, [In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array Orders, out int ErrNO, [In, Optional, DefaultParameterValue(0)] int SPCOrder);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x1a)]
        int PeleSecurities([In] int sessionId, [In, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array Orders, [In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array PeleSec, [In, MarshalAs(UnmanagedType.BStr)] string Account, [In, MarshalAs(UnmanagedType.BStr)] string Branch, [In, MarshalAs(UnmanagedType.BStr)] string Tik);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x1b)]
        int GetMargin([In] int sessionId, [In, MarshalAs(UnmanagedType.BStr)] string Account, [In, MarshalAs(UnmanagedType.BStr)] string Branch, [In] BaseAssetTypes asset, out double Margin);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x1c)]
        int SendRezefOrderSpeed([In] int sessionId, [In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array Orders, [In, MarshalAs(UnmanagedType.BStr)] string Location, [In, MarshalAs(UnmanagedType.BStr)] string Trade_Type);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x1d)]
        int GetPremium([In] int sessionId, [In, MarshalAs(UnmanagedType.BStr)] string Account, [In, MarshalAs(UnmanagedType.BStr)] string Branch, [In] BaseAssetTypes asset, out double premia);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(30)]
        int CalculateSecurities([In] int sessionId, [In] ref MaofOrderType Order, [In, MarshalAs(UnmanagedType.BStr)] string Account, [In, MarshalAs(UnmanagedType.BStr)] string Branch, [In, MarshalAs(UnmanagedType.BStr)] string Tik, [In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array psaAssets);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x1f)]
        int MaofOrderParams([In] int sessionId, [In, MarshalAs(UnmanagedType.BStr)] string Account, [In, MarshalAs(UnmanagedType.BStr)] string Branch, [In] BaseAssetTypes asset, out double Madad, out double emzaimNezilim, out double nezilimBoker, out double regularMargin, out double peleMargin, out double regularSecurities, out double PeleSecurities, out double OrdersPremium, out double executionPermium);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(50)]
        int MaofSecuritiesParams([In] int sessionId, [In, MarshalAs(UnmanagedType.BStr)] string Account, [In, MarshalAs(UnmanagedType.BStr)] string Branch, [In] BaseAssetTypes asset, out double Madad, out double emzaimNezilim, out double nezilimBoker, out double regularMargin, out double peleMargin, out double regularSecurities, out double PeleSecurities, out double OrdersPremium, out double executionPermium, out double regularSecuritiesNoQoutesKizuz, out double peleSecuritiesNoQoutesKizuz, out double worstScenarioMadad, out int worstScenarioIndex);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x17)]
        int StartMaofSession([In] int sessionId, [In, MarshalAs(UnmanagedType.BStr)] string Account, [In, MarshalAs(UnmanagedType.BStr)] string Branch, [In] SecurityCalcType calcType);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x18)]
        int StopMaofSession([In] int sessionId, [In, MarshalAs(UnmanagedType.BStr)] string Account, [In, MarshalAs(UnmanagedType.BStr)] string Branch);
        [DispId(0x19)]
        int this[string Account, string Branch] { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x19)] get; }
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x34)]
        int StartRezefSession([In] int sessionId, [In, MarshalAs(UnmanagedType.BStr)] string Account, [In, MarshalAs(UnmanagedType.BStr)] string Branch);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x35)]
        int StopRezefSession([In] int sessionId, [In, MarshalAs(UnmanagedType.BStr)] string Account, [In, MarshalAs(UnmanagedType.BStr)] string Branch);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x11)]
        int GetShortAccounts([In] int sessionId, [In, MarshalAs(UnmanagedType.BStr)] string strQuery, [In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array psaStrRecords, [In, Out, MarshalAs(UnmanagedType.BStr)] ref string ErrMsg);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x12)]
        void Encrypt([In, MarshalAs(UnmanagedType.BStr)] string OrgStr, [In, MarshalAs(UnmanagedType.BStr)] string EncKey, [MarshalAs(UnmanagedType.BStr)] out string EncStr);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x13)]
        void Decrypt([In, MarshalAs(UnmanagedType.BStr)] string EncStr, [In, MarshalAs(UnmanagedType.BStr)] string EncKey, [MarshalAs(UnmanagedType.BStr)] out string OrgStr);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x16)]
        int GetOnlineRm([In] int sessionId, [In, Out] ref RmOnlineTotalInfoType RmOnlineTotalInfo, [In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array psaStrRecords, [In, MarshalAs(UnmanagedType.BStr)] string bstrBranch, [In, MarshalAs(UnmanagedType.BStr)] string bstrAccount, [In, MarshalAs(UnmanagedType.BStr)] string bstrStockNumber);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x40)]
        int GetOnlineRmEx([In] int sessionId, [In] OnlineSessionType type, [In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array psaStrRecords, [In, Optional, DefaultParameterValue(0)] int Branch, [In, Optional, DefaultParameterValue(0)] int Account, [In, Out, Optional, DefaultParameterValue(0)] ref int LastCounter);
        [DispId(8)]
        ConnectionState State { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(8)] get; }
        [DispId(9)]
        string this[int sessionId] { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(9)] get; }
        [DispId(11)]
        string Cust { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(11)] get; }
        [DispId(12)]
        string System { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(12)] get; }
        [DispId(0x2b)]
        string SystemName { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x2b)] get; }
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(40)]
        int GetUserApp([In] int sessionId, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_BSTR)] out Array psaStrRecords, [In, Optional, DefaultParameterValue(""), MarshalAs(UnmanagedType.BStr)] string AppName);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x2f)]
        int GetMaofCnt([In] int sessionId, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] out Array vecK300, out int vecK300Len, [In, Out, MarshalAs(UnmanagedType.BStr)] ref string K300LastTime, [In, MarshalAs(UnmanagedType.BStr)] string K300Bno, [In] BaseAssetTypes strMadad, [In] MonthType Month, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] out Array vecOrders, out int vecOrdersLen, [In, MarshalAs(UnmanagedType.BStr)] string Account, [In, MarshalAs(UnmanagedType.BStr)] string Branch, [In, Out, MarshalAs(UnmanagedType.BStr)] ref string LastTime);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x30)]
        int GetRezefCnt([In] int sessionId, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] out Array vecK300, out int vecK300Len, [In, Out, MarshalAs(UnmanagedType.BStr)] ref string K300LastTime, [In, MarshalAs(UnmanagedType.BStr)] string K300Bno, [In] BaseAssetTypes strMadad, [In] MonthType Month, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] out Array vecOrders, out int vecOrdersLen, [In, MarshalAs(UnmanagedType.BStr)] string Account, [In, MarshalAs(UnmanagedType.BStr)] string Branch, [In, Out, MarshalAs(UnmanagedType.BStr)] ref string LastTime);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x33)]
        int SendMaofAsynchSpeedOrder([In] int sessionId, [In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array Orders, out int ErrNO, [In, Optional, DefaultParameterValue(0)] int SPCOrder);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x36)]
        int SendOrderRZ([In] int sessionId, [In] ref RezefSimpleOrder Order, [In, Out] ref int AsmachtaFmr, [In] int AsmachtaRezef, [MarshalAs(UnmanagedType.BStr)] out string VBMsg, out int ErrNO, out OrdersErrorTypes ErrorType, [In, Out] ref int OrderID, [In, MarshalAs(UnmanagedType.BStr)] string AuthUserName, [In, MarshalAs(UnmanagedType.BStr)] string AuthPassword, [In, MarshalAs(UnmanagedType.BStr)] string ReEnteredValue);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x3a)]
        int SendContOrderRZ([In] int sessionId, [In, Out] ref RezefContinuousOrder Order, [In, Out] ref int AsmachtaFmr, [In] int AsmachtaRezef, [MarshalAs(UnmanagedType.BStr)] out string VBMsg, out int ErrNO, out OrdersErrorTypes ErrorType, [In, Out] ref int OrderID, [In, MarshalAs(UnmanagedType.BStr)] string AuthUserName, [In, MarshalAs(UnmanagedType.BStr)] string AuthPassword, [In, MarshalAs(UnmanagedType.BStr)] string ReEnteredValue);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x3b)]
        int SendOrderSpeedRZ([In] int sessionId, [In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array Orders, [In, MarshalAs(UnmanagedType.BStr)] string Location, [In, MarshalAs(UnmanagedType.BStr)] string Trade_Type, [In, Out] ref int ErrNO);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x37)]
        int StartOnlineUserSession([In] int sessionId, [In] OnlineSessionType type);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x38)]
        int StopOnlineUserSession([In] int sessionId, [In] OnlineSessionType type);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x39)]
        int GetOnlineSessionBalance([In] int sessionId, [In] OnlineSessionType type, [In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array psaStrRecords, [In, Optional, DefaultParameterValue(0)] int Branch, [In, Optional, DefaultParameterValue(0)] int Account, [In, Out, Optional, DefaultParameterValue(0)] ref int LastCounter);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(60)]
        int GetAccountYieldInitial([In] int sessionId, [In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array psaStrRecords, [In, Optional, DefaultParameterValue(0)] int Branch, [In, Optional, DefaultParameterValue(0)] int Account, [In, Optional, DefaultParameterValue(0)] int AccountYieldOnly);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x3d)]
        int GetAccountYieldByRequirement([In] int sessionId, [In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array psaStrRecords, [In] int Branch, [In] int Account, [In] int StartDate, [In] YieldDataType DataType, [In, Optional, DefaultParameterValue(0)] int AccountYieldOnly);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x3e)]
        int GetAccountYieldDetailed([In] int sessionId, [In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array psaStrRecords, [In] int Branch, [In] int Account, [In] int StartDate, [In] int EndDate, [In, Optional, DefaultParameterValue(0)] int AccountYieldOnly);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x3f)]
        int GetKranotAccounts([In] int sessionId, [In, MarshalAs(UnmanagedType.BStr)] string strQuery, [In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array psaStrRecords, [In, Out, MarshalAs(UnmanagedType.BStr)] ref string ErrMsg);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x45)]
        int GetOnlineRmFromMaofSession([In] int sessionId, [In, MarshalAs(UnmanagedType.BStr)] string Account, [In, MarshalAs(UnmanagedType.BStr)] string Branch, [In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array psaStrRecords);
    }

    [ComImport, Guid("259FD37B-C3D0-4933-BF7C-DB646A625274"), CoClass(typeof(K300Class))]
    public interface K300 : IK300, IK300Event_Event
    {
    }

    [ComImport, TypeLibType((short) 2), Guid("1B9BDD96-84A2-4D73-8EF5-C1BA2252BDE7"), ClassInterface((short) 0), ComSourceInterfaces("TaskBarLib.IK300Event\0")]
    public class K300Class : IK300, K300, IK300Event_Event
    {
        // Events
        public event IK300Event_FireMaofEventHandler FireMaof;
        public event IK300Event_FireMaofCNTEventHandler FireMaofCNT;
        public event IK300Event_FireRezefEventHandler FireRezef;
        public event IK300Event_FireRezefCNTEventHandler FireRezefCNT;

        // Methods
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(4)]
        public virtual extern bool CalculateScenarios();
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x25)]
        public virtual extern int CreateK300EventsFile([In, MarshalAs(UnmanagedType.BStr)] string CacheFolder, [In, MarshalAs(UnmanagedType.BStr)] string DataFileName, [In] int EventsPerFile, [In] K300StreamType streamType, [MarshalAs(UnmanagedType.BStr)] out string ErrorMessage);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(2)]
        public virtual extern int GetBaseAssets([MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] out Array psaRecords, [In, Optional, DefaultParameterValue(-1)] int BaseAssetCode);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x2a)]
        public virtual extern int GetBaseAssets2([MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] out Array psaRecords, [In, Optional, DefaultParameterValue(-1)] int BaseAssetCode);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x11)]
        public virtual extern int GetConstStock([In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array psaStrRecords);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(6)]
        public virtual extern int GetIndexes([In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array psaStrRecords);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(7)]
        public virtual extern int GetIndexStructure([In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_BSTR)] ref Array psaStrRecords);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x23)]
        public virtual extern int GetK300Madad([MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] out Array vecRecords, [In, Optional, DefaultParameterValue("0"), MarshalAs(UnmanagedType.BStr)] string BNO);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x20)]
        public virtual extern int GetK300MF([MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] out Array vecRecords, [In, Out, MarshalAs(UnmanagedType.BStr)] ref string LastTime, [In, MarshalAs(UnmanagedType.BStr)] string BNO, [In, Optional, DefaultParameterValue(-1)] BaseAssetTypes strMadad, [In, Optional, DefaultParameterValue(-1)] MonthType Month);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x24)]
        public virtual extern int GetK300RZ([MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] out Array vecRecords, [In, Out, MarshalAs(UnmanagedType.BStr)] ref string LastTime, [In, Optional, DefaultParameterValue("00000000"), MarshalAs(UnmanagedType.BStr)] string BNO, [In, Optional, DefaultParameterValue(-1)] StockKind kind, [In, Optional, DefaultParameterValue(-1)] MadadTypes madadType);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(14)]
        public virtual extern int GetMadadHistory([In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array psaStrRecords, [In, Optional, DefaultParameterValue("000"), MarshalAs(UnmanagedType.BStr)] string MadadCode, [In, Optional, DefaultParameterValue("-1"), MarshalAs(UnmanagedType.BStr)] string strLastTime);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(1)]
        public virtual extern int GetMAOF([In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array vecRecords, [In, Out, MarshalAs(UnmanagedType.BStr)] ref string strLastTime, [In, MarshalAs(UnmanagedType.BStr)] string strOptionNumber, [In, Optional, DefaultParameterValue(-1)] MadadTypes strMadad);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x2b)]
        public virtual extern int GetMAOFByBaseAsset([In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array vecRecords, [In, Optional, DefaultParameterValue(-1)] int baseAssetBno);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x12)]
        public virtual extern int GetMaofCnt([In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_BSTR)] ref Array psaStrRecords, [In, MarshalAs(UnmanagedType.BStr)] string strBranch, [In, MarshalAs(UnmanagedType.BStr)] string strHelpAccount, [In, MarshalAs(UnmanagedType.BStr)] string strTimeOrders, [In, MarshalAs(UnmanagedType.BStr)] string strTimeK300, [In, MarshalAs(UnmanagedType.BStr)] string strBaseAsset, [In, MarshalAs(UnmanagedType.BStr)] string strOnlyK300, [In, MarshalAs(UnmanagedType.BStr)] string strOnlyOrders, [In, MarshalAs(UnmanagedType.BStr)] string strOptionMonth);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(30)]
        public virtual extern int GetMAOFRaw([In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_BSTR)] ref Array vecRecords, [In, Out, MarshalAs(UnmanagedType.BStr)] ref string strLastTime, [In, MarshalAs(UnmanagedType.BStr)] string strOptionNumber, [In, Optional, DefaultParameterValue(-1)] MadadTypes strMadad);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x1c)]
        public virtual extern int GetMaofScenarios([In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array vecRecords, [In, MarshalAs(UnmanagedType.BStr)] string strOptionNumber, [In, Optional, DefaultParameterValue(-1)] MadadTypes strMadad);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(15)]
        public virtual extern int GetMaofStocks([In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array psaStrRecords, [In, Optional, DefaultParameterValue(0)] MadadTypes MadadSymbol);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(5)]
        public virtual extern int GetRezef([In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array vecStrRecords, [In, Out, MarshalAs(UnmanagedType.BStr)] ref string strLastTime, [In, Optional, DefaultParameterValue("00000000"), MarshalAs(UnmanagedType.BStr)] string strStockNumber, [In, Optional, DefaultParameterValue(-1)] MadadTypes strMadad);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x2c)]
        public virtual extern int GetRezefByIndex([In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array vecStrRecords, [In, Optional, DefaultParameterValue(-1)] int indexBno);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x1d)]
        public virtual extern int GetRezefRaw([In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_BSTR)] ref Array vecStrRawRecords, [In, Out, MarshalAs(UnmanagedType.BStr)] ref string strLastTime, [In, Optional, DefaultParameterValue("00000000"), MarshalAs(UnmanagedType.BStr)] string strStockNumber, [In, Optional, DefaultParameterValue(-1)] MadadTypes strMadad);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x13)]
        public virtual extern int GetRzfCNT([In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_BSTR)] ref Array psaStrRecords, [In, MarshalAs(UnmanagedType.BStr)] string strOPName, [In, MarshalAs(UnmanagedType.BStr)] string strBranch, [In, MarshalAs(UnmanagedType.BStr)] string strHelpAccount, [In, MarshalAs(UnmanagedType.BStr)] string strTimeOrders, [In, MarshalAs(UnmanagedType.BStr)] string strTimeK300, [In, MarshalAs(UnmanagedType.BStr)] string strBaseAsset, [In, MarshalAs(UnmanagedType.BStr)] string strBaseAssetIndication);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x29)]
        public virtual extern int GetSH161([In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array vecRecords, [In] MadadTypes MadadSymbol);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x27)]
        public virtual extern int GetSH500([In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array psaStrRecords, [In, MarshalAs(UnmanagedType.BStr)] string Query);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x26)]
        public virtual extern int GetShacharBondsInFuture([In, MarshalAs(UnmanagedType.BStr)] string BNO, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] out Array vecRecords);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x1f)]
        public virtual extern int GetShortTradeOptions([In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array psaStrRecords);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(13)]
        public virtual extern int GetStatistics([In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array psaStrRecords);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(11)]
        public virtual extern int GetStockHistory([In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_BSTR)] ref Array psaStrRecords, [In, MarshalAs(UnmanagedType.BStr)] string StockNumber);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(8)]
        public virtual extern int GetStocksRZF([In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_BSTR)] ref Array psaStrRecords);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x10)]
        public virtual extern int GetStockStage([In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array psaStrRecords, [In, MarshalAs(UnmanagedType.BStr)] string StockNumber);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(12)]
        public virtual extern int GetStockValue([In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_BSTR)] ref Array psaStrRecords, [In, Optional, DefaultParameterValue("00000000"), MarshalAs(UnmanagedType.BStr)] string strLastTime);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(3)]
        public virtual extern void GetTradeOptions([In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_BSTR)] ref Array psaStrRecords, [In, Out] ref int retVal);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x21)]
        public virtual extern int K300StartStream([In] K300StreamType streamType);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x22)]
        public virtual extern int K300StopStream([In] K300StreamType streamType);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(10)]
        public virtual extern int StartStream([In, Optional, DefaultParameterValue(0)] K300StreamType streamType, [In, Optional, DefaultParameterValue("00000000"), MarshalAs(UnmanagedType.BStr)] string strStockNumber, [In, Optional, DefaultParameterValue(-1)] MadadTypes strMadad, [In, Optional, DefaultParameterValue(1)] int withEvents);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(9)]
        public virtual extern void StopUpdate(int pnID);

        // Properties
        [DispId(0x17)]
        public virtual string CNTBaseAsset { [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x17)] set; }
        [DispId(0x18)]
        public virtual string CNTBaseAssetIndication { [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x18)] set; }
        [DispId(0x15)]
        public virtual string CNTBranch { [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x15)] set; }
        [DispId(0x16)]
        public virtual string CNTHelpAccount { [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x16)] set; }
        [DispId(0x19)]
        public virtual string CNTOnlyK300 { [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x19)] set; }
        [DispId(0x1a)]
        public virtual string CNTOnlyOrders { [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x1a)] set; }
        [DispId(20)]
        public virtual string CNTOPName { [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(20)] set; }
        [DispId(0x1b)]
        public virtual string CNTOptionMonth { [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x1b)] set; }
        [DispId(40)]
        public virtual int K300SessionId { [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(40)] set; }
        [DispId(0x17)]
        public virtual string TaskBarLib.IK300.CNTBaseAsset { [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x17)] set; }
        [DispId(0x18)]
        public virtual string TaskBarLib.IK300.CNTBaseAssetIndication { [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x18)] set; }
        [DispId(0x15)]
        public virtual string TaskBarLib.IK300.CNTBranch { [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x15)] set; }
        [DispId(0x16)]
        public virtual string TaskBarLib.IK300.CNTHelpAccount { [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x16)] set; }
        [DispId(0x19)]
        public virtual string TaskBarLib.IK300.CNTOnlyK300 { [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x19)] set; }
        [DispId(0x1a)]
        public virtual string TaskBarLib.IK300.CNTOnlyOrders { [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x1a)] set; }
        [DispId(20)]
        public virtual string TaskBarLib.IK300.CNTOPName { [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(20)] set; }
        [DispId(0x1b)]
        public virtual string TaskBarLib.IK300.CNTOptionMonth { [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x1b)] set; }
        [DispId(40)]
        public virtual int TaskBarLib.IK300.K300SessionId { [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(40)] set; }
    }

    [ComImport, CoClass(typeof(K300EventsClass)), Guid("A271F002-AD1E-4F27-B86F-8A4F49B92A8A")]
    public interface K300Events : IK300Events, _IK300EventsEvents_Event
    {
    }

    [ComImport, Guid("6E3DF86A-AB50-4BCD-A3E0-C4A35A358F13"), ClassInterface((short) 0), ComSourceInterfaces("TaskBarLib._IK300EventsEvents\0"), TypeLibType((short) 2)]
    public class K300EventsClass : IK300Events, K300Events, _IK300EventsEvents_Event
    {
        // Events
        public event _IK300EventsEvents_OnMadadEventHandler OnMadad;
        public event _IK300EventsEvents_OnMaofEventHandler OnMaof;
        public event _IK300EventsEvents_OnRezefEventHandler OnRezef;

        // Properties
        [DispId(1)]
        public virtual BaseAssetTypes EventsFilterBaseAsset { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(1)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(1)] set; }
        [DispId(3)]
        public virtual int EventsFilterBno { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(3)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(3)] set; }
        [DispId(6)]
        public virtual int EventsFilterMadad { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(6)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(6)] set; }
        [DispId(4)]
        public virtual int EventsFilterMaof { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(4)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(4)] set; }
        [DispId(2)]
        public virtual MonthType EventsFilterMonth { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(2)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(2)] set; }
        [DispId(5)]
        public virtual int EventsFilterRezef { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(5)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(5)] set; }
        [DispId(7)]
        public virtual StockKind EventsFilterStockKind { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(7)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(7)] set; }
        [DispId(8)]
        public virtual MadadTypes EventsFilterStockMadad { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(8)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(8)] set; }
        [DispId(1)]
        public virtual BaseAssetTypes TaskBarLib.IK300Events.EventsFilterBaseAsset { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(1)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(1)] set; }
        [DispId(3)]
        public virtual int TaskBarLib.IK300Events.EventsFilterBno { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(3)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(3)] set; }
        [DispId(6)]
        public virtual int TaskBarLib.IK300Events.EventsFilterMadad { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(6)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(6)] set; }
        [DispId(4)]
        public virtual int TaskBarLib.IK300Events.EventsFilterMaof { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(4)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(4)] set; }
        [DispId(2)]
        public virtual MonthType TaskBarLib.IK300Events.EventsFilterMonth { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(2)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(2)] set; }
        [DispId(5)]
        public virtual int TaskBarLib.IK300Events.EventsFilterRezef { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(5)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(5)] set; }
        [DispId(7)]
        public virtual StockKind TaskBarLib.IK300Events.EventsFilterStockKind { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(7)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(7)] set; }
        [DispId(8)]
        public virtual MadadTypes TaskBarLib.IK300Events.EventsFilterStockMadad { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(8)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(8)] set; }
    }

    [StructLayout(LayoutKind.Sequential, Pack=4), Guid("D5D1A5B2-8EB8-4A88-8592-11AD4446CDF2")]
    public struct K300MadadHistType
    {
        [MarshalAs(UnmanagedType.BStr)]
        public string POTEH;
        [MarshalAs(UnmanagedType.BStr)]
        public string SUG_REC;
        [MarshalAs(UnmanagedType.BStr)]
        public string MDD_COD;
        [MarshalAs(UnmanagedType.BStr)]
        public string MDD_NAME;
        [MarshalAs(UnmanagedType.BStr)]
        public string MDD_SUG;
        [MarshalAs(UnmanagedType.BStr)]
        public string MDD_NUM;
        [MarshalAs(UnmanagedType.BStr)]
        public string MDD;
        [MarshalAs(UnmanagedType.BStr)]
        public string MDD_DF;
        [MarshalAs(UnmanagedType.BStr)]
        public string CALC_TIME;
        [MarshalAs(UnmanagedType.BStr)]
        public string FIL;
        [MarshalAs(UnmanagedType.BStr)]
        public string SOGER;
        [MarshalAs(UnmanagedType.BStr)]
        public string UPD_DAT;
        [MarshalAs(UnmanagedType.BStr)]
        public string UPD_TIME;
        [MarshalAs(UnmanagedType.BStr)]
        public string MDD_BASIS;
    }

    [StructLayout(LayoutKind.Sequential, Pack=4), Guid("D717A43E-6DFD-4521-9D14-F2D5D28A39EF")]
    public struct K300MadadType
    {
        [MarshalAs(UnmanagedType.BStr)]
        public string SUG_RC;
        [MarshalAs(UnmanagedType.BStr)]
        public string BNO_N;
        [MarshalAs(UnmanagedType.BStr)]
        public string FIL1_VK;
        [MarshalAs(UnmanagedType.BStr)]
        public string MDD_COD;
        [MarshalAs(UnmanagedType.BStr)]
        public string MDD_SUG;
        [MarshalAs(UnmanagedType.BStr)]
        public string MDD_N;
        [MarshalAs(UnmanagedType.BStr)]
        public string FIL2_VK;
        [MarshalAs(UnmanagedType.BStr)]
        public string MDD_NAME;
        [MarshalAs(UnmanagedType.BStr)]
        public string Madad;
        [MarshalAs(UnmanagedType.BStr)]
        public string FIL3_VK;
        [MarshalAs(UnmanagedType.BStr)]
        public string MDD_DF;
        [MarshalAs(UnmanagedType.BStr)]
        public string CALC_TIME;
        [MarshalAs(UnmanagedType.BStr)]
        public string FIL6_VK;
        [MarshalAs(UnmanagedType.BStr)]
        public string UPD_DAT;
        [MarshalAs(UnmanagedType.BStr)]
        public string UPD_TIME;
    }

    [StructLayout(LayoutKind.Sequential, Pack=8), Guid("F61FBBEC-19FE-462C-BD81-5D1F6550BC54")]
    public struct K300MaofScenariosType
    {
        public int nBaseAsset;
        public double dStrike;
        public int nDaysToExpiary;
        public int nExpMonth;
        public int nNumber;
        public int nOptType;
        public double dLastPrice;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=0x2d)]
        public double[] dScenarios;
    }

    [StructLayout(LayoutKind.Sequential, Pack=4), Guid("D717A43E-4447-4506-95AD-992A486B6003")]
    public struct K300MaofType
    {
        [MarshalAs(UnmanagedType.BStr)]
        public string SUG_REC;
        [MarshalAs(UnmanagedType.BStr)]
        public string TRADE_METH;
        [MarshalAs(UnmanagedType.BStr)]
        public string BNO_Num;
        [MarshalAs(UnmanagedType.BStr)]
        public string LAST_REC;
        [MarshalAs(UnmanagedType.BStr)]
        public string SIDURI_Num;
        [MarshalAs(UnmanagedType.BStr)]
        public string SYMBOL_E;
        [MarshalAs(UnmanagedType.BStr)]
        public string Symbol;
        [MarshalAs(UnmanagedType.BStr)]
        public string BNO_NAME_E;
        [MarshalAs(UnmanagedType.BStr)]
        public string BNO_NAME;
        [MarshalAs(UnmanagedType.BStr)]
        public string BRANCH_NO;
        [MarshalAs(UnmanagedType.BStr)]
        public string BRANCH_U;
        [MarshalAs(UnmanagedType.BStr)]
        public string SUG_BNO;
        [MarshalAs(UnmanagedType.BStr)]
        public string MIN_UNIT;
        [MarshalAs(UnmanagedType.BStr)]
        public string HARIG_NV;
        [MarshalAs(UnmanagedType.BStr)]
        public string MIN_PR;
        [MarshalAs(UnmanagedType.BStr)]
        public string MAX_PR;
        [MarshalAs(UnmanagedType.BStr)]
        public string BASIS_PRC;
        [MarshalAs(UnmanagedType.BStr)]
        public string BASIS_COD;
        [MarshalAs(UnmanagedType.BStr)]
        public string STATUS_COD;
        [MarshalAs(UnmanagedType.BStr)]
        public string EX_DATE;
        [MarshalAs(UnmanagedType.BStr)]
        public string EX_PRC;
        [MarshalAs(UnmanagedType.BStr)]
        public string VL_MULT;
        [MarshalAs(UnmanagedType.BStr)]
        public string VL_COD;
        [MarshalAs(UnmanagedType.BStr)]
        public string ZERO_COD;
        [MarshalAs(UnmanagedType.BStr)]
        public string shlav;
        [MarshalAs(UnmanagedType.BStr)]
        public string STATUS;
        [MarshalAs(UnmanagedType.BStr)]
        public string TRD_STP_CD;
        [MarshalAs(UnmanagedType.BStr)]
        public string TRD_STP_N;
        [MarshalAs(UnmanagedType.BStr)]
        public string STP_OPN_TM;
        [MarshalAs(UnmanagedType.BStr)]
        public string LMT_BY1;
        [MarshalAs(UnmanagedType.BStr)]
        public string LMT_BY2;
        [MarshalAs(UnmanagedType.BStr)]
        public string LMT_BY3;
        [MarshalAs(UnmanagedType.BStr)]
        public string LMY_BY1_NV;
        [MarshalAs(UnmanagedType.BStr)]
        public string LMY_BY2_NV;
        [MarshalAs(UnmanagedType.BStr)]
        public string LMY_BY3_NV;
        [MarshalAs(UnmanagedType.BStr)]
        public string RWR_FE;
        [MarshalAs(UnmanagedType.BStr)]
        public string LMT_SL1;
        [MarshalAs(UnmanagedType.BStr)]
        public string LMT_SL2;
        [MarshalAs(UnmanagedType.BStr)]
        public string LMT_SL3;
        [MarshalAs(UnmanagedType.BStr)]
        public string LMY_SL1_NV;
        [MarshalAs(UnmanagedType.BStr)]
        public string LMY_SL2_NV;
        [MarshalAs(UnmanagedType.BStr)]
        public string LMY_SL3_NV;
        [MarshalAs(UnmanagedType.BStr)]
        public string RWR_FF;
        [MarshalAs(UnmanagedType.BStr)]
        public string PRC;
        [MarshalAs(UnmanagedType.BStr)]
        public string COD_PRC;
        [MarshalAs(UnmanagedType.BStr)]
        public string SUG_PRC;
        [MarshalAs(UnmanagedType.BStr)]
        public string LST_DF_BS;
        [MarshalAs(UnmanagedType.BStr)]
        public string RWR_FG;
        [MarshalAs(UnmanagedType.BStr)]
        public string LST_DL_PR;
        [MarshalAs(UnmanagedType.BStr)]
        public string LST_DL_TM;
        [MarshalAs(UnmanagedType.BStr)]
        public string LST_DL_VL;
        [MarshalAs(UnmanagedType.BStr)]
        public string DAY_VL;
        [MarshalAs(UnmanagedType.BStr)]
        public string DAY_VL_NIS;
        [MarshalAs(UnmanagedType.BStr)]
        public string DAY_DIL_NO;
        [MarshalAs(UnmanagedType.BStr)]
        public string RWR_FH;
        [MarshalAs(UnmanagedType.BStr)]
        public string DAY_MAX_PR;
        [MarshalAs(UnmanagedType.BStr)]
        public string DAY_MIN_PR;
        [MarshalAs(UnmanagedType.BStr)]
        public string POS_OPN;
        [MarshalAs(UnmanagedType.BStr)]
        public string POS_OPN_DF;
        [MarshalAs(UnmanagedType.BStr)]
        public string STS_NXT_DY;
        [MarshalAs(UnmanagedType.BStr)]
        public string UPD_DAT;
        [MarshalAs(UnmanagedType.BStr)]
        public string UPD_TIME;
        [MarshalAs(UnmanagedType.BStr)]
        public string FILER;
    }

    [StructLayout(LayoutKind.Sequential, Pack=4), Guid("D717A43D-4447-4506-95AD-992A486B6003")]
    public struct K300RzfType
    {
        [MarshalAs(UnmanagedType.BStr)]
        public string SUG_REC;
        [MarshalAs(UnmanagedType.BStr)]
        public string BNO_Num;
        [MarshalAs(UnmanagedType.BStr)]
        public string BNO_NAME;
        [MarshalAs(UnmanagedType.BStr)]
        public string Symbol;
        [MarshalAs(UnmanagedType.BStr)]
        public string TRADE_METH;
        [MarshalAs(UnmanagedType.BStr)]
        public string SIDURI_Num;
        [MarshalAs(UnmanagedType.BStr)]
        public string RWR_VA;
        [MarshalAs(UnmanagedType.BStr)]
        public string MIN_UNIT;
        [MarshalAs(UnmanagedType.BStr)]
        public string HARIG_NV;
        [MarshalAs(UnmanagedType.BStr)]
        public string MIN_PR_OPN;
        [MarshalAs(UnmanagedType.BStr)]
        public string MAX_PR_OPN;
        [MarshalAs(UnmanagedType.BStr)]
        public string MIN_PR_CNT;
        [MarshalAs(UnmanagedType.BStr)]
        public string MAX_PR_CNT;
        [MarshalAs(UnmanagedType.BStr)]
        public string BASIS_PRC;
        [MarshalAs(UnmanagedType.BStr)]
        public string STATUS;
        [MarshalAs(UnmanagedType.BStr)]
        public string EX_COD;
        [MarshalAs(UnmanagedType.BStr)]
        public string EX_DETAIL;
        [MarshalAs(UnmanagedType.BStr)]
        public string RWR_VB;
        [MarshalAs(UnmanagedType.BStr)]
        public string shlav;
        [MarshalAs(UnmanagedType.BStr)]
        public string LAST_PRC;
        [MarshalAs(UnmanagedType.BStr)]
        public string TRD_STP_N;
        [MarshalAs(UnmanagedType.BStr)]
        public string STP_OPN_TM;
        [MarshalAs(UnmanagedType.BStr)]
        public string RWR_VD;
        [MarshalAs(UnmanagedType.BStr)]
        public string LMT_BY1;
        [MarshalAs(UnmanagedType.BStr)]
        public string LMT_BY2;
        [MarshalAs(UnmanagedType.BStr)]
        public string LMT_BY3;
        [MarshalAs(UnmanagedType.BStr)]
        public string LMY_BY1_NV;
        [MarshalAs(UnmanagedType.BStr)]
        public string LMY_BY2_NV;
        [MarshalAs(UnmanagedType.BStr)]
        public string LMY_BY3_NV;
        [MarshalAs(UnmanagedType.BStr)]
        public string MKT_NV_BY;
        [MarshalAs(UnmanagedType.BStr)]
        public string MKT_NV_BY_NUM;
        [MarshalAs(UnmanagedType.BStr)]
        public string RWR_VE;
        [MarshalAs(UnmanagedType.BStr)]
        public string LMT_SL1;
        [MarshalAs(UnmanagedType.BStr)]
        public string LMT_SL2;
        [MarshalAs(UnmanagedType.BStr)]
        public string LMT_SL3;
        [MarshalAs(UnmanagedType.BStr)]
        public string LMY_SL1_NV;
        [MarshalAs(UnmanagedType.BStr)]
        public string LMY_SL2_NV;
        [MarshalAs(UnmanagedType.BStr)]
        public string LMY_SL3_NV;
        [MarshalAs(UnmanagedType.BStr)]
        public string MKT_NV_SL;
        [MarshalAs(UnmanagedType.BStr)]
        public string MKT_NV_SL_NUM;
        [MarshalAs(UnmanagedType.BStr)]
        public string RWR_VF;
        [MarshalAs(UnmanagedType.BStr)]
        public string THEOR_PR;
        [MarshalAs(UnmanagedType.BStr)]
        public string THEOR_VL;
        [MarshalAs(UnmanagedType.BStr)]
        public string RWR_VG;
        [MarshalAs(UnmanagedType.BStr)]
        public string LST_DL_PR;
        [MarshalAs(UnmanagedType.BStr)]
        public string LST_DL_TM;
        [MarshalAs(UnmanagedType.BStr)]
        public string LST_DF_BS;
        [MarshalAs(UnmanagedType.BStr)]
        public string LST_DF_OPN;
        [MarshalAs(UnmanagedType.BStr)]
        public string LST_DL_VL;
        [MarshalAs(UnmanagedType.BStr)]
        public string DAY_VL;
        [MarshalAs(UnmanagedType.BStr)]
        public string DAY_VL_NIS;
        [MarshalAs(UnmanagedType.BStr)]
        public string DAY_DIL_NO;
        [MarshalAs(UnmanagedType.BStr)]
        public string DAY_MAX_PR;
        [MarshalAs(UnmanagedType.BStr)]
        public string DAY_MIN_PR;
        [MarshalAs(UnmanagedType.BStr)]
        public string BNO_NAME_E;
        [MarshalAs(UnmanagedType.BStr)]
        public string SYMBOL_E;
        [MarshalAs(UnmanagedType.BStr)]
        public string STP_COD;
        [MarshalAs(UnmanagedType.BStr)]
        public string COD_SHAAR;
        [MarshalAs(UnmanagedType.BStr)]
        public string UPD_DAT;
        [MarshalAs(UnmanagedType.BStr)]
        public string UPD_TIME;
    }

    [Guid("A71C84F9-600D-4869-88F9-E14ED5FFB0CA")]
    public enum K300StreamType
    {
        IndexStream = 0x34,
        MaofCNTStream = 50,
        MaofStream = 0x30,
        RezefCNTStream = 0x33,
        RezefStream = 0x31
    }

    [StructLayout(LayoutKind.Sequential, Pack=4), Guid("D717A439-4447-4506-95AD-992A486B6003")]
    public struct K300STSType
    {
        [MarshalAs(UnmanagedType.BStr)]
        public string POTEH;
        [MarshalAs(UnmanagedType.BStr)]
        public string SUG_REC;
        [MarshalAs(UnmanagedType.BStr)]
        public string TRADE_METH;
        [MarshalAs(UnmanagedType.BStr)]
        public string DIL_NO;
        [MarshalAs(UnmanagedType.BStr)]
        public string TOT_MAHZOR;
        [MarshalAs(UnmanagedType.BStr)]
        public string VL_NIS_MKT;
        [MarshalAs(UnmanagedType.BStr)]
        public string VL_NIS_UP;
        [MarshalAs(UnmanagedType.BStr)]
        public string VL_NIS_DN;
        [MarshalAs(UnmanagedType.BStr)]
        public string VL_NIS_STT;
        [MarshalAs(UnmanagedType.BStr)]
        public string BNO_UP;
        [MarshalAs(UnmanagedType.BStr)]
        public string BNO_DN;
        [MarshalAs(UnmanagedType.BStr)]
        public string BNO_STT;
        [MarshalAs(UnmanagedType.BStr)]
        public string SYS_TIME;
        [MarshalAs(UnmanagedType.BStr)]
        public string FIL;
        [MarshalAs(UnmanagedType.BStr)]
        public string SOGER;
        [MarshalAs(UnmanagedType.BStr)]
        public string UPD_DAT;
        [MarshalAs(UnmanagedType.BStr)]
        public string UPD_TIME;
    }

    [StructLayout(LayoutKind.Sequential, Pack=8), Guid("0CAE5D87-2B25-4A58-80FA-FE4D43369DBD")]
    public struct KerenBasicType
    {
        public int BNO_Num;
        public int NV_LMT_ERR;
        [MarshalAs(UnmanagedType.BStr)]
        public string BNO_NAME;
        [MarshalAs(UnmanagedType.BStr)]
        public string BNO_NAME2;
        public double BASIS_RATE;
        [MarshalAs(UnmanagedType.BStr)]
        public string SH;
        [MarshalAs(UnmanagedType.BStr)]
        public string CLOS_TM;
        [MarshalAs(UnmanagedType.BStr)]
        public string DOLLAR_FUND;
    }

    [StructLayout(LayoutKind.Sequential, Pack=4), Guid("4315A163-2DF4-4D42-9321-C99BD680117A")]
    public struct KrnBizuimType
    {
        [MarshalAs(UnmanagedType.BStr)]
        public string SUGREC;
        [MarshalAs(UnmanagedType.BStr)]
        public string BNO;
        [MarshalAs(UnmanagedType.BStr)]
        public string BNOSTS;
        [MarshalAs(UnmanagedType.BStr)]
        public string SHKAV;
        [MarshalAs(UnmanagedType.BStr)]
        public string BnoName;
        [MarshalAs(UnmanagedType.BStr)]
        public string MBR;
        [MarshalAs(UnmanagedType.BStr)]
        public string MBRNAME;
        [MarshalAs(UnmanagedType.BStr)]
        public string IZIROT;
        [MarshalAs(UnmanagedType.BStr)]
        public string IZIRAPRC;
        [MarshalAs(UnmanagedType.BStr)]
        public string PIDYONOT;
        [MarshalAs(UnmanagedType.BStr)]
        public string PIDYPRC;
        [MarshalAs(UnmanagedType.BStr)]
        public string BASICPRC;
        [MarshalAs(UnmanagedType.BStr)]
        public string TIME;
        [MarshalAs(UnmanagedType.BStr)]
        public string TOTAL;
        [MarshalAs(UnmanagedType.BStr)]
        public string DATE;
    }

    [Guid("52389AD5-6624-445A-AC5D-188285799188")]
    public enum LoginLevel
    {
        LoginLevelAccounts = 3,
        LoginLevelMax = 50,
        LoginLevelOptionsStocks = 2,
        LoginLevelPermissions = 1
    }

    [Guid("6F67DB39-BC83-4655-A11B-EB9478FFF6E2")]
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

    [Guid("9690C0EF-2F33-4F1F-A4D2-C6604E0B614D")]
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

    [StructLayout(LayoutKind.Sequential, Pack=4), Guid("D717A432-4447-4506-95AD-992A486B6003")]
    public struct MaofOrderType
    {
        [MarshalAs(UnmanagedType.BStr)]
        public string Branch;
        [MarshalAs(UnmanagedType.BStr)]
        public string Account;
        [MarshalAs(UnmanagedType.BStr)]
        public string Option;
        [MarshalAs(UnmanagedType.BStr)]
        public string operation;
        [MarshalAs(UnmanagedType.BStr)]
        public string ammount;
        [MarshalAs(UnmanagedType.BStr)]
        public string price;
        [MarshalAs(UnmanagedType.BStr)]
        public string Sug_Pkuda;
        [MarshalAs(UnmanagedType.BStr)]
        public string Asmachta;
        [MarshalAs(UnmanagedType.BStr)]
        public string AsmachtaFmr;
        public int Pass;
        public int OrderID;
    }

    [StructLayout(LayoutKind.Sequential, Pack=4), Guid("D717A43A-4447-4506-95AD-992A486B6003")]
    public struct MOFINQType
    {
        [MarshalAs(UnmanagedType.BStr)]
        public string ERR;
        [MarshalAs(UnmanagedType.BStr)]
        public string SYS_TYPE;
        [MarshalAs(UnmanagedType.BStr)]
        public string SND_RCV;
        [MarshalAs(UnmanagedType.BStr)]
        public string ORDR_TYPE;
        [MarshalAs(UnmanagedType.BStr)]
        public string OPR_NAME;
        [MarshalAs(UnmanagedType.BStr)]
        public string SUG_INQ;
        [MarshalAs(UnmanagedType.BStr)]
        public string SEQ_PIC;
        [MarshalAs(UnmanagedType.BStr)]
        public string MANA_PIC;
        [MarshalAs(UnmanagedType.BStr)]
        public string BNO_PIC;
        [MarshalAs(UnmanagedType.BStr)]
        public string BNO_NAME;
        [MarshalAs(UnmanagedType.BStr)]
        public string OP;
        [MarshalAs(UnmanagedType.BStr)]
        public string BRANCH_PIC;
        [MarshalAs(UnmanagedType.BStr)]
        public string TIK_PIC;
        [MarshalAs(UnmanagedType.BStr)]
        public string SUG_MAVR_PIC;
        [MarshalAs(UnmanagedType.BStr)]
        public string ID_MAVR_PIC;
        [MarshalAs(UnmanagedType.BStr)]
        public string SUG_PIC;
        [MarshalAs(UnmanagedType.BStr)]
        public string ID_PIC;
        [MarshalAs(UnmanagedType.BStr)]
        public string ID_NAME;
        [MarshalAs(UnmanagedType.BStr)]
        public string SUG_ID_PIC;
        [MarshalAs(UnmanagedType.BStr)]
        public string NOSTRO;
        [MarshalAs(UnmanagedType.BStr)]
        public string ORDR_NV_PIC;
        [MarshalAs(UnmanagedType.BStr)]
        public string ORDR_SUG;
        [MarshalAs(UnmanagedType.BStr)]
        public string ORDR_PRC_PIC;
        [MarshalAs(UnmanagedType.BStr)]
        public string ORDR_TIME;
        [MarshalAs(UnmanagedType.BStr)]
        public string DIL_NV_PIC;
        [MarshalAs(UnmanagedType.BStr)]
        public string DIL_PRC_PIC;
        [MarshalAs(UnmanagedType.BStr)]
        public string DIL_TIME_PIC;
        [MarshalAs(UnmanagedType.BStr)]
        public string MBR_SEQ_PIC;
        [MarshalAs(UnmanagedType.BStr)]
        public string RZF_SEQ_PIC;
        [MarshalAs(UnmanagedType.BStr)]
        public string RZF_ORD_PIC;
        [MarshalAs(UnmanagedType.BStr)]
        public string ORDER_NO_PIC;
        [MarshalAs(UnmanagedType.BStr)]
        public string DIL_PIC;
        [MarshalAs(UnmanagedType.BStr)]
        public string COD_UPD;
        [MarshalAs(UnmanagedType.BStr)]
        public string STS;
        [MarshalAs(UnmanagedType.BStr)]
        public string ERR_DATA;
        [MarshalAs(UnmanagedType.BStr)]
        public string ERR_INQ;
        [MarshalAs(UnmanagedType.BStr)]
        public string ERR_UPD;
        [MarshalAs(UnmanagedType.BStr)]
        public string SUG_INFO;
        [MarshalAs(UnmanagedType.BStr)]
        public string DSP_FMR;
    }

    [Guid("24508459-0E4C-4550-A037-E7E2BF41BCF9")]
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

    [StructLayout(LayoutKind.Sequential, Pack=4), Guid("171EF2F6-6CF2-46F5-9065-E5591162C7BB")]
    public struct NvPerOption
    {
        public int BNO;
        public int NV;
    }

    [ComImport, CoClass(typeof(OnlineEventsClass)), Guid("622ECF94-4A34-48A3-8B05-C1120ECD7144")]
    public interface OnlineEvents : IOnlineEvents, IOnlineEventsEvents_Event
    {
    }

    [ComImport, TypeLibType((short) 2), Guid("6D63B490-5C90-4F3C-A518-3241F3C833F1"), ClassInterface((short) 0), ComSourceInterfaces("TaskBarLib.IOnlineEventsEvents\0")]
    public class OnlineEventsClass : IOnlineEvents, OnlineEvents, IOnlineEventsEvents_Event
    {
        // Events
        public event IOnlineEventsEvents_OnOnlineKerenEventHandler OnOnlineKeren;

        // Properties
        [DispId(2)]
        public virtual int EventsFilterAccount { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(2)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(2)] set; }
        [DispId(3)]
        public virtual int EventsFilterBranch { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(3)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(3)] set; }
        [DispId(4)]
        public virtual int EventsFilterCustodian { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(4)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(4)] set; }
        [DispId(1)]
        public virtual int EventsFilterKrn { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(1)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(1)] set; }
        [DispId(5)]
        public virtual int EventsFilterKupa { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(5)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(5)] set; }
        [DispId(6)]
        public virtual int EventsFilterRegAccount { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(6)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(6)] set; }
        [DispId(2)]
        public virtual int TaskBarLib.IOnlineEvents.EventsFilterAccount { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(2)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(2)] set; }
        [DispId(3)]
        public virtual int TaskBarLib.IOnlineEvents.EventsFilterBranch { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(3)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(3)] set; }
        [DispId(4)]
        public virtual int TaskBarLib.IOnlineEvents.EventsFilterCustodian { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(4)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(4)] set; }
        [DispId(1)]
        public virtual int TaskBarLib.IOnlineEvents.EventsFilterKrn { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(1)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(1)] set; }
        [DispId(5)]
        public virtual int TaskBarLib.IOnlineEvents.EventsFilterKupa { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(5)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(5)] set; }
        [DispId(6)]
        public virtual int TaskBarLib.IOnlineEvents.EventsFilterRegAccount { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(6)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(6)] set; }
    }

    [StructLayout(LayoutKind.Sequential, Pack=8), Guid("0BF1B137-CBC7-44D4-8D96-3368DFADF981")]
    public struct OnlineSessionBalance
    {
        public int Onl_sug_rc;
        public int Onl_sug;
        public int Onl_id_number;
        [MarshalAs(UnmanagedType.BStr)]
        public string Onl_id_name;
        public int Onl_tik;
        public int Onl_bgt;
        public int Onl_branch;
        public int Onl_bank;
        public int Onl_sug_id;
        [MarshalAs(UnmanagedType.BStr)]
        public string Onl_id_teur;
        public int Onl_mdl;
        [MarshalAs(UnmanagedType.BStr)]
        public string Onl_sivug;
        [MarshalAs(UnmanagedType.BStr)]
        public string Onl_tat_sivug;
        public int Onl_sg_tik_mf;
        public int Onl_id_tik_mf;
        public int Onl_sg_bgt_mf;
        public int Onl_id_bgt_mf;
        public double Onl_yld_id_yr;
        public double Onl_yld_id_mon;
        public int Onl_bno;
        public int Onl_sug_bno;
        public int Onl_sug_cur;
        public int Onl_bs_bno;
        public int Onl_bno_bank;
        [MarshalAs(UnmanagedType.BStr)]
        public string Onl_symbol_nam;
        [MarshalAs(UnmanagedType.BStr)]
        public string Onl_isin;
        [MarshalAs(UnmanagedType.BStr)]
        public string Onl_bno_name;
        [MarshalAs(UnmanagedType.BStr)]
        public string Onl_bno_teur;
        public int Onl_mdr;
        public int Onl_manpik;
        public int Onl_deriv_code;
        public int Onl_sort;
        public int Onl_sug_pr;
        public double Onl_prc;
        public double Onl_pr_matach;
        [MarshalAs(UnmanagedType.BStr)]
        public string Onl_flag_mth;
        public double Onl_hon_rashum;
        public int Onl_invst_plcy;
        public int Onl_asset_cod;
        public int Onl_sector;
        public double Onl_mult_prc;
        public int Onl_main_cur;
        public int Onl_sub_cur;
        public int Onl_rating;
        [MarshalAs(UnmanagedType.BStr)]
        public string Onl_lvl1;
        [MarshalAs(UnmanagedType.BStr)]
        public string Onl_lvl2;
        [MarshalAs(UnmanagedType.BStr)]
        public string Onl_lvl3;
        [MarshalAs(UnmanagedType.BStr)]
        public string Onl_lvl4;
        [MarshalAs(UnmanagedType.BStr)]
        public string Onl_lvl5;
        [MarshalAs(UnmanagedType.BStr)]
        public string Onl_lvl6;
        [MarshalAs(UnmanagedType.BStr)]
        public string Onl_dirug_ag;
        [MarshalAs(UnmanagedType.BStr)]
        public string Onl_sug_opt;
        public double Onl_exp_prc;
        [MarshalAs(UnmanagedType.BStr)]
        public string Onl_exp_dat;
        public double Onl_std_dev;
        public double Onl_intrst;
        public double Onl_delta;
        public int Onl_opt_basis;
        public int Onl_sum_hoze;
        public int Onl_mislaka;
        [MarshalAs(UnmanagedType.BStr)]
        public string Onl_cntr_rlt;
        public int Onl_siduri;
        public int Onl_mbr_dst;
        public int Onl_anaf;
        public double Onl_prc_basis;
        public double Onl_beta;
        public double Onl_machpil;
        public double Onl_amlza_pcnt;
        public double Onl_value_mdd;
        public int Onl_cod_mdd;
        public double Onl_com_pcnt;
        public double Onl_com_sum;
        public double Onl_nv_bkr;
        public double Onl_cost_bkr;
        public double Onl_fifo_bkr;
        public double Onl_cost_mth;
        public double Onl_vl_bkr;
        public double Onl_nv_ashala;
        public double Onl_nv_tzl_bkr;
        public double Onl_vl_tzl_bkr;
        public double Onl_mrgn_bkr;
        public double Onl_tkb_sm_bkr;
        public double Onl_tkb_nv_bkr;
        public double Onl_tkb_vl_bkr;
        public double Onl_pcnt_bkr;
        public double Onl_yld_bn_yr;
        public double Onl_yld_bn_mon;
        public double Onl_nv_ord_by;
        public double Onl_pr_ord_by;
        public double Onl_pr_m_ord_b;
        public double Onl_vl_ord_by;
        public double Onl_nv_dil_by;
        public double Onl_pr_dil_by;
        public double Onl_pr_m_dil_b;
        public double Onl_vl_dil_by;
        public double Onl_vl_def_by;
        public double Onl_nv_ord_sl;
        public double Onl_pr_ord_sl;
        public double Onl_pr_m_ord_s;
        public double Onl_vl_ord_sl;
        public double Onl_nv_dil_sl;
        public double Onl_pr_dil_sl;
        public double Onl_pr_m_dil_s;
        public double Onl_vl_dil_sl;
        public double Onl_vl_def_sl;
        public double Onl_nv_hf;
        public double Onl_prc_hf;
        public double Onl_pr_mth_hf;
        public double Onl_vl_hf;
        public double Onl_nv_msh;
        public double Onl_prc_msh;
        public double Onl_pr_mth_msh;
        public double Onl_vl_msh;
        public double Onl_vl_itzirot;
        public double Onl_vl_pedion;
        public double Onl_pr_mth_cls;
        public double Onl_vl_cls;
        public double Onl_prc_cls;
        public double Onl_prc_cls_d;
        public double Onl_prc_bs_bno;
        public double Onl_tkb_sm_cls;
        public double Onl_tkb_nv_cls;
        public double Onl_tkb_vl_cls;
        public double Onl_nv_ash_cls;
        public double Onl_val_hasifa;
        public double Onl_req_mrgn;
        public double Onl_tot_val;
        public double Onl_tot_nv;
        public double Onl_factor;
        public double Onl_stt_prof;
        public double Onl_real_prof;
        [MarshalAs(UnmanagedType.BStr)]
        public string Onl_clnt_bkr;
        [MarshalAs(UnmanagedType.BStr)]
        public string Onl_clnt_onl;
        [MarshalAs(UnmanagedType.BStr)]
        public string Onl_id_pail;
        [MarshalAs(UnmanagedType.BStr)]
        public string Onl_rec_40;
        [MarshalAs(UnmanagedType.BStr)]
        public string Onl_frgn_bno;
        [MarshalAs(UnmanagedType.BStr)]
        public string Onl_bno_duali;
        public int Onl_upd_date;
        public double Onl_duration;
        public int Onl_upd_time;
        [MarshalAs(UnmanagedType.BStr)]
        public string Onl_eng_name;
        public double Onl_yld_ytm;
        public double Onl_dur_kalkali;
        public int Onl_stk_type;
        [MarshalAs(UnmanagedType.BStr)]
        public string Onl_fil;
        public int Onl_global_counter;
    }

    [Guid("C325EA95-9848-4FD6-9F6F-8284437C0CB8")]
    public enum OnlineSessionType
    {
        OnlineSessionTypeAccounts,
        OnlineSessionTypeAll,
        OnlineSessionTypeCustodian,
        OnlineSessionTypeKranot,
        OnlineSessionTypeKupa
    }

    [Guid("7DC7B7BF-4671-4171-A633-0D697A5F6803")]
    public enum OrderOperation
    {
        OrderOperationNewBuy,
        OrderOperationNewSell,
        OrderOperationUpdBuy,
        OrderOperationUpdSell,
        OrderOperationDelete
    }

    [Guid("A71C84F7-600D-4869-88F9-E14ED5FFB0CA")]
    public enum OrdersErrorTypes
    {
        Alert = 0x34,
        Confirmation = 0x31,
        Fatal = 0x30,
        NoError = 0x35,
        PasswordReq = 0x33,
        ReEnter = 50
    }

    [StructLayout(LayoutKind.Sequential, Pack=8), Guid("9FAA45D6-068B-4378-9537-BA102D832532")]
    public struct PeleSecurityType
    {
        public int BaseAsset;
        public double Margin;
        public double Security;
        public double dailyPremium;
        public double OrdersPremium;
        public int Pass;
    }

    [ComImport, CoClass(typeof(PermissionsClass)), Guid("24508459-7E53-43F2-B7AD-199DAFFE5163")]
    public interface Permissions : IPermissions
    {
    }

    [ComImport, ClassInterface((short) 0), Guid("24508459-475B-4DAC-BE6E-328D24844520"), TypeLibType((short) 2)]
    public class PermissionsClass : IPermissions, Permissions
    {
        // Methods
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(2)]
        public virtual extern int GetUserApp([In] int sessionId, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_BSTR)] out Array psaStrRecords, [In, Optional, DefaultParameterValue(""), MarshalAs(UnmanagedType.BStr)] string AppName);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(1)]
        public virtual extern int GetUserPermissions([In] int sessionId, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] out Array psaStrRecords, [In, Optional, DefaultParameterValue(0)] int ReloadPermissions);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(3)]
        public virtual extern int RefreshUserPermissions([In] int sessionId);
    }

    [Guid("F38FA8FA-B65A-4C97-BEA9-030CDD126924")]
    public enum QueryType
    {
        qtDetailed = 0x30,
        qtSummary = 0x31
    }

    [StructLayout(LayoutKind.Sequential, Pack=4), Guid("D717A435-4447-4506-95AD-992A486B6003")]
    public struct RezefBasicOrder
    {
        [MarshalAs(UnmanagedType.BStr)]
        public string operation;
        [MarshalAs(UnmanagedType.BStr)]
        public string asmachta_fmr;
        [MarshalAs(UnmanagedType.BStr)]
        public string ammount;
        [MarshalAs(UnmanagedType.BStr)]
        public string price;
        [MarshalAs(UnmanagedType.BStr)]
        public string Stock_Number;
        [MarshalAs(UnmanagedType.BStr)]
        public string OP;
        [MarshalAs(UnmanagedType.BStr)]
        public string Branch;
        [MarshalAs(UnmanagedType.BStr)]
        public string Account;
        [MarshalAs(UnmanagedType.BStr)]
        public string order_type;
        [MarshalAs(UnmanagedType.BStr)]
        public string asmachta_rezef;
        [MarshalAs(UnmanagedType.BStr)]
        public string price_percent;
        [MarshalAs(UnmanagedType.BStr)]
        public string shlav;
        [MarshalAs(UnmanagedType.BStr)]
        public string Nv_Del;
        [MarshalAs(UnmanagedType.BStr)]
        public string ORDR_TYPE;
        [MarshalAs(UnmanagedType.BStr)]
        public string Mana;
        [MarshalAs(UnmanagedType.BStr)]
        public string Zira;
        [MarshalAs(UnmanagedType.BStr)]
        public string Nv_Min;
        [MarshalAs(UnmanagedType.BStr)]
        public string Strat_Date;
        [MarshalAs(UnmanagedType.BStr)]
        public string end_date;
    }

    [StructLayout(LayoutKind.Sequential, Pack=8), Guid("67BADCB9-DD55-41EF-8D57-3C9FD785C9A1")]
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

    [Guid("E2A5E70B-3138-484A-A113-B8BB1640ED12")]
    public enum RezefOrderKind
    {
        RezefOrderKindLMT,
        RezefOrderKindMKT,
        RezefOrderKindATC,
        RezefOrderKindLMO,
        RezefOrderKindKRN,
        RezefOrderKindJMB
    }

    [StructLayout(LayoutKind.Sequential, Pack=8), Guid("768A62AC-3D61-41F7-A439-DA65581AFB52")]
    public struct RezefSimpleOrder
    {
        public int BNO;
        public int Amount;
        public double price;
        public int Branch;
        public int Account;
        public OrderOperation operation;
        public RezefOrderKind OrderKind;
        public int Query;
    }

    [StructLayout(LayoutKind.Sequential, Pack=4), Guid("85B84A22-9EB6-4C76-9E96-561AB3D527D7")]
    public struct RmOnlineRecordType
    {
        [MarshalAs(UnmanagedType.BStr)]
        public string sBnoName;
        [MarshalAs(UnmanagedType.BStr)]
        public string sBnoNum;
        [MarshalAs(UnmanagedType.BStr)]
        public string sSugBno;
        [MarshalAs(UnmanagedType.BStr)]
        public string sBasePrice;
        [MarshalAs(UnmanagedType.BStr)]
        public string sBaseNV;
        [MarshalAs(UnmanagedType.BStr)]
        public string sBaseVL;
        [MarshalAs(UnmanagedType.BStr)]
        public string sLastPrice;
        [MarshalAs(UnmanagedType.BStr)]
        public string sLastNV;
        [MarshalAs(UnmanagedType.BStr)]
        public string sLastVL;
        [MarshalAs(UnmanagedType.BStr)]
        public string sVLChange;
        [MarshalAs(UnmanagedType.BStr)]
        public string sMoneyMoved;
        [MarshalAs(UnmanagedType.BStr)]
        public string sProfitLoss;
        [MarshalAs(UnmanagedType.BStr)]
        public string sTotalCost;
        [MarshalAs(UnmanagedType.BStr)]
        public string sCngCost;
        [MarshalAs(UnmanagedType.BStr)]
        public string sAverageCost;
        [MarshalAs(UnmanagedType.BStr)]
        public string sCngCostPcnt;
        [MarshalAs(UnmanagedType.BStr)]
        public string LVL1;
        [MarshalAs(UnmanagedType.BStr)]
        public string LVL2;
        [MarshalAs(UnmanagedType.BStr)]
        public string LVL3;
        [MarshalAs(UnmanagedType.BStr)]
        public string LVL4;
        [MarshalAs(UnmanagedType.BStr)]
        public string LVL5;
        [MarshalAs(UnmanagedType.BStr)]
        public string LVL6;
    }

    [StructLayout(LayoutKind.Sequential, Pack=4), Guid("73D20C57-FA21-4BFE-B71B-39A4E1F11900")]
    public struct RmOnlineTotalInfoType
    {
        [MarshalAs(UnmanagedType.BStr)]
        public string sBaseCash;
        [MarshalAs(UnmanagedType.BStr)]
        public string sLastCash;
        [MarshalAs(UnmanagedType.BStr)]
        public string sCashChange;
        [MarshalAs(UnmanagedType.BStr)]
        public string sBaseVL;
        [MarshalAs(UnmanagedType.BStr)]
        public string sLastVL;
        [MarshalAs(UnmanagedType.BStr)]
        public string sProfitLoss;
        [MarshalAs(UnmanagedType.BStr)]
        public string sDate;
        [MarshalAs(UnmanagedType.BStr)]
        public string sCngCost;
    }

    [StructLayout(LayoutKind.Sequential, Pack=4), Guid("D717A43C-4447-4506-95AD-992A486B6003")]
    public struct RMType
    {
        [MarshalAs(UnmanagedType.BStr)]
        public string Sug;
        [MarshalAs(UnmanagedType.BStr)]
        public string ID;
        [MarshalAs(UnmanagedType.BStr)]
        public string BNO;
        [MarshalAs(UnmanagedType.BStr)]
        public string SUG_RC;
        [MarshalAs(UnmanagedType.BStr)]
        public string Tik;
        [MarshalAs(UnmanagedType.BStr)]
        public string Branch;
        [MarshalAs(UnmanagedType.BStr)]
        public string ID_NAME;
        [MarshalAs(UnmanagedType.BStr)]
        public string BS_BNO;
        [MarshalAs(UnmanagedType.BStr)]
        public string SYMBOL_NAM;
        [MarshalAs(UnmanagedType.BStr)]
        public string BNO_NAME;
        [MarshalAs(UnmanagedType.BStr)]
        public string SUG_ID;
        [MarshalAs(UnmanagedType.BStr)]
        public string SUG_BNO;
        [MarshalAs(UnmanagedType.BStr)]
        public string LAST_OP;
        [MarshalAs(UnmanagedType.BStr)]
        public string LAST_DT;
        [MarshalAs(UnmanagedType.BStr)]
        public string STRT_DT;
        [MarshalAs(UnmanagedType.BStr)]
        public string END_DT;
        [MarshalAs(UnmanagedType.BStr)]
        public string AV_TERM;
        [MarshalAs(UnmanagedType.BStr)]
        public string MDR;
        [MarshalAs(UnmanagedType.BStr)]
        public string MANPIK;
        [MarshalAs(UnmanagedType.BStr)]
        public string MNG_REP;
        [MarshalAs(UnmanagedType.BStr)]
        public string DERIV_COD;
        [MarshalAs(UnmanagedType.BStr)]
        public string SORT;
        [MarshalAs(UnmanagedType.BStr)]
        public string SH;
        [MarshalAs(UnmanagedType.BStr)]
        public string SUG_PR;
        [MarshalAs(UnmanagedType.BStr)]
        public string PRC;
        [MarshalAs(UnmanagedType.BStr)]
        public string PRC_CHNG;
        [MarshalAs(UnmanagedType.BStr)]
        public string SUG_CUR;
        [MarshalAs(UnmanagedType.BStr)]
        public string PR_MATACH;
        [MarshalAs(UnmanagedType.BStr)]
        public string HON_RASHUM;
        [MarshalAs(UnmanagedType.BStr)]
        public string NV;
        [MarshalAs(UnmanagedType.BStr)]
        public string COST;
        [MarshalAs(UnmanagedType.BStr)]
        public string VL;
        [MarshalAs(UnmanagedType.BStr)]
        public string EXT_MARGIN;
        [MarshalAs(UnmanagedType.BStr)]
        public string REQ_MARGIN;
        [MarshalAs(UnmanagedType.BStr)]
        public string SUM1;
        [MarshalAs(UnmanagedType.BStr)]
        public string SUM2;
        [MarshalAs(UnmanagedType.BStr)]
        public string ID_PCNT;
        [MarshalAs(UnmanagedType.BStr)]
        public string TK_PCNT;
        [MarshalAs(UnmanagedType.BStr)]
        public string LVL1;
        [MarshalAs(UnmanagedType.BStr)]
        public string LVL2;
        [MarshalAs(UnmanagedType.BStr)]
        public string LVL3;
        [MarshalAs(UnmanagedType.BStr)]
        public string LVL4;
        [MarshalAs(UnmanagedType.BStr)]
        public string LVL5;
        [MarshalAs(UnmanagedType.BStr)]
        public string LVL6;
        [MarshalAs(UnmanagedType.BStr)]
        public string MANG_COD;
        [MarshalAs(UnmanagedType.BStr)]
        public string BANK;
        [MarshalAs(UnmanagedType.BStr)]
        public string BNK_BRANCH;
        [MarshalAs(UnmanagedType.BStr)]
        public string ID_BANK;
        [MarshalAs(UnmanagedType.BStr)]
        public string MDL;
        [MarshalAs(UnmanagedType.BStr)]
        public string BLC;
        [MarshalAs(UnmanagedType.BStr)]
        public string BLC_NEG;
        [MarshalAs(UnmanagedType.BStr)]
        public string BGT;
        [MarshalAs(UnmanagedType.BStr)]
        public string SIVUG;
        [MarshalAs(UnmanagedType.BStr)]
        public string INTRST_COD;
        [MarshalAs(UnmanagedType.BStr)]
        public string PAY_MNG;
        [MarshalAs(UnmanagedType.BStr)]
        public string FIL;
    }

    [StructLayout(LayoutKind.Sequential, Pack=4), Guid("D717A43B-4447-4506-95AD-992A486B6003")]
    public struct RZFINQType
    {
        [MarshalAs(UnmanagedType.BStr)]
        public string ERR;
        [MarshalAs(UnmanagedType.BStr)]
        public string SYS_TYPE;
        [MarshalAs(UnmanagedType.BStr)]
        public string SND_RCV;
        [MarshalAs(UnmanagedType.BStr)]
        public string ORDR_TYPE;
        [MarshalAs(UnmanagedType.BStr)]
        public string OPR_NAME;
        [MarshalAs(UnmanagedType.BStr)]
        public string SUG_INQ;
        [MarshalAs(UnmanagedType.BStr)]
        public string SEQ_N;
        [MarshalAs(UnmanagedType.BStr)]
        public string MANA_N;
        [MarshalAs(UnmanagedType.BStr)]
        public string BNO_N;
        [MarshalAs(UnmanagedType.BStr)]
        public string BNO_NAME;
        [MarshalAs(UnmanagedType.BStr)]
        public string OP;
        [MarshalAs(UnmanagedType.BStr)]
        public string BRANCH_N;
        [MarshalAs(UnmanagedType.BStr)]
        public string TIK_N;
        [MarshalAs(UnmanagedType.BStr)]
        public string SUG_MAVR_N;
        [MarshalAs(UnmanagedType.BStr)]
        public string ID_MAVR_N;
        [MarshalAs(UnmanagedType.BStr)]
        public string SUG_N;
        [MarshalAs(UnmanagedType.BStr)]
        public string ID_N;
        [MarshalAs(UnmanagedType.BStr)]
        public string ID_NAME;
        [MarshalAs(UnmanagedType.BStr)]
        public string SUG_ID_N;
        [MarshalAs(UnmanagedType.BStr)]
        public string NOSTRO;
        [MarshalAs(UnmanagedType.BStr)]
        public string ORDR_NV_N;
        [MarshalAs(UnmanagedType.BStr)]
        public string ORDR_SUG;
        [MarshalAs(UnmanagedType.BStr)]
        public string ORDR_PRC_N;
        [MarshalAs(UnmanagedType.BStr)]
        public string ORDR_TIME;
        [MarshalAs(UnmanagedType.BStr)]
        public string DIL_NV_N;
        [MarshalAs(UnmanagedType.BStr)]
        public string DIL_PRC_N;
        [MarshalAs(UnmanagedType.BStr)]
        public string DIL_TIME_N;
        [MarshalAs(UnmanagedType.BStr)]
        public string MBR_SEQ_N;
        [MarshalAs(UnmanagedType.BStr)]
        public string RZF_SEQ_N;
        [MarshalAs(UnmanagedType.BStr)]
        public string RZF_ORD_N;
        [MarshalAs(UnmanagedType.BStr)]
        public string ORDER_NO_N;
        [MarshalAs(UnmanagedType.BStr)]
        public string DIL_N;
        [MarshalAs(UnmanagedType.BStr)]
        public string COD_UPD;
        [MarshalAs(UnmanagedType.BStr)]
        public string STS;
        [MarshalAs(UnmanagedType.BStr)]
        public string ERR_DATA;
        [MarshalAs(UnmanagedType.BStr)]
        public string ERR_INQ;
        [MarshalAs(UnmanagedType.BStr)]
        public string ERR_UPD;
        [MarshalAs(UnmanagedType.BStr)]
        public string SUG_INFO;
        [MarshalAs(UnmanagedType.BStr)]
        public string MSG1;
        [MarshalAs(UnmanagedType.BStr)]
        public string STS_NAME;
        [MarshalAs(UnmanagedType.BStr)]
        public string DSP_FMR;
    }

    [Guid("24508459-07E6-4884-BEBC-7B958EBE4D3E")]
    public enum SecurityCalcType
    {
        RMOnly,
        RM_Dill,
        RM_Dill_Orders,
        RM_Dill_ShortOrders
    }

    [StructLayout(LayoutKind.Sequential, Pack=8), Guid("3A1EFDB9-CB6F-4856-8917-4A0AE2F88472")]
    public struct SH161Type
    {
        public int BNO;
        [MarshalAs(UnmanagedType.BStr)]
        public string BNO_NAME;
        public double PRC;
        public double HON_RASHUM;
        public double PCNT;
        public int MIN_NV;
        public double BNO_IN_MDD;
        public double PUBLIC_PRCNT;
        public double NV_TZAFA;
    }

    [StructLayout(LayoutKind.Sequential, Pack=4), Guid("24508459-4437-4506-95AD-992A486B6003")]
    public struct SH500Type
    {
        [MarshalAs(UnmanagedType.BStr)]
        public string TRD_DAT;
        [MarshalAs(UnmanagedType.BStr)]
        public string BNO_NO;
        [MarshalAs(UnmanagedType.BStr)]
        public string BNO_NAME;
        [MarshalAs(UnmanagedType.BStr)]
        public string Symbol;
        [MarshalAs(UnmanagedType.BStr)]
        public string SUG_BNO;
        [MarshalAs(UnmanagedType.BStr)]
        public string ANAF;
        [MarshalAs(UnmanagedType.BStr)]
        public string TRADE_METH;
        [MarshalAs(UnmanagedType.BStr)]
        public string DAY_WEEK;
        [MarshalAs(UnmanagedType.BStr)]
        public string HAFSAKA;
        [MarshalAs(UnmanagedType.BStr)]
        public string STR_TM_PRE;
        [MarshalAs(UnmanagedType.BStr)]
        public string STR_TM_OPN;
        [MarshalAs(UnmanagedType.BStr)]
        public string STR_TM_CLS;
        [MarshalAs(UnmanagedType.BStr)]
        public string END_TM;
        [MarshalAs(UnmanagedType.BStr)]
        public string UNIT_OPEN;
        [MarshalAs(UnmanagedType.BStr)]
        public string UNIT_CONT;
        [MarshalAs(UnmanagedType.BStr)]
        public string UNIT_CLOS;
        [MarshalAs(UnmanagedType.BStr)]
        public string LMT_UP_OPN;
        [MarshalAs(UnmanagedType.BStr)]
        public string LMT_DN_OPN;
        [MarshalAs(UnmanagedType.BStr)]
        public string LMT_UP_CNT;
        [MarshalAs(UnmanagedType.BStr)]
        public string LMT_DN_CNT;
        [MarshalAs(UnmanagedType.BStr)]
        public string NV_LMT_ERR;
        [MarshalAs(UnmanagedType.BStr)]
        public string NV_LMT_FTL;
        [MarshalAs(UnmanagedType.BStr)]
        public string MDR;
        [MarshalAs(UnmanagedType.BStr)]
        public string SUG_MANPIK;
        [MarshalAs(UnmanagedType.BStr)]
        public string MANPIK_NUM;
        [MarshalAs(UnmanagedType.BStr)]
        public string HON_RASHUM;
        [MarshalAs(UnmanagedType.BStr)]
        public string SORT_NUM;
        [MarshalAs(UnmanagedType.BStr)]
        public string TAX_COD;
        [MarshalAs(UnmanagedType.BStr)]
        public string TAX_PCNT;
        [MarshalAs(UnmanagedType.BStr)]
        public string MISTNM_COD;
        [MarshalAs(UnmanagedType.BStr)]
        public string MAOF_COD;
        [MarshalAs(UnmanagedType.BStr)]
        public string LVL1;
        [MarshalAs(UnmanagedType.BStr)]
        public string LVL2;
        [MarshalAs(UnmanagedType.BStr)]
        public string LVL3;
        [MarshalAs(UnmanagedType.BStr)]
        public string LVL4;
        [MarshalAs(UnmanagedType.BStr)]
        public string LVL5;
        [MarshalAs(UnmanagedType.BStr)]
        public string LVL6;
        [MarshalAs(UnmanagedType.BStr)]
        public string ENG_NAME;
        [MarshalAs(UnmanagedType.BStr)]
        public string RATING_MRG;
        [MarshalAs(UnmanagedType.BStr)]
        public string RATING_CRT;
        [MarshalAs(UnmanagedType.BStr)]
        public string RISK_COD;
        [MarshalAs(UnmanagedType.BStr)]
        public string DIRUG_AG;
        [MarshalAs(UnmanagedType.BStr)]
        public string TRAD_ORDR;
        [MarshalAs(UnmanagedType.BStr)]
        public string BASIS_PRC;
        [MarshalAs(UnmanagedType.BStr)]
        public string BASIS_YLD;
        [MarshalAs(UnmanagedType.BStr)]
        public string MDD_MAOF;
        [MarshalAs(UnmanagedType.BStr)]
        public string MDD_TLV100;
        [MarshalAs(UnmanagedType.BStr)]
        public string UNIT_CONT_8;
        [MarshalAs(UnmanagedType.BStr)]
        public string RDM_DAYS;
        [MarshalAs(UnmanagedType.BStr)]
        public string MDD_TLV75;
        [MarshalAs(UnmanagedType.BStr)]
        public string MDD_TL_TK;
        [MarshalAs(UnmanagedType.BStr)]
        public string MDD_BANKS;
        [MarshalAs(UnmanagedType.BStr)]
        public string COD_RISHUM;
        [MarshalAs(UnmanagedType.BStr)]
        public string MDD_ITR30;
        [MarshalAs(UnmanagedType.BStr)]
        public string MDD_FNNC;
        [MarshalAs(UnmanagedType.BStr)]
        public string MDD_NDLN15;
        [MarshalAs(UnmanagedType.BStr)]
        public string MDD_BINUI;
    }

    [StructLayout(LayoutKind.Sequential, Pack=4), Guid("24508459-A714-4AE7-BF5A-3CDE03B93BD4")]
    public struct ShacharBondInFutureType
    {
        [MarshalAs(UnmanagedType.BStr)]
        public string BNO_Future;
        [MarshalAs(UnmanagedType.BStr)]
        public string BNO_Bond;
        [MarshalAs(UnmanagedType.BStr)]
        public string ConvCoeff;
        [MarshalAs(UnmanagedType.BStr)]
        public string AccumInterest;
    }

    [StructLayout(LayoutKind.Sequential, Pack=4), Guid("1D4DFB4D-5F79-4CDA-B7BE-7DDC4622540C")]
    public struct ShortAccountType
    {
        [MarshalAs(UnmanagedType.BStr)]
        public string Tik;
        [MarshalAs(UnmanagedType.BStr)]
        public string Branch;
        [MarshalAs(UnmanagedType.BStr)]
        public string Sug;
        [MarshalAs(UnmanagedType.BStr)]
        public string ID;
        [MarshalAs(UnmanagedType.BStr)]
        public string AccountName;
        [MarshalAs(UnmanagedType.BStr)]
        public string SortAccountName;
        [MarshalAs(UnmanagedType.BStr)]
        public string MaofCode;
        [MarshalAs(UnmanagedType.BStr)]
        public string FutureCode;
        [MarshalAs(UnmanagedType.BStr)]
        public string BnoCode;
        [MarshalAs(UnmanagedType.BStr)]
        public string CashCode;
        [MarshalAs(UnmanagedType.BStr)]
        public string CompanyEmployee;
        [MarshalAs(UnmanagedType.BStr)]
        public string SIVUG;
        [MarshalAs(UnmanagedType.BStr)]
        public string MefazelCode;
        [MarshalAs(UnmanagedType.BStr)]
        public string SugId;
        [MarshalAs(UnmanagedType.BStr)]
        public string Model;
        [MarshalAs(UnmanagedType.BStr)]
        public string SubModel;
        [MarshalAs(UnmanagedType.BStr)]
        public string PermittedStockPercent;
        [MarshalAs(UnmanagedType.BStr)]
        public string YearlyStd;
        [MarshalAs(UnmanagedType.BStr)]
        public string WeightedStocks;
        [MarshalAs(UnmanagedType.BStr)]
        public string InternetPswd;
        [MarshalAs(UnmanagedType.BStr)]
        public string BnoCredit;
        [MarshalAs(UnmanagedType.BStr)]
        public string StocksCommission;
        [MarshalAs(UnmanagedType.BStr)]
        public string BondsCommission;
        [MarshalAs(UnmanagedType.BStr)]
        public string MakamCommission;
        [MarshalAs(UnmanagedType.BStr)]
        public string FundsCommission;
        [MarshalAs(UnmanagedType.BStr)]
        public string BLC;
        [MarshalAs(UnmanagedType.BStr)]
        public string BalanceGrp;
        [MarshalAs(UnmanagedType.BStr)]
        public string BudgetSeif;
        [MarshalAs(UnmanagedType.BStr)]
        public string MinMaofCommission;
        [MarshalAs(UnmanagedType.BStr)]
        public string TatSivug;
        [MarshalAs(UnmanagedType.BStr)]
        public string OpeningDate;
        [MarshalAs(UnmanagedType.BStr)]
        public string AccountMaavar;
        [MarshalAs(UnmanagedType.BStr)]
        public string TelephoneCode;
    }

    [StructLayout(LayoutKind.Sequential, Pack=4), Guid("1D4DFB4E-5F79-4CDA-B7BE-7DDC4622540C")]
    public struct ShortStockType
    {
        [MarshalAs(UnmanagedType.BStr)]
        public string Stock;
        [MarshalAs(UnmanagedType.BStr)]
        public string Name_Stock;
        [MarshalAs(UnmanagedType.BStr)]
        public string Sug;
        [MarshalAs(UnmanagedType.BStr)]
        public string Name2;
        [MarshalAs(UnmanagedType.BStr)]
        public string Symbol;
        [MarshalAs(UnmanagedType.BStr)]
        public string Isin;
    }

    [Guid("24508459-71DF-41F8-9D02-30D6F692FD9D")]
    public enum StockKind
    {
        StockKindAgach = 1,
        StockKindAll = -1,
        StockKindKeren = 3,
        StockKindMakam = 2,
        StockKindMenaya = 0
    }

    [StructLayout(LayoutKind.Sequential, Pack=4), Guid("94914BF5-A714-4AE7-BF5A-3CDE03B93BD4")]
    public struct StockPartInIndexType
    {
        [MarshalAs(UnmanagedType.BStr)]
        public string BNO_NO;
        [MarshalAs(UnmanagedType.BStr)]
        public string BNO_NAME;
        [MarshalAs(UnmanagedType.BStr)]
        public string PRC;
        [MarshalAs(UnmanagedType.BStr)]
        public string HON_RASHUM;
        [MarshalAs(UnmanagedType.BStr)]
        public string PCNT;
    }

    [StructLayout(LayoutKind.Sequential, Pack=4), Guid("D717A433-4447-4506-95AD-992A486B6003")]
    public struct StockStageType
    {
        [MarshalAs(UnmanagedType.BStr)]
        public string BNO_NO;
        [MarshalAs(UnmanagedType.BStr)]
        public string TRADE_METH;
        [MarshalAs(UnmanagedType.BStr)]
        public string TRADE_STAGE;
        [MarshalAs(UnmanagedType.BStr)]
        public string STOCK_STATE;
        [MarshalAs(UnmanagedType.BStr)]
        public string RSN_TRADE_STP;
        [MarshalAs(UnmanagedType.BStr)]
        public string TM_TRADE_STP;
        [MarshalAs(UnmanagedType.BStr)]
        public string BASE_RATE_STG;
    }

    [ComImport, Guid("14F44D18-F88D-4260-B843-22084A594345"), CoClass(typeof(TradeOptionsClass))]
    public interface TradeOptions : ITradeOptions
    {
    }

    [ComImport, TypeLibType((short) 2), ClassInterface((short) 0), Guid("F19F7DC8-5195-4FEC-A074-54EC95756D02")]
    public class TradeOptionsClass : ITradeOptions, TradeOptions
    {
        // Methods
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(1)]
        public virtual extern int BlackAndScholesEx([In] int CallType, [In] int nOptionsToExcersize, [In] int nOptionsToBuy, [In] double dIV, [In] double dDividend, [In] double dInterest, [In] double dAssetPrice, [In] double dStrikePrice, [In] int nDaysToExpire, out BSDerivativesType Derivatives);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(3)]
        public virtual extern int GetOptionByBaseAsset([In] BaseAssetTypes BaseAsset, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] out Array records, [In, Optional, DefaultParameterValue(-1)] MonthType Month);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(2)]
        public virtual extern int GetOptionByBno([In] int BNO, out ExtOptRecord record);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(4)]
        public virtual extern int GetOptionByStrike([In] double dStrike, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] out Array records, [In, Optional, DefaultParameterValue(-1)] BaseAssetTypes BaseAsset, [In, Optional, DefaultParameterValue(-1)] MonthType Month);
    }

    [StructLayout(LayoutKind.Sequential, Pack=8), Guid("ED565CC0-E9FA-4C49-A63C-744C780D0088")]
    public struct TradeOptionType
    {
        public int BNO;
        public double ExpPrice;
        [MarshalAs(UnmanagedType.BStr)]
        public string ExpDate;
        public int BaseAsset;
        [MarshalAs(UnmanagedType.BStr)]
        public string OpType;
        [MarshalAs(UnmanagedType.BStr)]
        public string BnoName;
    }

    [Guid("A48CD25E-4E11-4AAB-AC9B-222323E9C900")]
    public enum TradeType
    {
        ALLTradeType = -1,
        MF = 0,
        RZ = 1
    }

    [ComImport, CoClass(typeof(UserClass)), Guid("1F6A77E6-EAFE-47ED-9DE5-6E96C67E632F")]
    public interface User : IUser
    {
    }

    [ComImport, TypeLibType((short) 2), ClassInterface((short) 0), Guid("8ECA9C7F-387B-4D6B-9189-DB3D3656E511")]
    public class UserClass : IUser, User
    {
        // Methods
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(30)]
        public virtual extern int CalculateSecurities([In] int sessionId, [In] ref MaofOrderType Order, [In, MarshalAs(UnmanagedType.BStr)] string Account, [In, MarshalAs(UnmanagedType.BStr)] string Branch, [In, MarshalAs(UnmanagedType.BStr)] string Tik, [In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array psaAssets);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x13)]
        public virtual extern void Decrypt([In, MarshalAs(UnmanagedType.BStr)] string EncStr, [In, MarshalAs(UnmanagedType.BStr)] string EncKey, [MarshalAs(UnmanagedType.BStr)] out string OrgStr);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x12)]
        public virtual extern void Encrypt([In, MarshalAs(UnmanagedType.BStr)] string OrgStr, [In, MarshalAs(UnmanagedType.BStr)] string EncKey, [MarshalAs(UnmanagedType.BStr)] out string EncStr);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(13)]
        public virtual extern int GetAccounts([In] int sessionId, [In, MarshalAs(UnmanagedType.BStr)] string strQuery, [In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array psaStrRecords, [In, Out, MarshalAs(UnmanagedType.BStr)] ref string ErrMsg);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x3d)]
        public virtual extern int GetAccountYieldByRequirement([In] int sessionId, [In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array psaStrRecords, [In] int Branch, [In] int Account, [In] int StartDate, [In] YieldDataType DataType, [In, Optional, DefaultParameterValue(0)] int AccountYieldOnly);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x3e)]
        public virtual extern int GetAccountYieldDetailed([In] int sessionId, [In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array psaStrRecords, [In] int Branch, [In] int Account, [In] int StartDate, [In] int EndDate, [In, Optional, DefaultParameterValue(0)] int AccountYieldOnly);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(60)]
        public virtual extern int GetAccountYieldInitial([In] int sessionId, [In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array psaStrRecords, [In, Optional, DefaultParameterValue(0)] int Branch, [In, Optional, DefaultParameterValue(0)] int Account, [In, Optional, DefaultParameterValue(0)] int AccountYieldOnly);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(7)]
        public virtual extern int GetHoldings([In] int sessionId, [In, MarshalAs(UnmanagedType.BStr)] string strTik, [In, MarshalAs(UnmanagedType.BStr)] string strAccount, [In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array vecStrRecords, [In, Optional, DefaultParameterValue("000"), MarshalAs(UnmanagedType.BStr)] string strBranch, [In, Optional, DefaultParameterValue("00000000"), MarshalAs(UnmanagedType.BStr)] string strStock);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(6)]
        public virtual extern int GetHoldingsEx([In] int sessionId, [In, MarshalAs(UnmanagedType.BStr)] string strTik, [In, MarshalAs(UnmanagedType.BStr)] string strAccount, [In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array vecStrRecords, [In] TradeType type, [In, Optional, DefaultParameterValue("000"), MarshalAs(UnmanagedType.BStr)] string strBranch, [In, Optional, DefaultParameterValue("00000000"), MarshalAs(UnmanagedType.BStr)] string strStock, [In, Optional, DefaultParameterValue("000000"), MarshalAs(UnmanagedType.BStr)] string strMefazel);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x3f)]
        public virtual extern int GetKranotAccounts([In] int sessionId, [In, MarshalAs(UnmanagedType.BStr)] string strQuery, [In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array psaStrRecords, [In, Out, MarshalAs(UnmanagedType.BStr)] ref string ErrMsg);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x22)]
        public virtual extern void GetLoginActivity(ref int sessionId, out int percent, [MarshalAs(UnmanagedType.BStr)] out string description);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x2f)]
        public virtual extern int GetMaofCnt([In] int sessionId, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] out Array vecK300, out int vecK300Len, [In, Out, MarshalAs(UnmanagedType.BStr)] ref string K300LastTime, [In, MarshalAs(UnmanagedType.BStr)] string K300Bno, [In] BaseAssetTypes strMadad, [In] MonthType Month, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] out Array vecOrders, out int vecOrdersLen, [In, MarshalAs(UnmanagedType.BStr)] string Account, [In, MarshalAs(UnmanagedType.BStr)] string Branch, [In, Out, MarshalAs(UnmanagedType.BStr)] ref string LastTime);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x1b)]
        public virtual extern int GetMargin([In] int sessionId, [In, MarshalAs(UnmanagedType.BStr)] string Account, [In, MarshalAs(UnmanagedType.BStr)] string Branch, [In] BaseAssetTypes asset, out double Margin);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x16)]
        public virtual extern int GetOnlineRm([In] int sessionId, [In, Out] ref RmOnlineTotalInfoType RmOnlineTotalInfo, [In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array psaStrRecords, [In, MarshalAs(UnmanagedType.BStr)] string bstrBranch, [In, MarshalAs(UnmanagedType.BStr)] string bstrAccount, [In, MarshalAs(UnmanagedType.BStr)] string bstrStockNumber);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x40)]
        public virtual extern int GetOnlineRmEx([In] int sessionId, [In] OnlineSessionType type, [In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array psaStrRecords, [In, Optional, DefaultParameterValue(0)] int Branch, [In, Optional, DefaultParameterValue(0)] int Account, [In, Out, Optional, DefaultParameterValue(0)] ref int LastCounter);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x45)]
        public virtual extern int GetOnlineRmFromMaofSession([In] int sessionId, [In, MarshalAs(UnmanagedType.BStr)] string Account, [In, MarshalAs(UnmanagedType.BStr)] string Branch, [In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array psaStrRecords);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x39)]
        public virtual extern int GetOnlineSessionBalance([In] int sessionId, [In] OnlineSessionType type, [In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array psaStrRecords, [In, Optional, DefaultParameterValue(0)] int Branch, [In, Optional, DefaultParameterValue(0)] int Account, [In, Out, Optional, DefaultParameterValue(0)] ref int LastCounter);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(5)]
        public virtual extern int GetOrdersMaof([In] int sessionId, [In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array vecStrRecords, [In, Out, MarshalAs(UnmanagedType.BStr)] ref string strLastTime, [In, Optional, DefaultParameterValue(0)] QueryType qType, [In, Optional, DefaultParameterValue("000000"), MarshalAs(UnmanagedType.BStr)] string strAccount, [In, Optional, DefaultParameterValue("000"), MarshalAs(UnmanagedType.BStr)] string strBranch, [In, Optional, DefaultParameterValue("00000000"), MarshalAs(UnmanagedType.BStr)] string strOptionNumber);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x24)]
        public virtual extern int GetOrdersMF([In] int sessionId, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] out Array vecRecords, [In, MarshalAs(UnmanagedType.BStr)] string Account, [In, MarshalAs(UnmanagedType.BStr)] string Branch, [In, Out, Optional, DefaultParameterValue("0"), MarshalAs(UnmanagedType.BStr)] ref string LastTime);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(4)]
        public virtual extern int GetOrdersRezef([In] int sessionId, [In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array psaStrRecords, [In, Out, Optional, DefaultParameterValue("00000000"), MarshalAs(UnmanagedType.BStr)] ref string strLastTime, [In, Optional, DefaultParameterValue(0)] QueryType QueryType, [In, Optional, DefaultParameterValue("000000"), MarshalAs(UnmanagedType.BStr)] string strAccount, [In, Optional, DefaultParameterValue("000"), MarshalAs(UnmanagedType.BStr)] string strBranch, [In, Optional, DefaultParameterValue("00000000"), MarshalAs(UnmanagedType.BStr)] string strStockNumber);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x27)]
        public virtual extern int GetOrdersRZ([In] int sessionId, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] out Array vecRecords, [In, MarshalAs(UnmanagedType.BStr)] string Account, [In, MarshalAs(UnmanagedType.BStr)] string Branch, [In, Out, Optional, DefaultParameterValue("0"), MarshalAs(UnmanagedType.BStr)] ref string LastTime);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x1d)]
        public virtual extern int GetPremium([In] int sessionId, [In, MarshalAs(UnmanagedType.BStr)] string Account, [In, MarshalAs(UnmanagedType.BStr)] string Branch, [In] BaseAssetTypes asset, out double premia);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x30)]
        public virtual extern int GetRezefCnt([In] int sessionId, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] out Array vecK300, out int vecK300Len, [In, Out, MarshalAs(UnmanagedType.BStr)] ref string K300LastTime, [In, MarshalAs(UnmanagedType.BStr)] string K300Bno, [In] BaseAssetTypes strMadad, [In] MonthType Month, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] out Array vecOrders, out int vecOrdersLen, [In, MarshalAs(UnmanagedType.BStr)] string Account, [In, MarshalAs(UnmanagedType.BStr)] string Branch, [In, Out, MarshalAs(UnmanagedType.BStr)] ref string LastTime);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x11)]
        public virtual extern int GetShortAccounts([In] int sessionId, [In, MarshalAs(UnmanagedType.BStr)] string strQuery, [In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array psaStrRecords, [In, Out, MarshalAs(UnmanagedType.BStr)] ref string ErrMsg);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(40)]
        public virtual extern int GetUserApp([In] int sessionId, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_BSTR)] out Array psaStrRecords, [In, Optional, DefaultParameterValue(""), MarshalAs(UnmanagedType.BStr)] string AppName);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(14)]
        public virtual extern int GetUserPermissions([In] int sessionId, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] out Array psaStrRecords);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(1)]
        public virtual extern int Login([In, MarshalAs(UnmanagedType.BStr)] string username, [In, MarshalAs(UnmanagedType.BStr)] string AS400Password, [In, MarshalAs(UnmanagedType.BStr)] string AppPassword, [MarshalAs(UnmanagedType.BStr)] out string message, out int sessionId);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(2)]
        public virtual extern int LoginAndChangePassword([In, MarshalAs(UnmanagedType.BStr)] string username, [In, MarshalAs(UnmanagedType.BStr)] string oldAS400Password, [In, MarshalAs(UnmanagedType.BStr)] string newAS400Password, [In, MarshalAs(UnmanagedType.BStr)] string AppPassword, [MarshalAs(UnmanagedType.BStr)] out string message, out int sessionId);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x41)]
        public virtual extern int LoginByLevel([In, MarshalAs(UnmanagedType.BStr)] string username, [In, MarshalAs(UnmanagedType.BStr)] string AS400Password, [In, MarshalAs(UnmanagedType.BStr)] string AppPassword, [In, MarshalAs(UnmanagedType.BStr)] string sysName, [In] int customer, [In] LoginLevel level, [MarshalAs(UnmanagedType.BStr)] out string message, out int sessionId);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x42)]
        public virtual extern int LoginByLevelAndChangePassword([In, MarshalAs(UnmanagedType.BStr)] string username, [In, MarshalAs(UnmanagedType.BStr)] string oldAS400Password, [In, MarshalAs(UnmanagedType.BStr)] string newAS400Password, [In, MarshalAs(UnmanagedType.BStr)] string AppPassword, [In, MarshalAs(UnmanagedType.BStr)] string sysName, [In] int customer, [In] LoginLevel req_level, [MarshalAs(UnmanagedType.BStr)] out string message, out int sessionId);
        [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x44)]
        public virtual extern string LoginErrorDescByLevel([In] ref int sessionId, [In] LoginLevel req_level);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x43)]
        public virtual extern LoginStatus LoginStateByLevel([In] ref int sessionId, [In] LoginLevel req_level);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x31)]
        public virtual extern int Logout([In] int sessionId);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x1f)]
        public virtual extern int MaofOrderParams([In] int sessionId, [In, MarshalAs(UnmanagedType.BStr)] string Account, [In, MarshalAs(UnmanagedType.BStr)] string Branch, [In] BaseAssetTypes asset, out double Madad, out double emzaimNezilim, out double nezilimBoker, out double regularMargin, out double peleMargin, out double regularSecurities, out double PeleSecurities, out double OrdersPremium, out double executionPermium);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(50)]
        public virtual extern int MaofSecuritiesParams([In] int sessionId, [In, MarshalAs(UnmanagedType.BStr)] string Account, [In, MarshalAs(UnmanagedType.BStr)] string Branch, [In] BaseAssetTypes asset, out double Madad, out double emzaimNezilim, out double nezilimBoker, out double regularMargin, out double peleMargin, out double regularSecurities, out double PeleSecurities, out double OrdersPremium, out double executionPermium, out double regularSecuritiesNoQoutesKizuz, out double peleSecuritiesNoQoutesKizuz, out double worstScenarioMadad, out int worstScenarioIndex);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x25)]
        public virtual extern int OrdersStreamStart([In] int sessionId, [In, MarshalAs(UnmanagedType.BStr)] string Account, [In, MarshalAs(UnmanagedType.BStr)] string Branch, [In] TradeType streamType);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x26)]
        public virtual extern int OrdersStreamStop([In] int sessionId, [In, MarshalAs(UnmanagedType.BStr)] string Account, [In, MarshalAs(UnmanagedType.BStr)] string Branch, [In] TradeType streamType);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x1a)]
        public virtual extern int PeleSecurities([In] int sessionId, [In, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array Orders, [In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array PeleSec, [In, MarshalAs(UnmanagedType.BStr)] string Account, [In, MarshalAs(UnmanagedType.BStr)] string Branch, [In, MarshalAs(UnmanagedType.BStr)] string Tik);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x3a)]
        public virtual extern int SendContOrderRZ([In] int sessionId, [In, Out] ref RezefContinuousOrder Order, [In, Out] ref int AsmachtaFmr, [In] int AsmachtaRezef, [MarshalAs(UnmanagedType.BStr)] out string VBMsg, out int ErrNO, out OrdersErrorTypes ErrorType, [In, Out] ref int OrderID, [In, MarshalAs(UnmanagedType.BStr)] string AuthUserName, [In, MarshalAs(UnmanagedType.BStr)] string AuthPassword, [In, MarshalAs(UnmanagedType.BStr)] string ReEnteredValue);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x33)]
        public virtual extern int SendMaofAsynchSpeedOrder([In] int sessionId, [In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array Orders, out int ErrNO, [In, Optional, DefaultParameterValue(0)] int SPCOrder);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(20)]
        public virtual extern int SendMaofOrder([In] int sessionId, [In, Out] ref MaofOrderType Order, [In, Out, MarshalAs(UnmanagedType.BStr)] ref string VBMsg, [In, Out] ref int ErrNO, out OrdersErrorTypes ErrorType, [In, Out] ref int OrderID, [In, MarshalAs(UnmanagedType.BStr)] string AuthUserName, [In, MarshalAs(UnmanagedType.BStr)] string AuthPassword, [In, MarshalAs(UnmanagedType.BStr)] string ReEnteredValue, [In, Optional, DefaultParameterValue(0)] int SPCOrder);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x15)]
        public virtual extern int SendMaofOrderPele([In] int sessionId, [In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array Orders, [In, Out, MarshalAs(UnmanagedType.BStr)] ref string VBMsg, [In, Out] ref int ErrNO, out OrdersErrorTypes ErrorType, [In, Out] ref int OrderID, [In, MarshalAs(UnmanagedType.BStr)] string AuthUserName, [In, MarshalAs(UnmanagedType.BStr)] string AuthPassword, [In, MarshalAs(UnmanagedType.BStr)] string ReEnteredValue, [In, Optional, DefaultParameterValue(0)] int SPCOrder);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x20)]
        public virtual extern int SendMaofSpeedOrderPele([In] int sessionId, [In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array Orders, out int ErrNO, [In, Optional, DefaultParameterValue(0)] int SPCOrder);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x36)]
        public virtual extern int SendOrderRZ([In] int sessionId, [In] ref RezefSimpleOrder Order, [In, Out] ref int AsmachtaFmr, [In] int AsmachtaRezef, [MarshalAs(UnmanagedType.BStr)] out string VBMsg, out int ErrNO, out OrdersErrorTypes ErrorType, [In, Out] ref int OrderID, [In, MarshalAs(UnmanagedType.BStr)] string AuthUserName, [In, MarshalAs(UnmanagedType.BStr)] string AuthPassword, [In, MarshalAs(UnmanagedType.BStr)] string ReEnteredValue);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x3b)]
        public virtual extern int SendOrderSpeedRZ([In] int sessionId, [In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array Orders, [In, MarshalAs(UnmanagedType.BStr)] string Location, [In, MarshalAs(UnmanagedType.BStr)] string Trade_Type, [In, Out] ref int ErrNO);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x10)]
        public virtual extern int SendRezefOrder([In] int sessionId, [In, Out] ref RezefBasicOrder Order, [In, MarshalAs(UnmanagedType.BStr)] string Location, [In, MarshalAs(UnmanagedType.BStr)] string Trade_Type, [In, Out, MarshalAs(UnmanagedType.BStr)] ref string VBMsg, [In, Out] ref int ErrNO, out OrdersErrorTypes ErrorType, [In, Out] ref int OrderID, [In, MarshalAs(UnmanagedType.BStr)] string AuthUserName, [In, MarshalAs(UnmanagedType.BStr)] string AuthPassword, [In, MarshalAs(UnmanagedType.BStr)] string ReEnteredValue);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x1c)]
        public virtual extern int SendRezefOrderSpeed([In] int sessionId, [In, Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_RECORD)] ref Array Orders, [In, MarshalAs(UnmanagedType.BStr)] string Location, [In, MarshalAs(UnmanagedType.BStr)] string Trade_Type);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x17)]
        public virtual extern int StartMaofSession([In] int sessionId, [In, MarshalAs(UnmanagedType.BStr)] string Account, [In, MarshalAs(UnmanagedType.BStr)] string Branch, [In] SecurityCalcType calcType);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x37)]
        public virtual extern int StartOnlineUserSession([In] int sessionId, [In] OnlineSessionType type);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x34)]
        public virtual extern int StartRezefSession([In] int sessionId, [In, MarshalAs(UnmanagedType.BStr)] string Account, [In, MarshalAs(UnmanagedType.BStr)] string Branch);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x18)]
        public virtual extern int StopMaofSession([In] int sessionId, [In, MarshalAs(UnmanagedType.BStr)] string Account, [In, MarshalAs(UnmanagedType.BStr)] string Branch);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x38)]
        public virtual extern int StopOnlineUserSession([In] int sessionId, [In] OnlineSessionType type);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x35)]
        public virtual extern int StopRezefSession([In] int sessionId, [In, MarshalAs(UnmanagedType.BStr)] string Account, [In, MarshalAs(UnmanagedType.BStr)] string Branch);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(15)]
        public virtual extern int UpdateUserPassword([In] int sessionId, [In, MarshalAs(UnmanagedType.BStr)] string strOldPassword, [In, MarshalAs(UnmanagedType.BStr)] string strNewPassword, [In, MarshalAs(UnmanagedType.BStr)] string strUserNumber);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x2e)]
        public virtual extern int UserAuthentication([In] int sessionId, [In, MarshalAs(UnmanagedType.BStr)] string Password);

        // Properties
        [DispId(11)]
        public virtual string Cust { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(11)] get; }
        [DispId(0x2c)]
        public virtual string this[ref int sessionId] { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x2c)] get; }
        [DispId(0x29)]
        public virtual string this[ref int sessionId] { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x29)] get; }
        [DispId(0x2d)]
        public virtual string this[ref int sessionId] { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x2d)] get; }
        [DispId(0x21)]
        public virtual LoginStatus this[ref int sessionId] { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x21)] get; }
        [DispId(0x19)]
        public virtual int this[string Account, string Branch] { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x19)] get; }
        [DispId(0x2a)]
        public virtual int this[ref int sessionId] { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x2a)] get; }
        [DispId(8)]
        public virtual ConnectionState State { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(8)] get; }
        [DispId(12)]
        public virtual string System { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(12)] get; }
        [DispId(0x2b)]
        public virtual string SystemName { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x2b)] get; }
        [DispId(11)]
        public virtual string TaskBarLib.IUser.Cust { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(11)] get; }
        [DispId(0x2c)]
        public virtual string this[ref int sessionId] { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x2c)] get; }
        [DispId(0x29)]
        public virtual string this[ref int sessionId] { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x29)] get; }
        [DispId(0x2d)]
        public virtual string this[ref int sessionId] { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x2d)] get; }
        [DispId(0x21)]
        public virtual LoginStatus this[ref int sessionId] { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x21)] get; }
        [DispId(0x19)]
        public virtual int this[string Account, string Branch] { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x19)] get; }
        [DispId(0x2a)]
        public virtual int this[ref int sessionId] { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x2a)] get; }
        [DispId(8)]
        public virtual ConnectionState TaskBarLib.IUser.State { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(8)] get; }
        [DispId(12)]
        public virtual string TaskBarLib.IUser.System { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(12)] get; }
        [DispId(0x2b)]
        public virtual string TaskBarLib.IUser.SystemName { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x2b)] get; }
        [DispId(9)]
        public virtual string this[int sessionId] { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(9)] get; }
        [DispId(9)]
        public virtual string this[int sessionId] { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(9)] get; }
    }

    [StructLayout(LayoutKind.Sequential, Pack=4), Guid("D717A436-4447-4506-95AD-992A486B6003")]
    public struct UserPasswordType
    {
        [MarshalAs(UnmanagedType.BStr)]
        public string SUG_RC;
        [MarshalAs(UnmanagedType.BStr)]
        public string USR_NAME;
        [MarshalAs(UnmanagedType.BStr)]
        public string USR_PW;
        [MarshalAs(UnmanagedType.BStr)]
        public string Branch;
        [MarshalAs(UnmanagedType.BStr)]
        public string Sug;
        [MarshalAs(UnmanagedType.BStr)]
        public string MFZL_ID;
        [MarshalAs(UnmanagedType.BStr)]
        public string TIK_N;
        [MarshalAs(UnmanagedType.BStr)]
        public string ID_N;
        [MarshalAs(UnmanagedType.BStr)]
        public string APPL_N;
        [MarshalAs(UnmanagedType.BStr)]
        public string DEVICE_N;
        [MarshalAs(UnmanagedType.BStr)]
        public string AUTH_LVL;
        [MarshalAs(UnmanagedType.BStr)]
        public string FIELD;
        [MarshalAs(UnmanagedType.BStr)]
        public string OPERATOR;
        [MarshalAs(UnmanagedType.BStr)]
        public string val;
        [MarshalAs(UnmanagedType.BStr)]
        public string UPD_DAT;
        [MarshalAs(UnmanagedType.BStr)]
        public string END_DAT;
        [MarshalAs(UnmanagedType.BStr)]
        public string UPD_USR_N;
        [MarshalAs(UnmanagedType.BStr)]
        public string FROM_USR_N;
        [MarshalAs(UnmanagedType.BStr)]
        public string FIL;
    }

    [Guid("33548B33-F643-43C0-AD93-C87F23D06E15")]
    public enum YieldDataType
    {
        YieldData5YearsbyYear = 4,
        YieldDataMonthbyDay = 1,
        YieldDataTwelveMonths = 3,
        YieldDataYearbyMonth = 2,
        YieldDataYearbyQuater = 5
    }
}

 
 

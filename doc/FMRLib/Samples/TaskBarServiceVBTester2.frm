VERSION 5.00
Begin VB.Form FMRLibTester 
   Caption         =   "TBTester"
   ClientHeight    =   9315
   ClientLeft      =   2160
   ClientTop       =   975
   ClientWidth     =   11280
   Icon            =   "TaskBarServiceVBTester2.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   9315
   ScaleWidth      =   11280
   Begin VB.TextBox SendRBranch 
      Height          =   285
      Left            =   1680
      TabIndex        =   34
      Text            =   "000"
      Top             =   960
      Width           =   855
   End
   Begin VB.TextBox txt_rezef_asmachta_rezef 
      Height          =   285
      Left            =   1680
      TabIndex        =   41
      Top             =   3720
      Width           =   855
   End
   Begin VB.TextBox txt_rezef_asmachta_fmr 
      Height          =   285
      Left            =   1680
      TabIndex        =   40
      Top             =   3285
      Width           =   855
   End
   Begin VB.CommandButton Send_Order 
      Caption         =   "Send"
      Height          =   375
      Left            =   480
      TabIndex        =   43
      Top             =   4560
      Width           =   735
   End
   Begin VB.ComboBox Order_Type 
      Height          =   315
      ItemData        =   "TaskBarServiceVBTester2.frx":1272
      Left            =   1680
      List            =   "TaskBarServiceVBTester2.frx":1274
      Style           =   2  'Dropdown List
      TabIndex        =   42
      Top             =   4080
      Width           =   855
   End
   Begin VB.TextBox Operation 
      Height          =   285
      Left            =   1680
      TabIndex        =   38
      Text            =   "N"
      Top             =   2520
      Width           =   855
   End
   Begin VB.TextBox Limit 
      Height          =   285
      Left            =   1680
      TabIndex        =   37
      Text            =   "2029"
      Top             =   2160
      Width           =   855
   End
   Begin VB.TextBox Amount 
      Height          =   285
      Left            =   1680
      TabIndex        =   36
      Text            =   "2000000"
      Top             =   1680
      Width           =   855
   End
   Begin VB.TextBox Stock_Number 
      Height          =   285
      Left            =   1680
      TabIndex        =   35
      Text            =   "01234335"
      Top             =   1320
      Width           =   855
   End
   Begin VB.Timer tmrCntRezef 
      Enabled         =   0   'False
      Interval        =   1
      Left            =   4080
      Top             =   9360
   End
   Begin VB.Timer tmrCntMaof 
      Enabled         =   0   'False
      Interval        =   1
      Left            =   3600
      Top             =   9360
   End
   Begin VB.CommandButton btnDirectDlv 
      Caption         =   "DirectDlv"
      Height          =   495
      Left            =   240
      TabIndex        =   53
      Top             =   8280
      Width           =   1215
   End
   Begin VB.CommandButton btnRzfSpeed 
      Caption         =   "Speed"
      Height          =   375
      Left            =   1920
      TabIndex        =   44
      Top             =   4560
      Width           =   735
   End
   Begin VB.ListBox lst_full 
      BeginProperty Font 
         Name            =   "Arial"
         Size            =   8.25
         Charset         =   177
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   3000
      ItemData        =   "TaskBarServiceVBTester2.frx":1276
      Left            =   3120
      List            =   "TaskBarServiceVBTester2.frx":1278
      TabIndex        =   0
      Top             =   360
      Width           =   6255
   End
   Begin VB.TextBox StatusBar 
      Alignment       =   2  'Center
      BackColor       =   &H80000000&
      Height          =   285
      Left            =   0
      TabIndex        =   52
      Top             =   9000
      Width           =   15375
   End
   Begin VB.ComboBox Combo_Madad 
      Height          =   315
      ItemData        =   "TaskBarServiceVBTester2.frx":127A
      Left            =   9480
      List            =   "TaskBarServiceVBTester2.frx":127C
      Style           =   2  'Dropdown List
      TabIndex        =   51
      Top             =   8280
      Width           =   1695
   End
   Begin VB.TextBox txt_rezef_cod_upd 
      Height          =   285
      Left            =   1680
      TabIndex        =   39
      Text            =   "B"
      Top             =   2880
      Width           =   855
   End
   Begin VB.Frame Frame1 
      Caption         =   "User Info"
      Height          =   1455
      Left            =   240
      TabIndex        =   45
      Top             =   6480
      Width           =   2535
      Begin VB.Label lblSessionIdNum 
         Height          =   255
         Left            =   1440
         TabIndex        =   55
         Top             =   720
         Width           =   975
      End
      Begin VB.Label lblSessionId 
         Caption         =   "Session Id"
         Height          =   255
         Left            =   240
         TabIndex        =   54
         Top             =   720
         Width           =   975
      End
      Begin VB.Label lbl_system_val 
         Height          =   255
         Left            =   960
         TabIndex        =   49
         Top             =   1080
         Width           =   1455
      End
      Begin VB.Label lbl_system 
         Caption         =   "System:"
         Height          =   255
         Left            =   240
         TabIndex        =   48
         Top             =   1080
         Width           =   615
      End
      Begin VB.Label lbl_user_name_val 
         Height          =   255
         Left            =   1320
         TabIndex        =   47
         Top             =   360
         Width           =   975
      End
      Begin VB.Label lbl_usr_name 
         Caption         =   "User Name:"
         Height          =   255
         Left            =   240
         TabIndex        =   46
         Top             =   360
         Width           =   855
      End
   End
   Begin VB.TextBox txt_Ord_Account 
      Height          =   285
      Left            =   1680
      TabIndex        =   33
      Text            =   "652859"
      Top             =   600
      Width           =   855
   End
   Begin VB.Frame Frm_Enc 
      Caption         =   "Encryption"
      Height          =   1215
      Left            =   240
      TabIndex        =   28
      Top             =   5160
      Width           =   2535
      Begin VB.TextBox txt_key 
         Height          =   285
         Left            =   600
         TabIndex        =   32
         Top             =   720
         Width           =   1815
      End
      Begin VB.TextBox txt_orgtxt 
         Height          =   285
         Left            =   600
         TabIndex        =   30
         Top             =   360
         Width           =   1815
      End
      Begin VB.Label lbl_key 
         Caption         =   "Key:"
         Height          =   255
         Left            =   120
         TabIndex        =   31
         Top             =   720
         Width           =   495
      End
      Begin VB.Label lbl_string 
         Caption         =   "Text:"
         Height          =   255
         Left            =   120
         TabIndex        =   29
         Top             =   360
         Width           =   495
      End
   End
   Begin VB.Timer RezefCNT_Timer1 
      Enabled         =   0   'False
      Interval        =   1
      Left            =   3120
      Top             =   9360
   End
   Begin VB.Timer MaofCNT_Timer1 
      Enabled         =   0   'False
      Interval        =   1
      Left            =   2640
      Top             =   9360
   End
   Begin VB.TextBox txt_query 
      Height          =   285
      Left            =   7320
      TabIndex        =   26
      Top             =   8640
      Width           =   3855
   End
   Begin VB.TextBox txt_tik 
      Height          =   285
      Left            =   10200
      TabIndex        =   24
      Text            =   "000"
      Top             =   7920
      Width           =   375
   End
   Begin VB.Timer OrdersRezef_Timer1 
      Enabled         =   0   'False
      Interval        =   1
      Left            =   2160
      Top             =   9360
   End
   Begin VB.Timer OrdersMaof_Timer1 
      Enabled         =   0   'False
      Interval        =   1
      Left            =   1680
      Top             =   9360
   End
   Begin VB.TextBox txt_account 
      Height          =   285
      Left            =   7320
      TabIndex        =   20
      Text            =   "53254"
      Top             =   7920
      Width           =   615
   End
   Begin VB.TextBox txt_branch 
      Height          =   285
      Left            =   9000
      TabIndex        =   19
      Text            =   "061"
      Top             =   7920
      Width           =   375
   End
   Begin VB.TextBox txt_option 
      Height          =   285
      Left            =   7320
      TabIndex        =   18
      Text            =   "00000000"
      Top             =   8280
      Width           =   855
   End
   Begin VB.Timer Rezef_Timer1 
      Enabled         =   0   'False
      Interval        =   1
      Left            =   1200
      Top             =   9360
   End
   Begin VB.TextBox txt_active_stream 
      Height          =   285
      Left            =   8520
      TabIndex        =   17
      Text            =   "0"
      Top             =   7200
      Width           =   975
   End
   Begin VB.Timer Maof_Timer1 
      Enabled         =   0   'False
      Interval        =   1
      Left            =   720
      Top             =   9360
   End
   Begin VB.TextBox txt_last_time 
      Height          =   285
      Left            =   4680
      TabIndex        =   15
      Text            =   "0"
      Top             =   8640
      Width           =   1335
   End
   Begin VB.TextBox txt_delta_list_record_number 
      Height          =   285
      Left            =   4680
      TabIndex        =   13
      Text            =   "0"
      Top             =   7560
      Width           =   975
   End
   Begin VB.TextBox txt_full_list_record_number 
      Height          =   285
      Left            =   4680
      TabIndex        =   11
      Text            =   "0"
      Top             =   7200
      Width           =   975
   End
   Begin VB.TextBox txt_pushed_iterations_value 
      Height          =   285
      Left            =   4680
      TabIndex        =   7
      Text            =   "0"
      Top             =   8280
      Width           =   735
   End
   Begin VB.TextBox txt_pulled_iterations_value 
      Height          =   285
      Left            =   4680
      TabIndex        =   5
      Text            =   "0"
      Top             =   7920
      Width           =   735
   End
   Begin VB.ListBox lst_delta 
      BeginProperty Font 
         Name            =   "Arial"
         Size            =   8.25
         Charset         =   177
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   3210
      Left            =   3120
      TabIndex        =   1
      Top             =   3720
      Width           =   6255
   End
   Begin VB.Label LabelSendR 
      Caption         =   "Branch"
      Height          =   255
      Left            =   480
      TabIndex        =   65
      Top             =   960
      Width           =   735
   End
   Begin VB.Label Label4 
      Caption         =   "Asmachta RZF"
      Height          =   255
      Left            =   480
      TabIndex        =   64
      Top             =   3675
      Width           =   1215
   End
   Begin VB.Label Label3 
      Caption         =   "Asmachta fmr"
      Height          =   255
      Left            =   480
      TabIndex        =   63
      Top             =   3285
      Width           =   975
   End
   Begin VB.Label lbl_rezef_operation 
      Caption         =   "Operation"
      Height          =   255
      Index           =   7
      Left            =   480
      TabIndex        =   62
      Top             =   2490
      Width           =   735
   End
   Begin VB.Label lbl_Account_Num 
      Caption         =   "Account"
      Height          =   255
      Left            =   480
      TabIndex        =   61
      Top             =   600
      Width           =   1095
   End
   Begin VB.Label Label2 
      Caption         =   "Type"
      Height          =   255
      Index           =   4
      Left            =   480
      TabIndex        =   60
      Top             =   4080
      Width           =   735
   End
   Begin VB.Label lbl_rezef_cod_upd 
      Caption         =   "Cod_Upd"
      Height          =   255
      Index           =   3
      Left            =   480
      TabIndex        =   59
      Top             =   2880
      Width           =   735
   End
   Begin VB.Label Label2 
      Caption         =   "Limit"
      Height          =   255
      Index           =   2
      Left            =   480
      TabIndex        =   58
      Top             =   2085
      Width           =   735
   End
   Begin VB.Label Label2 
      Caption         =   "Amount"
      Height          =   255
      Index           =   1
      Left            =   480
      TabIndex        =   57
      Top             =   1695
      Width           =   735
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      Caption         =   "Stock Number"
      Height          =   195
      Index           =   0
      Left            =   480
      TabIndex        =   56
      Top             =   1350
      Width           =   1020
   End
   Begin VB.Label lbl_madad 
      Caption         =   "Madad:"
      Height          =   255
      Left            =   8640
      TabIndex        =   50
      Top             =   8280
      Width           =   735
   End
   Begin VB.Label lbl_query 
      Caption         =   "Query"
      Height          =   255
      Left            =   6480
      TabIndex        =   27
      Top             =   8640
      Width           =   735
   End
   Begin VB.Label lbl_tik 
      Caption         =   "Tik"
      Height          =   255
      Left            =   9720
      TabIndex        =   25
      Top             =   7920
      Width           =   375
   End
   Begin VB.Label lbl_account 
      Caption         =   "Account:"
      Height          =   255
      Left            =   6480
      TabIndex        =   23
      Top             =   7920
      Width           =   975
   End
   Begin VB.Label lbl_branch 
      Caption         =   "Branch"
      Height          =   255
      Left            =   8400
      TabIndex        =   22
      Top             =   7920
      Width           =   975
   End
   Begin VB.Label lbl_option 
      Caption         =   "Option"
      Height          =   255
      Left            =   6480
      TabIndex        =   21
      Top             =   8280
      Width           =   975
   End
   Begin VB.Label lbl_active_stream 
      Caption         =   "Active Streams ID:"
      Height          =   255
      Left            =   6480
      TabIndex        =   16
      Top             =   7200
      Width           =   1935
   End
   Begin VB.Label lbl_last_time 
      Caption         =   "Last Time Received:"
      Height          =   255
      Left            =   3120
      TabIndex        =   14
      Top             =   8640
      Width           =   2175
   End
   Begin VB.Label lbl_delta_list_record_number 
      Caption         =   "Delta List Records:"
      Height          =   255
      Left            =   3120
      TabIndex        =   12
      Top             =   7560
      Width           =   2055
   End
   Begin VB.Label lbl_full_list_record_number 
      Caption         =   "Full List Records:"
      Height          =   255
      Left            =   3120
      TabIndex        =   10
      Top             =   7200
      Width           =   1935
   End
   Begin VB.Label lbl_delta_list_header 
      Caption         =   "Delta List"
      BeginProperty Font 
         Name            =   "Arial"
         Size            =   12
         Charset         =   177
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   9480
      TabIndex        =   9
      Top             =   6600
      Width           =   1095
   End
   Begin VB.Label lbl_full_list_header 
      Caption         =   "Full/Basic List"
      BeginProperty Font 
         Name            =   "Arial"
         Size            =   12
         Charset         =   177
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   9480
      TabIndex        =   8
      Top             =   3120
      Width           =   1695
   End
   Begin VB.Label lbl_pushed_iterations 
      Caption         =   "Pushed Iterations:"
      Height          =   255
      Left            =   3120
      TabIndex        =   6
      Top             =   8280
      Width           =   1935
   End
   Begin VB.Label lbl_pulled_iterations 
      Caption         =   "Pulled Iterations:"
      Height          =   255
      Left            =   3120
      TabIndex        =   4
      Top             =   7920
      Width           =   1815
   End
   Begin VB.Label txt_status_ind 
      BackColor       =   &H008080FF&
      BorderStyle     =   1  'Fixed Single
      Caption         =   "OFF"
      Height          =   255
      Left            =   8520
      TabIndex        =   3
      Top             =   7560
      Width           =   975
   End
   Begin VB.Label Label1 
      Caption         =   "Status:"
      Height          =   255
      Left            =   6480
      TabIndex        =   2
      Top             =   7560
      Width           =   735
   End
   Begin VB.Menu K300 
      Caption         =   "K300"
      Index           =   1
      NegotiatePosition=   1  'Left
      Begin VB.Menu Maof 
         Caption         =   "Maof"
         Index           =   1
         Begin VB.Menu GetK300MF 
            Caption         =   "שליפת נתוני שוק"
         End
         Begin VB.Menu mnuK300FileCacheMaof 
            Caption         =   "שמירת אירועי שוק לקובץ"
         End
         Begin VB.Menu GetMaofScenarios 
            Caption         =   "GetMaofScenarios"
         End
         Begin VB.Menu BlackAndScholes 
            Caption         =   "Black and Scholes"
         End
      End
      Begin VB.Menu Rezef 
         Caption         =   "Rezef"
         Begin VB.Menu GetK300RZ 
            Caption         =   "שליפת נתוני שוק"
         End
      End
      Begin VB.Menu mnuIndices 
         Caption         =   "Indices"
      End
   End
   Begin VB.Menu User 
      Caption         =   "User"
      Begin VB.Menu mnuUserLogin 
         Caption         =   "Login"
      End
      Begin VB.Menu mnuUserLogout 
         Caption         =   "Logout"
      End
      Begin VB.Menu mnuReloadPermssions 
         Caption         =   "טען הרשאות מחדש"
      End
      Begin VB.Menu OrdersMaof 
         Caption         =   "הוראות מעוף"
         Begin VB.Menu GetOrdersMF 
            Caption         =   "רענון הוראות שוטף"
         End
      End
      Begin VB.Menu OrdersRezef 
         Caption         =   "הוראות רצף"
         Begin VB.Menu OrdersRezefDetails 
            Caption         =   "מידע פרטני להוראות"
         End
         Begin VB.Menu GetOrdersRZ 
            Caption         =   "רענון הוראות שוטף"
         End
         Begin VB.Menu SendNewRezefOrder 
            Caption         =   "הוראות רצף"
         End
      End
   End
   Begin VB.Menu ShortAccounts 
      Caption         =   "Short Accounts"
      Begin VB.Menu ShortAccountsGetAll 
         Caption         =   "Get ShortAccounts (Query)"
      End
   End
   Begin VB.Menu ShortStocks 
      Caption         =   "Short Stocks"
      Begin VB.Menu ShortStocksGetSingle 
         Caption         =   "Get Single"
      End
      Begin VB.Menu ShortStocksGetAll 
         Caption         =   "Get All"
      End
   End
   Begin VB.Menu SH 
      Caption         =   "Static Info"
      Begin VB.Menu Indexes 
         Caption         =   "מדדים"
      End
      Begin VB.Menu IndexStructure 
         Caption         =   "מבנה מדדים"
      End
      Begin VB.Menu mnuSh161 
         Caption         =   "סוג חומר 161"
      End
      Begin VB.Menu mnu_GetMadadHistory 
         Caption         =   "הסטוריית מדדים"
      End
      Begin VB.Menu mnuMaofScenarios 
         Caption         =   "תרחישי מעוף"
      End
      Begin VB.Menu StockRezef 
         Caption         =   "ניירות רצף"
      End
      Begin VB.Menu mnuSh500 
         Caption         =   "סוג חומר 500"
      End
      Begin VB.Menu Holdings 
         Caption         =   "ייתרות"
      End
      Begin VB.Menu BaseAssests 
         Caption         =   "נכסי בסיס"
      End
      Begin VB.Menu StockValue 
         Caption         =   "שערים לניירות ערך"
      End
      Begin VB.Menu StockHistory 
         Caption         =   "נתוני עסקאות לאופציה"
      End
      Begin VB.Menu TradeOptions 
         Caption         =   "ניירות נגזרים"
      End
      Begin VB.Menu ShortTradeOptions 
         Caption         =   "ניירות נגזרים קצר"
      End
      Begin VB.Menu StocksInIndex 
         Caption         =   "משקל מניות במדד"
      End
      Begin VB.Menu StatisticsInfo 
         Caption         =   "Statictics"
      End
      Begin VB.Menu StockStage 
         Caption         =   "שלב למניה"
      End
      Begin VB.Menu ConstStock 
         Caption         =   "ניירות ערך קבועים"
      End
      Begin VB.Menu mnu_UserApp 
         Caption         =   "יישומי משתמש"
      End
      Begin VB.Menu mnu_UserPermissions 
         Caption         =   "הרשאות משתמש"
      End
      Begin VB.Menu mnu_UserAuth 
         Caption         =   "אישור סיסמה שניה"
      End
      Begin VB.Menu mnuAgachShacharForFuture 
         Caption         =   "אג""חי שחר לחוזים"
      End
   End
   Begin VB.Menu Accounts 
      Caption         =   "Accounts"
      Begin VB.Menu GetAccounts 
         Caption         =   "Get Accounts (Query)"
      End
   End
   Begin VB.Menu Orders 
      Caption         =   "הוראות מעו""ף&"
      Begin VB.Menu SendMaofOrder 
         Caption         =   "שלח הוראת מעו""ף"
      End
   End
   Begin VB.Menu Encryption 
      Caption         =   "Encryption"
      Begin VB.Menu EncryptTxt 
         Caption         =   "Encrypt"
      End
      Begin VB.Menu DecryptTxt 
         Caption         =   "Decrypt"
      End
   End
   Begin VB.Menu Funds 
      Caption         =   "קרנות"
      Begin VB.Menu KrnBizuim 
         Caption         =   "ביצועי קרן"
      End
      Begin VB.Menu GetKranot 
         Caption         =   "GetKranot"
      End
      Begin VB.Menu StartOnlineUserSession 
         Caption         =   "החל זרימת נתוני און-ליין "
      End
      Begin VB.Menu StopOnlineUserSession 
         Caption         =   "עצור זרימת נתוני און-ליין "
      End
      Begin VB.Menu KranotOnlineSession 
         Caption         =   "קרנות און ליין"
      End
      Begin VB.Menu GetAccountYields 
         Caption         =   "שליפת תשואות"
      End
      Begin VB.Menu GetKranotAccounts 
         Caption         =   "שליפת חשבונות קרנות"
      End
   End
   Begin VB.Menu Config 
      Caption         =   "קונפיגורציה"
      Begin VB.Menu AS400Time 
         Caption         =   "AS400 Time"
      End
   End
End
Attribute VB_Name = "FMRLibTester"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
Public FMRUser As TaskBarLib.User
'Public WithEvents FMRK300 As TaskBarLib.K300
Public FMRK300 As TaskBarLib.K300
Public FMRFunds As TaskBarLib.Funds
Public DirectDlv As TaskBarLib.DirectDlv
Public FMRShortStocks As TaskBarLib.IShortStocks
Public FMRConfig As TaskBarLib.Config

Public SessionId As Long

Private Declare Function GetTickCount Lib "Kernel32" () As Long
Private Declare Sub Sleep Lib "Kernel32" (ByVal milliseconds As Long)


Private nLastTimePos As Long  'the position within the string where the last time field is located
Private strLastTime As String           'string representation of the last update time
Dim FirstTime As Boolean
Private nLastTime As Long               'numeric representation of the last update time
Private MaofID As Long  'Maof Stream Id
Private MaofCNTID As Long  'MaofCNTStream Id
Private RezefID As Long  'Rezef Stream Id
Private RezefCNTID As Long  'RezefCNTStream Id
Private RezefRawData As Boolean
Private MaofRawData As Boolean

Private ActivateCalled As Boolean
Private AppLeft As Long

Private strOrdersLastTime As String  'string representation of the last update time
Private strK300LastTime As String    'string representation of the last update time

Private nOrdersLastTime  As Long  'the position within the string where the last time field is located
Private nK300LastTime  As Long  'the position within the string where the last time field is located
Private bActiveMaofSession As Boolean


Private sheetLastCellRow As Long
Private sheetLastCellCol As Long


Private CNTK300LastTime As String
Private CNTOrdersLastTime As String


Private FormsCollection(1000) As Form
Private FormsCollectionCount As Long

Private WithEvents OnlineEvents As TaskBarLib.OnlineEvents
Attribute OnlineEvents.VB_VarHelpID = -1

Private Sub AS400Time_Click()

    Dim timeRec As TaskBarLib.AS400DateTime
    Dim retval As Long
    Dim latency As Long
    Dim dtDateTime As Date
    retval = FMRConfig.GetAS400DateTime(timeRec, latency)
    If retval = 0 Then
        lst_full.Clear
        dtDateTime = CDate(DateSerial(timeRec.Year, timeRec.Month, timeRec.Day) & " " & _
            TimeSerial(timeRec.Hour, timeRec.Minute, timeRec.Second))
        lst_full.AddItem dtDateTime
    End If
End Sub

'Private Type Order_To_Send_Type
'    operation As String * 1
'    asmachta_fmr As String * 8
'    ammount As String * 9
'    price As String * 9
'    Stock_Number As String * 8
'    Op As String * 1
'    Branch As String * 3
'    Account As String * 6
'    order_type As String * 3
'    asmachta_rezef As String * 8
'    price_percent As String * 9
'    shlav As String * 1
'    Nv_Del As String * 9
'    ORDR_TYPE As String * 4
'    Mana As String * 8
'    Zira As String * 4
'    Nv_Min As String * 11
'    Strat_Date As String * 8
'    end_date As String * 8
'End Type

Private Sub BaseAssests_Click()
    Dim vecData() As BaseAssetType
    Dim rc As Integer
    Dim i As Integer            'record iterator
    Dim lst As ListBox          'the list-box to output to
          
   'clear the output lists
    ClearLists
    DisableAllTimers
    
    Set lst = lst_full
        
    CreateTaskBar
   
   'make the query using the TaskBarLib User object
    rc = FMRK300.GetBaseAssets2(vecData)
     
    If rc > 0 Then
        lst.AddItem "Name Code  Madad  IV  IVChange  Dividend  Interest"
        For i = LBound(vecData) To UBound(vecData)
            lst.AddItem vecData(i).NameHeb & " " & _
                vecData(i).BaseAssetCode & " " & _
                vecData(i).Bno & " " & _
                vecData(i).value & " " & _
                vecData(i).StdIv & " " & _
                vecData(i).StdIdChange & " " & _
                vecData(i).Dividend & " " & _
                vecData(i).Interest
            StatusBar.Text = "Updating List - Row " & i & " of " & UBound(vecData)
        Next
        StatusBar.Text = "Updating List - Row " & i - 1 & " of " & UBound(vecData)
   Else
        StatusBar.Text = "No records were found for the requested query."
   End If
 
 Erase vecData
End Sub

Private Sub BlackAndScholes_Click()
    Set FormsCollection(FormsCollectionCount) = New frmBlackAndScholes
    FormsCollectionCount = FormsCollectionCount + 1
    FormsCollection(FormsCollectionCount - 1).Show , Me
End Sub

Private Sub btnRzfSpeed_Click()
    DlgStockSelect.Show vbModal
End Sub

Private Sub btnDirectDlv_Click()
    Dim vecData() As String
    Dim res As VbMsgBoxResult
    Dim i As Long
    Dim errmsg As String
    Dim Err As Long
    Dim ret As Long
    
    res = MsgBox("האם לשלוח את השדר" & vbCrLf & txt_query.Text, vbYesNo)
    If res = vbNo Then
        Exit Sub
    End If
    
    ret = DirectDlv.SafeExecDirect2(SessionId, txt_query.Text, vecData, Err, errmsg, 0)
    If ret > 0 Then
        lst_full.Clear
        For i = LBound(vecData) To UBound(vecData)
            lst_full.AddItem (vecData(i))
            txt_full_list_record_number = i + 1
        Next i
    ElseIf Err <> 0 Then
        MsgBox "שגיאה בבקשה" & vbCrLf & errmsg & vbCrLf & "Error Code = " & Err, vbCritical
    End If
End Sub


Private Sub ConstStock_Click()
Dim vecData() As ConsStockType 'query results
    Dim nRecords As Integer     'number of records returned
    Dim i As Integer            'record iterator
    Dim lst As ListBox          'the list-box to output to
          
       'clear the output lists
    ClearLists
    DisableAllTimers
    Set lst = lst_full
    CreateTaskBar
   
   'make the query using the TaskBarLib User object
    nRecords = FMRK300.GetConstStock(vecData)
     
   If nRecords > 0 Then
         
         For i = LBound(vecData) To UBound(vecData)
             lst.AddItem vecData(i).BNO_NAME & " " & vecData(i).ENG_NAME & " " & vecData(i).LMT_DN_OPN & " " & vecData(i).LMT_UP_OPN & " " & vecData(i).Symbol & " " & vecData(i).TRADE_METH & " " & vecData(i).SUG_BNO & " " & vecData(i).ANAF & " " & vecData(i).BNO_NO
             StatusBar.Text = "Updating List - Row " & i & " of " & UBound(vecData) + 1
        Next
        StatusBar.Text = "Updating List - Row " & i & " of " & UBound(vecData) + 1
   Else
        StatusBar.Text = "No records were found for the requested query."
 End If
 
 Erase vecData
End Sub

Private Sub DecryptTxt_Click()
    'clear the output lists
    ClearLists
    DisableAllTimers
    CreateTaskBar
    
    Dim OrgTxt As String
  
    FMRUser.Decrypt txt_orgtxt.Text, txt_key.Text, OrgTxt
    txt_orgtxt.Text = OrgTxt

End Sub

Private Sub EncryptTxt_Click()
    'clear the output lists
    ClearLists
    DisableAllTimers
    CreateTaskBar
    
    Dim EncTxt As String
    
    FMRUser.Encrypt txt_orgtxt.Text, txt_key.Text, EncTxt
    txt_orgtxt.Text = EncTxt
End Sub

Private Sub Form_Activate()
    If ActivateCalled = False Then
        frmLogin.Show vbModal, Me
        ActivateCalled = True
        
        ' restore left coordinate
        Me.Left = AppLeft
        
        Me.lbl_user_name_val = FMRUser.username(SessionId)
        Me.lblSessionIdNum = str(SessionId)
        If FMRLibTester.SessionId <> 0 Then
            FMRK300.K300SessionId = SessionId
        End If
    End If
End Sub

Private Sub Form_Load()
    CreateTaskBar
    '
    ActivateCalled = False
    AppLeft = Me.Left
    Me.Left = -20000 ' kind of hide
    
    Me.Order_Type.AddItem "LMT"
    Me.Order_Type.AddItem "MKT"
    Me.Order_Type.AddItem "LMO"
    Me.Order_Type.AddItem "KRN"
    Me.Order_Type.AddItem "ATC"
    Me.Order_Type.AddItem "JMB"
    Me.Order_Type.ListIndex = 0
    
    
   
    Me.Combo_Madad.AddItem "MDD_ALL", 0  'ALL
    Me.Combo_Madad.ItemData(Me.Combo_Madad.NewIndex) = -1
    Me.Combo_Madad.AddItem "TLV25", 1 'TLV25
    Me.Combo_Madad.ItemData(Me.Combo_Madad.NewIndex) = 0
    Me.Combo_Madad.AddItem "TLV75", 2 'TLV75
    Me.Combo_Madad.ItemData(Me.Combo_Madad.NewIndex) = 1
    Me.Combo_Madad.AddItem "TLTK", 3 'TLTK
    Me.Combo_Madad.ItemData(Me.Combo_Madad.NewIndex) = 2
    Me.Combo_Madad.AddItem "TLV100", 4 'TLV100
    Me.Combo_Madad.ItemData(Me.Combo_Madad.NewIndex) = 3
    Me.Combo_Madad.AddItem "MDD_BANK", 5   'MDD_BANK
    Me.Combo_Madad.ItemData(Me.Combo_Madad.NewIndex) = 4
'   Me.Combo_Madad.AddItem "MDD_DOLLAR", 6   'dollar
'    Me.Combo_Madad.ItemData(Me.Combo_Madad.NewIndex) = 5
    Me.Combo_Madad.AddItem "MDD_BINUI", 6  ''נדלן בינוי וחקלאות
    Me.Combo_Madad.ItemData(Me.Combo_Madad.NewIndex) = 6
    Me.Combo_Madad.AddItem "MDD_TLVFIN", 7 '' תא פיננסים 15
    Me.Combo_Madad.ItemData(Me.Combo_Madad.NewIndex) = 7
    Me.Combo_Madad.AddItem "MDD_TLVNADLAN15", 8 '' תא נדלן 15
    Me.Combo_Madad.ItemData(Me.Combo_Madad.NewIndex) = 8
    Me.Combo_Madad.AddItem "MDD_YETER30", 9 '' יתר 30
    Me.Combo_Madad.ItemData(Me.Combo_Madad.NewIndex) = 9
    Me.Combo_Madad.AddItem "MDD_TELDIV20", 10 ' תלדיב 20
    Me.Combo_Madad.ItemData(Me.Combo_Madad.NewIndex) = 10
    Me.Combo_Madad.AddItem "MDD_TELBOND", 11 ' תל-בונד
    Me.Combo_Madad.ItemData(Me.Combo_Madad.NewIndex) = 11
    Me.Combo_Madad.AddItem "MDD_YETER120", 12 ' יתר 120
    Me.Combo_Madad.ItemData(Me.Combo_Madad.NewIndex) = 12
    Me.Combo_Madad.AddItem "MDD_YETER50", 13 ' יתר 50
    Me.Combo_Madad.ItemData(Me.Combo_Madad.NewIndex) = 13
    Me.Combo_Madad.AddItem "MDD_MAALE", 14 ' מעלה
    Me.Combo_Madad.ItemData(Me.Combo_Madad.NewIndex) = 14
    Me.Combo_Madad.AddItem "MDD_TELBOND40", 15 ' תל-בונד 40
    Me.Combo_Madad.ItemData(Me.Combo_Madad.NewIndex) = 15
    Me.Combo_Madad.AddItem "MDD_TELBOND60", 16 ' תל-בונד 60
    Me.Combo_Madad.ItemData(Me.Combo_Madad.NewIndex) = 16
    Me.Combo_Madad.AddItem "MDD_GOV_FIXED0", 17 ' אגח ממשלתי ריבית קבועה 0 - 2
    Me.Combo_Madad.ItemData(Me.Combo_Madad.NewIndex) = 17
    Me.Combo_Madad.AddItem "MDD_GOV_FIXED2", 18 ' אגח ממשלתי ריבית קבועה 2 - 5
    Me.Combo_Madad.ItemData(Me.Combo_Madad.NewIndex) = 18
    Me.Combo_Madad.AddItem "MDD_GOV_FIXED5", 19 ' אגח ממשלתי ריבית קבועה 5+
    Me.Combo_Madad.ItemData(Me.Combo_Madad.NewIndex) = 19
    Me.Combo_Madad.AddItem "GOV_CHG0", 20 ' אגח ממשלתי ריבית משתנה 0 - 5
    Me.Combo_Madad.ItemData(Me.Combo_Madad.NewIndex) = 20
    Me.Combo_Madad.AddItem "GOV_CHG5", 21 ' אגח ממשלתי ריבית משתנה 5+
    Me.Combo_Madad.ItemData(Me.Combo_Madad.NewIndex) = 21
    Me.Combo_Madad.AddItem "MAKAM", 22 'מקמ
    Me.Combo_Madad.ItemData(Me.Combo_Madad.NewIndex) = 22
    Me.Combo_Madad.AddItem "CURRENCYLNKD", 23 ' נסחרות וצמודות מטח
    Me.Combo_Madad.ItemData(Me.Combo_Madad.NewIndex) = 23
    
    Me.Combo_Madad.ListIndex = 0
    
    bActiveMaofSession = False
    '
    
    mnuUserLogin.Enabled = False
    
    FormsCollectionCount = 0
    
End Sub


Private Sub Form_Unload(Cancel As Integer)
    Dim i As Long
    For i = LBound(FormsCollection) To UBound(FormsCollection)
        Set FormsCollection(i) = Nothing
    Next i
    
End Sub

Private Sub GetAccounts_Click()
    Dim vecData() As AccountType 'query results
    'Dim vecData2() As AccountRecord
    Dim nRecords As Integer     'number of records returned
    Dim i As Integer            'record iterator
    Dim lst As ListBox          'the list-box to output to
    Dim errmsg As String
              
       'clear the output lists
    ClearLists
    DisableAllTimers
    Set lst = lst_full
    CreateTaskBar
   
   'make the query using the TaskBarLib User object
   txt_last_time.Text = Trim$(str(GetTickCount()))
   
'    If txt_orgtxt.Text = "TCP" Then
'        For i = 0 To 1000
'        nRecords = ProxyUser.GetAccounts(SessionId, txt_query.Text, vecData2, ErrMsg)
'        Next i
'          If nRecords > 0 Then
'                For i = LBound(vecData2) To UBound(vecData2)
'                    lst.AddItem vecData2(i).BRANCH_NO & " " & vecData2(i).COD_H_KUPA & " " & vecData2(i).EXT_MARGIN & " " & vecData2(i).ID_NAME & " " & vecData2(i).ID_NO & " " & vecData2(i).ID_STREET & " " & vecData2(i).ID_TOT_VL & " " & vecData2(i).MIN_C_FUTR & " " & vecData2(i).MIN_C_MAOF & " " & vecData2(i).SIVUG & " " & vecData2(i).TIK_NO
'                    StatusBar.Text = "Updating Index List - Row " & i & " of " & UBound(vecData2)
'               Next
'               StatusBar.Text = "Updating Index  List - Row " & i & " of " & UBound(vecData2) + 1
'          End If
'          txt_last_time.Text = GetTickCount() - Val(txt_last_time.Text)
'          Exit Sub
'    Else
    nRecords = FMRUser.GetAccounts(SessionId, txt_query.Text, vecData, errmsg)
'    End if
    
     If Trim(errmsg) <> "" Then
        MsgBox (errmsg)
    Else
     
          If nRecords > 0 Then
                
                For i = LBound(vecData) To UBound(vecData)
                    lst.AddItem vecData(i).BRANCH_NO & " " & vecData(i).COD_H_KUPA & " " & vecData(i).EXT_MARGIN & " " & vecData(i).ID_NAME & " " & vecData(i).ID_NO & " " & vecData(i).ID_STREET & " " & vecData(i).ID_TOT_VL & " " & vecData(i).MIN_C_FUTR & " " & vecData(i).MIN_C_MAOF & " " & vecData(i).SIVUG & " " & vecData(i).TIK_NO
                    StatusBar.Text = "Updating Index List - Row " & i & " of " & UBound(vecData)
               Next
               StatusBar.Text = "Updating Index  List - Row " & i & " of " & UBound(vecData) + 1
          Else
               StatusBar.Text = "No records were found for the requested query."
        End If
        
    End If
    
    txt_last_time.Text = GetTickCount() - Val(txt_last_time.Text)
End Sub

Private Sub GetK300RZ_Click()
    CreateTaskBar
    'AddFormToCollection (New K300RZ)
    Set FormsCollection(FormsCollectionCount) = New K300RZ
    FormsCollectionCount = FormsCollectionCount + 1
    FormsCollection(FormsCollectionCount - 1).Show , Me
End Sub

Private Sub GetKranot_Click()
    Dim vecData() As KerenBasicType
    Dim Bno As String
    Dim errmsg As String
    Dim ret As Long
    Dim i As Long
    FMRLibTester.lst_full.Clear
    
    ret = FMRLibTester.FMRFunds.GetKranot(vecData, Bno, errmsg)
    If UBound(vecData) >= 0 Then
        FMRLibTester.lst_full.Clear
        For i = 0 To UBound(vecData)
            FMRLibTester.lst_full.AddItem vecData(i).BNO_Num & " " & vecData(i).BASIS_RATE
        Next i
    End If
End Sub


Private Sub GetKranotAccounts_Click()
    Dim vecData() As AccountType 'query results
    Dim nRecords As Integer     'number of records returned
    Dim i As Integer            'record iterator
    Dim lst As ListBox          'the list-box to output to
    Dim errmsg As String
              
       'clear the output lists
    ClearLists
    DisableAllTimers
    Set lst = lst_full
    CreateTaskBar
   
    nRecords = FMRUser.GetKranotAccounts(SessionId, txt_query.Text, vecData, errmsg)
'    End if
    
     If Trim(errmsg) <> "" Then
        MsgBox (errmsg)
    Else
     
          If nRecords > 0 Then
                
                For i = LBound(vecData) To UBound(vecData)
                    lst.AddItem vecData(i).BRANCH_NO & " " & vecData(i).COD_H_KUPA & " " & vecData(i).EXT_MARGIN & " " & vecData(i).ID_NAME & " " & vecData(i).ID_NO & " " & vecData(i).ID_STREET & " " & vecData(i).ID_TOT_VL & " " & vecData(i).MIN_C_FUTR & " " & vecData(i).MIN_C_MAOF & " " & vecData(i).SIVUG & " " & vecData(i).TIK_NO
                    StatusBar.Text = "Updating Index List - Row " & i & " of " & UBound(vecData)
               Next
               StatusBar.Text = "Updating Index  List - Row " & i & " of " & UBound(vecData) + 1
          Else
               StatusBar.Text = "No records were found for the requested query."
        End If
        
    End If
End Sub

Private Sub GetOrdersMF_Click()
    Dim o As Object
    
    CreateTaskBar
    
    'Set o = New OrdersSheet
    'o.OrdesrType = "MF"
    'AddFormToCollection (o)
    Set FormsCollection(FormsCollectionCount) = New OrdersSheet
    'et FormsCollection(FormsCollectionCount) = New Orders
    FormsCollection(FormsCollectionCount).OrdersType = "MF"
    FormsCollectionCount = FormsCollectionCount + 1
    FormsCollection(FormsCollectionCount - 1).Show , Me
    
End Sub

Private Sub GetOrdersRZ_Click()
    Dim o As Object
    CreateTaskBar
    
'    Set o = New OrdersSheet
'    o.OrdersType = "RZ"
'    AddFormToCollection (o)
    Set FormsCollection(FormsCollectionCount) = New OrdersSheet
    FormsCollection(FormsCollectionCount).OrdersType = "RZ"
    FormsCollectionCount = FormsCollectionCount + 1
    FormsCollection(FormsCollectionCount - 1).Show , Me
End Sub

Private Sub GetAccountYields_Click()
    CreateTaskBar
    
    Dim InitialRecords() As TaskBarLib.AccountYieldInitial
    Dim DetailedRecords() As TaskBarLib.AccountYieldDetailed
    Dim ReqRecords() As TaskBarLib.AccountYieldByRequirement
    Dim rc As Long
    Dim i As Integer
    Dim dataType As TaskBarLib.YieldDataType
    Dim accYldOnly As Integer
    Dim StartDate As Long
    Dim EndDate As Long
    StartDate = 0
    EndDate = 0
    
    Dim lst As ListBox          'the list-box to output to
              
       'clear the output lists
    ClearLists
    Set lst = lst_full
    CreateTaskBar
    
    frmYield.Show vbModal
    If frmYield.ButtonPressed = vbCancel Then
        Unload frmYield
        Exit Sub
    End If
    If frmYield.AccYieldOnly = True Then
        accYldOnly = 1
    Else
        accYldOnly = 0
    End If
    Select Case frmYield.Radio
        Case 1
            rc = FMRUser.GetAccountYieldInitial(SessionId, InitialRecords, Val(frmYield.yldBranch), Val(frmYield.yldAccount), accYldOnly)
            If rc > 0 Then
                For i = LBound(InitialRecords) To UBound(InitialRecords)
                    lst.AddItem InitialRecords(i).Yld_sug & " " & InitialRecords(i).Yld_id & " " & InitialRecords(i).Yld_bno & " " & InitialRecords(i).Yld_vl_mm & " " & InitialRecords(i).Yld_yld_mm & _
                    InitialRecords(i).Yld_vl_dd & " " & InitialRecords(i).Yld_yld_dd & " " & InitialRecords(i).Yld_vl_yy & " " & InitialRecords(i).Yld_yld_yy & " " & InitialRecords(i).Yld_vl_3m & " " & InitialRecords(i).Yld_yld_3m _
                    & " " & InitialRecords(i).Yld_vl_12 & " " & InitialRecords(i).Yld_yld_12 & " " & InitialRecords(i).Yld_date
                Next
            End If
            Erase InitialRecords
        Case 2
            
             Select Case frmYield.Combo1.ListIndex
                Case 0
                    dataType = YieldDataMonthbyDay
                Case 1
                    dataType = YieldDataYearbyMonth
                Case 2
                    dataType = YieldDataTwelveMonths
                Case 3
                    dataType = YieldData5YearsbyYear
                Case 4
                    dataType = YieldDataYearbyQuater
            End Select
            
            StartDate = Format(frmYield.dtFromDate, "yyyyMMdd")
            rc = FMRUser.GetAccountYieldByRequirement(SessionId, ReqRecords, Val(frmYield.yldBranch), Val(frmYield.yldAccount), _
            StartDate, dataType, accYldOnly)
            If rc > 0 Then
                For i = LBound(ReqRecords) To UBound(ReqRecords)
                    lst.AddItem ReqRecords(i).Yld_sug & " " & ReqRecords(i).Yld_id & " " & ReqRecords(i).Yld_bno & " " & ReqRecords(i).Yld_vl_dd & " " & ReqRecords(i).Yld_yld_dd & " " & ReqRecords(i).Yld_count & " " & ReqRecords(i).Yld_date
                Next
            End If
            Erase ReqRecords
        Case 3
           
            StartDate = Format(frmYield.dtFromDate, "yyyyMMdd")
            EndDate = Format(frmYield.dtToDate, "yyyyMMdd")
            rc = FMRUser.GetAccountYieldDetailed(SessionId, DetailedRecords, Val(frmYield.yldBranch), Val(frmYield.yldAccount), _
            StartDate, EndDate, accYldOnly)
            If rc > 0 Then
                For i = LBound(DetailedRecords) To UBound(DetailedRecords)
                    lst.AddItem DetailedRecords(i).Yld_sug & " " & DetailedRecords(i).Yld_id & " " & DetailedRecords(i).Yld_bno & " " & DetailedRecords(i).Yld_vl_dd & " " & DetailedRecords(i).Yld_yld_dd & " " & DetailedRecords(i).Yld_date
                Next
            End If
            Erase DetailedRecords
    End Select
    
     
End Sub

Private Sub KranotOnlineSession_Click()
    Dim o As Object
    CreateTaskBar
    
    Set FormsCollection(FormsCollectionCount) = New frmOnlineKranot
    FormsCollectionCount = FormsCollectionCount + 1
    FormsCollection(FormsCollectionCount - 1).Show , Me
End Sub


Private Sub mnuAgachShacharForFuture_Click()
    Dim rc As Long
    Dim vecData() As ShacharBondInFutureType
    Dim Bno As String
    
    Bno = "0"
    rc = FMRK300.GetShacharBondsInFuture(Bno, vecData)
    If rc > 0 Then
        lst_full.Clear
        Dim i As Long
        For i = LBound(vecData) To UBound(vecData)
            lst_full.AddItem vecData(i).BNO_Future & " " & vecData(i).BNO_Bond & " " & _
                vecData(i).ConvCoeff & " " & vecData(i).AccumInterest
        Next i
    End If
End Sub

Private Sub mnuIndices_Click()
    Set FormsCollection(FormsCollectionCount) = New K300Indices
    FormsCollectionCount = FormsCollectionCount + 1
    FormsCollection(FormsCollectionCount - 1).Show , Me
End Sub

Private Sub mnuK300FileCacheMaof_Click()
    Dim rc As Long
    Dim errmsg As String
    
    rc = MsgBox("Files will be created in" & vbCrLf & vbTab & ".\K300Events" & vbCrLf & vbCrLf _
        & "and will be called" & vbCrLf & vbTab & "K3Data-nn.csv" & vbCrLf & vbCrLf _
        & "and will have 32000 events per file." & vbCrLf & vbCrLf _
        & "Is this Ok?", vbOKCancel Or vbQuestion)
        
    If rc = vbCancel Then
        Exit Sub
    End If
    
    rc = FMRK300.CreateK300EventsFile(".\K300Events", "K3Data", 10000000, MaofStream, errmsg)
    If rc <> 0 Then
        MsgBox "שמירת אירועי שוק לקובץ נכשלה" & vbCrLf & errmsg, vbCritical Or vbMsgBoxRight
        Exit Sub
    End If
    
    MsgBox "הפעולה הסתיימה בהצלחה"
    
End Sub

Private Sub GetK300MF_Click()
    
    ClearLists
    CreateTaskBar
    
    'AddFormToCollection (New K300MF)
    Set FormsCollection(FormsCollectionCount) = New K300MF
    FormsCollectionCount = FormsCollectionCount + 1
    FormsCollection(FormsCollectionCount - 1).Show , Me
End Sub


Private Sub GetMaofScenarios_Click()
    Dim vecData() As K300MaofScenariosType
    Dim nRecords As Integer     'number of records returned
    Dim i As Integer            'record iterator
    Dim lst As ListBox          'the list-box to output to
    Dim RecType As String
    Dim CurRec As String
    
    ClearLists
    CreateTaskBar
    
    nRecords = FMRK300.GetMaofScenarios(vecData, txt_option.Text, Me.Combo_Madad.ItemData(Me.Combo_Madad.ListIndex))
   ' nRecords = FMRK300.GetMaofScenarios(vecData, txt_option.Text, Me.Combo_Madad.ItemData(Me.Combo_Madad.ListIndex))
   '  nRecords = FMRK300.GetMaofScenarios(vecData, "00000000", TLV25)
     
    ClearLists
    DisableAllTimers
    Set lst = lst_full
    
    If nRecords > 0 Then
        For i = 0 To 44
              lst.AddItem vecData(0).dScenarios(i)
        Next
    End If
    
End Sub

Private Sub Holdings_Click()
Dim vecData() As RMType     'query results
    Dim nRecords As Integer     'number of records returned
    Dim i As Integer            'record iterator
    Dim lst As ListBox          'the list-box to output to
    Dim trdType As TradeType
          
    DisableAllTimers
    
    Set lst = lst_full
        
    CreateTaskBar
   
    DlgHoldings.Show vbModal
    If DlgHoldings.ButtonPressed = vbCancel Then
        Unload DlgHoldings
        Exit Sub
    End If
   
    If DlgHoldings.Radio = 1 Then
        trdType = RZ
    ElseIf DlgHoldings.Radio = 2 Then
        trdType = MF
    Else
        trdType = ALLTradeType
    End If
    
   Dim RmOnlineHeader As TaskBarLib.RmOnlineTotalInfoType
   Dim RmOnLineRecords() As TaskBarLib.RmOnlineRecordType
    
    If DlgHoldings.chk_Online.value = 1 Then
        nRecords = FMRUser.GetOnlineRm(SessionId, RmOnlineHeader, RmOnLineRecords, DlgHoldings.DlgHoldingsBranch, DlgHoldings.DlgHoldingsAccount, "000000")
        If nRecords > 0 Then
            For i = LBound(RmOnLineRecords) To UBound(RmOnLineRecords)
                lst.AddItem DlgHoldings.DlgHoldingsAccount & " " & RmOnLineRecords(i).sBnoName & " " & RmOnLineRecords(i).sBnoNum & " " & RmOnLineRecords(i).sLastNV
                StatusBar.Text = "Updating List - Row " & i & " of " & UBound(RmOnLineRecords)
            Next
            StatusBar.Text = "Updating List - Row " & i - 1 & " of " & UBound(RmOnLineRecords)
        Else
            StatusBar.Text = "No records were found for the requested query."
        End If
    
    ElseIf DlgHoldings.chk_mahzikim.value = 1 Then
        nRecords = FMRUser.GetHoldingsEx(SessionId, txt_tik.Text, DlgHoldings.DlgHoldingsAccount, _
            vecData, trdType, DlgHoldings.DlgHoldingsBranch, DlgHoldings.txtBno.Text)
        If nRecords > 0 Then
            For i = LBound(vecData) To UBound(vecData)
                lst.AddItem vecData(i).ID & " " & vecData(i).BNO_NAME & " " & vecData(i).Bno & " " & vecData(i).Nv
                StatusBar.Text = "Updating List - Row " & i & " of " & UBound(vecData)
            Next
            StatusBar.Text = "Updating List - Row " & i - 1 & " of " & UBound(vecData)
        Else
            StatusBar.Text = "No records were found for the requested query."
        End If
    Else
        If DlgHoldings.DlgHoldingsMefazel = False Then
            nRecords = FMRUser.GetHoldingsEx(SessionId, txt_tik.Text, DlgHoldings.DlgHoldingsAccount, _
                vecData, trdType, DlgHoldings.DlgHoldingsBranch, txt_option.Text)
        Else
            nRecords = FMRUser.GetHoldingsEx(SessionId, txt_tik.Text, "000000", vecData, trdType, _
                DlgHoldings.DlgHoldingsBranch, txt_option.Text, DlgHoldings.DlgHoldingsAccount)
        End If
   
    
        If nRecords > 0 Then
            For i = LBound(vecData) To UBound(vecData)
                lst.AddItem vecData(i).ID & " " & vecData(i).BNO_NAME & " " & vecData(i).Bno & _
                " " & Val(vecData(i).Nv) / 100
                StatusBar.Text = "Updating List - Row " & i & " of " & UBound(vecData)
            Next
            StatusBar.Text = "Updating List - Row " & i - 1 & " of " & UBound(vecData)
        Else
            StatusBar.Text = "No records were found for the requested query."
        End If
End If
 
 Erase vecData
End Sub

Private Sub Indexes_Click()
    Dim vecData() As IndexInfoType 'query results
    Dim nRecords As Integer     'number of records returned
    Dim i As Integer            'record iterator
    Dim lst As ListBox          'the list-box to output to
          
   'clear the output lists
    ClearLists
    DisableAllTimers
    
    Set lst = lst_full
        
   CreateTaskBar
   
   'make the query using the TaskBarLib User object
    nRecords = FMRK300.GetIndexes(vecData)
     
   If nRecords > 0 Then
         
         For i = LBound(vecData) To UBound(vecData)
             lst.AddItem vecData(i).COD_MDD & " " & vecData(i).NAME_MDD & " " & vecData(i).VAL_MDD & " " & vecData(i).CHG_PCNT
             StatusBar.Text = "Updating Index List - Row " & i & " of " & UBound(vecData)
        Next
        StatusBar.Text = "Updating Index  List - Row " & i - 1 & " of " & UBound(vecData)
   Else
        StatusBar.Text = "No records were found for the requested query."
 End If
End Sub

Private Sub IndexStructure_Click()
   Dim vecData() As String     'query results
    Dim nRecords As Integer     'number of records returned
    Dim i As Integer            'record iterator
    Dim lst As ListBox          'the list-box to output to
          
   'clear the output lists
    ClearLists
    DisableAllTimers
    
    Set lst = lst_full
        
    CreateTaskBar
   
   'make the query using the TaskBarLib User object
    nRecords = FMRK300.GetIndexStructure(vecData)
     
   If nRecords > 0 Then
         
         For i = LBound(vecData) To UBound(vecData)
             lst.AddItem vecData(i)
             StatusBar.Text = "Updating Index List - Row " & i & " of " & UBound(vecData)
        Next
        StatusBar.Text = "Updating Index  List - Row " & i - 1 & " of " & UBound(vecData)
   Else
        StatusBar.Text = "No records were found for the requested query."
 End If
End Sub


Private Sub KrnBizuim_Click()
    
    CreateTaskBar
    KRNBizuimDialog.Show vbModal

End Sub


Private Sub Maof_Timer1_Timer()
    Dim vecData() As K300MaofType 'query results
    Dim vecStr() As String      'query results for raw data
    Dim nRecords As Integer     'number of records returned
    Dim i As Integer            'record iterator
    Dim lst As ListBox          'the list-box to output to
        
    'create the supporting TaskBarLib object if neccessery
    CreateTaskBar
    
    If MaofRawData = False Then
        nRecords = FMRK300.GetMAOF(vecData, strLastTime, "00000000", Me.Combo_Madad.ItemData(Me.Combo_Madad.ListIndex))
    Else
        nRecords = FMRK300.GetMAOFRaw(vecStr, strLastTime, "00000000", Me.Combo_Madad.ItemData(Me.Combo_Madad.ListIndex))
    End If

    'choose which list to output to
    Set lst = IIf(FirstTime, lst_full, lst_delta)
    FirstTime = False

    If (nRecords > 0) Then
        'go through records, output to list and look for the most recent update time
        For i = 0 To nRecords - 1
            If MaofRawData = False Then
                lst.AddItem vecData(i).BNO_Num & " " & vecData(i).BNO_NAME & " " & vecData(i).UPD_TIME & " " & vecData(i).EX_DATE & " " & vecData(i).DAY_VL
            Else
                lst.AddItem vecStr(i)
            End If
            txt_last_time.Text = strLastTime
        Next
   End If

    txt_full_list_record_number.Text = lst_full.ListCount
    txt_delta_list_record_number.Text = lst_delta.ListCount
    
    txt_pulled_iterations_value.Text = txt_pulled_iterations_value.Text + 1
    
    Erase vecData
            
End Sub

Private Sub MaofCNT_Timer1_Timer()
    Dim vecData() As String 'query results
    Dim nRecords As Integer     'number of records returned
    Dim i As Integer            'record iterator
    Dim lst As ListBox          'the list-box to output to
    Dim RecType As String
    Dim CurRec As String
    
    CreateTaskBar
    
    nRecords = FMRK300.GetMaofCnt(vecData, txt_branch.Text, txt_account.Text, strOrdersLastTime, strK300LastTime, " ", "N", "N", "  ")
       
    'choose which list to output to
    Set lst = IIf(strK300LastTime = "00000000", lst_full, lst_delta)
    
    If (nRecords > 0) Then
        'go through records, output to list and look for the most recent update time
        For i = 0 To nRecords - 1
            lst.AddItem vecData(i)
            CurRec = Space(10) & vecData(i)
            RecType = Mid(CurRec, 11, 1)
                If RecType = "M" Or RecType = "B" Or RecType = "D" Then
                    strK300LastTime = Mid(CurRec, 12, 8)
                      If Trim(strK300LastTime) <> "" Then
                        If CLng(strK300LastTime) > nK300LastTime Then
                            nK300LastTime = CLng(strK300LastTime)
                        End If
                   End If
                End If
                
                If RecType = "O" Then
                    strOrdersLastTime = Mid(CurRec, 12, 8)
                   If Trim(strOrdersLastTime) <> "" Then
                        If CLng(strOrdersLastTime) > nOrdersLastTime Then
                            nOrdersLastTime = CLng(strOrdersLastTime)
                        End If
                   End If
                End If
        Next
        txt_last_time.Text = strK300LastTime & "," & strOrdersLastTime
    End If
    
    txt_full_list_record_number.Text = lst_full.ListCount
    txt_delta_list_record_number.Text = lst_delta.ListCount
    
    txt_pulled_iterations_value.Text = txt_pulled_iterations_value.Text + 1
    
    'Erase vecData
End Sub

Private Sub mnu_GetMadadHistory_Click()
    Dim vecData() As TaskBarLib.K300MadadHistType
    Dim rc As Long, i As Long
    Dim lastTime As String
    
    lastTime = "-1"
    rc = FMRK300.GetMadadHistory(vecData, "000", lastTime)
    If rc > 0 Then
        lst_full.Clear
        For i = LBound(vecData) To UBound(vecData)
            lst_full.AddItem vecData(i).MDD_NAME & vecData(i).MDD & vecData(i).MDD_COD & vecData(i).MDD_DF & _
                vecData(i).MDD_NUM & vecData(i).MDD_SUG & vecData(i).SUG_REC & vecData(i).UPD_TIME & vecData(i).MDD_BASIS
        Next i
    End If
End Sub

Private Sub mnu_UserApp_Click()
        Dim arrApp() As String
        Dim nRecords As Long
        Dim i As Long
        Dim lst As ListBox          'the list-box to output to
        
        Set lst = lst_full
        
        'clear the output lists
        ClearLists
        
        'create taskbar instance
        CreateTaskBar
        
         nRecords = FMRUser.GetUserApp(SessionId, arrApp)
         
         If (nRecords > 0) Then
            'go through records, output to list and look for the most recent update time
            For i = 0 To nRecords - 1
                lst.AddItem arrApp(i)
            Next
            txt_last_time.Text = strLastTime
      End If
End Sub

Private Sub mnu_UserPermissions_Click()
    Dim vecData() As UserPasswordType
    Dim rc As Long
    Dim i As Long
    
    rc = FMRUser.GetUserPermissions(SessionId, vecData)
    If rc > 0 Then
        lst_full.Clear
        For i = LBound(vecData) To UBound(vecData)
            If vecData(i).SUG_RC = "02" Or vecData(i).SUG_RC = "13" Then
                lst_full.AddItem (vecData(i).APPL_N & vbTab & vecData(i).AUTH_LVL)
            ElseIf vecData(i).SUG_RC = "03" Or vecData(i).SUG_RC = "14" Then
                lst_full.AddItem (vecData(i).FIELD & vbTab & vecData(i).OPERATOR & vbTab & Val(vecData(i).Val) & vbTab & vecData(i).AUTH_LVL)
            End If
        Next i
    Else
        MsgBox "לא נמצאו רשומות"
    End If
End Sub

Private Sub mnu_UserAuth_Click()
    Dim rc As Long
    Dim i As Long
    
    If txt_orgtxt.Text = "" Then
        MsgBox "יש להקיש את הסיסמה בתיבת הטקסט"
        txt_orgtxt.SetFocus
        Exit Sub
    End If
    
    rc = FMRUser.UserAuthentication(SessionId, txt_orgtxt.Text)
    If rc = 1 Then
        MsgBox "סיסמה תקינה"
    Else
        MsgBox "סיסמה שגויה"
    End If
    
End Sub

Private Sub mnuMaofScenarios_Click()
    Set FormsCollection(FormsCollectionCount) = New frmScenarios
    FormsCollectionCount = FormsCollectionCount + 1
    FormsCollection(FormsCollectionCount - 1).Show , Me
End Sub

Private Sub mnuReloadPermssions_Click()
    Dim per As TaskBarLib.Permissions
    Set per = New TaskBarLib.Permissions
    
    per.RefreshUserPermissions (SessionId)
    Set per = Nothing
End Sub

Private Sub mnuSh161_Click()
    Dim rec() As TaskBarLib.SH161Type
    Dim ret As Long
    Dim i As Long
    
    lst_full.Clear
    ret = FMRK300.GetSH161(rec, Me.Combo_Madad.ItemData(Me.Combo_Madad.ListIndex))
    If ret > 0 Then
        lst_full.Clear
        txt_full_list_record_number.Text = str(ret)
        For i = LBound(rec) To UBound(rec)
            lst_full.AddItem rec(i).Bno & " " & rec(i).BNO_NAME & " " & rec(i).NV_TZAFA _
            & " " & rec(i).PCNT & " " & rec(i).PUBLIC_PRCNT
        Next i
    Else
        lst_full.AddItem "no records"
    End If
End Sub

Private Sub mnuSh500_Click()
    Dim rec() As TaskBarLib.SH500Type
    Dim ret As Long
    Dim i As Long
    
    ret = FMRK300.GetSH500(rec, txt_query.Text)
    If ret > 0 Then
        lst_full.Clear
        txt_full_list_record_number.Text = str(ret)
        For i = LBound(rec) To UBound(rec)
            lst_full.AddItem rec(i).BNO_NO & " " & rec(i).BNO_NAME
        Next i
    Else
        lst_full.AddItem "no records"
    End If
        
    
End Sub

Private Sub mnuUserLogin_Click()
    If SessionId = 0 Then
        frmLogin.Show vbModal
        mnuUserLogin.Enabled = False
        mnuUserLogout.Enabled = True
        lbl_user_name_val = FMRUser.username(SessionId)
        lblSessionIdNum = str(SessionId)
    End If
End Sub

Private Sub mnuUserLogout_Click()
    If SessionId <> 0 Then
        Dim ret As Long
        
        ret = FMRUser.Logout(SessionId)
        MsgBox " יצא מהמערכת " & lbl_user_name_val.Caption & " משתמש "
        lbl_user_name_val.Caption = ""
        lblSessionIdNum.Caption = ""
        SessionId = 0
        mnuUserLogin.Enabled = True
        mnuUserLogout.Enabled = False
    End If
End Sub

Private Sub OnlineEvents_OnOnlineKeren(ByRef psaStrRecords() As TaskBarLib.OnlineSessionBalance)
    Dim i As Integer            'record iterator
    Dim lst As ListBox          'the list-box to output to
          
   'clear the output lists
    ClearLists
    
    Set lst = lst_full
        
    'If nRecords > 0 Then
         
         For i = LBound(psaStrRecords) To UBound(psaStrRecords)
             lst.AddItem psaStrRecords(i).Onl_global_counter & " " & _
             psaStrRecords(i).Onl_id_number & " " & psaStrRecords(i).Onl_id_name & _
             " " & psaStrRecords(i).Onl_sug & " " & psaStrRecords(i).Onl_tik & " " & _
             psaStrRecords(i).Onl_bno
             StatusBar.Text = "Updating Index List - Row " & i & " of " & UBound(psaStrRecords)
        Next
        StatusBar.Text = "Updating Index  List - Row " & i - 1 & " of " & UBound(psaStrRecords)
    'End If

End Sub

Private Sub OrdersRezefDetails_Click()
    Dim nRecords As Long
    Dim vecData() As TaskBarLib.RZFINQType
    
    DlgOrdersDetails.Show vbModal, Me
    If DlgOrdersDetails.Cancelled = True Then
        Exit Sub
    End If
    
    If DlgOrdersDetails.chkByPaper.value = True Then
        nRecords = FMRUser.GetOrdersRezef(SessionId, vecData, "00000000", qtDetailed, _
        DlgOrdersDetails.txtAccount.Text, DlgOrdersDetails.txtBranch.Text, _
        DlgOrdersDetails.txtBno.Text)
    ElseIf DlgOrdersDetails.chkBySeq.value = True Then
        nRecords = FMRUser.GetOrdersRezef(SessionId, vecData, DlgOrdersDetails.txtSeq.Text, _
        qtDetailed, DlgOrdersDetails.txtAccount.Text, DlgOrdersDetails.txtBranch.Text, _
        "00000000")
    End If
    
    lst_full.Clear
    If (nRecords > 0) Then
        Dim i As Long
        'go through records, output to list
        For i = 0 To nRecords - 1
            lst_full.AddItem vecData(i).BNO_N & " " & vecData(i).SEQ_N & " " & vecData(i).ID_N & " " & " " & vecData(i).DIL_N & " " & vecData(i).DIL_NV_N
        Next
    End If
    
   txt_full_list_record_number.Text = lst_full.ListCount
   
   Erase vecData
    
End Sub

Private Sub OrdersMaof_Timer1_Timer()
    Dim vecData() As MOFINQType 'query results
    Dim nRecords As Integer     'number of records returned
    Dim i As Integer            'record iterator
    Dim lst As ListBox          'the list-box to output to
    Dim first As Boolean
    
     'create the supporting TaskBarLib object if neccessery
    CreateTaskBar
    
    'choose which list to output to
    If Val(strLastTime) = 0 Then
        first = True
    End If
    Set lst = IIf(strLastTime = "00000000", lst_full, lst_delta)
    
    nRecords = FMRUser.GetOrdersMaof(SessionId, vecData, strLastTime, qtSummary, txt_account.Text, txt_branch.Text, txt_option.Text)
    'nRecords = FMRUser.GetOrdersMaof(vecData, strLastTime, qtDetailed, txt_account.Text, txt_branch.Text, txt_option.Text)
    If (nRecords > 0) Then
        'go through records, output to list and look for the most recent update time
        For i = 0 To nRecords - 1
            lst.AddItem vecData(i).BNO_NAME & " " & vecData(i).BNO_PIC & " " & vecData(i).BRANCH_PIC & " " & vecData(i).COD_UPD & " " & vecData(i).DIL_NV_PIC & " " & vecData(i).DIL_PIC & " " & vecData(i).DIL_PRC_PIC & " " & vecData(i).DIL_TIME_PIC & " " & vecData(i).DSP_FMR
        Next
        txt_last_time.Text = strLastTime
    End If

    'strLastTime = str(Val(strLastTime) + 1)
    txt_full_list_record_number.Text = lst_full.ListCount
    txt_delta_list_record_number.Text = lst_delta.ListCount
    txt_pulled_iterations_value.Text = txt_pulled_iterations_value.Text + 1
   
   Erase vecData
    
End Sub

Private Sub OrdersRezef_Timer1_Timer()
    Dim vecData() As RZFINQType 'query results
    Dim nRecords As Integer     'number of records returned
    Dim i As Integer            'record iterator
    Dim lst As ListBox          'the list-box to output to
    
     'create the supporting TaskBarLib object if neccessery
    CreateTaskBar
     
    Set lst = IIf(strLastTime = "00000000", lst_full, lst_delta)
    nRecords = FMRUser.GetOrdersRezef(SessionId, vecData, strLastTime, qtSummary, txt_account.Text, txt_branch.Text, txt_option.Text)
    'choose which list to output to
    
    txt_last_time.Text = strLastTime
    If (nRecords > 0) Then
        'go through records, output to list
        For i = 1 To nRecords - 1
            lst.AddItem vecData(i).BNO_N & " " & vecData(i).BNO_NAME & " " & vecData(i).BRANCH_N & " " & vecData(i).COD_UPD & " " & vecData(i).DIL_N & " " & vecData(i).DIL_NV_N
        Next
    End If
    
   txt_full_list_record_number.Text = lst_full.ListCount
   txt_delta_list_record_number.Text = lst_delta.ListCount
   txt_pulled_iterations_value.Text = txt_pulled_iterations_value.Text + 1
   
   Erase vecData
    
End Sub

Private Sub OrdersRezefStartPull_Click()
         'clear the output lists
        ClearLists
        
        CreateTaskBar
        
         'zero last time control variable
        strLastTime = "00000000"
        
        'start the refresh timer
        DisableAllTimers
        OrdersRezef_Timer1.Enabled = True
        OrdersRezef_Timer1.Interval = 500
           
        txt_status_ind.Caption = "ON"
        txt_status_ind.BackColor = RGB(128, 255, 128)
        txt_pulled_iterations_value.Text = 0

End Sub

Private Sub Rezef_Timer1_Timer()
    Dim vecData() As K300RzfType 'query results
    Dim vecStr() As String       'query results for raw data
    Dim nRecords As Integer     'number of records returned
    Dim i As Integer            'record iterator
    Dim lst As ListBox          'the list-box to output to
    
    'create the supporting TaskBarLib object if neccessery
    CreateTaskBar
  
   'choose which list to output to
    Set lst = IIf(strLastTime = "00000000", lst_full, lst_delta)
    
    If RezefRawData = False Then
        nRecords = FMRK300.GetRezef(vecData, strLastTime, txt_option.Text, Me.Combo_Madad.ItemData(Me.Combo_Madad.ListIndex))
    Else
        nRecords = FMRK300.GetRezefRaw(vecStr, strLastTime, , Me.Combo_Madad.ItemData(Me.Combo_Madad.ListIndex))
    End If
     
    If (nRecords > 0) Then
        
        'go through records, output to list and look for the most recent update time
        For i = 0 To nRecords - 1
            If RezefRawData = False Then
                lst.AddItem vecData(i).BNO_Num & " " & vecData(i).BNO_NAME & " " & vecData(i).UPD_TIME & " " & vecData(i).LST_DL_PR & " " & vecData(i).shlav
            Else
                lst.AddItem vecStr(i)
            End If
        Next
    
    End If
    
    
    txt_last_time.Text = strLastTime
    txt_full_list_record_number.Text = lst_full.ListCount
    txt_delta_list_record_number.Text = lst_delta.ListCount
    
    txt_pulled_iterations_value.Text = txt_pulled_iterations_value.Text + 1
    
    Erase vecData
    Erase vecStr
End Sub

Private Sub RezefCNT_Timer1_Timer()
    Dim vecData() As String 'query results
    Dim nRecords As Integer     'number of records returned
    Dim i As Integer            'record iterator
    Dim lst As ListBox          'the list-box to output to
    Dim RecType As String
    Dim CurRec As String
    
    CreateTaskBar
    
    nRecords = FMRK300.GetRzfCNT(vecData, "VBALL", txt_branch.Text, txt_account.Text, strOrdersLastTime, strK300LastTime, " ", " ")
       
    'choose which list to output to
    Set lst = IIf(strK300LastTime = "00000000", lst_full, lst_delta)
    
    If (nRecords > 0) Then
        'go through records, output to list and look for the most recent update time
        For i = 0 To nRecords - 1
            lst.AddItem vecData(i)
            CurRec = Space(10) & vecData(i)
            RecType = Mid(CurRec, 11, 1)
                If RecType = "M" Or RecType = "B" Or RecType = "D" Then
                    strK300LastTime = Mid(CurRec, 12, 8)
                      If Trim(strK300LastTime) <> "" Then
                        If CLng(strK300LastTime) > nK300LastTime Then
                            nK300LastTime = CLng(strK300LastTime)
                        End If
                   End If
                End If
                
                If RecType = "O" Then
                    strOrdersLastTime = Mid(CurRec, 12, 8)
                   If Trim(strOrdersLastTime) <> "" Then
                        If CLng(strOrdersLastTime) > nOrdersLastTime Then
                            nOrdersLastTime = CLng(strOrdersLastTime)
                        End If
                   End If
                End If
        Next
        txt_last_time.Text = strK300LastTime & "," & strOrdersLastTime
    End If
    
    txt_full_list_record_number.Text = lst_full.ListCount
    txt_delta_list_record_number.Text = lst_delta.ListCount
    
    txt_pulled_iterations_value.Text = txt_pulled_iterations_value.Text + 1
End Sub

Private Sub Send_Order_Click()
    SendRezefOrder_Click
End Sub

Private Sub SendRezefOrder_Click()
    Dim basic_order As RezefBasicOrder
    Dim VBMsg As String
    Dim ErrNo As Long
    Dim OrderID As Long
    Dim Continue As Boolean
    Dim AuthUser As String
    Dim AuthPassword As String
    Dim ReEnteredValue As String
       
     
    Dim rc As Integer
    Dim ErrorType As OrdersErrorTypes
        basic_order.Account = Me.txt_Ord_Account.Text
        'basic_order.Branch = "061"
        basic_order.Branch = Me.SendRBranch.Text
        basic_order.Asmachta_fmr = "        "
        basic_order.Asmachta_rezef = "        "
        basic_order.Operation = "N"
        basic_order.ORDR_TYPE = "0000"

        basic_order.ammount = Format(Val(Me.Amount), "000000000")
        basic_order.op = UCase(Trim$(Me.txt_rezef_cod_upd))
        basic_order.Order_Type = Me.Order_Type
        basic_order.price = Val(Me.Limit)
        basic_order.Stock_Number = Format(Me.Stock_Number, "00000000")
        basic_order.Operation = Me.Operation
        basic_order.Asmachta_fmr = txt_rezef_asmachta_fmr.Text
        basic_order.Asmachta_rezef = txt_rezef_asmachta_rezef.Text
        
        '
         'create the supporting TaskBarLib object if neccessery
        CreateTaskBar
       
       
       OrderID = 0
        Continue = True
        
        rc = FMRUser.SendRezefOrder(SessionId, basic_order, "CNT", "REZEF", VBMsg, ErrNo, ErrorType, OrderID, AuthUser, AuthPassword, ReEnteredValue)
        Do While ((ErrNo <> 0) And (Continue = True))
          Select Case ErrorType
            
            Case Fatal
                MsgBox VBMsg
                Continue = False
            
            Case Alert
                MsgBox VBMsg Or vbOKOnly
                Continue = True
            
            Case Confirmation
                rc = MsgBox(VBMsg, vbYesNo)
                If rc = vbYes Then
                    ReEnteredValue = "Yes"
                Else
                    ReEnteredValue = "No"
                End If
                
                rc = FMRUser.SendRezefOrder(SessionId, basic_order, "CNT", "REZEF", VBMsg, ErrNo, ErrorType, OrderID, AuthUser, AuthPassword, ReEnteredValue)
            
            Case PasswordReq
                
                MsgBox VBMsg
                AuthUser = InputBox("Please Enter Auth User Name")
                AuthPassword = InputBox("Please Enter Auth Password")
                rc = FMRUser.SendRezefOrder(SessionId, basic_order, "CNT", "REZEF", VBMsg, ErrNo, ErrorType, OrderID, AuthUser, AuthPassword, ReEnteredValue)
            
            Case ReEnter
                MsgBox VBMsg
                ReEnteredValue = InputBox("Please Enter the value again")
                rc = FMRUser.SendRezefOrder(SessionId, basic_order, "CNT", "REZEF", VBMsg, ErrNo, ErrorType, OrderID, AuthUser, AuthPassword, ReEnteredValue)
         
         End Select
                
        Loop
        
       If ErrNo = 0 Then
            MsgBox ("Order" & OrderID & " was submitted successfully.")
            txt_rezef_asmachta_fmr.Text = basic_order.Asmachta_fmr
       End If
            
   
End Sub

Private Sub SendMaofOrder_Click()
    frmMaofOrder.Show vbModal
    If frmMaofOrder.ButtonPressed = vbCancel Then
        frmMaofOrder.Visible = False
        Exit Sub
    End If
    
End Sub

Private Sub SendNewRezefOrder_Click()
    
    'Set FormsCollection(FormsCollectionCount) = New frmRezefOrder
    'FormsCollectionCount = FormsCollectionCount + 1
    'FormsCollection(FormsCollectionCount - 1).Show , Me
    frmRezefOrder.Show 'vbModal
End Sub

Private Sub ShortAccountsGetAll_Click()
    Dim vecData() As ShortAccountType
    Dim nRecords As Integer     'number of records returned
    Dim i As Integer            'record iterator
    Dim lst As ListBox          'the list-box to output to
    Dim errmsg As String
          
       'clear the output lists
    ClearLists
    DisableAllTimers
    Set lst = lst_full
    CreateTaskBar
   
   'make the query using the TaskBarLib User object
    nRecords = FMRUser.GetShortAccounts(SessionId, txt_query.Text, vecData, errmsg)
    
    If Trim(errmsg) <> "" Then
        MsgBox (errmsg)
    Else
          
          If nRecords > 0 Then
                
                For i = LBound(vecData) To UBound(vecData)
                    lst.AddItem vecData(i).Tik & " " & vecData(i).Branch & " " & vecData(i).Sug & " " & vecData(i).ID & " " & vecData(i).AccountName & " " & vecData(i).MaofCode & " " & vecData(i).FutureCode & " " & vecData(i).BnoCode & " " & vecData(i).CashCode
                    StatusBar.Text = "Updating Index List - Row " & i & " of " & UBound(vecData)
               Next
               StatusBar.Text = "Updating Index  List - Row " & i & " of " & UBound(vecData) + 1
          Else
               StatusBar.Text = "No records were found for the requested query."
        End If
    
    End If
    
End Sub


Private Sub ShortStocksGetAll_Click()
    Dim rc As Integer
    Dim Result As String
    Dim Shorts() As ShortStockType
    Dim i As Integer            'record iterator
    Dim lst As ListBox          'the list-box to output to
    
   CreateTaskBar
    
    'clear the output lists
    ClearLists
    DisableAllTimers
    
     Set lst = lst_full
          
    If FMRShortStocks Is Nothing Then
        Set FMRShortStocks = New IShortStocks
        If FMRShortStocks Is Nothing Then
            MsgBox "כשלון ביצירת ממשק", vbCritical
            Exit Sub
        End If
    End If
    
    rc = FMRShortStocks.GetAllSafeShortStocks(Shorts)
    For i = 1 To UBound(Shorts)
        lst.AddItem Shorts(i).Isin & " " & Shorts(i).Stock & " " & Shorts(i).Name_Stock & " " & Shorts(i).Name2 & " " & Shorts(i).Symbol & " " & Shorts(i).Sug
        If i Mod 20 = 0 Then
                   StatusBar.Text = "Updating ShortStocks List - Row " & i & " of " & UBound(Shorts)
        End If
    Next
    StatusBar.Text = "Updating ShortStocks List - Row " & i - 1 & " of " & UBound(Shorts)
    
    Erase Shorts
End Sub

Private Sub ShortStocksGetSingle_Click()
    Dim a As IShortStocks
    Dim rc As Integer
    Dim Result As String
    Dim Shorts() As ShortStockType
    Dim lst As ListBox          'the list-box to output to
    
    CreateTaskBar
    
    'clear the output lists
    ClearLists
    DisableAllTimers
    
     Set lst = lst_full
          
    If FMRShortStocks Is Nothing Then
        Set FMRShortStocks = New IShortStocks
        If FMRShortStocks Is Nothing Then
            MsgBox "כשלון ביצירת ממשק", vbCritical
            Exit Sub
        End If
    End If
    
    rc = FMRShortStocks.GetSafeStock(txt_option.Text, Shorts)
    If rc = 1 Then
          lst.AddItem Shorts(0).Isin & " " & Shorts(0).Stock & " " & Shorts(0).Name_Stock & " " & Shorts(0).Name2 & " " & Shorts(0).Symbol & " " & Shorts(0).Sug
    Else
         StatusBar.Text = "No records were found for the requested query."
    End If
    
    Erase Shorts
   
End Sub


Private Sub ShortTradeOptions_Click()
    Dim vecData() As TradeOptionType
    Dim rc As Long
    Dim i As Integer
     
    Call CreateTaskBar
    rc = FMRK300.GetShortTradeOptions(vecData)
    If rc > 0 Then
        lst_full.Clear
        For i = LBound(vecData) To UBound(vecData)
            lst_full.AddItem (vecData(i).Bno & " " & vecData(i).BnoName & " " & vecData(i).ExpDate & " " & vecData(i).ExpPrice)
        Next i
    End If
End Sub

Private Sub StartOnlineUserSession_Click()
    Dim rc As Long
    CreateTaskBar
    
    rc = FMRUser.StartOnlineUserSession(SessionId, OnlineSessionTypeKranot)
    If rc <> 0 Then
        MsgBox "אתחול נכשל", vbCritical
        Exit Sub
    End If
End Sub



Private Sub StartPull_Click(index As Integer)
        'zero last time control variable
        strLastTime = "00000000"
                
        MaofRawData = False
        
        FirstTime = True
        'start the refresh timer
        DisableAllTimers
        Maof_Timer1.Enabled = True

       txt_status_ind.Caption = "ON"
       txt_status_ind.BackColor = RGB(128, 255, 128)
       txt_pulled_iterations_value.Text = 0
        
        'create the supporting TaskBarLib object if neccessery
        CreateTaskBar

        'clear the output lists
        ClearLists
End Sub

Private Sub StatisticsInfo_Click()
    Dim vecData() As K300STSType 'query results
    Dim nRecords As Integer     'number of records returned
    Dim i As Integer            'record iterator
    Dim lst As ListBox          'the list-box to output to
          
   'clear the output lists
    ClearLists
    DisableAllTimers
    
    Set lst = lst_full
        
    CreateTaskBar
   
   'make the query using the TaskBarLib User object
    nRecords = FMRK300.GetStatistics(vecData)
     
   If nRecords > 0 Then
         
         For i = LBound(vecData) To UBound(vecData)
             lst.AddItem vecData(i).BNO_DN & " " & vecData(i).BNO_STT & " " & vecData(i).BNO_UP & " " & vecData(i).DIL_NO & " " & vecData(i).SUG_REC & " " & vecData(i).SYS_TIME & " " & vecData(i).TOT_MAHZOR & " " & vecData(i).TRADE_METH & " " & vecData(i).UPD_DAT & " " & vecData(i).UPD_TIME & " " & vecData(i).VL_NIS_DN & " " & vecData(i).VL_NIS_MKT & " " & vecData(i).VL_NIS_STT & " " & vecData(i).VL_NIS_UP
             StatusBar.Text = "Updating List - Row " & i & " of " & UBound(vecData)
        Next
        StatusBar.Text = "Updating List - Row " & i - 1 & " of " & UBound(vecData)
Else
        StatusBar.Text = "No records were found for the requested query."
 End If
 
 Erase vecData
End Sub

Private Sub StockHistory_Click()
Dim vecData() As String     'query results
    Dim nRecords As Integer     'number of records returned
    Dim i As Integer            'record iterator
    Dim lst As ListBox          'the list-box to output to
          
   'clear the output lists
    ClearLists
    DisableAllTimers
    
    Set lst = lst_full
        
    CreateTaskBar
   
   'make the query using the TaskBarLib User object
    nRecords = FMRK300.GetStockHistory(vecData, txt_option.Text)
     
   If nRecords > 0 Then
         For i = LBound(vecData) To UBound(vecData)
             lst.AddItem vecData(i)
             StatusBar.Text = "Updating List - Row " & i & " of " & UBound(vecData)
        Next
        StatusBar.Text = "Updating List - Row " & i - 1 & " of " & UBound(vecData)
  Else
        StatusBar.Text = "No records were found for the requested query."
  End If
 
 Erase vecData
End Sub

Private Sub StockRezef_Click()
 Dim vecData() As String     'query results
    Dim nRecords As Integer     'number of records returned
    Dim i As Integer            'record iterator
    Dim lst As ListBox          'the list-box to output to
          
   'clear the output lists
    ClearLists
    DisableAllTimers
    
    Set lst = lst_full
        
    CreateTaskBar
   
   'make the query using the TaskBarLib User object
    nRecords = FMRK300.GetStocksRZF(vecData)
     
   If nRecords > 0 Then
         
         For i = LBound(vecData) To UBound(vecData)
             lst.AddItem vecData(i)
             StatusBar.Text = "Updating List - Row " & i & " of " & UBound(vecData)
        Next
        StatusBar.Text = "Updating List - Row " & i - 1 & " of " & UBound(vecData)
Else
        StatusBar.Text = "No records were found for the requested query."
 End If
 
 Erase vecData
 
End Sub

Private Sub StocksInIndex_Click()
Dim vecData() As StockPartInIndexType
    Dim nRecords As Integer     'number of records returned
    Dim i As Integer            'record iterator
    Dim lst As ListBox          'the list-box to output to
          
      'clear the output lists
    ClearLists
    DisableAllTimers
    Set lst = lst_full
    CreateTaskBar
   
   'make the query using the TaskBarLib User object
    nRecords = FMRK300.GetMaofStocks(vecData(), Me.Combo_Madad.ItemData(Me.Combo_Madad.ListIndex))
           
   If nRecords > 0 Then
         
         For i = LBound(vecData) To UBound(vecData)
             lst.AddItem vecData(i).BNO_NAME & " " & vecData(i).BNO_NO & " " & vecData(i).PCNT
             StatusBar.Text = "Updating List - Row " & i & " of " & UBound(vecData)
        Next
        StatusBar.Text = "Updating List - Row " & i - 1 & " of " & UBound(vecData)
   Else
        StatusBar.Text = "No records were found for the requested query."
   End If
 
 Erase vecData
End Sub

Private Sub StockStage_Click()
Dim vecData() As StockStageType 'query results
    Dim nRecords As Integer     'number of records returned
    Dim i As Integer            'record iterator
    Dim lst As ListBox          'the list-box to output to
          
       'clear the output lists
    ClearLists
    DisableAllTimers
    Set lst = lst_full
    CreateTaskBar
   
   'make the query using the TaskBarLib User object
    nRecords = FMRK300.GetStockStage(vecData, txt_option.Text)
     
   If nRecords > 0 Then
         
         For i = LBound(vecData) To UBound(vecData)
             lst.AddItem vecData(i).BNO_NO & " " & vecData(i).BASE_RATE_STG & " " & vecData(i).RSN_TRADE_STP & " " & vecData(i).STOCK_STATE & " " & vecData(i).TM_TRADE_STP & " " & vecData(i).TRADE_METH & " " & vecData(i).TRADE_STAGE
             StatusBar.Text = "Updating List - Row " & i & " of " & UBound(vecData) + 1
        Next
        StatusBar.Text = "Updating List - Row " & i & " of " & UBound(vecData) + 1
   Else
        StatusBar.Text = "No records were found for the requested query."
 End If
 
 Erase vecData
End Sub

Private Sub StockValue_Click()
 Dim vecData() As String     'query results
    Dim nRecords As Integer     'number of records returned
    Dim i As Integer            'record iterator
    Dim lst As ListBox          'the list-box to output to
          
   'clear the output lists
    ClearLists
    DisableAllTimers
    
    Set lst = lst_full
        
    CreateTaskBar
   
   'make the query using the TaskBarLib User object
    nRecords = FMRK300.GetStockValue(vecData)
     
   If nRecords > 0 Then
         
         For i = LBound(vecData) To UBound(vecData)
             lst.AddItem vecData(i)
             StatusBar.Text = "Updating List - Row " & i & " of " & UBound(vecData) + 1
        Next
        StatusBar.Text = "Updating List - Row " & i & " of " & UBound(vecData) + 1
   Else
        StatusBar.Text = "No records were found for the requested query."
 End If
 
 Erase vecData
End Sub


Private Sub StopOnlineUserSession_Click()
    Dim rc As Long
    rc = FMRUser.StopOnlineUserSession(SessionId, OnlineSessionTypeKranot)
    If rc <> 0 Then
        MsgBox "הפסקת זרימה נכשלה", vbCritical
        Exit Sub
    End If
End Sub

Private Sub FMRK300_FireMaof(vecData() As K300MaofType, nRecords As Long)

    Dim i As Integer            'record iterator
    Dim lst As ListBox          'the list-box to output to
       
    If nRecords > 0 Then
              
            'choose which list to output to
           If txt_full_list_record_number.Text = 0 Then
            Set lst = lst_full
           Else
            Set lst = lst_delta
          End If
                                  
        If (nRecords > 0) Then
            'go through records, output to list and look for the most recent update time
            For i = 0 To nRecords - 1
                lst.AddItem vecData(i).BNO_Num & " " & vecData(i).UPD_TIME & " " & vecData(i).EX_DATE & " " & vecData(i).DAY_VL
                strLastTime = vecData(i).UPD_TIME
                If Trim(strLastTime) <> "" Then
                    If CLng(strLastTime) > nLastTime Then _
                        nLastTime = CLng(strLastTime)
                End If
            Next
            strLastTime = CStr(nLastTime)
            txt_last_time.Text = strLastTime
        End If
    
     txt_full_list_record_number.Text = lst_full.ListCount
     txt_delta_list_record_number.Text = lst_delta.ListCount
    
     txt_pushed_iterations_value.Text = txt_pushed_iterations_value.Text + 1
    End If
End Sub

Private Sub FMRK300_FireMaofCNT(vecData() As String, nRecords As Long)

    Dim i As Integer            'record iterator
    Dim lst As ListBox          'the list-box to output to
    Dim CurRec As String
    Dim RecType As String
    
    If nRecords > 0 Then
              
            'choose which list to output to
           If txt_full_list_record_number.Text = 0 Then
            Set lst = lst_full
           Else
            Set lst = lst_delta
          End If
                                  
     If (nRecords > 0) Then
        'go through records, output to list and look for the most recent update time
        For i = 0 To nRecords - 1
            lst.AddItem vecData(i)
            CurRec = Space(10) & vecData(i)
            RecType = Mid(CurRec, 11, 1)
                If RecType = "M" Or RecType = "B" Or RecType = "D" Then
                    strK300LastTime = Mid(CurRec, 12, 8)
                      If Trim(strK300LastTime) <> "" Then
                        If CLng(strK300LastTime) > nK300LastTime Then
                            nK300LastTime = CLng(strK300LastTime)
                        End If
                   End If
                End If
                
                If RecType = "O" Then
                    strOrdersLastTime = Mid(CurRec, 12, 8)
                   If Trim(strOrdersLastTime) <> "" Then
                        If CLng(strOrdersLastTime) > nOrdersLastTime Then
                            nOrdersLastTime = CLng(strOrdersLastTime)
                        End If
                   End If
                End If
        Next
        txt_last_time.Text = strK300LastTime & "," & strOrdersLastTime

 End If
    
    
     txt_full_list_record_number.Text = lst_full.ListCount
     txt_delta_list_record_number.Text = lst_delta.ListCount
    
     txt_pushed_iterations_value.Text = txt_pushed_iterations_value.Text + 1
    End If
End Sub


Private Sub FMRK300_FireRezef(vecData() As K300RzfType, nRecords As Long)

    Dim i As Integer            'record iterator
    Dim lst As ListBox          'the list-box to output to
       
    If nRecords > 0 Then
              
            'choose which list to output to
           If txt_full_list_record_number.Text = 0 Then
            Set lst = lst_full
           Else
            Set lst = lst_delta
          End If
                                  
        If (nRecords > 0) Then
            'go through records, output to list and look for the most recent update time
            For i = 0 To nRecords - 1
               lst.AddItem vecData(i).BNO_Num & " " & vecData(i).UPD_TIME & " " & vecData(i).LST_DL_PR
                strLastTime = vecData(i).UPD_TIME
                If Trim(strLastTime) <> "" Then
                    If CLng(strLastTime) > nLastTime Then _
                        nLastTime = CLng(strLastTime)
                End If
            Next
            strLastTime = CStr(nLastTime)
            txt_last_time.Text = strLastTime
        End If
    
     txt_full_list_record_number.Text = lst_full.ListCount
     txt_delta_list_record_number.Text = lst_delta.ListCount
    
     txt_pushed_iterations_value.Text = txt_pushed_iterations_value.Text + 1
    End If
End Sub

Private Sub FMRK300_FireRezefCNT(vecData() As String, nRecords As Long)

    Dim i As Integer            'record iterator
    Dim lst As ListBox          'the list-box to output to
    Dim CurRec As String
    Dim RecType As String
    
    If nRecords > 0 Then
              
            'choose which list to output to
           If txt_full_list_record_number.Text = 0 Then
            Set lst = lst_full
           Else
            Set lst = lst_delta
          End If
                                  
     If (nRecords > 0) Then
        'go through records, output to list and look for the most recent update time
        For i = 0 To nRecords - 1
            lst.AddItem vecData(i)
            CurRec = Space(10) & vecData(i)
            RecType = Mid(CurRec, 11, 1)
                If RecType = "M" Or RecType = "B" Or RecType = "D" Then
                    strK300LastTime = Mid(CurRec, 12, 8)
                      If Trim(strK300LastTime) <> "" Then
                        If CLng(strK300LastTime) > nK300LastTime Then
                            nK300LastTime = CLng(strK300LastTime)
                        End If
                   End If
                End If
                
                If RecType = "O" Then
                    strOrdersLastTime = Mid(CurRec, 12, 8)
                   If Trim(strOrdersLastTime) <> "" Then
                        If CLng(strOrdersLastTime) > nOrdersLastTime Then
                            nOrdersLastTime = CLng(strOrdersLastTime)
                        End If
                   End If
                End If
        Next
        txt_last_time.Text = strK300LastTime & "," & strOrdersLastTime

 End If
    
    
     txt_full_list_record_number.Text = lst_full.ListCount
     txt_delta_list_record_number.Text = lst_delta.ListCount
    
     txt_pushed_iterations_value.Text = txt_pushed_iterations_value.Text + 1
    End If
    
    Erase vecData()
    
End Sub

Public Sub AddToStream(streamID As Long)
    If txt_active_stream.Text = "0" Then
        txt_active_stream.Text = streamID
    Else
        txt_active_stream.Text = txt_active_stream.Text & "," & streamID
    End If
'   Dim streamarr() As Long
'   streamarr = Split(txt_active_stream.Text, ",")
'   ReDim Preserve streamarr(streamarr.GetUpperBound(0) + 1)
'   streamarr(GetUpperBound(0)) = streamID
'   txt_active_stream.Text = Join(streamarr, ",")
End Sub

Public Sub RemoveFromStream(streamID As Long)
    txt_active_stream.Text = Replace(txt_active_stream.Text, streamID & ",", "")
    txt_active_stream.Text = Replace(txt_active_stream.Text, "," & streamID, "")
End Sub

Public Sub ClearLists()
        lst_full.Clear
        lst_delta.Clear
        txt_full_list_record_number.Text = 0
        txt_delta_list_record_number.Text = 0
        txt_pulled_iterations_value.Text = 0
        txt_pushed_iterations_value.Text = 0
        txt_full_list_record_number.Text = 0
        txt_delta_list_record_number.Text = 0
        txt_last_time.Text = 0
        StatusBar.Text = ""
End Sub


Public Sub CreateTaskBar()

    If FMRK300 Is Nothing Then
        Set FMRK300 = New TaskBarLib.K300
        If SessionId <> 0 Then
            FMRK300.K300SessionId = SessionId
        End If
        If FMRK300 Is Nothing Then
             MsgBox "failed to create K300 object"
         End If
    End If
   
    If FMRFunds Is Nothing Then
        Set FMRFunds = New TaskBarLib.Funds
        If FMRFunds Is Nothing Then
             MsgBox "failed to create Funds object"
             Exit Sub
         End If
    End If
   
    If FMRUser Is Nothing Then
        Set FMRUser = New TaskBarLib.User
        If FMRUser Is Nothing Then
             MsgBox "failed to create User object"
             Exit Sub
         End If
    End If
          
    If DirectDlv Is Nothing Then
        Set DirectDlv = New TaskBarLib.DirectDlv
        If DirectDlv Is Nothing Then
             MsgBox "failed to create DirectDlv object"
             Exit Sub
         End If
    End If
    
    If FMRConfig Is Nothing Then
        Set FMRConfig = New TaskBarLib.Config
        If FMRConfig Is Nothing Then
            MsgBox "Failed to create Config object"
            Exit Sub
        End If
    End If
        

End Sub


Public Sub DisableAllTimers()
    Maof_Timer1.Enabled = False
    Rezef_Timer1.Enabled = False
    OrdersMaof_Timer1.Enabled = False
    OrdersRezef_Timer1.Enabled = False
    MaofCNT_Timer1.Enabled = False
    RezefCNT_Timer1.Enabled = False
End Sub

Private Sub tmrCntMaof_Timer()
    Dim rc As Long
    Dim vecK300() As K300MaofType
    Dim vecOrders() As MOFINQType
    Dim vecK300Len As Long
    Dim vecOrdersLen As Long
    Dim i As Long
    
    rc = FMRUser.GetMaofCnt(SessionId, vecK300, vecK300Len, CNTK300LastTime, "00000000", BaseAssetMaof, AllMonths, _
        vecOrders, vecOrdersLen, txt_account.Text, txt_branch.Text, CNTOrdersLastTime)
    
    If vecK300Len > 0 Then
        For i = LBound(vecK300) To UBound(vecK300)
            lst_full.AddItem (vecK300(i).BNO_NAME & "  " & vecK300(i).LST_DL_PR)
        Next i
        txt_full_list_record_number.Text = lst_full.ListCount
    End If
    
    If vecOrdersLen > 0 Then
        For i = LBound(vecOrders) To UBound(vecOrders)
            lst_delta.AddItem (vecOrders(i).BNO_NAME & "  " & vecOrders(i).ORDR_NV_PIC & "  " & vecOrders(i).ORDR_PRC_PIC)
        Next i
        txt_delta_list_record_number.Text = lst_delta.ListCount
    End If
End Sub

Private Sub TradeOptions_Click()
Dim vecData() As String     'query results
    Dim nRecords As Long     'number of records returned
    Dim i As Integer            'record iterator
    Dim lst As ListBox          'the list-box to output to
          
   'clear the output lists
    ClearLists
    DisableAllTimers
    
    Set lst = lst_full
        
    CreateTaskBar
   
   'make the query using the TaskBarLib User object
  FMRK300.GetTradeOptions vecData, nRecords
     
   If nRecords > 0 Then
         
         For i = LBound(vecData) To UBound(vecData)
             lst.AddItem vecData(i)
             StatusBar.Text = "Updating List - Row " & i & " of " & UBound(vecData)
        Next
        StatusBar.Text = "Updating List - Row " & i - 1 & " of " & UBound(vecData)
Else
        StatusBar.Text = "No records were found for the requested query."
 End If
 
 Erase vecData
End Sub

Private Sub txt_Ord_Account_GotFocus()
    txt_Ord_Account.SelStart = 0
    txt_Ord_Account.SelLength = Len(txt_Ord_Account.Text)
End Sub

Private Sub SendRBranch_GotFocus()
    SendRBranch.SelStart = 0
    SendRBranch.SelLength = Len(SendRBranch.Text)
End Sub

Private Sub Stock_Number_GotFocus()
    Stock_Number.SelStart = 0
    Stock_Number.SelLength = Len(Stock_Number.Text)
End Sub

Private Sub Amount_GotFocus()
    Amount.SelStart = 0
    Amount.SelLength = Len(Amount.Text)
End Sub

Private Sub Limit_GotFocus()
    Limit.SelStart = 0
    Limit.SelLength = Len(Limit.Text)
End Sub

Private Sub Operation_GotFocus()
    Operation.SelStart = 0
    Operation.SelLength = Len(Operation.Text)
End Sub

Private Sub txt_rezef_cod_upd_GotFocus()
    txt_rezef_cod_upd.SelStart = 0
    txt_rezef_cod_upd.SelLength = Len(txt_rezef_cod_upd.Text)
End Sub

Private Sub txt_rezef_asmachta_fmr_GotFocus()
    txt_rezef_asmachta_fmr.SelStart = 0
    txt_rezef_asmachta_fmr.SelLength = Len(txt_rezef_asmachta_fmr.Text)
End Sub

Private Sub txt_rezef_asmachta_rezef_GotFocus()
    txt_rezef_asmachta_rezef.SelStart = 0
    txt_rezef_asmachta_rezef.SelLength = Len(txt_rezef_asmachta_rezef.Text)
End Sub




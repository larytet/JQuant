VERSION 5.00
Object = "{5E9E78A0-531B-11CF-91F6-C2863C385E30}#1.0#0"; "MSFLXGRD.OCX"
Begin VB.Form frmMaofOrder 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Dialog Caption"
   ClientHeight    =   8085
   ClientLeft      =   1185
   ClientTop       =   2175
   ClientWidth     =   11835
   ForeColor       =   &H8000000F&
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   8085
   ScaleWidth      =   11835
   ShowInTaskbar   =   0   'False
   StartUpPosition =   1  'CenterOwner
   Begin VB.TextBox txtScenarioIndex 
      Alignment       =   2  'Center
      BackColor       =   &H80000018&
      BorderStyle     =   0  'None
      Height          =   255
      Left            =   6360
      TabIndex        =   66
      Top             =   1680
      Width           =   735
   End
   Begin VB.TextBox txtMadadInScenario 
      Alignment       =   2  'Center
      BackColor       =   &H80000018&
      BorderStyle     =   0  'None
      Height          =   255
      Left            =   6360
      TabIndex        =   61
      Top             =   1320
      Width           =   735
   End
   Begin VB.Timer tmrOrders 
      Enabled         =   0   'False
      Left            =   840
      Top             =   3480
   End
   Begin VB.CheckBox chkSendConfirm 
      Alignment       =   1  'Right Justify
      Caption         =   "אישור שליחה"
      Height          =   255
      Left            =   6240
      RightToLeft     =   -1  'True
      TabIndex        =   59
      Top             =   7560
      Width           =   1455
   End
   Begin VB.CheckBox chkSelectAllOrders 
      Alignment       =   1  'Right Justify
      Caption         =   "בחר הכל"
      Height          =   255
      Left            =   4800
      TabIndex        =   58
      Top             =   7800
      Width           =   1215
   End
   Begin VB.CheckBox chkSpeedFromOrders 
      Alignment       =   1  'Right Justify
      Caption         =   " ספיד מחלון הוראות"
      Height          =   255
      Left            =   4080
      TabIndex        =   57
      Top             =   7560
      Width           =   1935
   End
   Begin VB.CommandButton btnSpeed 
      Caption         =   "&ספיד"
      Height          =   375
      Left            =   8640
      TabIndex        =   56
      Top             =   3720
      Width           =   1215
   End
   Begin VB.CheckBox chkPeleOrder 
      Alignment       =   1  'Right Justify
      Caption         =   "פלא"
      Height          =   255
      Left            =   8760
      TabIndex        =   55
      Top             =   840
      Width           =   735
   End
   Begin VB.CommandButton btnClearOrders 
      Caption         =   "  הוראות &נקה"
      Height          =   375
      Left            =   1440
      TabIndex        =   52
      Top             =   7560
      Visible         =   0   'False
      Width           =   1215
   End
   Begin VB.TextBox txtMadad 
      Alignment       =   2  'Center
      BackColor       =   &H80000018&
      BorderStyle     =   0  'None
      Height          =   255
      Left            =   6360
      TabIndex        =   34
      Top             =   720
      Width           =   735
   End
   Begin VB.ComboBox comboPeleAsset 
      Height          =   315
      ItemData        =   "MaofOrder.frx":0000
      Left            =   6960
      List            =   "MaofOrder.frx":0002
      Style           =   2  'Dropdown List
      TabIndex        =   31
      Top             =   240
      Width           =   1215
   End
   Begin VB.CommandButton btnStatus 
      Caption         =   "&מצב"
      Height          =   375
      Left            =   7200
      TabIndex        =   33
      Top             =   2520
      Width           =   975
   End
   Begin MSFlexGridLib.MSFlexGrid lstOrders 
      Height          =   3015
      Left            =   120
      TabIndex        =   29
      Top             =   4320
      Width           =   11535
      _ExtentX        =   20346
      _ExtentY        =   5318
      _Version        =   393216
      Rows            =   1
      Cols            =   0
      FixedCols       =   0
      HighLight       =   0
      GridLinesFixed  =   1
      AllowUserResizing=   1
   End
   Begin VB.TextBox txtTik 
      Height          =   285
      Left            =   8520
      TabIndex        =   6
      Top             =   240
      Width           =   375
   End
   Begin VB.CommandButton btnOrders 
      Caption         =   "&רענון הוראות"
      Height          =   375
      Left            =   2760
      TabIndex        =   28
      Top             =   7560
      Visible         =   0   'False
      Width           =   1215
   End
   Begin VB.CommandButton btnMaofSession 
      Caption         =   "&החל מסחר"
      Height          =   375
      Left            =   10440
      TabIndex        =   26
      Top             =   7560
      Width           =   1215
   End
   Begin VB.TextBox txtAsmachtaFmr 
      Height          =   285
      Left            =   8640
      TabIndex        =   22
      Top             =   3360
      Width           =   855
   End
   Begin VB.TextBox txtAsmachtaRzf 
      Height          =   285
      Left            =   8640
      TabIndex        =   20
      Top             =   3000
      Width           =   855
   End
   Begin VB.ComboBox comboSugPeula 
      Height          =   315
      ItemData        =   "MaofOrder.frx":0004
      Left            =   8640
      List            =   "MaofOrder.frx":0011
      Style           =   2  'Dropdown List
      TabIndex        =   18
      Top             =   2640
      Width           =   855
   End
   Begin VB.ComboBox comboSugHoraa 
      Height          =   315
      ItemData        =   "MaofOrder.frx":001E
      Left            =   8640
      List            =   "MaofOrder.frx":002B
      Style           =   2  'Dropdown List
      TabIndex        =   15
      Top             =   2280
      Width           =   855
   End
   Begin VB.TextBox txtPrice 
      Height          =   285
      Left            =   8640
      TabIndex        =   14
      Top             =   1920
      Width           =   855
   End
   Begin VB.TextBox txtAmmount 
      Height          =   285
      Left            =   8640
      TabIndex        =   12
      Top             =   1560
      Width           =   855
   End
   Begin VB.TextBox txtOpNumber 
      Height          =   285
      Left            =   8640
      TabIndex        =   10
      Top             =   1200
      Width           =   855
   End
   Begin VB.OptionButton rdSell 
      Alignment       =   1  'Right Justify
      Caption         =   "מ"
      Height          =   255
      Left            =   10440
      TabIndex        =   8
      Top             =   840
      Width           =   375
   End
   Begin VB.OptionButton rdBuy 
      Alignment       =   1  'Right Justify
      Caption         =   "ק"
      Height          =   255
      Left            =   11040
      TabIndex        =   7
      Top             =   840
      Width           =   375
   End
   Begin VB.TextBox txtBranch 
      Height          =   285
      Left            =   9480
      TabIndex        =   4
      Top             =   240
      Width           =   375
   End
   Begin VB.TextBox txtAccount 
      Height          =   285
      Left            =   10560
      TabIndex        =   2
      Top             =   240
      Width           =   735
   End
   Begin VB.CommandButton CancelButton 
      Caption         =   "&צא"
      Height          =   375
      Left            =   120
      TabIndex        =   0
      Top             =   7560
      Width           =   1215
   End
   Begin VB.CommandButton btnSend 
      Caption         =   "&שלח"
      Height          =   375
      Left            =   10320
      TabIndex        =   23
      Top             =   3720
      Width           =   1215
   End
   Begin VB.Line Line5 
      X1              =   120
      X2              =   8400
      Y1              =   3000
      Y2              =   3000
   End
   Begin VB.Label lbMadadInScenraioTxt 
      Alignment       =   1  'Right Justify
      Caption         =   "מדד בתרחיש"
      Height          =   255
      Left            =   7200
      TabIndex        =   60
      Top             =   1320
      Width           =   975
   End
   Begin VB.Label lbScenarioIndex 
      Alignment       =   1  'Right Justify
      Caption         =   "מס. תרחיש"
      Height          =   255
      Left            =   7200
      TabIndex        =   67
      Top             =   1680
      Width           =   975
   End
   Begin VB.Label lbNeedPeleSecurityNoKizuz 
      Alignment       =   2  'Center
      BackColor       =   &H80000018&
      Height          =   255
      Left            =   240
      TabIndex        =   65
      Top             =   960
      Width           =   1215
   End
   Begin VB.Label lb_NeededPeleSecNoKizuz 
      Alignment       =   1  'Right Justify
      Caption         =   "בט ללא קיזוז"
      Height          =   255
      Left            =   1440
      TabIndex        =   64
      Top             =   960
      Width           =   1335
   End
   Begin VB.Label lbNeedSecNoKizuz 
      Alignment       =   2  'Center
      BackColor       =   &H80000018&
      Height          =   255
      Left            =   3120
      TabIndex        =   63
      Top             =   960
      Width           =   1215
   End
   Begin VB.Label lb_NeededSecNoKizuz 
      Alignment       =   1  'Right Justify
      Caption         =   "בט ללא קיזוז"
      Height          =   255
      Left            =   4440
      TabIndex        =   62
      Top             =   960
      Width           =   1335
   End
   Begin VB.Label lbPremiaYomit 
      Alignment       =   2  'Center
      BackColor       =   &H80000018&
      Height          =   255
      Left            =   240
      TabIndex        =   54
      Top             =   2400
      Width           =   1215
   End
   Begin VB.Label lb_PremiaYomit 
      Alignment       =   1  'Right Justify
      Caption         =   "פרמיה יומית"
      Height          =   255
      Left            =   1560
      TabIndex        =   53
      Top             =   2400
      Width           =   1215
   End
   Begin VB.Label lbSecLeft 
      Alignment       =   2  'Center
      BackColor       =   &H80000018&
      Height          =   255
      Left            =   3120
      TabIndex        =   51
      Top             =   1320
      Width           =   1215
   End
   Begin VB.Label lb_SecLeft 
      Alignment       =   1  'Right Justify
      Caption         =   "ייתרת בטחונות"
      Height          =   255
      Left            =   4560
      TabIndex        =   50
      Top             =   1320
      Width           =   1215
   End
   Begin VB.Label lbEmNizilim 
      Alignment       =   2  'Center
      BackColor       =   &H80000018&
      Height          =   255
      Left            =   3120
      TabIndex        =   49
      Top             =   1680
      Width           =   1215
   End
   Begin VB.Label lb_EmNezilim 
      Alignment       =   1  'Right Justify
      Caption         =   "אמ. נזילים"
      Height          =   255
      Left            =   4680
      TabIndex        =   48
      Top             =   1680
      Width           =   1095
   End
   Begin VB.Label lbNezilimBoker 
      Alignment       =   2  'Center
      BackColor       =   &H80000018&
      Height          =   255
      Left            =   3120
      TabIndex        =   47
      Top             =   2040
      Width           =   1215
   End
   Begin VB.Label lb_NezilimBoker 
      Alignment       =   1  'Right Justify
      Caption         =   "נזילים בקר"
      Height          =   255
      Left            =   4800
      TabIndex        =   46
      Top             =   2040
      Width           =   975
   End
   Begin VB.Label lb_PremiaLimit 
      Alignment       =   1  'Right Justify
      Caption         =   "מגבלת פרמיה"
      Height          =   255
      Left            =   1680
      TabIndex        =   45
      Top             =   1320
      Width           =   1095
   End
   Begin VB.Label lb_Margin 
      Alignment       =   1  'Right Justify
      Caption         =   "מסגרת מעו""ף"
      Height          =   255
      Left            =   4680
      TabIndex        =   44
      Top             =   240
      Width           =   1095
   End
   Begin VB.Label lb_PeleMargin 
      Alignment       =   1  'Right Justify
      Caption         =   "מסגרת פלא"
      Height          =   255
      Left            =   1680
      TabIndex        =   43
      Top             =   240
      Width           =   1095
   End
   Begin VB.Label lb_DayRevenue 
      Alignment       =   1  'Right Justify
      Caption         =   "פרמיית ביצוע"
      Height          =   255
      Left            =   1560
      TabIndex        =   42
      Top             =   2040
      Width           =   1215
   End
   Begin VB.Label lb_OrdersPremia 
      Alignment       =   1  'Right Justify
      Caption         =   "פרמיית פקודות"
      Height          =   255
      Left            =   1560
      TabIndex        =   41
      Top             =   1680
      Width           =   1215
   End
   Begin VB.Label lb_NeededPeleSec 
      Alignment       =   1  'Right Justify
      Caption         =   "בט' פלא נדרשים"
      Height          =   255
      Left            =   1440
      TabIndex        =   40
      Top             =   600
      Width           =   1335
   End
   Begin VB.Label lb_NeededSec 
      Alignment       =   1  'Right Justify
      Caption         =   "בטחונות נדרשים"
      Height          =   255
      Left            =   4440
      TabIndex        =   39
      Top             =   600
      Width           =   1335
   End
   Begin VB.Label lbExecPramia 
      Alignment       =   2  'Center
      BackColor       =   &H80000018&
      Height          =   255
      Left            =   240
      TabIndex        =   38
      Top             =   2040
      Width           =   1215
   End
   Begin VB.Label lbPeleMargin 
      Alignment       =   2  'Center
      BackColor       =   &H80000018&
      Height          =   255
      Left            =   240
      TabIndex        =   37
      Top             =   240
      Width           =   1215
   End
   Begin VB.Label lbNeedSec 
      Alignment       =   2  'Center
      BackColor       =   &H80000018&
      Height          =   255
      Left            =   3120
      TabIndex        =   36
      Top             =   600
      Width           =   1215
   End
   Begin VB.Label lbOrderPremia 
      Alignment       =   2  'Center
      BackColor       =   &H80000018&
      Height          =   255
      Left            =   240
      TabIndex        =   35
      Top             =   1680
      Width           =   1215
   End
   Begin VB.Label lbMadadTxt 
      Alignment       =   1  'Right Justify
      Caption         =   "מדד"
      Height          =   255
      Left            =   7440
      TabIndex        =   32
      Top             =   720
      Width           =   735
   End
   Begin VB.Label lbNeedPeleSecurity 
      Alignment       =   2  'Center
      BackColor       =   &H80000018&
      Height          =   255
      Left            =   240
      TabIndex        =   30
      Top             =   600
      Width           =   1215
   End
   Begin VB.Label lbTik 
      Alignment       =   1  'Right Justify
      Caption         =   "תיק"
      Height          =   255
      Left            =   8880
      TabIndex        =   5
      Top             =   240
      Width           =   375
   End
   Begin VB.Label lbMaofSession 
      Alignment       =   2  'Center
      Height          =   375
      Left            =   9000
      TabIndex        =   27
      Top             =   7560
      Width           =   1215
   End
   Begin VB.Label lbAsmachtaFmr 
      Alignment       =   1  'Right Justify
      Caption         =   "אסמכתא אפ.אמ.אר"
      Height          =   255
      Left            =   9960
      TabIndex        =   21
      Top             =   3360
      Width           =   1455
   End
   Begin VB.Label lbAsmachtaBursa 
      Alignment       =   1  'Right Justify
      Caption         =   "אסמכתא בורסה"
      Height          =   255
      Left            =   9960
      TabIndex        =   19
      Top             =   3000
      Width           =   1455
   End
   Begin VB.Label lbPremiaLimit 
      Alignment       =   2  'Center
      BackColor       =   &H80000018&
      Height          =   255
      Left            =   240
      TabIndex        =   25
      Top             =   1320
      Width           =   1215
   End
   Begin VB.Line Line9 
      X1              =   120
      X2              =   11640
      Y1              =   7440
      Y2              =   7440
   End
   Begin VB.Label lbMargin 
      Alignment       =   2  'Center
      BackColor       =   &H80000018&
      Height          =   255
      Left            =   3120
      TabIndex        =   24
      Top             =   240
      Width           =   1215
   End
   Begin VB.Label lbSugPeula 
      Alignment       =   1  'Right Justify
      Caption         =   "סוג פעולה"
      Height          =   255
      Left            =   10560
      TabIndex        =   17
      Top             =   2640
      Width           =   855
   End
   Begin VB.Label lbSugHoraa 
      Alignment       =   1  'Right Justify
      Caption         =   "סוג הוראה"
      Height          =   255
      Left            =   10560
      TabIndex        =   16
      Top             =   2280
      Width           =   855
   End
   Begin VB.Line Line4 
      X1              =   8520
      X2              =   8520
      Y1              =   4200
      Y2              =   720
   End
   Begin VB.Line Line3 
      X1              =   8520
      X2              =   11640
      Y1              =   4200
      Y2              =   4200
   End
   Begin VB.Line Line2 
      X1              =   11640
      X2              =   11640
      Y1              =   720
      Y2              =   4200
   End
   Begin VB.Line Line1 
      X1              =   8520
      X2              =   11640
      Y1              =   720
      Y2              =   720
   End
   Begin VB.Label lbPrice 
      Alignment       =   1  'Right Justify
      Caption         =   "מחיר"
      Height          =   255
      Left            =   11040
      TabIndex        =   13
      Top             =   1920
      Width           =   375
   End
   Begin VB.Label lbAmount 
      Alignment       =   1  'Right Justify
      Caption         =   "כמות"
      Height          =   255
      Left            =   10920
      TabIndex        =   11
      Top             =   1560
      Width           =   495
   End
   Begin VB.Label lbOpNumber 
      Alignment       =   1  'Right Justify
      Caption         =   "מס. אופציה"
      Height          =   255
      Left            =   10320
      TabIndex        =   9
      Top             =   1200
      Width           =   1095
   End
   Begin VB.Label lbBranch 
      Alignment       =   1  'Right Justify
      Caption         =   "סניף"
      Height          =   255
      Left            =   9840
      TabIndex        =   3
      Top             =   240
      Width           =   495
   End
   Begin VB.Label lbAccount 
      Alignment       =   1  'Right Justify
      Caption         =   "ח-ן"
      Height          =   255
      Left            =   11280
      TabIndex        =   1
      Top             =   240
      Width           =   375
   End
   Begin VB.Label Label2 
      BackColor       =   &H80000001&
      Height          =   855
      Left            =   6240
      TabIndex        =   68
      Top             =   1200
      Width           =   2055
   End
End
Attribute VB_Name = "frmMaofOrder"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False

Option Explicit
Public ButtonPressed As Integer
Public bActiveMaofSession As Boolean
Private activeSessionAccount As String
Private activeSessionBranch As String
Private lastMaofTime As String
Private ordersLastTime As String
Private ordersLastRow As Integer
Private sortAccending As Boolean
Private CTRL_KEY As Boolean

Public BaseAsset As Long
Private Const SplitChar As String = " == "

Private colOrders As Collection
Private OpNames As Collection

Private Function OrderPreConditions() As Boolean

    OrderPreConditions = False
    If txtAmmount = "" Then
        ' yesh lehazin kamut mevukeshet
        MsgBox "יש להזין כמות מבוקשת", vbExclamation
        txtAmmount.SetFocus
        Exit Function
    End If
    If txtOpNumber = "" Then
        ' yesh lehazin mispar optzia
        MsgBox "יש להזין מספר אופציה", vbExclamation
        txtOpNumber.SetFocus
        Exit Function
    End If
    If txtPrice = "" Then
        ' yesh lehazin mehir mevukash
        MsgBox "יש להזין מחיר מבוקש", vbExclamation
        txtPrice.SetFocus
        Exit Function
    End If
    If rdBuy.value = False And rdSell.value = False Then
        MsgBox "חובה לבחור קניה או מכירה", vbExclamation
        rdBuy.SetFocus
        rdBuy = False
        Exit Function
    End If
    
    OrderPreConditions = True
    
End Function

Private Sub btnClearOrders_Click()
    ordersLastRow = 0
    ordersLastTime = "00000000"
    lstOrders.Clear
    lstOrders.Rows = 1
    lstOrders.TextArray(0) = "מצב"
    lstOrders.TextArray(1) = "שם"
    lstOrders.TextArray(2) = "נייר"
    lstOrders.TextArray(3) = "ק/מ"
    lstOrders.TextArray(4) = "כמות"
    lstOrders.TextArray(5) = "מחיר"
    lstOrders.TextArray(6) = "בוצע"
    lstOrders.TextArray(7) = "ייתרה"
    lstOrders.TextArray(8) = "אסמכתא"
    lstOrders.TextArray(9) = "אסמכתא פ.מ.ר"
    lstOrders.TextArray(10) = "שעת עדכון"
    lstOrders.TextArray(11) = "סידורי"
    Set colOrders = Nothing
    Set OpNames = Nothing
End Sub

Private Sub btnSpeed_Click()
    ' check accont + branch
    If PreConditions() = False Then
        Exit Sub
    End If

    frmMaofSpeed.Show vbModal
End Sub

Private Sub btnStatus_Click()
    Dim asset As BaseAssetTypes
    Dim Orders() As MaofOrderType
    Dim rc As Long
    Dim premiaLimit As Double
    Dim Madad As Double, madaInScenraio As Double
    Dim nezilimBoker As Double, emzaimNezilim As Double
    Dim margin As Double, peleMargin As Double
    Dim ordersPremia As Double, dayRevenue As Double
    Dim nezilim As Double
    Dim scenarioIndex As Long
    Dim neededSec As Double, neededPeleSec As Double, neededSecNoKizuz As Double, neededPeleSecKizuz As Double
    Dim i As Integer
    
    ' check accont + branch
    If PreConditions() = False Then
        Exit Sub
    End If
    
    asset = BaseAsset
    
    ReDim Orders(0)

    Screen.MousePointer = vbHourglass
    rc = FMRLibTester.FMRUser.MaofSecuritiesParams( _
        FMRLibTester.SessionId, _
        txtAccount.Text, txtBranch.Text, asset, Madad, _
        emzaimNezilim, nezilimBoker, margin, peleMargin, _
        neededSec, neededPeleSec, ordersPremia, dayRevenue, _
        neededSecNoKizuz, neededPeleSecKizuz, madaInScenraio, _
        scenarioIndex _
        )
    Screen.MousePointer = vbDefault
    txtMadad.Text = Madad
    lbEmNizilim.Caption = Val(Format(emzaimNezilim, "#.##"))
    lbNezilimBoker.Caption = nezilimBoker
    lbMargin.Caption = margin
    lbPeleMargin.Caption = peleMargin
    lbNeedSec.Caption = Val(Format(neededSec, "#.##"))
    lbNeedPeleSecurity.Caption = Val(Format(neededPeleSec, "#.##"))
    lbOrderPremia.Caption = ordersPremia
    lbExecPramia.Caption = dayRevenue
    lbSecLeft.Caption = Val(lbEmNizilim.Caption) + Val(lbNeedSec.Caption)
    lbPremiaYomit.Caption = ordersPremia + dayRevenue
    If madaInScenraio = 0 Then
        txtMadadInScenario.Text = ""
    Else
        txtMadadInScenario.Text = Val(Format(madaInScenraio, ".##"))
    End If
    lbNeedPeleSecurityNoKizuz.Caption = Val(Format(neededPeleSecKizuz, ".##"))
    lbNeedSecNoKizuz.Caption = Val(Format(neededSecNoKizuz, ".##"))
    txtScenarioIndex.Text = scenarioIndex
            
    rc = FMRLibTester.FMRUser.GetPremium(FMRLibTester.SessionId, txtAccount.Text, txtBranch.Text, asset, premiaLimit)
    lbPremiaLimit.Caption = premiaLimit
End Sub


Private Sub chkPeleOrder_Click()
    If chkPeleOrder.value = 1 Then
        btnSpeed.Enabled = True
    Else
        btnSpeed.Enabled = False
    End If
End Sub


Private Sub comboPeleAsset_Click()
    If comboPeleAsset.ListIndex = -1 Then
        Exit Sub
    End If
    Dim tok() As String
    tok = Split(comboPeleAsset.List(comboPeleAsset.ListIndex), SplitChar)
    BaseAsset = tok(0)
End Sub

Private Sub comboSugPeula_Click()
    If comboSugPeula.List(comboSugPeula.ListIndex) = "N" Then
        txtAsmachtaRzf = ""
        txtAsmachtaFmr = ""
    End If
End Sub

Private Sub Form_Load()
    ButtonPressed = vbCancel
    comboSugPeula.ListIndex = 0
    comboSugHoraa.ListIndex = 0
    
    bActiveMaofSession = False
    
    Dim assets() As TaskBarLib.BaseAssetType
    Dim rc As Long
    Dim i As Integer
    
    comboPeleAsset.Clear
    rc = FMRLibTester.FMRK300.GetBaseAssets2(assets)
    If rc > 0 Then
        For i = LBound(assets) To UBound(assets)
            Dim entry As String
            entry = assets(i).BaseAssetCode & _
                SplitChar & _
                 assets(i).NameHeb
            comboPeleAsset.AddItem (entry)
            If i = 0 Then
                comboPeleAsset.ListIndex = 0
            End If
         Next
    End If
 
    lstOrders.cols = 12
    lstOrders.TextArray(0) = "מצב"
    lstOrders.TextArray(1) = "שם"
    lstOrders.TextArray(2) = "נייר"
    lstOrders.TextArray(3) = "ק/מ"
    lstOrders.TextArray(4) = "כמות"
    lstOrders.TextArray(5) = "מחיר"
    lstOrders.TextArray(6) = "בוצע"
    lstOrders.TextArray(7) = "ייתרה"
    lstOrders.TextArray(8) = "אסמכתא"
    lstOrders.TextArray(9) = "אסמכתא פ.מ.ר"
    lstOrders.TextArray(10) = "שעת עדכון"
    lstOrders.TextArray(11) = "סידורי"
    
    lastMaofTime = "00000000"
    ordersLastTime = "00000000"
    
    sortAccending = True
    
    btnSpeed.Enabled = False
    
    chkSelectAllOrders.value = 0
    
    CTRL_KEY = False
End Sub

Private Sub Form_Unload(Cancel As Integer)
    Dim rc As Long
    
    Set colOrders = Nothing
    Set OpNames = Nothing
    
    If bActiveMaofSession = True Then
        rc = FMRLibTester.FMRUser.StopMaofSession(FMRLibTester.SessionId, activeSessionAccount, activeSessionBranch)
        If rc <> 0 Then
            MsgBox "עצירת עדכון רקע נכשלה"
        End If
    End If
    
    Unload frmMaofSpeed
End Sub

Function PreConditions() As Boolean
    If txtAccount.Text = "" Then
        ' chova livchor mispar cheshbon
        MsgBox "חובה לבחור מספר חשבון", vbExclamation
        txtAccount.SetFocus
        PreConditions = False
        Exit Function
    End If
    If txtBranch.Text = "" Then
        txtBranch.Text = "000"
    End If
    If txtTik.Text = "" Then
        txtTik.Text = "000"
    End If
    PreConditions = True
End Function

Private Sub btnMaofSession_Click()
    Dim b As Integer
    
    If PreConditions() = False Then
        Exit Sub
    End If
    
    bActiveMaofSession = FMRLibTester.FMRUser.MaofSessionActive(txtAccount.Text, txtBranch.Text)
    
    If bActiveMaofSession = True Then

        tmrOrders.Enabled = False
        
        b = FMRLibTester.FMRUser.StopMaofSession(FMRLibTester.SessionId, txtAccount.Text, txtBranch.Text)
        If b <> 0 Then
            MsgBox "עצירת עדכוני רקע נכשלה"
            Exit Sub
        End If
        bActiveMaofSession = False
        btnMaofSession.Caption = "&החל מסחר"
        lbMaofSession.BackColor = &H8000000F
        lbMaofSession.Caption = ""
        txtAccount.Enabled = True
        txtBranch.Enabled = True
        txtTik.Enabled = True
        Set colOrders = Nothing
        Set OpNames = Nothing
        Call btnClearOrders_Click
    Else
        b = FMRLibTester.FMRUser.StartMaofSession(FMRLibTester.SessionId, txtAccount.Text, txtBranch.Text, RM_Dill_ShortOrders)
        If b <> 0 Then
            MsgBox "אתחול עדכון רקע נכשל"
            Exit Sub
        End If
    
        Call ClearDynaFields
        
        ordersLastTime = "00000000"
        ordersLastRow = 0
    
        bActiveMaofSession = True
        activeSessionAccount = txtAccount.Text
        activeSessionBranch = txtBranch.Text
        btnMaofSession.Caption = "&עצור"
        lbMaofSession.BackColor = &HFF
        lbMaofSession.Caption = "פעיל"  'pail
        txtAccount.Enabled = False
        txtBranch.Enabled = False
        txtTik.Enabled = False
        
        Call btnStatus_Click
        'Call btnOrders_Click
        
        If colOrders Is Nothing Then
            Set colOrders = New Collection
        End If
        If OpNames Is Nothing Then
            Set OpNames = New Collection
        End If

        Dim ret As Long, i As Long
        Dim vecOp() As TradeOptionType
        ret = FMRLibTester.FMRK300.GetShortTradeOptions(vecOp)
        If ret > 0 Then
            For i = LBound(vecOp) To UBound(vecOp)
            Dim tmp As String
            tmp = vecOp(i).Bno
                Call OpNames.Add(vecOp(i).BnoName, tmp)
            Next i
        End If
        tmrOrders.Interval = 100
        tmrOrders.Enabled = True
        
    End If

End Sub

Public Sub btnOrders_Click()
    Dim ret As Long
    Dim vecData() As MOFINQType
    Dim i As Integer
    Dim J As Integer
    Dim OpStr As String
    Dim curRow As Long
    Dim strRec As String
    Dim tokens() As String
    Dim Var As Long
    Dim index As Long
    Dim Madad As Double, emzaimNez As Double, nezBoker As Double, regMargin As Double
    Dim peleMargin As Double, regSec As Double, peleSec As Double, orderPremia As Double, execPremia As Double
    
    If PreConditions() = False Then
        Exit Sub
    End If
    
    If colOrders Is Nothing Then
        Set colOrders = New Collection
    End If
    If OpNames Is Nothing Then
        Set OpNames = New Collection
    End If
    
End Sub

Private Sub CancelButton_Click()
    ButtonPressed = vbCancel
    Me.Visible = False
End Sub


Private Sub btnSend_Click()
    Dim Order() As MaofOrderType
    Dim rc As Integer
    Dim VBMsg As String
    Dim ErrNo As Long
    Dim ErrorType As OrdersErrorTypes
    Dim OrderID As Long
    Dim Continue As Boolean
    Dim AuthUser As String
    Dim AuthPassword As String
    Dim ReEnteredValue As String
    Dim SpecialOrder As Long
    
    If PreConditions() = False Then
        Exit Sub
    End If
    
    If OrderPreConditions() = False Then
        Exit Sub
    End If
    
    ReDim Order(0)
    Order(0).Account = txtAccount.Text
    If rdBuy.value = True Then
        Order(0).ammount = txtAmmount.Text
    Else
        Order(0).ammount = "-" & txtAmmount.Text
    End If
     
    '
    ' flag for orders sent from automatic machines
    '
    SpecialOrder = 1
    
    Order(0).Branch = txtBranch.Text
    Order(0).Operation = comboSugPeula.List(comboSugPeula.ListIndex)
    Order(0).Option = txtOpNumber.Text
    Order(0).price = txtPrice.Text
    Order(0).Sug_Pkuda = comboSugHoraa.List(comboSugHoraa.ListIndex)
    Order(0).Asmachta = txtAsmachtaRzf.Text
    Order(0).AsmachtaFmr = txtAsmachtaFmr.Text
    
    Continue = True
    If chkPeleOrder.value = 0 Then
        rc = FMRLibTester.FMRUser.SendMaofOrder( _
            FMRLibTester.SessionId, Order(0), VBMsg, ErrNo, ErrorType, OrderID, _
            AuthUser, AuthPassword, ReEnteredValue, _
            SpecialOrder _
            )
    Else
        rc = FMRLibTester.FMRUser.SendMaofOrderPele( _
            FMRLibTester.SessionId, Order, VBMsg, ErrNo, ErrorType, OrderID, _
            AuthUser, AuthPassword, ReEnteredValue, _
            SpecialOrder _
            )
    End If
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
            If chkPeleOrder.value = 0 Then
                rc = FMRLibTester.FMRUser.SendMaofOrder( _
                    FMRLibTester.SessionId, Order(0), VBMsg, ErrNo, ErrorType, OrderID, _
                    AuthUser, AuthPassword, ReEnteredValue, _
                    SpecialOrder _
                    )
            Else
                rc = FMRLibTester.FMRUser.SendMaofOrderPele( _
                    FMRLibTester.SessionId, Order, VBMsg, ErrNo, ErrorType, OrderID, _
                    AuthUser, AuthPassword, ReEnteredValue, _
                    SpecialOrder _
                    )
            End If
    
        Case PasswordReq
    
            MsgBox VBMsg
            AuthUser = InputBox("Please Enter Auth User Name")
            AuthPassword = InputBox("Please Enter Auth Password")
            If chkPeleOrder.value = 0 Then
                rc = FMRLibTester.FMRUser.SendMaofOrder( _
                    FMRLibTester.SessionId, Order(0), VBMsg, ErrNo, ErrorType, OrderID, _
                    AuthUser, AuthPassword, ReEnteredValue, SpecialOrder _
                    )
            Else
                rc = FMRLibTester.FMRUser.SendMaofOrderPele( _
                    FMRLibTester.SessionId, Order, VBMsg, ErrNo, ErrorType, OrderID, _
                    AuthUser, AuthPassword, ReEnteredValue, SpecialOrder _
                    )
            End If
        
        Case ReEnter
        
            MsgBox VBMsg
            ReEnteredValue = InputBox("Please Enter the value again")
            If chkPeleOrder.value = 0 Then
                rc = FMRLibTester.FMRUser.SendMaofOrder( _
                    FMRLibTester.SessionId, Order(0), VBMsg, ErrNo, ErrorType, OrderID, _
                    AuthUser, AuthPassword, ReEnteredValue, SpecialOrder _
                    )
            Else
                rc = FMRLibTester.FMRUser.SendMaofOrderPele( _
                    FMRLibTester.SessionId, Order, VBMsg, ErrNo, ErrorType, OrderID, _
                    AuthUser, AuthPassword, ReEnteredValue, SpecialOrder _
                    )
            End If
    
        Case Else
            Continue = False
        End Select
    
    Loop
    
    If ErrNo = 0 And chkSendConfirm.value = 1 Then
        MsgBox ("Order" & OrderID & " was submitted successfully.")
        Call btnOrders_Click
        Call btnStatus_Click
    End If
     
End Sub

Private Sub lstOrders_Click()
    Dim i As Integer
    
    Call lstOrders.SetFocus
    
    ' הקשה בשורה 0 מיועדת למיון
    If lstOrders.Rows <= 1 Then
        Exit Sub
    End If
    
    If lstOrders.MouseRow = 0 Then
        lstOrders.col = lstOrders.MouseCol
        If sortAccending = True Then
            lstOrders.SORT = flexSortGenericAscending
            sortAccending = False
        Else
            lstOrders.SORT = flexSortGenericDescending
            sortAccending = True
        End If
        Exit Sub
    End If
    
    If CTRL_KEY = True Then
        For i = 0 To lstOrders.cols - 1
            lstOrders.row = lstOrders.MouseRow
            lstOrders.col = i
            If lstOrders.CellBackColor <> GridSelectBackColor Then
                lstOrders.CellBackColor = GridSelectBackColor
                lstOrders.CellForeColor = GridSelectForeColor
            Else
                lstOrders.CellBackColor = lstOrders.BackColor
                lstOrders.CellForeColor = lstOrders.ForeColor
            End If
        Next i
    End If
    
    If lstOrders.TextMatrix(lstOrders.MouseRow, 3) = "ק" Then
        rdBuy.value = True
    Else
        rdSell.value = True
    End If
    txtAsmachtaRzf.Text = lstOrders.TextMatrix(lstOrders.MouseRow, 8)
    txtAsmachtaFmr.Text = lstOrders.TextMatrix(lstOrders.MouseRow, 9)
    txtAmmount.Text = lstOrders.TextMatrix(lstOrders.MouseRow, 4)
    txtPrice.Text = Val(lstOrders.TextMatrix(lstOrders.MouseRow, 5))
    txtOpNumber.Text = lstOrders.TextMatrix(lstOrders.MouseRow, 2)
    comboSugPeula.ListIndex = 1 ' U
    
End Sub

Private Sub lstOrders_KeyDown(KeyCode As Integer, Shift As Integer)
    If KeyCode = 17 Then
        CTRL_KEY = True
        Exit Sub
    End If
End Sub

Private Sub lstOrders_KeyUp(KeyCode As Integer, Shift As Integer)
    If KeyCode = 17 Then
        CTRL_KEY = False
        Exit Sub
    End If
End Sub


Private Sub tmrOrders_Timer()
    Dim ret As Long
    Dim vecData() As MOFINQType
    Dim i As Integer
    Dim index As Long
    Dim curRow As Long
    
    ret = FMRLibTester.FMRUser.GetOrdersMF(FMRLibTester.SessionId, vecData, txtAccount.Text, txtBranch.Text, ordersLastTime)
    
    If ret > 0 Then
    
        For i = LBound(vecData) To UBound(vecData)
        
            On Error Resume Next
            index = -1
            index = colOrders(Trim$(vecData(i).SEQ_PIC))
            If index = -1 Then
                Call colOrders.Add(lstOrders.Rows, vecData(i).SEQ_PIC)
                lstOrders.Rows = lstOrders.Rows + 1
            End If
        
            curRow = colOrders.Item(vecData(i).SEQ_PIC)
        
            lstOrders.TextMatrix(curRow, 0) = StatusToString(vecData(i).STS)
            
            'lstOrders.TextMatrix(curRow, 1) = vecData(i).BNO_NAME
            lstOrders.TextMatrix(curRow, 1) = OpNames.Item(vecData(i).BNO_PIC)
            lstOrders.TextMatrix(curRow, 2) = vecData(i).BNO_PIC
            If vecData(i).op = "B" Then
                lstOrders.TextMatrix(curRow, 3) = "ק" 'buy
            Else
                lstOrders.TextMatrix(curRow, 3) = "מ" 'sell
            End If
            lstOrders.TextMatrix(curRow, 4) = Val(vecData(i).ORDR_NV_PIC)
            lstOrders.TextMatrix(curRow, 5) = Val(vecData(i).ORDR_PRC_PIC) / 100
            lstOrders.TextMatrix(curRow, 6) = Val(vecData(i).DIL_NV_PIC)
            lstOrders.TextMatrix(curRow, 7) = Val(vecData(i).ORDR_NV_PIC) - Val(vecData(i).DIL_NV_PIC)
            lstOrders.TextMatrix(curRow, 8) = vecData(i).RZF_ORD_PIC
            lstOrders.TextMatrix(curRow, 9) = vecData(i).SEQ_PIC
            lstOrders.TextMatrix(curRow, 10) = Val(vecData(i).ORDR_TIME)
            lstOrders.TextMatrix(curRow, 11) = curRow
            
            lstOrders.row = curRow
            If vecData(i).op = "B" Then
                lstOrders.col = 6
                lstOrders.CellForeColor = RGB(0, 120, 0)
                lstOrders.col = 4
                lstOrders.CellForeColor = RGB(0, 120, 0)
            Else
                lstOrders.col = 6
                lstOrders.CellForeColor = RGB(255, 0, 0)
                lstOrders.col = 4
                lstOrders.CellForeColor = RGB(255, 0, 0)
            End If
            lstOrders.TopRow = curRow
            
        Next i
    End If

End Sub
    
Private Sub txtAccount_GotFocus()
    txtAccount.SelStart = 0
    txtAccount.SelLength = Len(txtAccount.Text)
End Sub

Private Sub txtAccount_KeyPress(KeyAscii As Integer)
    If KeyAscii = 13 Then
        Call btnMaofSession_Click
    End If
End Sub

Private Sub txtAmmount_GotFocus()
    txtAmmount.SelStart = 0
    txtAmmount.SelLength = Len(txtAmmount.Text)
End Sub

Private Sub txtAsmachtaFmr_GotFocus()
    txtAsmachtaFmr.SelStart = 0
    txtAsmachtaFmr.SelLength = Len(txtAsmachtaFmr.Text)
End Sub

Private Sub txtAsmachtaRzf_GotFocus()
    txtAsmachtaRzf.SelStart = 0
    txtAsmachtaRzf.SelLength = Len(txtAsmachtaRzf.Text)
End Sub

Private Sub txtBranch_GotFocus()
    txtBranch.SelStart = 0
    txtBranch.SelLength = Len(txtBranch.Text)
End Sub

Private Sub txtBranch_KeyPress(KeyAscii As Integer)
    If KeyAscii = 13 Then
        Call btnMaofSession_Click
    End If
End Sub

Private Sub txtOpNumber_GotFocus()
    txtOpNumber.SelStart = 0
    txtOpNumber.SelLength = Len(txtOpNumber.Text)
End Sub

Private Sub txtOpNumber_KeyDown(KeyCode As Integer, Shift As Integer)
    If KeyCode = 114 And Shift = 0 Then
        frmOptions.Show vbModal
    End If
End Sub

Private Sub txtPrice_GotFocus()
    txtPrice.SelStart = 0
    txtPrice.SelLength = Len(txtPrice.Text)
End Sub

Private Sub ClearDynaFields()
    lbPeleMargin.Caption = ""
    lbNeedPeleSecurity.Caption = ""
    lbPremiaLimit.Caption = ""
    lbOrderPremia.Caption = ""
    lbExecPramia.Caption = ""
    lbPremiaYomit.Caption = ""
    lbMargin.Caption = ""
    lbNeedSec.Caption = ""
    lbSecLeft.Caption = ""
    lbEmNizilim.Caption = ""
    lbNezilimBoker.Caption = ""
    txtOpNumber.Text = ""
    txtAmmount.Text = ""
    txtPrice.Text = ""
    txtAsmachtaRzf.Text = ""
    txtAsmachtaFmr.Text = ""
End Sub

Function StatusToString(STS As String) As String
    Select Case STS
        Case "1"
             StatusToString = "נקלט ע.ת."
         Case "2"
             StatusToString = "עדכון ע.ת."
         Case "3"
             StatusToString = "בטול ע.ת."
         Case "4"
             StatusToString = "נקלט"
         Case "5"
             StatusToString = "בצוע חלקי"
         Case "6"
             StatusToString = "בצוע מלא"
         Case "7"
             StatusToString = "בוטל"
         Case "8"
             StatusToString = "מושעה"
         Case "A"
             StatusToString = "שגוי"
         Case "B"
             StatusToString = "לא עודכן"
         Case "C"
             StatusToString = "לא בוטל"
         Case "R"
             StatusToString = "שוגר פתח"
         Case "S"
             StatusToString = "שוגר עדכון"
         Case "T"
             StatusToString = "שוגר בטל"
         Case "X"
             StatusToString = "הועבר"
         Case Else
             StatusToString = ""
    End Select
End Function

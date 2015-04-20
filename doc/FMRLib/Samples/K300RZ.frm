VERSION 5.00
Object = "{5E9E78A0-531B-11CF-91F6-C2863C385E30}#1.0#0"; "MSFLXGRD.OCX"
Begin VB.Form K300RZ 
   Caption         =   "נתוני שוק - רצף"
   ClientHeight    =   8310
   ClientLeft      =   1650
   ClientTop       =   1935
   ClientWidth     =   11595
   LinkTopic       =   "Form1"
   ScaleHeight     =   8310
   ScaleWidth      =   11595
   Begin VB.CommandButton btnStartPush 
      Caption         =   "התחל דחיפה"
      Height          =   375
      Left            =   1320
      TabIndex        =   8
      Top             =   7920
      Width           =   1215
   End
   Begin VB.Timer tmrGetK300RZ 
      Enabled         =   0   'False
      Left            =   5640
      Top             =   4680
   End
   Begin VB.TextBox txtBno 
      Height          =   285
      Left            =   5040
      TabIndex        =   7
      Top             =   7920
      Width           =   975
   End
   Begin VB.ComboBox cmboSug 
      Height          =   315
      ItemData        =   "K300RZ.frx":0000
      Left            =   7320
      List            =   "K300RZ.frx":0013
      Style           =   2  'Dropdown List
      TabIndex        =   5
      Top             =   7920
      Width           =   1215
   End
   Begin VB.ComboBox cmboMadad 
      Height          =   315
      ItemData        =   "K300RZ.frx":003C
      Left            =   9840
      List            =   "K300RZ.frx":005B
      Style           =   2  'Dropdown List
      TabIndex        =   3
      Top             =   7920
      Width           =   1215
   End
   Begin VB.CommandButton btnStart 
      Caption         =   "התחל משיכה"
      Height          =   375
      Left            =   0
      TabIndex        =   1
      Top             =   7920
      Width           =   1215
   End
   Begin MSFlexGridLib.MSFlexGrid grid 
      Height          =   7815
      Left            =   -120
      TabIndex        =   0
      Top             =   0
      Width           =   11655
      _ExtentX        =   20558
      _ExtentY        =   13785
      _Version        =   393216
      RightToLeft     =   -1  'True
   End
   Begin VB.Label lblBno 
      Alignment       =   1  'Right Justify
      Caption         =   "מס. מניה"
      Height          =   255
      Left            =   5880
      TabIndex        =   6
      Top             =   7920
      Width           =   855
   End
   Begin VB.Label lblSug 
      Alignment       =   1  'Right Justify
      Caption         =   "סוג נייר"
      Height          =   255
      Left            =   8520
      TabIndex        =   4
      Top             =   7920
      Width           =   735
   End
   Begin VB.Label lblMadad 
      Alignment       =   1  'Right Justify
      Caption         =   "מדד"
      Height          =   255
      Left            =   10920
      TabIndex        =   2
      Top             =   7920
      Width           =   615
   End
End
Attribute VB_Name = "K300RZ"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False

Public sheetLastCellCol As Long
Public sheetLastCellRow As Long

Public StockNumber As String
Private MadadType As MadadTypes
Private StockType As StockKind
Private lastTime As String
Private OpColl As Collection
Private sortAcc As Boolean

Private K300Obj As TaskBarLib.K300
Private WithEvents K3Events As TaskBarLib.K300Events
Attribute K3Events.VB_VarHelpID = -1

Private Const COL_SIDURI As Integer = 0
Private Const COL_NAME As Integer = 1
Private Const COL_PRC As Integer = 2
Private Const COL_PRCBASIS As Integer = 3
Private Const COL_PRCBID As Integer = 4
Private Const COL_NVBID As Integer = 5
Private Const COL_PRCASK As Integer = 6
Private Const COL_NVASK As Integer = 7
Private Const COL_BNO As Integer = 8
Private Const COL_CHANGE As Integer = 9
Private Const COL_STAGE As Integer = 10
Private Const COL_STATUS As Integer = 11
Private Const COL_CHANGE_OPEN As Integer = 12
Private Const COLMAX As Integer = 13


Private Sub btnStartPush_Click()
   If btnStartPush.Caption = "התחל דחיפה" Then
   
        If txtBno.Text <> "" And IsNumeric(txtBno.Text) Then
            StockNumber = txtBno.Text
        End If
        
        sheetLastCellCol = 0
        sheetLastCellRow = 0
        
        btnStartPush.Caption = "עצור דחיפה"
        btnStart.Enabled = False
        cmboMadad.Enabled = False
        cmboSug.Enabled = False
        Set OpColl = New Collection
        If K300Obj.K300StartStream(RezefStream) <> 0 Then
            MsgBox "כשלון באיתחול זרימת נתוני שוק"
            Call btnStart_Click
        End If
        
        ' prepare the collection and the grid
        Call GetAll
        
        ' must be after the call to GetAll so we won't get
        ' events befroe we are ready
        If K3Events Is Nothing Then
            Set K3Events = New TaskBarLib.K300Events
        End If
        K3Events.EventsFilterBno = StockNumber
        K3Events.EventsFilterRezef = True
        K3Events.EventsFilterStockKind = StockType
        K3Events.EventsFilterStockMadad = MadadType
       
    Else
        K3Events.EventsFilterRezef = False
        Set K3Events = Nothing
        K300Obj.K300StopStream (RezefStream)
        Set OpColl = Nothing
        btnStartPush.Caption = "התחל דחיפה"
        btnStart.Enabled = True
        cmboMadad.Enabled = True
        cmboSug.Enabled = True
        lastTime = "0"
        OptionNumber = "00000000"
    End If
End Sub

Private Sub Form_Load()
    cmboMadad.ListIndex = 0
    cmboSug.ListIndex = 0
    
    MadadType = AllMadad
    StockType = StockKindAll
    StockNumber = "00000000"
    lastTime = "0"
    
    If K300Obj Is Nothing Then
        Set K300Obj = New TaskBarLib.K300
        K300Obj.K300SessionId = FMRLibTester.SessionId
    End If
    
    tmrGetK300RZ.Enabled = False
    tmrGetK300RZ.Interval = 1
    
    grid.AllowUserResizing = flexResizeColumns
    grid.Cols = COLMAX
    grid.Rows = 1
    grid.TextMatrix(0, COL_SIDURI) = "סידורי"
    grid.TextMatrix(0, COL_NAME) = "נייר"
    grid.TextMatrix(0, COL_PRC) = "שער"
    grid.TextMatrix(0, COL_PRCBASIS) = "בסיס"
    grid.TextMatrix(0, COL_PRCBID) = "מ. ביקוש 1"
    grid.TextMatrix(0, COL_NVBID) = "כ. ביקוש 1"
    grid.TextMatrix(0, COL_PRCASK) = "מ. היצע 1"
    grid.TextMatrix(0, COL_NVASK) = "כ. היצע 1"
    grid.TextMatrix(0, COL_BNO) = "מס. נייר"
    grid.TextMatrix(0, COL_CHANGE) = "% שינוי יומי"
    grid.TextMatrix(0, COL_STAGE) = "שלב"
    grid.TextMatrix(0, COL_STATUS) = "מצב נייר"
    grid.TextMatrix(0, COL_CHANGE_OPEN) = "עליה מפתיחה"
    
    cmboMadad.List(0) = "הכל"
    cmboMadad.List(1) = "ת""א 25"
    cmboMadad.List(2) = "ת""א 75"
    cmboMadad.List(3) = "ת""א 100"
    cmboMadad.List(4) = "תל-טק"
    cmboMadad.List(5) = "בנקים"
    cmboMadad.List(6) = "בינוי"
    cmboMadad.List(7) = "פיננסים"
    cmboMadad.List(8) = "יתר 30"
    cmboMadad.List(9) = "נדל""ן"
    cmboMadad.List(10) = "תל-דיב"
    cmboMadad.List(11) = "תל-בונד"
    cmboMadad.List(12) = "יתר 120"
    cmboMadad.List(13) = "יתר 50"
    cmboMadad.List(14) = "מעלה"
    cmboMadad.List(15) = "תל-בונד 40"
    cmboMadad.List(16) = "תל-בונד 60"
    
End Sub

Private Sub Form_Resize()
    Dim hGap As Integer
    
    hGap = 1000
    If Me.Height < hGap Then
        hGap = Me.Height
    End If

    grid.Height = Me.Height - hGap
    grid.Width = Me.Width - 100
    grid.Top = 0
    grid.Left = 0
    
    lblMadad.Top = Me.Height - lblMadad.Height - hGap / 2
    lblMadad.Left = Me.Width - lblMadad.Width - 300
    
    cmboMadad.Top = Me.Height - cmboMadad.Height - hGap / 2
    cmboMadad.Left = Me.Width - cmboMadad.Width - 700
    
    lblSug.Top = Me.Height - lblSug.Height - hGap / 2
    lblSug.Left = Me.Width - lblSug.Width - 2200
    
    cmboSug.Top = Me.Height - cmboSug.Height - hGap / 2
    cmboSug.Left = Me.Width - cmboSug.Width - 2900
    
    lblBno.Top = Me.Height - lblBno.Height - hGap / 2
    lblBno.Left = Me.Width - lblBno.Width - 4400
    
    txtBno.Top = Me.Height - txtBno.Height - hGap / 2
    txtBno.Left = Me.Width - txtBno.Width - 5100

    btnStart.Top = Me.Height - btnStart.Height - hGap / 2
    btnStart.Left = 0

    btnStartPush.Top = Me.Height - btnStartPush.Height - hGap / 2
    btnStartPush.Left = btnStart.Left + 20 + btnStart.Width

End Sub


Private Sub cmboMadad_Click()
    If cmboMadad.ListIndex <> 0 Then
        cmboSug.ListIndex = 1  ''  מדדי המניות
    End If
    If cmboMadad.ListIndex = 0 Then '' הכל
        MadadType = AllMadad
    ElseIf cmboMadad.ListIndex = 1 Then '' תא 25
        MadadType = TLV25
    ElseIf cmboMadad.ListIndex = 2 Then '' תא 75
        MadadType = TLV75
    ElseIf cmboMadad.ListIndex = 3 Then '' תא 100
        MadadType = TLV100
    ElseIf cmboMadad.ListIndex = 4 Then '' תלטק
        MadadType = TLTK
    ElseIf cmboMadad.ListIndex = 5 Then '' בנקים
        MadadType = BANK
    ElseIf cmboMadad.ListIndex = 6 Then '' בינוי
        MadadType = BINUI
    ElseIf cmboMadad.ListIndex = 7 Then '' פיננסים
        MadadType = TLVFIN
    ElseIf cmboMadad.ListIndex = 8 Then '' יתר 30
        MadadType = YETER30
    ElseIf cmboMadad.ListIndex = 9 Then '' נדלן
        MadadType = TLVNADLAN15
    ElseIf cmboMadad.ListIndex = 10 Then '' תל דיב
        MadadType = TELDIV20
    ElseIf cmboMadad.ListIndex = 11 Then '' תל בונד
        MadadType = TELBOND
    ElseIf cmboMadad.ListIndex = 12 Then ''  יתר 120
        MadadType = YETER120
    ElseIf cmboMadad.ListIndex = 13 Then ''  יתר 50
        MadadType = YETER50
    ElseIf cmboMadad.ListIndex = 14 Then ''  מעלה
        MadadType = MAALE
    ElseIf cmboMadad.ListIndex = 15 Then ''  תל בונד 40
        MadadType = TELBOND40
    ElseIf cmboMadad.ListIndex = 16 Then ''  תל בונד 60
        MadadType = TELBOND60
        
    End If
End Sub


Private Sub cmboSug_Click()
    If cmboSug.ListIndex = 0 Then
        StockType = StockKindAll
    ElseIf cmboSug.ListIndex = 1 Then
        StockType = StockKindMenaya
    ElseIf cmboSug.ListIndex = 2 Then
        StockType = StockKindAgach
    ElseIf cmboSug.ListIndex = 3 Then
        StockType = StockKindMakam
    ElseIf cmboSug.ListIndex = 4 Then
        StockType = StockKindKeren
    End If
End Sub


Private Sub btnStart_Click()
   If tmrGetK300RZ.Enabled = False Then
        ''
        ''      התחל עדכונים
        ''
        tmrGetK300RZ.Enabled = True
        btnStart.Caption = "עצור"
        cmboMadad.Enabled = False
        cmboSug.Enabled = False
        txtBno.Enabled = False
        btnStartPush.Enabled = False
        Set OpColl = Nothing
        Set OpColl = New Collection
        grid.Rows = 1
        sheetLastCellCol = 0
        sheetLastCellRow = 0
        
        If txtBno.Text <> "" And IsNumeric(txtBno.Text) Then
            StockNumber = txtBno.Text
        End If
        If K300Obj.K300StartStream(RezefStream) <> 0 Then
            MsgBox "כשלון באיתחול זרימת נתוני שוק"
            Call btnStart_Click
        End If
    Else
        ''
        ''      עצור עדכונים
        ''
        K300Obj.K300StopStream (RezefStream)
        tmrGetK300RZ.Enabled = False
        Set OpColl = Nothing
        btnStart.Caption = "התחל"
        cmboMadad.Enabled = True
        cmboSug.Enabled = True
        txtBno.Enabled = True
        btnStartPush.Enabled = True
        lastTime = "0"
    End If
End Sub

Private Sub grid_Click()
    Call grid.SetFocus
    
    ' הקשה בשורה 0 מיועדת למיון
    If grid.Rows <= 1 Then
        Exit Sub
    End If
    
    If grid.MouseRow = 0 Then
        grid.col = grid.MouseCol
        If sortAcc = True Then
            grid.SORT = flexSortGenericAscending
            sortAcc = False
        Else
            grid.SORT = flexSortGenericDescending
            sortAcc = True
        End If
        Exit Sub
    End If
End Sub

Private Sub tmrGetK300RZ_Timer()
    Dim rc As Long
    Dim vecData() As TaskBarLib.K300RzfType
    Dim rec As TaskBarLib.K300RzfType
    Dim i As Long
    Dim rowInd As Long
    Dim tmp As String
    Dim Key As Long


    tmp = lastTime
    rc = K300Obj.GetK300RZ(vecData, lastTime, StockNumber, StockType, MadadType)
    If rc <= 0 Then
        Exit Sub
    End If
    
'    If Val(tmp) = 0 Then
'        grid.Rows = grid.Rows + rc
'        For i = LBound(vecData) To UBound(vecData)
'            rowInd = i + 1
'            Call OpColl.Add(rowInd, Trim$(vecData(i).BNO_Num))
'        Next i
'    End If

    If OpColl Is Nothing Then
        Set OpColl = New Collection
    End If

    Const ColSize As Long = 2000
    
    If OpColl.Count = 0 Then
        grid.Rows = 2
'        Dim ii As Long
'        For ii = 0 To ColSize - 1
'            Call OpColl.Add("-1", str(ii))
'        Next ii
        
        For i = LBound(vecData) To UBound(vecData)
            Key = Val(Trim$(vecData(i).BNO_Num))
            'Call OpColl.Remove(str$(key))
            Call OpColl.Add(str$(grid.Rows), str$(Key))
            grid.Rows = grid.Rows + 1
        Next i
    End If
    
    For i = LBound(vecData) To UBound(vecData)
        
'        Dim indx As Long
'        indx = Val(vecData(i).BNO_Num) Mod 2000
'        If OpColl.Item(str(indx)) = "-1" Then
'            grid.Rows = grid.Rows + 1
'            Call OpColl.Remove(str(indx))
'            Call OpColl.Add(str$(grid.Rows), str(indx))
'        End If
            
        
        Key = Val(Trim$(vecData(i).BNO_Num))
        rowInd = OpColl.Item(str$(Key))
        'rowInd = Val(OpColl.Item(str(indx))) - 1
        grid.TextMatrix(rowInd, COL_SIDURI) = rowInd - 1
        grid.TextMatrix(rowInd, COL_NAME) = vecData(i).BNO_NAME
        grid.TextMatrix(rowInd, COL_BNO) = vecData(i).BNO_Num
        grid.TextMatrix(rowInd, COL_CHANGE) = vecData(i).LST_DF_BS
        grid.TextMatrix(rowInd, COL_STAGE) = vecData(i).shlav
        grid.TextMatrix(rowInd, COL_STATUS) = vecData(i).Status
        Call FillSheetCell(Val(vecData(i).LST_DL_PR), rowInd, COL_PRC)
        Call FillSheetCell(Val(vecData(i).BASIS_PRC), rowInd, COL_PRCBASIS)
        Call FillSheetCell(Val(vecData(i).LMT_BY1), rowInd, COL_PRCBID)
        Call FillSheetCell(Val(vecData(i).LMY_BY1_NV), rowInd, COL_NVBID)
        Call FillSheetCell(Val(vecData(i).LMT_SL1), rowInd, COL_PRCASK)
        Call FillSheetCell(Val(vecData(i).LMY_SL1_NV), rowInd, COL_NVASK)
        grid.TextMatrix(rowInd, COL_CHANGE_OPEN) = vecData(i).LST_DF_OPN
    Next i
    
    Erase vecData
   
End Sub


Sub FillSheetCell(value As Double, rowInd As Long, col As Long)
    If Val(grid.TextMatrix(rowInd, col)) <> 0 And Val(grid.TextMatrix(rowInd, col)) <> value Then
        grid.col = sheetLastCellCol
        grid.row = sheetLastCellRow
        grid.CellBackColor = RGB(0, 0, 0)
        grid.col = col
        grid.row = rowInd
        grid.CellBackColor = RGB(255, 0, 0)
        sheetLastCellCol = col
        sheetLastCellRow = rowInd
    End If
    grid.TextMatrix(rowInd, col) = value

End Sub

Private Sub K3Events_OnRezef(data As K300RzfType)
    Dim rowInd As Long
    
    rowInd = Val(OpColl.Item(Trim$(data.BNO_Num)))
    grid.TextMatrix(rowInd, COL_SIDURI) = rowInd - 1
    grid.TextMatrix(rowInd, COL_NAME) = data.BNO_NAME
    grid.TextMatrix(rowInd, COL_BNO) = data.BNO_Num
    grid.TextMatrix(rowInd, COL_CHANGE) = data.LST_DF_BS
    grid.TextMatrix(rowInd, COL_STAGE) = data.shlav
    grid.TextMatrix(rowInd, COL_STATUS) = data.Status
    Call FillSheetCell(Val(data.LST_DL_PR), rowInd, COL_PRC)
    Call FillSheetCell(Val(data.BASIS_PRC), rowInd, COL_PRCBASIS)
    Call FillSheetCell(Val(data.LMT_BY1), rowInd, COL_PRCBID)
    Call FillSheetCell(Val(data.LMY_BY1_NV), rowInd, COL_NVBID)
    Call FillSheetCell(Val(data.LMT_SL1), rowInd, COL_PRCASK)
    Call FillSheetCell(Val(data.LMY_SL1_NV), rowInd, COL_NVASK)
    grid.TextMatrix(rowInd, COL_CHANGE_OPEN) = data.LST_DF_OPN

End Sub

Private Sub GetAll()
    Dim rc As Long
    Dim vecData() As TaskBarLib.K300RzfType
    Dim rec As TaskBarLib.K300RzfType
    Dim i As Long
    Dim rowInd As Long
    Dim tmp As String

    rc = K300Obj.GetK300RZ(vecData, "0", StockNumber, StockType, MadadType)
    If rc <= 0 Then
        Exit Sub
    End If
    
    If OpColl Is Nothing Then
        Set OpColl = New Collection
    End If

    grid.Rows = 2 ' title and madad row
    grid.Rows = grid.Rows + UBound(vecData) + 1 ' row for each paper
    
    If OpColl.Count = 0 Then
        For i = LBound(vecData) To UBound(vecData)
            Call OpColl.Add(str$(i + 2), Trim$(vecData(i).BNO_Num))
        Next i
    End If
    
    For i = LBound(vecData) To UBound(vecData)
        rowInd = OpColl.Item(Trim$(vecData(i).BNO_Num))
        grid.TextMatrix(rowInd, COL_SIDURI) = rowInd - 1
        grid.TextMatrix(rowInd, COL_NAME) = vecData(i).BNO_NAME
        grid.TextMatrix(rowInd, COL_BNO) = vecData(i).BNO_Num
        grid.TextMatrix(rowInd, COL_CHANGE) = vecData(i).LST_DF_BS
        grid.TextMatrix(rowInd, COL_STAGE) = vecData(i).shlav
        grid.TextMatrix(rowInd, COL_STATUS) = vecData(i).Status
        Call FillSheetCell(Val(vecData(i).LST_DL_PR), rowInd, COL_PRC)
        Call FillSheetCell(Val(vecData(i).BASIS_PRC), rowInd, COL_PRCBASIS)
        Call FillSheetCell(Val(vecData(i).LMT_BY1), rowInd, COL_PRCBID)
        Call FillSheetCell(Val(vecData(i).LMY_BY1_NV), rowInd, COL_NVBID)
        Call FillSheetCell(Val(vecData(i).LMT_SL1), rowInd, COL_PRCASK)
        Call FillSheetCell(Val(vecData(i).LMY_SL1_NV), rowInd, COL_NVASK)
        grid.TextMatrix(rowInd, COL_CHANGE_OPEN) = vecData(i).LST_DF_OPN
    Next i
    
    Erase vecData
End Sub



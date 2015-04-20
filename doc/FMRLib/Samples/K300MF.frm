VERSION 5.00
Object = "{5E9E78A0-531B-11CF-91F6-C2863C385E30}#1.0#0"; "MSFLXGRD.OCX"
Begin VB.Form K300MF 
   Caption         =   "נתוני שוק - מעו""ף"
   ClientHeight    =   8340
   ClientLeft      =   1665
   ClientTop       =   1935
   ClientWidth     =   13035
   LinkTopic       =   "Form1"
   RightToLeft     =   -1  'True
   ScaleHeight     =   8340
   ScaleWidth      =   13035
   Begin VB.CommandButton btnStartPush 
      Caption         =   "התחל דחיפה"
      Height          =   375
      Left            =   1440
      RightToLeft     =   -1  'True
      TabIndex        =   8
      Top             =   7920
      Width           =   1215
   End
   Begin VB.CommandButton btnStart 
      Caption         =   "התחל משיכה"
      Height          =   375
      Left            =   120
      RightToLeft     =   -1  'True
      TabIndex        =   7
      Top             =   7920
      Width           =   1215
   End
   Begin VB.Timer tmrGetK300MF 
      Enabled         =   0   'False
      Left            =   5160
      Top             =   3960
   End
   Begin VB.TextBox txtOption 
      Alignment       =   1  'Right Justify
      Height          =   285
      Left            =   5760
      RightToLeft     =   -1  'True
      TabIndex        =   6
      Top             =   7920
      Width           =   1095
   End
   Begin VB.ComboBox cmboMonth 
      Height          =   315
      ItemData        =   "K300MF.frx":0000
      Left            =   8400
      List            =   "K300MF.frx":002B
      RightToLeft     =   -1  'True
      Style           =   2  'Dropdown List
      TabIndex        =   2
      Top             =   7920
      Width           =   1335
   End
   Begin VB.ComboBox cmboAsset 
      Height          =   315
      ItemData        =   "K300MF.frx":008C
      Left            =   10800
      List            =   "K300MF.frx":0093
      RightToLeft     =   -1  'True
      Style           =   2  'Dropdown List
      TabIndex        =   1
      Top             =   7920
      Width           =   1335
   End
   Begin MSFlexGridLib.MSFlexGrid grid 
      Height          =   7815
      Left            =   0
      TabIndex        =   0
      Top             =   0
      Width           =   13095
      _ExtentX        =   23098
      _ExtentY        =   13785
      _Version        =   393216
      RightToLeft     =   -1  'True
   End
   Begin VB.Label lblOption 
      Alignment       =   1  'Right Justify
      Caption         =   "מס אופציה"
      Height          =   255
      Left            =   6960
      RightToLeft     =   -1  'True
      TabIndex        =   5
      Top             =   7920
      Width           =   975
   End
   Begin VB.Label lblMonth 
      Alignment       =   1  'Right Justify
      Caption         =   "חודש"
      Height          =   255
      Left            =   9840
      RightToLeft     =   -1  'True
      TabIndex        =   4
      Top             =   7920
      Width           =   615
   End
   Begin VB.Label lblAsset 
      Alignment       =   1  'Right Justify
      Caption         =   "נכס בסיס"
      Height          =   255
      Left            =   11760
      RightToLeft     =   -1  'True
      TabIndex        =   3
      Top             =   7920
      Width           =   1215
   End
End
Attribute VB_Name = "K300MF"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Public sheetLastCellCol As Long
Public sheetLastCellRow As Long

Public OptionNumber As String
Public BaseAsset As Long
Public mon As MonthType
Private lastTime As String
Private OpColl As Collection

Private Const SplitChar As String = " == "


Private K300Obj As TaskBarLib.K300
Attribute K300Obj.VB_VarHelpID = -1
Private WithEvents K3Events As TaskBarLib.K300Events
Attribute K3Events.VB_VarHelpID = -1

Private Sub btnStart_Click()
   If tmrGetK300MF.Enabled = False Then
        btnStart.Caption = "עצור משיכה"
        btnStartPush.Enabled = False
        cmboAsset.Enabled = False
        cmboMonth.Enabled = False
        txtOption.Enabled = False
        Set OpColl = New Collection
        If K300Obj.K300StartStream(MaofStream) <> 0 Then
            MsgBox "כשלון באיתחול זרימת נתוני שוק"
            'Call btnStart_Click
        End If
        If txtOption.Text <> "" And IsNumeric(txtOption.Text) Then
            OptionNumber = txtOption.Text
        End If
        
        Call GetAll
        tmrGetK300MF.Enabled = True
    Else
        K300Obj.K300StopStream (MaofStream)
        tmrGetK300MF.Enabled = False
        Set OpColl = Nothing
        btnStart.Caption = "התחל משיכה"
        btnStartPush.Enabled = True
        cmboAsset.Enabled = True
        cmboMonth.Enabled = True
        txtOption.Enabled = True
        lastTime = "0"
        OptionNumber = "00000000"
    End If
End Sub

Private Sub btnStartPush_Click()
   If btnStartPush.Caption = "התחל דחיפה" Then
   
        btnStartPush.Caption = "עצור דחיפה"
        btnStart.Enabled = False
        cmboAsset.Enabled = False
        cmboMonth.Enabled = False
        txtOption.Enabled = False
        Set OpColl = New Collection
        If K300Obj.K300StartStream(MaofStream) <> 0 Then
            MsgBox "כשלון באיתחול זרימת נתוני שוק"
            'Call btnStart_Click
        End If
        If txtOption.Text <> "" And IsNumeric(txtOption.Text) Then
            OptionNumber = txtOption.Text
        End If
        
        ' prepare the collection and the grid
        Call GetAll
        
        ' must be after the call to GetAll so we won't get
        ' events befroe we are ready
        If K3Events Is Nothing Then
            Set K3Events = New TaskBarLib.K300Events
        End If
        K3Events.EventsFilterBno = OptionNumber
        K3Events.EventsFilterMaof = True
        K3Events.EventsFilterBaseAsset = BaseAsset
        K3Events.EventsFilterMonth = mon
        K3Events.EventsFilterMadad = True
        
    Else
        K3Events.EventsFilterMaof = True
        Set K3Events = Nothing
        K300Obj.K300StopStream (MaofStream)
        Set OpColl = Nothing
        btnStartPush.Caption = "התחל דחיפה"
        btnStart.Enabled = True
        cmboAsset.Enabled = True
        cmboMonth.Enabled = True
        txtOption.Enabled = True
        lastTime = "0"
        OptionNumber = "00000000"
    End If

End Sub

Private Sub cmboAsset_Click()
    Me.grid.Rows = 16
    sheetLastCellCol = 0
    sheetLastCellRow = 0
    If cmboAsset.ListIndex = 0 Then
        BaseAsset = -1
    Else
        Dim tok() As String
        tok = Split(cmboAsset.List(cmboAsset.ListIndex), SplitChar)
        BaseAsset = tok(0)
    End If
End Sub

Private Sub cmboMonth_Click()
    Me.grid.Rows = 16
    sheetLastCellCol = 0
    sheetLastCellRow = 0
    If cmboMonth.ListIndex = 0 Then
        mon = -1
    Else
        mon = cmboMonth.ListIndex
    End If
End Sub

Private Sub Form_Load()
    Dim i As Long

    OptionNumber = "00000000"
    BaseAsset = -1
    mon = -1
    lastTime = "0"
    
    If K300Obj Is Nothing Then
        Set K300Obj = New TaskBarLib.K300
    End If
    
    K300Obj.K300SessionId = FMRLibTester.SessionId
    
    tmrGetK300MF.Enabled = False
    tmrGetK300MF.Interval = 100
    
    cmboAsset.ListIndex = 0
    cmboMonth.ListIndex = 0
    
'    grid.Height = Me.Height - 500
'    grid.Width = Me.Width - 100
'    grid.Top = 0
'    grid.Left = 0
    Dim assets() As TaskBarLib.BaseAssetType
    Dim rc As Long
    
    rc = FMRLibTester.FMRK300.GetBaseAssets2(assets)
    If rc > 0 Then
        For i = LBound(assets) To UBound(assets)
            Dim entry As String
            entry = assets(i).BaseAssetCode & _
                SplitChar & _
                 assets(i).NameHeb
            cmboAsset.AddItem (entry)
         Next
    End If
     
    
    grid.AllowUserResizing = flexResizeColumns
    grid.cols = 12
    grid.Rows = 16
    grid.TextMatrix(0, 0) = "סידורי"
    grid.TextMatrix(0, 1) = "נייר"
    grid.TextMatrix(0, 2) = "מ. מימוש"
    grid.TextMatrix(0, 3) = "שער"
    grid.TextMatrix(0, 4) = "מ. ביקוש 1"
    grid.TextMatrix(0, 5) = "כ. ביקוש 1"
    grid.TextMatrix(0, 6) = "מ. היצע 1"
    grid.TextMatrix(0, 7) = "כ. היצע 1"
    grid.TextMatrix(0, 8) = "ת. פקיעה"
    grid.TextMatrix(0, 9) = "מס. אופ."
    grid.TextMatrix(0, 10) = "שלב"
    grid.TextMatrix(0, 11) = "מצב נייר"
       
    grid.TextMatrix(1, 0) = "ת""א 25"
    grid.TextMatrix(2, 0) = "ת""א 100"
    grid.TextMatrix(3, 0) = "ת""א 75"
    grid.TextMatrix(4, 0) = "תל-טק"
    grid.TextMatrix(5, 0) = "תל-טק 15"
    grid.TextMatrix(6, 0) = "בנקים"
    grid.TextMatrix(7, 0) = "יתר 50"
    grid.TextMatrix(8, 0) = "פיננסים 15"
    grid.TextMatrix(9, 0) = "נדל""ן 15"
    grid.TextMatrix(10, 0) = "תל-דיב 20"
    grid.TextMatrix(11, 0) = "תל בונד 20"
    grid.TextMatrix(12, 0) = "יתר 120"
    grid.TextMatrix(13, 0) = "מעלה"
    grid.TextMatrix(14, 0) = "תל-בונד 40"
    grid.TextMatrix(15, 0) = "תל-בונד 60"

End Sub

Private Sub Form_Unload(Cancel As Integer)
    tmrGetK300MF.Enabled = False
    Set OpColl = Nothing
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
    
    lblAsset.Top = Me.Height - lblAsset.Height - hGap / 2
    lblAsset.Left = Me.Width - lblAsset.Width - 300
    
    cmboAsset.Top = Me.Height - cmboAsset.Height - hGap / 2
    cmboAsset.Left = Me.Width - cmboAsset.Width - 1200
    
    lblMonth.Top = Me.Height - lblMonth.Height - hGap / 2
    lblMonth.Left = Me.Width - lblMonth.Width - 3000
    
    cmboMonth.Top = Me.Height - cmboMonth.Height - hGap / 2
    cmboMonth.Left = Me.Width - cmboMonth.Width - 3500
    
    lblOption.Top = Me.Height - lblOption.Height - hGap / 2
    lblOption.Left = Me.Width - lblOption.Width - 5200
    
    txtOption.Top = Me.Height - txtOption.Height - hGap / 2
    txtOption.Left = Me.Width - txtOption.Width - 6200

    btnStart.Top = Me.Height - btnStart.Height - hGap / 2
    btnStart.Left = 0
    
    btnStartPush.Top = Me.Height - btnStartPush.Height - hGap / 2
    btnStartPush.Left = btnStart.Left + 20 + btnStart.Width
End Sub

Sub FillSheetMadad(BnoNum As Long, BnoName As String, BnoVal As String)
    Dim col As Long
    Dim row As Long
    Dim name As String
    
    If BnoNum = 142 Then
        row = 1
        name = "TLV25"
    ElseIf BnoNum = 137 Then
        row = 2
        name = "TLV100"
    ElseIf BnoNum = 143 Then
        row = 3
        name = "TLV75"
    ElseIf BnoNum = 145 Then
        row = 4
        name = "TLTEK"
    ElseIf BnoNum = 146 Then
        row = 5
        name = "TLTEK15"
    ElseIf BnoNum = 164 Then
        row = 6
        name = "BANKS"
    ElseIf BnoNum = 147 Then
        row = 7
        name = "YETER50"
    ElseIf BnoNum = 148 Then
        row = 8
        name = "TLVFIN"
    ElseIf BnoNum = 149 Then
        row = 9
        name = "TLVNDLN"
    ElseIf BnoNum = 166 Then
        row = 10
        name = "TELDIV20"
    ElseIf BnoNum = 707 Then
        row = 11
        name = "TELBOND20"
    ElseIf BnoNum = 163 Then
        row = 12
        name = "YETER120"
    ElseIf BnoNum = 150 Then
        row = 13
        name = "MAALE"
    ElseIf BnoNum = 708 Then
        row = 14
        name = "TELBOND40"
    ElseIf BnoNum = 709 Then
        row = 15
        name = "TELBOND60"
        
    Else
        Exit Sub
    End If
    
    If Val(grid.TextMatrix(row, 1)) <> BnoVal Then
        grid.col = sheetLastCellCol
        grid.row = sheetLastCellRow
        grid.CellBackColor = RGB(0, 0, 0)
        grid.col = 1
        grid.row = row
        grid.CellBackColor = RGB(255, 0, 0)
        sheetLastCellCol = 1
        sheetLastCellRow = row
    End If
    grid.TextMatrix(row, 1) = BnoVal
End Sub

Sub FillSheetCell(value As Long, rowInd As Long, col As Long)
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

Private Sub tmrGetK300MF_Timer()
    Dim rc As Long
    Dim vecData() As TaskBarLib.K300MaofType
    Dim vecData1() As TaskBarLib.K300MadadType
    Dim rec As TaskBarLib.K300MaofType
    Dim i As Long
    Dim rowInd As Long
    Dim tmp As String

    tmp = lastTime
    rc = K300Obj.GetK300MF(vecData, lastTime, OptionNumber, BaseAsset, mon)
    If rc > 0 Then
        
        For i = LBound(vecData) To UBound(vecData)
        
            rowInd = OpColl.Item(Trim$(vecData(i).BNO_Num))
            grid.TextMatrix(rowInd, 0) = rowInd - 15
            grid.TextMatrix(rowInd, 1) = vecData(i).BNO_NAME
            grid.TextMatrix(rowInd, 2) = vecData(i).EX_PRC
            grid.TextMatrix(rowInd, 8) = vecData(i).EX_DATE
            grid.TextMatrix(rowInd, 9) = vecData(i).BNO_Num
            grid.TextMatrix(rowInd, 10) = vecData(i).shlav
            grid.TextMatrix(rowInd, 11) = vecData(i).Status
            'Call FillSheetCell(Val(vecData(i).LST_DL_PR), rowInd, 3)
            grid.TextMatrix(rowInd, 3) = vecData(i).LST_DL_PR
            'Call FillSheetCell(Val(vecData(i).LMT_BY1), rowInd, 4)
            grid.TextMatrix(rowInd, 4) = vecData(i).LMT_BY1
            Call FillSheetCell(Val(vecData(i).LMY_BY1_NV), rowInd, 5)
            'Call FillSheetCell(Val(vecData(i).LMT_SL1), rowInd, 6)
            grid.TextMatrix(rowInd, 6) = vecData(i).LMT_SL1
            Call FillSheetCell(Val(vecData(i).LMY_SL1_NV), rowInd, 7)
        Next i
        
        Erase vecData
    End If  ' rc > 0
    
    rc = K300Obj.GetK300Madad(vecData1)
    If rc > 0 Then
        For i = LBound(vecData1) To UBound(vecData1)
            Call FillSheetMadad(Val(vecData1(i).BNO_N), vecData1(i).MDD_NAME, vecData1(i).Madad)
        Next i
    End If
    Erase vecData
    
End Sub

Private Sub txtOption_KeyPress(KeyAscii As Integer)
    If KeyAscii = 13 And txtOption.Text <> "" Then
        grid.Rows = 16
        sheetLastCellCol = 0
        sheetLastCellRow = 0
        OptionNumber = txtOption.Text
    End If
End Sub

'
'   אירועי קו 300
'
Private Sub K3Events_OnMaof(data As K300MaofType)
    Dim rowInd As Long
    
    rowInd = Val(OpColl.Item(Trim$(data.BNO_Num)))
    grid.TextMatrix(rowInd, 0) = rowInd - 15
    grid.TextMatrix(rowInd, 1) = data.BNO_NAME
    grid.TextMatrix(rowInd, 2) = data.EX_PRC
    grid.TextMatrix(rowInd, 8) = data.EX_DATE
    grid.TextMatrix(rowInd, 9) = data.BNO_Num
    grid.TextMatrix(rowInd, 10) = data.shlav
    grid.TextMatrix(rowInd, 11) = data.Status
    'Call FillSheetCell(Val(data.LST_DL_PR), rowInd, 3)
    grid.TextMatrix(rowInd, 3) = data.LST_DL_PR
    'Call FillSheetCell(Val(data.LMT_BY1), rowInd, 4)
    grid.TextMatrix(rowInd, 4) = data.LMT_BY1
    Call FillSheetCell(Val(data.LMY_BY1_NV), rowInd, 5)
    'Call FillSheetCell(data.LMT_SL1, rowInd, 6)
    grid.TextMatrix(rowInd, 6) = data.LMT_SL1
    Call FillSheetCell(Val(data.LMY_SL1_NV), rowInd, 7)
    
End Sub

Private Sub K3Events_OnMadad(data As K300MadadType)
    Call FillSheetMadad(Val(data.BNO_N), data.MDD_NAME, data.Madad)
End Sub

Private Sub GetAll()
    Dim rc As Long
    Dim vecData() As TaskBarLib.K300MaofType
    Dim rec As TaskBarLib.K300MaofType
    Dim i As Long
    Dim rowInd As Long
    Dim tmp As String
    Dim vecData1() As TaskBarLib.K300MadadType

    rc = K300Obj.GetK300MF(vecData, "0", OptionNumber, BaseAsset, mon)
    If rc <= 0 Then
        Exit Sub
    End If
    
    If OpColl Is Nothing Then
        Set OpColl = New Collection
    End If

    grid.Rows = 16 ' title and 15 madad rows
    grid.Rows = grid.Rows + UBound(vecData) + 1 ' row for each paper
    
    If OpColl.Count = 0 Then
        For i = LBound(vecData) To UBound(vecData)
            Call OpColl.Add(str$(i + 16), Trim$(vecData(i).BNO_Num))
        Next i
    End If
    
    For i = LBound(vecData) To UBound(vecData)
        rowInd = OpColl.Item(Trim$(vecData(i).BNO_Num))
        grid.TextMatrix(rowInd, 0) = rowInd - 15
        grid.TextMatrix(rowInd, 1) = vecData(i).BNO_NAME
        grid.TextMatrix(rowInd, 2) = vecData(i).EX_PRC
        grid.TextMatrix(rowInd, 8) = vecData(i).EX_DATE
        grid.TextMatrix(rowInd, 9) = vecData(i).BNO_Num
        grid.TextMatrix(rowInd, 10) = vecData(i).shlav
        grid.TextMatrix(rowInd, 11) = vecData(i).Status
        'Call FillSheetCell(Val(vecData(i).LST_DL_PR), rowInd, 3)
        grid.TextMatrix(rowInd, 3) = vecData(i).LST_DL_PR
        'Call FillSheetCell(Val(vecData(i).LMT_BY1), rowInd, 4)
         grid.TextMatrix(rowInd, 4) = vecData(i).LMT_BY1
        Call FillSheetCell(Val(vecData(i).LMY_BY1_NV), rowInd, 5)
        'Call FillSheetCell(Val(vecData(i).LMT_SL1), rowInd, 6)
        grid.TextMatrix(rowInd, 6) = vecData(i).LMT_SL1
        Call FillSheetCell(Val(vecData(i).LMY_SL1_NV), rowInd, 7)
    Next i
    
    rc = K300Obj.GetK300Madad(vecData1)
    If rc > 0 Then
        For i = LBound(vecData1) To UBound(vecData1)
            Call FillSheetMadad(Val(vecData1(i).BNO_N), vecData1(i).MDD_NAME, vecData1(i).Madad)
        Next i
    End If
    
    Erase vecData
End Sub



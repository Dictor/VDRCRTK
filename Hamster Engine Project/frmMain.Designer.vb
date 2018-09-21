<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Form은 Dispose를 재정의하여 구성 요소 목록을 정리합니다.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows Form 디자이너에 필요합니다.
    Private components As System.ComponentModel.IContainer

    '참고: 다음 프로시저는 Windows Form 디자이너에 필요합니다.
    '수정하려면 Windows Form 디자이너를 사용하십시오.  
    '코드 편집기에서는 수정하지 마세요.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.txtProgName = New System.Windows.Forms.Label()
        Me.grpComm = New System.Windows.Forms.GroupBox()
        Me.btnMavSend = New System.Windows.Forms.Button()
        Me.btnMavConnect = New System.Windows.Forms.Button()
        Me.txtMavPort = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtMavIP = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btnDisconnect = New System.Windows.Forms.Button()
        Me.btnConnect = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtBaud = New System.Windows.Forms.TextBox()
        Me.txtComPort = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lstRawSerial = New System.Windows.Forms.ListBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.btnListReset = New System.Windows.Forms.Button()
        Me.txtKeywordCount = New System.Windows.Forms.Label()
        Me.timLstUpdate = New System.Windows.Forms.Timer(Me.components)
        Me.lstData = New Hamster_Engine_Project.DBListView()
        Me.colID = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colIssueTime = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colLength = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colRealLength = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colCRC = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colExplain = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Label5 = New System.Windows.Forms.Label()
        Me.grpComm.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtProgName
        '
        Me.txtProgName.Font = New System.Drawing.Font("맑은 고딕", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.txtProgName.Location = New System.Drawing.Point(12, 2)
        Me.txtProgName.Name = "txtProgName"
        Me.txtProgName.Size = New System.Drawing.Size(694, 38)
        Me.txtProgName.TabIndex = 0
        Me.txtProgName.Text = "Program Name"
        Me.txtProgName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'grpComm
        '
        Me.grpComm.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpComm.Controls.Add(Me.btnMavSend)
        Me.grpComm.Controls.Add(Me.btnMavConnect)
        Me.grpComm.Controls.Add(Me.txtMavPort)
        Me.grpComm.Controls.Add(Me.Label4)
        Me.grpComm.Controls.Add(Me.txtMavIP)
        Me.grpComm.Controls.Add(Me.Label3)
        Me.grpComm.Controls.Add(Me.btnDisconnect)
        Me.grpComm.Controls.Add(Me.btnConnect)
        Me.grpComm.Controls.Add(Me.Label2)
        Me.grpComm.Controls.Add(Me.txtBaud)
        Me.grpComm.Controls.Add(Me.txtComPort)
        Me.grpComm.Controls.Add(Me.Label1)
        Me.grpComm.Controls.Add(Me.lstRawSerial)
        Me.grpComm.Location = New System.Drawing.Point(12, 43)
        Me.grpComm.Name = "grpComm"
        Me.grpComm.Size = New System.Drawing.Size(826, 127)
        Me.grpComm.TabIndex = 2
        Me.grpComm.TabStop = False
        Me.grpComm.Text = "Communication"
        '
        'btnMavSend
        '
        Me.btnMavSend.Location = New System.Drawing.Point(278, 91)
        Me.btnMavSend.Name = "btnMavSend"
        Me.btnMavSend.Size = New System.Drawing.Size(51, 27)
        Me.btnMavSend.TabIndex = 15
        Me.btnMavSend.Text = "전송"
        Me.btnMavSend.UseVisualStyleBackColor = True
        '
        'btnMavConnect
        '
        Me.btnMavConnect.Location = New System.Drawing.Point(217, 91)
        Me.btnMavConnect.Name = "btnMavConnect"
        Me.btnMavConnect.Size = New System.Drawing.Size(51, 27)
        Me.btnMavConnect.TabIndex = 14
        Me.btnMavConnect.Text = "연결"
        Me.btnMavConnect.UseVisualStyleBackColor = True
        '
        'txtMavPort
        '
        Me.txtMavPort.Location = New System.Drawing.Point(217, 55)
        Me.txtMavPort.Name = "txtMavPort"
        Me.txtMavPort.Size = New System.Drawing.Size(112, 25)
        Me.txtMavPort.TabIndex = 13
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(148, 55)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(67, 25)
        Me.Label4.TabIndex = 12
        Me.Label4.Text = "MAV Port"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtMavIP
        '
        Me.txtMavIP.Location = New System.Drawing.Point(217, 21)
        Me.txtMavIP.Name = "txtMavIP"
        Me.txtMavIP.Size = New System.Drawing.Size(112, 25)
        Me.txtMavIP.TabIndex = 11
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(149, 21)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(62, 25)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "MAV IP"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'btnDisconnect
        '
        Me.btnDisconnect.Enabled = False
        Me.btnDisconnect.Location = New System.Drawing.Point(66, 91)
        Me.btnDisconnect.Name = "btnDisconnect"
        Me.btnDisconnect.Size = New System.Drawing.Size(78, 27)
        Me.btnDisconnect.TabIndex = 9
        Me.btnDisconnect.Text = "연결 끊기"
        Me.btnDisconnect.UseVisualStyleBackColor = True
        '
        'btnConnect
        '
        Me.btnConnect.Location = New System.Drawing.Point(9, 91)
        Me.btnConnect.Name = "btnConnect"
        Me.btnConnect.Size = New System.Drawing.Size(51, 27)
        Me.btnConnect.TabIndex = 3
        Me.btnConnect.Text = "연결"
        Me.btnConnect.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(6, 55)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(62, 25)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "Baudrate"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtBaud
        '
        Me.txtBaud.Location = New System.Drawing.Point(74, 55)
        Me.txtBaud.Name = "txtBaud"
        Me.txtBaud.Size = New System.Drawing.Size(70, 25)
        Me.txtBaud.TabIndex = 5
        Me.txtBaud.Text = "115200"
        '
        'txtComPort
        '
        Me.txtComPort.Location = New System.Drawing.Point(74, 21)
        Me.txtComPort.Name = "txtComPort"
        Me.txtComPort.Size = New System.Drawing.Size(70, 25)
        Me.txtComPort.TabIndex = 3
        Me.txtComPort.Text = "COM"
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(6, 21)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(62, 25)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Port"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lstRawSerial
        '
        Me.lstRawSerial.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstRawSerial.Font = New System.Drawing.Font("맑은 고딕", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.lstRawSerial.FormattingEnabled = True
        Me.lstRawSerial.Location = New System.Drawing.Point(334, 21)
        Me.lstRawSerial.Margin = New System.Windows.Forms.Padding(2)
        Me.lstRawSerial.Name = "lstRawSerial"
        Me.lstRawSerial.Size = New System.Drawing.Size(487, 69)
        Me.lstRawSerial.TabIndex = 1
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.btnListReset)
        Me.GroupBox1.Controls.Add(Me.txtKeywordCount)
        Me.GroupBox1.Controls.Add(Me.lstData)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 176)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(826, 377)
        Me.GroupBox1.TabIndex = 10
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Data"
        '
        'btnListReset
        '
        Me.btnListReset.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnListReset.Location = New System.Drawing.Point(709, 11)
        Me.btnListReset.Name = "btnListReset"
        Me.btnListReset.Size = New System.Drawing.Size(112, 25)
        Me.btnListReset.TabIndex = 16
        Me.btnListReset.Text = "리스트 초기화"
        Me.btnListReset.UseVisualStyleBackColor = True
        '
        'txtKeywordCount
        '
        Me.txtKeywordCount.Location = New System.Drawing.Point(6, 19)
        Me.txtKeywordCount.Name = "txtKeywordCount"
        Me.txtKeywordCount.Size = New System.Drawing.Size(465, 17)
        Me.txtKeywordCount.TabIndex = 1
        Me.txtKeywordCount.Text = "키워드 갯수 : "
        '
        'timLstUpdate
        '
        Me.timLstUpdate.Interval = 500
        '
        'lstData
        '
        Me.lstData.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstData.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colID, Me.colIssueTime, Me.colLength, Me.colRealLength, Me.colCRC, Me.colExplain})
        Me.lstData.Location = New System.Drawing.Point(6, 39)
        Me.lstData.Name = "lstData"
        Me.lstData.Size = New System.Drawing.Size(815, 332)
        Me.lstData.TabIndex = 0
        Me.lstData.UseCompatibleStateImageBehavior = False
        Me.lstData.View = System.Windows.Forms.View.Details
        '
        'colID
        '
        Me.colID.Text = "ID"
        '
        'colIssueTime
        '
        Me.colIssueTime.Text = "시간"
        Me.colIssueTime.Width = 80
        '
        'colLength
        '
        Me.colLength.Text = "길이"
        Me.colLength.Width = 50
        '
        'colRealLength
        '
        Me.colRealLength.Text = "실제길이"
        Me.colRealLength.Width = 70
        '
        'colCRC
        '
        Me.colCRC.Text = "CRC"
        Me.colCRC.Width = 100
        '
        'colExplain
        '
        Me.colExplain.Text = "메세지 설명"
        Me.colExplain.Width = 450
        '
        'Label5
        '
        Me.Label5.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label5.Font = New System.Drawing.Font("맑은 고딕", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.Label5.Location = New System.Drawing.Point(643, 9)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(191, 18)
        Me.Label5.TabIndex = 11
        Me.Label5.Text = "제작 김정현 (kimdictor@gmail.com)"
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.ClientSize = New System.Drawing.Size(850, 565)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.grpComm)
        Me.Controls.Add(Me.txtProgName)
        Me.Font = New System.Drawing.Font("맑은 고딕", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.Name = "frmMain"
        Me.Text = "VDRC NEOM8P Test Program"
        Me.grpComm.ResumeLayout(False)
        Me.grpComm.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents txtProgName As System.Windows.Forms.Label
    Friend WithEvents grpComm As System.Windows.Forms.GroupBox
    Friend WithEvents btnDisconnect As System.Windows.Forms.Button
    Friend WithEvents btnConnect As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtBaud As System.Windows.Forms.TextBox
    Friend WithEvents txtComPort As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lstRawSerial As System.Windows.Forms.ListBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents colLength As System.Windows.Forms.ColumnHeader
    Friend WithEvents colID As System.Windows.Forms.ColumnHeader
    Friend WithEvents colCRC As System.Windows.Forms.ColumnHeader
    Friend WithEvents txtKeywordCount As System.Windows.Forms.Label
    Friend WithEvents colExplain As System.Windows.Forms.ColumnHeader
    Friend WithEvents colRealLength As System.Windows.Forms.ColumnHeader
    Friend WithEvents timLstUpdate As System.Windows.Forms.Timer
    Friend WithEvents colIssueTime As System.Windows.Forms.ColumnHeader
    Friend WithEvents lstData As DBListView
    Friend WithEvents txtMavPort As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtMavIP As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents btnMavConnect As System.Windows.Forms.Button
    Friend WithEvents btnMavSend As System.Windows.Forms.Button
    Friend WithEvents btnListReset As System.Windows.Forms.Button
    Friend WithEvents Label5 As System.Windows.Forms.Label
End Class

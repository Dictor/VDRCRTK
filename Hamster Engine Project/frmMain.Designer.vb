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
        Me.grpSerial = New System.Windows.Forms.GroupBox()
        Me.btnDisconnect = New System.Windows.Forms.Button()
        Me.btnConnect = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtBaud = New System.Windows.Forms.TextBox()
        Me.txtComPort = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lstRawSerial = New System.Windows.Forms.ListBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.txtKeywordCount = New System.Windows.Forms.Label()
        Me.lstData = New Hamster_Engine_Project.DBListView()
        Me.colID = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colIssueTime = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colLength = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colRealLength = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colCRC = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colExplain = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.timLstUpdate = New System.Windows.Forms.Timer(Me.components)
        Me.grpSerial.SuspendLayout()
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
        'grpSerial
        '
        Me.grpSerial.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpSerial.Controls.Add(Me.btnDisconnect)
        Me.grpSerial.Controls.Add(Me.btnConnect)
        Me.grpSerial.Controls.Add(Me.Label2)
        Me.grpSerial.Controls.Add(Me.txtBaud)
        Me.grpSerial.Controls.Add(Me.txtComPort)
        Me.grpSerial.Controls.Add(Me.Label1)
        Me.grpSerial.Controls.Add(Me.lstRawSerial)
        Me.grpSerial.Location = New System.Drawing.Point(12, 43)
        Me.grpSerial.Name = "grpSerial"
        Me.grpSerial.Size = New System.Drawing.Size(826, 127)
        Me.grpSerial.TabIndex = 2
        Me.grpSerial.TabStop = False
        Me.grpSerial.Text = "Serial"
        '
        'btnDisconnect
        '
        Me.btnDisconnect.Enabled = False
        Me.btnDisconnect.Location = New System.Drawing.Point(172, 71)
        Me.btnDisconnect.Name = "btnDisconnect"
        Me.btnDisconnect.Size = New System.Drawing.Size(62, 50)
        Me.btnDisconnect.TabIndex = 9
        Me.btnDisconnect.Text = "연결" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "끊기"
        Me.btnDisconnect.UseVisualStyleBackColor = True
        '
        'btnConnect
        '
        Me.btnConnect.Location = New System.Drawing.Point(172, 15)
        Me.btnConnect.Name = "btnConnect"
        Me.btnConnect.Size = New System.Drawing.Size(62, 50)
        Me.btnConnect.TabIndex = 3
        Me.btnConnect.Text = "연결"
        Me.btnConnect.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(6, 75)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(62, 25)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "Baudrate"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtBaud
        '
        Me.txtBaud.Location = New System.Drawing.Point(74, 75)
        Me.txtBaud.Name = "txtBaud"
        Me.txtBaud.Size = New System.Drawing.Size(92, 25)
        Me.txtBaud.TabIndex = 5
        Me.txtBaud.Text = "115200"
        '
        'txtComPort
        '
        Me.txtComPort.Location = New System.Drawing.Point(74, 36)
        Me.txtComPort.Name = "txtComPort"
        Me.txtComPort.Size = New System.Drawing.Size(92, 25)
        Me.txtComPort.TabIndex = 3
        Me.txtComPort.Text = "COM"
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(6, 36)
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
        Me.lstRawSerial.Location = New System.Drawing.Point(239, 14)
        Me.lstRawSerial.Margin = New System.Windows.Forms.Padding(2)
        Me.lstRawSerial.Name = "lstRawSerial"
        Me.lstRawSerial.Size = New System.Drawing.Size(582, 108)
        Me.lstRawSerial.TabIndex = 1
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.txtKeywordCount)
        Me.GroupBox1.Controls.Add(Me.lstData)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 176)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(826, 377)
        Me.GroupBox1.TabIndex = 10
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Data"
        '
        'txtKeywordCount
        '
        Me.txtKeywordCount.Location = New System.Drawing.Point(6, 19)
        Me.txtKeywordCount.Name = "txtKeywordCount"
        Me.txtKeywordCount.Size = New System.Drawing.Size(465, 17)
        Me.txtKeywordCount.TabIndex = 1
        Me.txtKeywordCount.Text = "키워드 갯수 : "
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
        'timLstUpdate
        '
        Me.timLstUpdate.Interval = 500
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.ClientSize = New System.Drawing.Size(850, 565)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.grpSerial)
        Me.Controls.Add(Me.txtProgName)
        Me.Font = New System.Drawing.Font("맑은 고딕", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.Name = "frmMain"
        Me.Text = "VDRC NEOM8P Test Program"
        Me.grpSerial.ResumeLayout(False)
        Me.grpSerial.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents txtProgName As System.Windows.Forms.Label
    Friend WithEvents grpSerial As System.Windows.Forms.GroupBox
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
End Class

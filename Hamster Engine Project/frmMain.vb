Imports System.IO
Imports System.IO.Ports
Imports System.Net
Imports System.Text
Imports System.Threading
Imports System.Windows.Forms
Imports MavLinkNet

Public Class frmMain
    Private Serial As New SerialPort
    Private thSerial As Thread
    Private thMessage As New Thread(AddressOf ProcessMessage)

    Private bufRawSerial As New Queue(Of Byte)(1024)
    Private bufEachMessage As New List(Of Byte)
    Private latestData As New Dictionary(Of String, RTCMMessage)

    Private RTCMexplain As New Dictionary(Of String, String)
    Private MessageCount As Long = 0

    Private mavudp As MavLinkNet.MavLinkUdpTransport
    Private isMavReady As Boolean = False
    Private mavMessageFlag As Byte

    Private Sub txtProgName_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtProgName.Text = Project.Version.GetName & " " & Project.Version.GetVersion(True)
        For Each nowline In Split(File.ReadAllText(Application.StartupPath & "\RTCMexplain.txt"), vbCrLf)
            Dim pline = Split(nowline, ",")
            RTCMexplain.Add(pline(0), pline(1))
        Next
        lstData.Sorting = SortOrder.Descending
        lstData.ListViewItemSorter = New ListViewComparer(1, SortOrder.Ascending)
        Dim s = lstData.ListViewItemSorter()
    End Sub

    Private Function WaitRTCMMessage() As RTCMMessage
        Dim RTCMcount = 0, RTCMlen = 0, RTCMmsgid As Integer
        Dim RTCMcrc As Long = 0
        Dim RTCMdata As New List(Of Byte)
        Dim result As RTCMMessage
        Do While True
            Do Until bufRawSerial.Count > 0 '메세지 수신까지 대기
            Loop
            Dim nowbyte = bufRawSerial.Dequeue

            If RTCMcount = 0 Then
                If Not nowbyte = &HD3 Then 'RTCM 헤더 수신까지 대기
                    Continue Do
                End If
            ElseIf RTCMcount = 1 Then '두번째 세번째 바이트는 메세지 length
                RTCMlen = (nowbyte And &H3) << 6
            ElseIf RTCMcount = 2 Then
                RTCMlen += nowbyte
            ElseIf RTCMcount <= 2 + RTCMlen Then
                RTCMdata.Add(nowbyte)
            ElseIf RTCMcount > 2 + RTCMlen And RTCMcount < (2 + RTCMlen) + 2 Then
                RTCMcrc = RTCMcrc << 8
                RTCMcrc += nowbyte
            ElseIf RTCMcount = (2 + RTCMlen) + 2 Then
                RTCMcrc = RTCMcrc << 8
                RTCMcrc += nowbyte
                bufEachMessage.Add(nowbyte)
                result = New RTCMMessage(RTCMdata, bufEachMessage.ToArray, RTCMlen, RTCMcrc, Now)
                bufEachMessage.Clear()
                Exit Do
            End If

            bufEachMessage.Add(nowbyte)
            RTCMcount += 1
        Loop
        Return result
    End Function

    Private Sub ProcessMessage()
        Dim exmsg, exmsglen As String
        Do While True
            Try
                Dim r = WaitRTCMMessage()
                If latestData.ContainsKey(r.ID) Then
                    latestData(r.ID) = r
                Else
                    latestData.Add(r.ID, r)
                End If
                If RTCMexplain.ContainsKey(r.ID) Then
                    If isMavReady Then
                        MavSendRTCM(r)
                    End If
                End If
                'lstRawSerialPrint(ByteArrayToString(r.FullMessage))
                Project.LogWrite.DynamicInvoke("수신한 메세지 HEX : " & ByteArrayToString(r.FullMessage))
                MessageCount += 1
                txtKeywordCount.Text = "수신한 메세지 개수 : " & MessageCount & "   키워드 개수 : " & latestData.Count
            Catch ex As ThreadAbortException
            Catch ex As Exception
                Project.EngineShowErr.DynamicInvoke("PROCESS_MESSAGE_ERROR", "메서지를 처리하는데 실패했습니다! 메세지 내용 : " & exmsg & " (길이 : " & exmsglen & ")", "", "", ex)
            End Try
        Loop
    End Sub

    Private Sub SerialRead()
        Do While True
            Try
                If Not Serial.IsOpen Then
                    lstRawSerialPrint("[시리얼 포트가 예상치 못하게 닫혔습니다]")
                    btnDisconnect_Click(Nothing, Nothing)
                End If
                Do While Serial.BytesToRead > 0
                    bufRawSerial.Enqueue(Serial.ReadByte)
                Loop
            Catch ex As TimeoutException
            Catch ex As ThreadAbortException
            Catch ex As Exception
                lstRawSerialPrint("[시리얼 포트를 읽는 중 오류가 발생했습니다] → " & ex.GetType.FullName)
                btnDisconnect_Click(Nothing, Nothing)
            End Try
        Loop
    End Sub

    Private Sub lstRawSerialPrint(msg As String)
        lstRawSerial.Items.Add(msg)
        lstRawSerial.SelectedIndex = lstRawSerial.Items.Count - 1
        Project.LogWrite.DynamicInvoke(msg)
    End Sub

    Private Sub btnConnect_Click(sender As Object, e As EventArgs) Handles btnConnect.Click
        Try
            If txtComPort.Text = "" Or txtBaud.Text = "" Then
                Exit Sub
            End If
            btnConnect.Enabled = False
            btnDisconnect.Enabled = True

            Serial.PortName = txtComPort.Text
            Serial.BaudRate = txtBaud.Text
            lstRawSerialPrint("[시리얼 포트 열기] → " & Serial.PortName & ", " & Serial.BaudRate)
            Serial.Open()
            thSerial = New Thread(AddressOf SerialRead)
            thSerial.Start()
            thMessage = New Thread(AddressOf ProcessMessage)
            thMessage.Start()
            timLstUpdate.Enabled = True

            txtBaud.Enabled = False
            txtComPort.Enabled = False
            lstRawSerialPrint("[시리얼 포트 열기 성공]")
            latestData.Clear()
        Catch ex As Exception
            lstRawSerialPrint("[시리얼 포트 열기 실패] → " & ex.GetType.FullName)
            Project.EngineShowErr.DynamicInvoke("COMPORT_OPEN_ERROR", "시리얼 포트를 여는데 실패했습니다!", "", "", ex)
            btnDisconnect_Click(Nothing, Nothing)
        End Try
    End Sub

    Private Sub btnDisconnect_Click(sender As Object, e As EventArgs) Handles btnDisconnect.Click
        Try
            thSerial.Abort()
            thMessage.Abort()
            Serial.Close()
        Catch
        End Try
        timLstUpdate.Enabled = False
        btnConnect.Enabled = True
        btnDisconnect.Enabled = False
        txtBaud.Enabled = True
        txtComPort.Enabled = True
        lstRawSerialPrint("[시리얼 포트 닫기 성공]")
    End Sub

    Private Function RTCMIDtoExplain(id As String) As String
        If RTCMexplain.ContainsKey(id) Then
            Return RTCMexplain(id)
        Else
            Return ""
        End If
    End Function

    Public Shared Function ByteArrayToString(ByVal ba As Byte()) As String
        Dim hex As StringBuilder = New StringBuilder(ba.Length * 2)

        For Each b As Byte In ba
            hex.AppendFormat("{0:x2}", b)
        Next

        Return hex.ToString.ToUpper()
    End Function

    Private Sub timLstUpdate_Tick(sender As Object, e As EventArgs) Handles timLstUpdate.Tick
        Try
            lstData.Items.Clear()
            Dim displaylst As New Dictionary(Of String, RTCMMessage)(latestData)
            For Each nowele In displaylst
                Dim timediff As TimeSpan = Now - nowele.Value.IssueTime
                Dim lst As ListViewItem
                If chkShowDefID.Checked Then
                    If RTCMexplain.ContainsKey(nowele.Value.ID) Then
                        lst = New ListViewItem({nowele.Value.ID, Math.Round(timediff.TotalSeconds, 1) & "초 전", nowele.Value.Length, nowele.Value.FullMessage.Count, nowele.Value.CRC24.ToString("x6"), RTCMIDtoExplain(nowele.Value.ID)})
                        lstData.Items.Add(lst)
                    End If
                Else
                    lst = New ListViewItem({nowele.Value.ID, Math.Round(timediff.TotalSeconds, 1) & "초 전", nowele.Value.Length, nowele.Value.FullMessage.Count, nowele.Value.CRC24.ToString("x6"), RTCMIDtoExplain(nowele.Value.ID)})
                    lstData.Items.Add(lst)
                End If
            Next
            lstData.Sort()
        Catch ex As Exception
            Project.EngineShowErr.DynamicInvoke("MESSAGE_DISPLAY_ERROR", "메세지를 표출하는 중 오류가 발생했습니다", "", "", ex)
        End Try
    End Sub

    Private Sub btnMavConnect_Click(sender As Object, e As EventArgs) Handles btnMavConnect.Click 'https://mavlink.io/kr/messages/common.html#GPS_RTCM_DATA
        Try
            mavudp = New MavLinkUdpTransport
            Dim udpip As IPAddress
            IPAddress.TryParse(txtMavIP.Text, udpip)
            lstRawSerialPrint("[MavLink 연결 시도] → " & udpip.ToString & ":" & txtMavPort.Text)
            mavudp.TargetIpAddress = udpip
            mavudp.UdpTargetPort = txtMavPort.Text
            mavudp.UdpListeningPort = 19000
            AddHandler mavudp.OnPacketReceived, AddressOf MavPacketRecv
            mavudp.Initialize()
            lstRawSerialPrint("[MavLink 연결 성공]")
            isMavReady = True
        Catch ex As Exception
            lstRawSerialPrint("[MavLink 연결 실패] → " & ex.GetType.FullName)
            Project.EngineShowErr.DynamicInvoke("MAVLINK_OPEN_ERROR", "MAVLINK 연결을 여는데 실패했습니다!", "", "", ex)
            isMavReady = False
        End Try
    End Sub

    Private Sub MavPacketRecv(sender As Object, packet As MavLinkPacket)
        Try
            lstRawSerialPrint("[MavLink 수신 이벤트] → 메세지 ID : " & packet.MessageId.ToString)
        Catch ex As Exception
            lstRawSerialPrint("[MavLink 수신 실패] → " & ex.GetType.FullName)
            Project.EngineShowErr.DynamicInvoke("MAVLINK_RECEIVE_ERROR", "MAVLINK 메세지를 수신하는데 실패했습니다!", "", "", ex)
        End Try
    End Sub

    Private Sub txtMavSend_Click(sender As Object, e As EventArgs) Handles btnMavSend.Click
        If Not isMavReady Then
            Exit Sub
        End If
        Dim msgid = InputBox("보낼 메세지의 ID를 입력하세요", "MAVLINK 메세지 전송")
        MavSendRTCM(latestData(msgid))
    End Sub

    Private Sub MavSendRTCM(r As RTCMMessage)
        Try
            lstRawSerialPrint("[MavLink 전송 요청] → ID : " & r.ID & " FLAG : " & mavMessageFlag.ToString("x2"))
            Dim msg = New UasGpsRtcmData
            Dim data = MakeRTCMDataFrame(r.FullMessage)
            If data Is Nothing Then
                lstRawSerialPrint("[MavLink 메세지 생성 오류, 생략합니다] → ID : " & r.ID)
                Exit Sub
            End If
            msg.Data = data
            msg.Len = r.Length
            msg.Flags = mavMessageFlag
            NextFlag()
            mavudp.SendMessage(msg)
        Catch ex As Exception
            lstRawSerialPrint("[MavLink 전송 실패] → " & ex.GetType.FullName)
            Project.EngineShowErr.DynamicInvoke("MAVLINK_SEND_ERROR", "MAVLINK 메세지를 전송하는데 실패했습니다!", "", "", ex)
        End Try
    End Sub

    Private Sub NextFlag()
        If mavMessageFlag = &HF8 Then
            mavMessageFlag = 0
        Else
            mavMessageFlag += 8
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnListReset.Click
        latestData.Clear()
    End Sub

    Private Function MakeRTCMDataFrame(l As Byte())
        Dim b(179) As Byte
        If l.Count > 180 Then
            Return Nothing
        End If
        For i = 0 To l.Count - 1
            b(i) = l(i)
        Next
        For i = l.Count To b.Count - 1
            b(i) = 0
        Next
        Return b
    End Function

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles btnMavDIsconnect.Click
        isMavReady = False
        mavudp.Dispose()
        lstRawSerialPrint("[MavLink 닫기 성공]")
    End Sub
End Class

Public Class RTCMMessage
    Public Data As List(Of Byte)
    Public FullMessage As Byte()
    Public Length As Long
    Public CRC24 As Long
    Public ID As Integer
    Public IssueTime As Date

    Public Sub New(data As List(Of Byte), fullmsg As Byte(), len As Long, crc As Long, time As Date)
        data = data
        FullMessage = fullmsg
        Length = len
        CRC24 = crc
        ID = FullMessage(3)
        ID = ID << 4
        ID = ID Or ((FullMessage(4) >> 4) And &HF)
        IssueTime = time
    End Sub
End Class

Public Class DBListView
    Inherits ListView
    Public Sub DBListView()
        SetStyle(ControlStyles.DoubleBuffer, True)
        SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        SetStyle(ControlStyles.UserPaint, True)
        UpdateStyles()
    End Sub
End Class

Public Class ListViewComparer
    Implements IComparer

    Public col As Integer
    Public order As SortOrder

    Public Sub New()
        col = 0
        order = SortOrder.Ascending
    End Sub

    Public Sub New(ByVal column As Integer, ByVal order As SortOrder)
        col = column
        Me.order = order
    End Sub

    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements IComparer.Compare
        Dim returnVal As Integer = -1
        Dim a = Convert.ToDouble((CType(x, ListViewItem)).SubItems(col).Text.Replace("초 전", ""))
        Dim b = Convert.ToDouble((CType(y, ListViewItem)).SubItems(col).Text.Replace("초 전", ""))
        If a > b Then
            Return 1
        ElseIf a = b Then
            Return 0
        Else
            Return -1
        End If
    End Function
End Class
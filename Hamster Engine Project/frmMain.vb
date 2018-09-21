Imports System.IO
Imports System.IO.Ports
Imports System.Text
Imports System.Threading
Imports System.Windows.Forms

Public Class frmMain
    ' Private Serial As PInvokeSerialPort.SerialPort
    Private Serial As New SerialPort
    Private thSerial As Thread
    Private thMessage As New Thread(AddressOf ProcessMessage)

    Private bufRawSerial As New Queue(Of Byte)(1024)
    Private bufMessage As New Queue(Of GPSMessage)(100)
    Dim bufEachMessage As New List(Of Byte)

    Private GPSData As New Dictionary(Of String, GPSMessage)
    Private UnknownMessageID As Integer = 0

    Private Sub txtProgName_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtProgName.Text = Project.Version.GetName & " " & Project.Version.GetVersion(True)
        lstData.Columns.Item(2).Width = lstData.Width - (lstData.Columns.Item(0).Width + lstData.Columns.Item(1).Width + 15)
    End Sub

    Private Sub ProcessByteBuffer()
        Do While bufRawSerial.Count > 0
            Dim nowbyte = bufRawSerial.Dequeue()
            If nowbyte = Encoding.ASCII.GetBytes("$")(0) Then 'NMEA 메세지 시작 Cut
                If Not bufEachMessage.Count = 0 Then
                    bufMessage.Enqueue(New GPSMessage(GPSMessage.MessageType.UNKNOWN, Nothing, bufEachMessage.ToArray))
                    bufEachMessage.Clear()
                End If
                bufEachMessage.Add(nowbyte)
            ElseIf nowbyte = Encoding.ASCII.GetBytes(vbCr)(0) Then 'NMEA 메세지 종료 Cut
                If Not bufEachMessage.Count = 0 Then
                    bufMessage.Enqueue(New GPSMessage(GPSMessage.MessageType.UNKNOWN, Nothing, bufEachMessage.ToArray))
                    bufEachMessage.Clear()
                End If
            ElseIf nowbyte = &HD3 Then 'RTCM 메세지 시작 Cut
                If Not bufEachMessage.Count = 0 Then
                    bufMessage.Enqueue(New GPSMessage(GPSMessage.MessageType.UNKNOWN, Nothing, bufEachMessage.ToArray))
                    bufEachMessage.Clear()
                End If
                bufEachMessage.Add(nowbyte)
            ElseIf nowbyte = &HB5 Then 'UBX 메세지 시작 Cut
                If Not bufEachMessage.Count = 0 Then
                    bufMessage.Enqueue(New GPSMessage(GPSMessage.MessageType.UNKNOWN, Nothing, bufEachMessage.ToArray))
                    bufEachMessage.Clear()
                End If
                bufEachMessage.Add(nowbyte)
            ElseIf Not nowbyte = Encoding.ASCII.GetBytes(vbLf)(0) Then
                bufEachMessage.Add(nowbyte)
            End If
        Loop
    End Sub

    Private Function ProcessBytetoMessage(ele As GPSMessage) As GPSMessage
        Try
            If ele.Data.Count > 0 AndAlso ele.Data(0) = &HD3 Then
                ele.Type = GPSMessage.MessageType.RTCM
                ele.Message = ByteArrayToString(ele.Data)
            ElseIf ele.Data.Count > 1 AndAlso (ele.Data(0) = &HB5 And ele.Data(1) = &H62) Then
                ele.Type = GPSMessage.MessageType.UBX
                ele.Message = ByteArrayToString(ele.Data)
            ElseIf ele.Data.Count > 1 AndAlso Encoding.ASCII.GetString(ele.Data).Substring(0, 2) = "$G" Then
                ele.Type = GPSMessage.MessageType.NMEA
                ele.Message = Encoding.ASCII.GetString(ele.Data)
            Else
                ele.Type = GPSMessage.MessageType.UNKNOWN
                ele.Message = ByteArrayToString(ele.Data)
            End If
        Catch ex As Exception
            Project.EngineShowErr.DynamicInvoke("DECODE_MESSAGE_ERROR", "메서지를 해독하는데 실패했습니다! 메세지 내용 : " & ByteArrayToString(ele.Data) & " (길이 : " & ele.Data.Count & ")", "", "", ex)
        End Try
        Return ele
    End Function


    Private Sub ProcessMessage()
        Dim exmsg, exmsglen As String
        Do While True
            Try
                ProcessByteBuffer()
                If bufMessage.Count > 0 Then
                    Dim nowmsg As GPSMessage = ProcessBytetoMessage(bufMessage.Dequeue())
                    exmsg = nowmsg.Message
                    exmsglen = nowmsg.Data.Count
                    If nowmsg.Type = GPSMessage.MessageType.NMEA Then
                        Dim pmsg As String() = Split(nowmsg.Message.Substring(1), ",")
                        Dim detaildata As String = nowmsg.Message.Substring(nowmsg.Message.IndexOf(",") + 1)
                        nowmsg.Message = detaildata
                        If GPSData.ContainsKey(pmsg(0)) Then
                            GPSData(pmsg(0)) = nowmsg
                        Else
                            GPSData.Add(pmsg(0), nowmsg)
                        End If
                    ElseIf nowmsg.Type = GPSMessage.MessageType.RTCM Then
                        Dim packetid As Integer
                        If nowmsg.Data.Count < 5 Then
                            packetid = -1
                        Else
                            packetid = nowmsg.Data(3)
                            packetid = packetid << 4
                            packetid = packetid Or ((nowmsg.Data(4) >> 4) And &HF)
                        End If

                        If GPSData.ContainsKey(packetid.ToString) Then
                            GPSData(packetid.ToString) = nowmsg
                        Else
                            GPSData.Add(packetid.ToString, nowmsg)
                        End If
                    Else
                        'Dim unkid = "UNK" + UnknownMessageID.ToString
                        Dim unkid = "UNK"
                        If GPSData.ContainsKey(unkid) Then
                            GPSData(unkid) = nowmsg
                        Else
                            GPSData.Add(unkid, nowmsg)
                        End If
                        UnknownMessageID += 1
                    End If
                    lstData.Items.Clear()
                    txtKeywordCount.Text = "키워드 갯수 : " & GPSData.Count
                    For Each nowele In GPSData
                        Dim lst As New ListViewItem({nowele.Value.TypetoString(nowele.Value.Type), nowele.Key, nowele.Value.Message})
                        lstData.Items.Add(lst)
                    Next
                End If
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

            txtBaud.Enabled = False
            txtComPort.Enabled = False
            lstRawSerialPrint("[시리얼 포트 열기 성공]")
            GPSData.Clear()
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
        bufMessage.Clear()
        btnConnect.Enabled = True
        btnDisconnect.Enabled = False
        txtBaud.Enabled = True
        txtComPort.Enabled = True
        lstRawSerialPrint("[시리얼 포트 닫기 성공]")
    End Sub

    Public Shared Function ByteArrayToString(ByVal ba As Byte()) As String
        Dim hex As StringBuilder = New StringBuilder(ba.Length * 2)

        For Each b As Byte In ba
            hex.AppendFormat("{0:x2}", b)
        Next

        Return hex.ToString.ToUpper()
    End Function
End Class

Public Class GPSMessage
    Public Type As MessageType
    Public Message As String
    Public Data As Byte()

    Public Sub New(t As MessageType, m As String, d As Byte())
        Type = t
        Message = m
        Data = d
    End Sub

    Public Enum MessageType
        NMEA
        UBX
        RTCM
        UNKNOWN
    End Enum

    Public Function TypetoString(t As MessageType) As String
        If t = MessageType.NMEA Then
            Return "NMEA"
        ElseIf t = MessageType.RTCM Then
            Return "RTCM"
        ElseIf t = MessageType.UBX Then
            Return "UBX"
        ElseIf t = MessageType.UNKNOWN Then
            Return "UNKNOWN"
        End If
    End Function
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
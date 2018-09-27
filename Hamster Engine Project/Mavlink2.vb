'MAVLINK 버전 2의 구현입니다
Imports Hamster_Engine_Project.Mavlink.MavlinkException

Public Class Mavlink
    Public Class MavlinkDefMessage
        Public Class GPS_RTCM_DATA
            '구현은 c_library_v2/common/mavlink_msg_gps_rtcm_data.h 참고
            Inherits Mavlink1Message
            Public flags As Byte
            Public len As Byte
            Public data As Byte

            ''' <summary>
            ''' GPS_RTCM_DATA (#233) 메세지를 초기화 합니다.
            ''' </summary>
            ''' <param name="seq">시퀀스 ID</param>
            ''' <param name="sysid">Sender의 시스템 ID</param>
            ''' <param name="compid">Sender의 컴포넌트 ID</param>
            ''' <param name="flag">플래그 (LSB 첫번째 비트는 메시지가 조각난 것을 의미하고, 다음 2 비트는 프래그먼트 ID이며, 나머지 5 비트는 시퀀스 ID에 사용됩니다)</param>
            ''' <param name="length">RTCM 데이터의 길이</param>
            ''' <param name="rtcmdata">RTCM 메시지 (단편화되었을 수 있음, 최대 180바이트)</param>
            Public Sub New(seq As Byte, sysid As Byte, compid As Byte, flag As Byte, length As Byte, rtcmdata As Byte())
                MyBase.New(seq, sysid, compid, &HE9, rtcmdata, &H23)
                If rtcmdata.Count > 180 Then
                    Throw New MavlinkPayloadTooLargeException(rtcmdata.Count, 180)
                End If
                Dim payload As New List(Of Byte)
                payload.Add(flag)
                payload.Add(length)
                payload.AddRange(rtcmdata)
                MyBase.Payload = payload.ToArray
            End Sub
        End Class
    End Class

    Public Class Mavlink2Message
        '프로토콜 구현은 https://mavlink.io/kr/protocol/overview.html 을 참고하세요
        '메세지 데이터 시작
        Public MagicMarker As Byte
        Public Length As Byte
        Public InCompatibleFlags As Byte
        Public CompatibleFlags As Byte
        Public Sequence As Byte
        Public SystemID As Byte
        Public ComponentID As Byte
        Public MessageID As UInt32 '3 byte만 사용합니다
        Public TargetSystemID As Byte
        Public TargetComponentID As Byte
        Public Payload As Byte() '최대 크기 253 (bytes)
        Public Checksum As UInt16
        Public Signiture(13) As Byte
        ' 메세지 데이터 끝

        Private isInited As Boolean = False

        ''' <summary>
        ''' 새로운 Mavlink 버전2 Message를 초기화합니다, 이 메서드는 메세지의 Length와 Checksum을 자동으로 계산하므로 안전합니다.
        ''' mavlink/c_library_v2/blob/master/mavlink_helpers.h의 mavlink_finalize_message_buffer
        ''' </summary>
        ''' <param name="seq">패킷의 시퀀스</param>
        ''' <param name="sysid">Sender의 시스템 ID</param>
        ''' <param name="compid">Sender의 컴포넌트 ID</param>
        ''' <param name="msgid">메세지 ID (3바이트)</param>
        ''' <param name="tsysid">포인트to포인트 전송 모드에서 타겟의 시스템 ID (선택적, 현재 사용되지 않음)</param>
        ''' <param name="tcompid">포인트to포인트 전송 모드에서 타겟의 컴포넌트 ID (선택적, 현재 사용되지 않음)</param>
        ''' <param name="payloaddata">페이로드 데이터 (최대 253바이트)</param>
        ''' <param name="crcextra">CRC Extra (메세지의 CRC)</param>
        Public Sub New(seq As Byte, sysid As Byte, compid As Byte, msgid As UInt32, tsysid As Byte, tcompid As Byte, payloaddata As Byte(), crcextra As Byte)
            Sequence = seq
            SystemID = sysid
            ComponentID = compid
            MessageID = msgid
            TargetSystemID = tsysid
            TargetComponentID = tsysid

            If Not payloaddata.Count > MavlinkConst.Protocol.v2MaximumPayloadSize Then
                Payload = payloaddata
            Else
                Throw New MavlinkPayloadTooLargeException(payloaddata.Count)
            End If

            Length = Payload.Count
            MagicMarker = MavlinkConst.Protocol.MAVLINK_STX
            InCompatibleFlags = 0
            ComponentID = 0
            Dim crctemp As Byte() = {Length, InCompatibleFlags, CompatibleFlags, Sequence, SystemID, ComponentID, MessageID & &HFF, (MessageID >> 8) & &HFF, (MessageID >> 16) & &HFF}
            Checksum = CRCX25.Calculate(crctemp)
            CRCX25.AccumulateBuffer(Payload, Checksum)
            CRCX25.Accumulate(crcextra, Checksum)
            isInited = True
        End Sub

        ''' <summary>
        ''' 메세지를 바이트 배열로 직렬화 합니다
        ''' </summary>
        ''' <returns>직렬화된 바이트 배열</returns>
        Public Function Serialize() As Byte()
            If Not isInited Then
                Throw New MavlinkMessageNotInitialized
            End If
            Dim data As New List(Of Byte) From {MagicMarker, Length, InCompatibleFlags, CompatibleFlags, Sequence, SystemID, ComponentID, MessageID & &HFF, (MessageID >> 8) & &HFF, (MessageID >> 16) & &HFF}
            data.AddRange(Payload)
            data.AddRange({Checksum And &HFF, Checksum >> 8})
            Return data.ToArray
        End Function

        ''' <summary>
        ''' 디버그를 위해 메세지의 내용을 문자열로 모두 출력합니다 
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function ToString() As String
            If Not isInited Then
                Return "메세지가 초기화 되지 않음"
            End If
            Dim ContentName As String() = {"MagicMarker", "Length", "InCompatibleFlags", "CompatibleFlags", "Sequence", "SystemID", "ComponentID", "MessageID (first)", "MessageID (middle)", "MessageID (last)"}
            Dim ResultString As String = ""
            Dim ResultData As Byte() = Serialize()
            Dim addr = 0
            For addr = 0 To 9
                ResultString += ContentName(addr) & " = " & ResultData(addr).ToString("x2") & vbCrLf
            Next
            ResultString += "Payload = "
            For addr = 10 To ResultData.Count - 3
                ResultString += ResultData(addr).ToString("x2") & " "
            Next
            ResultString += vbCrLf & "CheckSum first = " & ResultData(ResultData.Count - 2).ToString("x2") & " CheckSum last = " & ResultData(ResultData.Count - 1).ToString("x2")
            Return "[메세지 정보]" & vbCrLf & ResultString & vbCrLf & "[메세지 끝]"
        End Function
    End Class

    Public Class Mavlink1Message
        '프로토콜 구현은 https://mavlink.io/kr/protocol/overview.html 을 참고하세요
        '메세지 데이터 시작
        Public MagicMarker As Byte
        Public Length As Byte
        Public Sequence As Byte
        Public SystemID As Byte
        Public ComponentID As Byte
        Public MessageID As Byte
        Public Payload As Byte() '최대 크기 255 (bytes)
        Public Checksum As UInt16
        ' 메세지 데이터 끝

        Private isInited As Boolean = False

        ''' <summary>
        ''' 새로운 Mavlink 버전 1 Message를 초기화합니다, 이 메서드는 메세지의 Length와 Checksum을 자동으로 계산하므로 안전합니다.
        ''' mavlink/c_library_v2/blob/master/mavlink_helpers.h의 mavlink_finalize_message_buffer
        ''' </summary>
        ''' <param name="seq">패킷의 시퀀스</param>
        ''' <param name="sysid">Sender의 시스템 ID</param>
        ''' <param name="compid">Sender의 컴포넌트 ID</param>
        ''' <param name="msgid">메세지 ID </param>
        ''' <param name="payloaddata">페이로드 데이터 (최대 255바이트)</param>
        ''' <param name="crcextra">CRC Extra (메세지의 CRC)</param>
        Public Sub New(seq As Byte, sysid As Byte, compid As Byte, msgid As Byte, payloaddata As Byte(), crcextra As Byte)
            Sequence = seq
            SystemID = sysid
            ComponentID = compid
            MessageID = msgid

            If Not payloaddata.Count > MavlinkConst.Protocol.v1MaximumPayloadSize Then
                Payload = payloaddata
            Else
                Throw New MavlinkPayloadTooLargeException(payloaddata.Count)
            End If

            Length = Convert.ToByte(Payload.Count)
            MagicMarker = MavlinkConst.Protocol.MAVLINK_STX_MAVLINK1
            ComponentID = 0
            Dim crctemp As Byte() = {Length, Sequence, SystemID, ComponentID, MessageID And &HFF}
            Checksum = CRCX25.Calculate(crctemp)
            CRCX25.AccumulateBuffer(Payload, Checksum)
            CRCX25.Accumulate(crcextra, Checksum)
            isInited = True
        End Sub

        ''' <summary>
        ''' 메세지를 바이트 배열로 직렬화 합니다
        ''' </summary>
        ''' <returns>직렬화된 바이트 배열</returns>
        Public Function Serialize() As Byte()
            If Not isInited Then
                Throw New MavlinkMessageNotInitialized
            End If
            Dim data As New List(Of Byte) From {MagicMarker, Length, Sequence, SystemID, ComponentID, MessageID And &HFF}
            data.AddRange(Payload)
            data.AddRange({Checksum And &HFF, Checksum >> 8})
            Return data.ToArray
        End Function

        ''' <summary>
        ''' 디버그를 위해 메세지의 내용을 문자열로 모두 출력합니다 
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function ToString() As String
            If Not isInited Then
                Return "메세지가 초기화 되지 않음"
            End If
            Dim ContentName As String() = {"MagicMarker", "Length", "Sequence", "SystemID", "ComponentID", "MessageID"}
            Dim ResultString As String = ""
            Dim ResultData As Byte() = Serialize()
            Dim addr = 0
            For addr = 0 To 5
                ResultString += ContentName(addr) & " = " & ResultData(addr).ToString("x2") & vbCrLf
            Next
            ResultString += "Payload = "
            For addr = 6 To ResultData.Count - 3
                ResultString += ResultData(addr).ToString("x2") & " "
            Next
            ResultString += vbCrLf & "CheckSum first = " & ResultData(ResultData.Count - 2).ToString("x2") & " CheckSum last = " & ResultData(ResultData.Count - 1).ToString("x2")
            Return "[메세지 정보]" & vbCrLf & ResultString & vbCrLf & "[메세지 끝]"
        End Function
    End Class

    Public Class MavlinkConst
        Public Class Protocol
            'MAVLINK 표준 상수
            Public Const MAVLINK_STX As Byte = 253
            Public Const MAVLINK_STX_MAVLINK1 As Byte = &HFE
            Public Const MAVLINK_CORE_HEADER_LEN As Byte = 9 'Message ID 까지 길이
            Public Const MAVLINK_CORE_HEADER_MAVLINK1_LEN As Byte = 5

            '커스텀 상수
            Public Const NecessaryDataLength As Integer = 14 '길이는 byte 단위 (페이로드, 시그니처 미포함 값)
            Public Const v2MaximumPayloadSize As Integer = 253
            Public Const v1MaximumPayloadSize As Integer = 253
        End Class
    End Class

    Private Class CRCX25
        Private Const X25_INIT_CRC As UInt16 = &HFFFF
        Private Const X25_VALIDATE_CRC As UInt16 = &HF0B8

        ''' <summary>
        ''' 16비트 X.25 CRC 변수에 문자 하나를 누적합니다
        ''' mavlink/c_library_v2/blob/master/checksum.h의 crc_accumulate
        ''' </summary>
        ''' <param name="data">누적할 문자</param>
        ''' <param name="accumcrc">축적할 CRC</param>
        Public Shared Sub Accumulate(data As Byte, ByRef accumcrc As UInt16)
            Dim temp As Byte
            temp = data Xor (accumcrc And &HFF)
            temp = temp Xor (temp << 4)
            accumcrc = (accumcrc >> 8) Xor (temp << 8) Xor (temp << 3) Xor (temp >> 4)
        End Sub

        ''' <summary>
        ''' 16비트 X.25 CRC 변수에 바이트 배열을 누적합니다
        ''' mavlink/c_library_v2/blob/master/checksum.h의 crc_accumulate_buffer
        ''' </summary>
        ''' <param name="data">누적할 바이트 배열</param>
        ''' <param name="accumcrc">축적할 CRC</param>
        Public Shared Sub AccumulateBuffer(ByVal data As Byte(), ByRef accumcrc As UInt16)
            For Each nowbyte In data
                Accumulate(nowbyte, accumcrc)
            Next
        End Sub

        ''' <summary>
        ''' 바이트 배열에 대해 X.25 CRC를 계산합니다
        ''' mavlink/c_library_v2/blob/master/checksum.h의 crc_calculate
        ''' </summary>
        ''' <param name="data">계산할 바이트 배열</param>
        ''' <returns>계산된 16비트 X.25 CRC</returns>
        Public Shared Function Calculate(ByVal data As Byte()) As UInt16
            Dim temp As UInt16
            temp = X25_INIT_CRC
            For Each nowbyte In data
                Accumulate(nowbyte, temp)
            Next
            Return temp
        End Function
    End Class

    Public Class MavlinkException
        Public Class MavlinkPayloadTooLargeException
            Inherits Exception
            Public Sub New()
                MyBase.New("페이로드 데이터가 너무 큽니다. 페이로드 데이터는 255bytes(v1)또는 253bytes(v2)를 초과할 수 없습니다")
            End Sub

            Public Sub New(PayloadSize As Integer)
                MyBase.New("페이로드 데이터가 " & PayloadSize.ToString & "bytes로 너무 큽니다. 페이로드 데이터는 255bytes(v1) 또는 253bytes(v2)를 초과할 수 없습니다")
            End Sub

            Public Sub New(PayloadSize As Integer, PayloadMaxSize As Integer)
                MyBase.New("페이로드 데이터가 " & PayloadSize.ToString & "bytes로 너무 큽니다. 페이로드 데이터는 " & PayloadMaxSize.ToString & "bytes 를 초과할 수 없습니다")
            End Sub
        End Class

        Public Class MavlinkMessageNotInitialized
            Inherits Exception
            Public Sub New()
                MyBase.New("메세지가 초기화되지 않았습니다")
            End Sub
        End Class
    End Class
End Class
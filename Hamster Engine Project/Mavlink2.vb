'MAVLINK 버전 2의 구현입니다
Public Class Mavlink2
    Public Class MavlinkMessage
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
        ''' 새로운 Mavlink Message를 초기화합니다, 이 메서드는 메세지의 Length와 Checksum을 자동으로 계산하므로 안전합니다.
        ''' 메세지 직렬화 구현은 https://github.com/mavlink/c_library_v2/blob/master/mavlink_helpers.h 을 참고하세요.
        ''' </summary>
        Public Sub New(marker As Byte, seq As Byte, sysid As Byte, compid As Byte, msgid As UInt32, tsysid As Byte, tcompid As Byte, payloaddata As Byte(), crcextra As Byte)
            MagicMarker = marker
            Sequence = seq
            SystemID = sysid
            ComponentID = compid
            MessageID = msgid
            TargetSystemID = tsysid
            TargetComponentID = tsysid

            If Not Payload.Count > MavlinkConst.Protocol.MaximumPayloadSize Then
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
            data.AddRange({Checksum & &HFF, Checksum >> 8})
            Return data.ToArray
        End Function

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
                ResultString += ResultData(addr).ToString("x2")
            Next
            ResultString += "CheckSum first = " & ResultData(ResultData.Count - 2).ToString("x2") & " CheckSum last = " & ResultData(ResultData.Count - 1).ToString("x2")
            Return "[메세지 정보]" & vbCrLf & ResultString & vbCrLf & "[메세지 끝]"
        End Function

        Public Class MavlinkPayloadTooLargeException
            Inherits Exception
            Public Sub New()
                MyBase.New("페이로드 데이터가 너무 큽니다. 페이로드 데이터를 255bytes는 초과할 수 없습니다")
            End Sub

            Public Sub New(PayloadSize As Integer)
                MyBase.New("페이로드 데이터가 " & PayloadSize.ToString & "bytes로 너무 큽니다. 페이로드 데이터는 255bytes를 초과할 수 없습니다")
            End Sub
        End Class

        Public Class MavlinkMessageNotInitialized
            Inherits Exception
            Public Sub New()
                MyBase.New("메세지가 초기화되지 않았습니다")
            End Sub
        End Class
    End Class

    Public Class MavlinkType

    End Class

    Public Class MavlinkConst
        Public Class Protocol
            Public Const MAVLINK_STX As Byte = 253
            Public Const MAVLINK_CORE_HEADER_LEN As Byte = 9 'Message ID 까지 길이
            Public Const NecessaryDataLength As Integer = 14 '길이는 byte 단위 (페이로드, 시그니처 미포함 값)
            Public Const MaximumPayloadSize As Integer = 253
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
            temp = data ^ CByte(accumcrc & &HFF)
            temp ^= (temp << 4)
            accumcrc = (accumcrc >> 8) ^ (temp << 8) ^ (temp << 3) ^ (temp >> 4)
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
End Class
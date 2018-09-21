Imports System.Runtime.InteropServices
Imports Microsoft.Win32.SafeHandles

Public Class Win32SerialPort
    <System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError:=True, CharSet:=System.Runtime.InteropServices.CharSet.Auto)>
    Friend Shared Function CreateFile(ByVal lpFileName As String,
        ByVal dwDesiredAccess As EFileAccess,
        ByVal dwShareMode As EFileShare,
        ByVal lpSecurityAttributes As IntPtr,
        ByVal dwCreationDisposition As ECreationDisposition,
        ByVal dwFlagsAndAttributes As EFileAttributes,
        ByVal hTemplateFile As IntPtr) As Microsoft.Win32.SafeHandles.SafeFileHandle
    End Function

    <DllImport("kernel32.dll")>
    Private Shared Function GetCommState(
     ByVal hFile As IntPtr,
     ByRef lpDCB As DCB) As Boolean
    End Function

    <DllImport("kernel32.dll", SetLastError:=True)>
    Private Shared Function SetCommState(
     ByVal hFile As IntPtr,
     ByRef lpDCB As DCB) As Boolean
    End Function

    <DllImport("kernel32.dll", SetLastError:=True)> Private Shared Function GetCommTimeouts(ByVal hFile As IntPtr, ByRef lpCommTimeouts As COMMTIMEOUTS) As Boolean
    End Function

    <DllImport("kernel32.dll", SetLastError:=True)> Private Shared Function SetCommTimeouts(ByVal hFile As IntPtr, ByRef lpCommTimeouts As COMMTIMEOUTS) As Boolean
    End Function

    Public Sub Open(PortName As String, Baudrate As Integer, Parity As Byte, Stopbit As Byte, ReadTimeOutSec As Double, WriteTimeOutSec As Double)
        Dim handle As SafeFileHandle = CreateFile(PortName, EFileAccess.GENERIC_WRITE Or EFileAccess.GENERIC_READ, 0, IntPtr.Zero, ECreationDisposition.OPEN_EXISTING, 0, IntPtr.Zero)
        If handle.IsInvalid Then
            Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error())
        End If

        Dim DcbConfig As DCB
        If GetCommState(handle.DangerousGetHandle, DcbConfig) Then
            DcbConfig.BaudRate = Baudrate
            DcbConfig.ByteSize = 8
            DcbConfig.Parity = Parity
            DcbConfig.StopBits = Stopbit
        Else
            Throw New Exception("시리얼 포트의 핸들이 올바르지 않습니다!")
        End If

        If Not SetCommState(handle.DangerousGetHandle, DcbConfig) Then
            Throw New Exception("시리얼 포트를 설정하는 중 오류가 발생했습니다!")
        End If

        Dim timeoutconfig As COMMTIMEOUTS
        If GetCommTimeouts(handle.DangerousGetHandle, timeoutconfig) Then
            timeoutconfig.ReadIntervalTimeout = 1000 * ReadTimeOutSec
            timeoutconfig.ReadTotalTimeoutConstant = 1000 * ReadTimeOutSec
            timeoutconfig.ReadTotalTimeoutMultiplier = 1000 * ReadTimeOutSec
            timeoutconfig.WriteTotalTimeoutConstant = 1000 * WriteTimeOutSec
            timeoutconfig.WriteTotalTimeoutMultiplier = 1000 * WriteTimeOutSec
        End If

        If Not SetCommTimeouts(handle.DangerousGetHandle, timeoutconfig) Then
            Throw New Exception("시리얼 포트의 타임아웃을 설정하는 중 오류가 발생했습니다!")
        End If
    End Sub

    Public Structure COMMTIMEOUTS
        Public ReadIntervalTimeout As Int32
        Public ReadTotalTimeoutMultiplier As Int32
        Public ReadTotalTimeoutConstant As Int32
        Public WriteTotalTimeoutMultiplier As Int32
        Public WriteTotalTimeoutConstant As Int32
    End Structure

    Public Structure DCB
        Public DCBlength As Int32
        Public BaudRate As Int32
        Public fBitFields As Int32
        Public wReserved As Int16
        Public XonLim As Int16
        Public XoffLim As Int16
        Public ByteSize As Byte
        Public Parity As Byte
        Public StopBits As Byte
        Public XonChar As Byte
        Public XoffChar As Byte
        Public ErrorChar As Byte
        Public EofChar As Byte
        Public EvtChar As Byte
        Public wReserved1 As Int16 'Reserved; Do Not Use
    End Structure

    Friend Structure STORAGE_DEVICE_NUMBER
        Friend DeviceType As Integer
        Friend DeviceNumber As Integer
        Friend PartitionNumber As Integer
    End Structure

    Friend Enum EFileAccess As System.Int32
        DELETE = &H10000
        READ_CONTROL = &H20000
        WRITE_DAC = &H40000
        WRITE_OWNER = &H80000
        SYNCHRONIZE = &H100000

        STANDARD_RIGHTS_REQUIRED = &HF0000
        STANDARD_RIGHTS_READ = READ_CONTROL
        STANDARD_RIGHTS_WRITE = READ_CONTROL
        STANDARD_RIGHTS_EXECUTE = READ_CONTROL
        STANDARD_RIGHTS_ALL = &H1F0000
        SPECIFIC_RIGHTS_ALL = &HFFFF

        ACCESS_SYSTEM_SECURITY = &H1000000

        MAXIMUM_ALLOWED = &H2000000

        GENERIC_READ = &H80000000
        GENERIC_WRITE = &H40000000
        GENERIC_EXECUTE = &H20000000
        GENERIC_ALL = &H10000000
    End Enum

    Friend Enum EFileShare
        FILE_SHARE_NONE = &H0
        FILE_SHARE_READ = &H1
        FILE_SHARE_WRITE = &H2
        FILE_SHARE_DELETE = &H4
    End Enum

    Friend Enum ECreationDisposition
        ''' <summary>
        ''' Creates a new file, only if it does not already exist.
        ''' If the specified file exists, the function fails and the last-error code is set to ERROR_FILE_EXISTS (80).
        ''' If the specified file does not exist and is a valid path to a writable location, a new file is created.
        ''' </summary>
        CREATE_NEW = 1

        ''' <summary>
        ''' Creates a new file, always.
        ''' If the specified file exists and is writable, the function overwrites the file, the function succeeds, and last-error code is set to ERROR_ALREADY_EXISTS (183).
        ''' If the specified file does not exist and is a valid path, a new file is created, the function succeeds, and the last-error code is set to zero.
        ''' For more information, see the Remarks section of this topic.
        ''' </summary>
        CREATE_ALWAYS = 2

        ''' <summary>
        ''' Opens a file or device, only if it exists.
        ''' If the specified file or device does not exist, the function fails and the last-error code is set to ERROR_FILE_NOT_FOUND (2).
        ''' For more information about devices, see the Remarks section.
        ''' </summary>
        OPEN_EXISTING = 3

        ''' <summary>
        ''' Opens a file, always.
        ''' If the specified file exists, the function succeeds and the last-error code is set to ERROR_ALREADY_EXISTS (183).
        ''' If the specified file does not exist and is a valid path to a writable location, the function creates a file and the last-error code is set to zero.
        ''' </summary>
        OPEN_ALWAYS = 4

        ''' <summary>
        ''' Opens a file and truncates it so that its size is zero bytes, only if it exists.
        ''' If the specified file does not exist, the function fails and the last-error code is set to ERROR_FILE_NOT_FOUND (2).
        ''' The calling process must open the file with the GENERIC_WRITE bit set as part of the dwDesiredAccess parameter.
        ''' </summary>
        TRUNCATE_EXISTING = 5
    End Enum

    Friend Enum EFileAttributes
        FILE_ATTRIBUTE_READONLY = &H1
        FILE_ATTRIBUTE_HIDDEN = &H2
        FILE_ATTRIBUTE_SYSTEM = &H4
        FILE_ATTRIBUTE_DIRECTORY = &H10
        FILE_ATTRIBUTE_ARCHIVE = &H20
        FILE_ATTRIBUTE_DEVICE = &H40
        FILE_ATTRIBUTE_NORMAL = &H80
        FILE_ATTRIBUTE_TEMPORARY = &H100
        FILE_ATTRIBUTE_SPARSE_FILE = &H200
        FILE_ATTRIBUTE_REPARSE_POINT = &H400
        FILE_ATTRIBUTE_COMPRESSED = &H800
        FILE_ATTRIBUTE_OFFLINE = &H1000
        FILE_ATTRIBUTE_NOT_CONTENT_INDEXED = &H2000
        FILE_ATTRIBUTE_ENCRYPTED = &H4000
        FILE_ATTRIBUTE_VIRTUAL = &H10000

        'This parameter can also contain combinations of flags (FILE_FLAG_*) 
        FILE_FLAG_BACKUP_SEMANTICS = &H2000000
        FILE_FLAG_DELETE_ON_CLOSE = &H4000000
        FILE_FLAG_NO_BUFFERING = &H20000000
        FILE_FLAG_OPEN_NO_RECALL = &H100000
        FILE_FLAG_OPEN_REPARSE_POINT = &H200000
        FILE_FLAG_OVERLAPPED = &H40000000
        FILE_FLAG_POSIX_SEMANTICS = &H1000000
        FILE_FLAG_RANDOM_ACCESS = &H10000000
        FILE_FLAG_SEQUENTIAL_SCAN = &H8000000
        FILE_FLAG_WRITE_THROUGH = &H80000000
    End Enum
End Class

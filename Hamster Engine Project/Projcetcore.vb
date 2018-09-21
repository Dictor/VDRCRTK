Imports System.Windows.Forms
Imports Hamster_Engine_Project.HE_Common_Component

Public Class Project
    Public Shared Version As New HamsterVersion("VDRC RTK Process Program", 0, 0, 180922, 14)

    '("개체 식별자 문자열", {"어셈블리 경로"})
    Private Shared LoadObject_PROJ_Info As New Dictionary(Of String, Object())
    Private Shared ApplicationStartupPath As String
    Private Shared MainForm As Form = New frmMain

    Public Shared EngineShowErr As [Delegate]
    Public Shared EngineShowWarn As [Delegate]

    Public Function initialization(EngineAsm As Dictionary(Of String, Object), enginefunc As [Delegate]()(), args As Object()) As Dictionary(Of String, Object())
        ApplicationStartupPath = args(0)
        Dim setMainFrm As [Delegate]
        setMainFrm = enginefunc(2)(0)
        EngineShowErr = enginefunc(3)(0)
        EngineShowErr = enginefunc(4)(0)
        setMainFrm.DynamicInvoke(MainForm)
        Return LoadObject_PROJ_Info
    End Function

    Public Sub main(ProjsideAsm As Dictionary(Of String, Object))
    End Sub
End Class

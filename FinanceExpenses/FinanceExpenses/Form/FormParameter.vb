Imports System.Threading
Public Class FormParameter
    Dim ProgressReportCallback1 As ProgressReportCallback = AddressOf myCallBack  'DoBackGround Progress Report ProgressReportCallback.Invoke
    Dim DoBackground1 As DoBackground = New DoBackground(Me, ProgressReportCallback1)

    Private Shared myform As FormParameter
    Dim myController As ParamAdapter

    Public Shared Function getInstance()
        If myform Is Nothing Then
            myform = New FormParameter
        ElseIf myform.IsDisposed Then
            myform = New FormParameter
        End If
        Return myform
    End Function

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        AddHandler DoBackground.CallBack, AddressOf EventCallback 'DoBackGround Progress Report RaiseEvent CallBack

    End Sub


    Private Sub RefreshToolStripButton_Click(sender As Object, e As EventArgs) Handles Me.Load
        LoadData()
    End Sub

    Private Sub LoadData()
        DoBackground1.doThread(AddressOf DoWork)
    End Sub

    Sub DoWork()
        myController = New ParamAdapter
        Try
            'DoBackground1.ProgressReport(1, "Loading...Please wait.")
            If myController.loaddata() Then
                DoBackground1.ProgressReport(4, "CallBack")
            End If
            'DoBackground1.ProgressReport(1, String.Format("Loading...Done. Records {0}", myController.BS.Count))
        Catch ex As Exception
            DoBackground1.ProgressReport(1, ex.Message)
        End Try
    End Sub

    Sub myCallBack()
        UcParams1.BindingControl(myController)
    End Sub

    Private Sub EventCallback(sender As Object, e As EventArgs)
        'Debug.Print(sender)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        myController.save()
        Me.Close()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Close()
    End Sub
End Class
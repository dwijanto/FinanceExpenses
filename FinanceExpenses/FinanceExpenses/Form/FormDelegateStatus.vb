Public Class FormDelegateStatus
    Dim ProgressReportCallback1 As ProgressReportCallback = AddressOf myCallBack  'DoBackGround Progress Report ProgressReportCallback.Invoke
    Dim DoBackground1 As DoBackground = New DoBackground(Me, ProgressReportCallback1)

    Private Shared myform As FormDelegateStatus
    Dim myController As DelegateController
    Private DRV As DataRowView


    Public Shared Function getInstance()
        If myform Is Nothing Then
            myform = New FormDelegateStatus
        ElseIf myform.IsDisposed Then
            myform = New FormDelegateStatus
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
        myController = New DelegateController
        Try
            Dim criteria = String.Format(" where u.isactive")
            DoBackground1.ProgressReport(1, "Loading...Please wait.")
            If myController.loaddata(criteria) Then
                DoBackground1.ProgressReport(4, "CallBack")
            End If
            DoBackground1.ProgressReport(1, String.Format("Loading...Done. Records {0}", myController.BS.Count))
        Catch ex As Exception
            DoBackground1.ProgressReport(1, ex.Message)
        End Try
    End Sub

    Sub myCallBack()
        DataGridView1.AutoGenerateColumns = False
        DataGridView1.DataSource = myController.BS
    End Sub

    Private Sub EventCallback(sender As Object, e As EventArgs)
        Debug.Print(sender)
    End Sub


    Public Overloads Function validate() As Boolean
        Dim myret As Boolean = True
        Return myret
    End Function


    Private Sub RefreshToolStripButton_Click_1(sender As Object, e As EventArgs) Handles RefreshToolStripButton.Click
        LoadData()
    End Sub

    Private Sub ToolStripTextBox1_Click(sender As Object, e As EventArgs) Handles ToolStripTextBox1.Click

    End Sub

    Private Sub ToolStripTextBox1_TextChanged(sender As Object, e As EventArgs) Handles ToolStripTextBox1.TextChanged
        myController.ApplyFilter = ToolStripTextBox1.Text
    End Sub
End Class
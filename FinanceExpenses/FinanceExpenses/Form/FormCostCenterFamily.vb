Public Class FormCostCenterFamily
    Dim ProgressReportCallback1 As ProgressReportCallback = AddressOf myCallBack  'DoBackGround Progress Report ProgressReportCallback.Invoke
    Dim DoBackground1 As DoBackground = New DoBackground(Me, ProgressReportCallback1)

    Private Shared myform As FormCostCenterFamily
    Dim myController As CostCenterFamilyController

    Public Shared Function getInstance()
        If myform Is Nothing Then
            myform = New FormCostCenterFamily
        ElseIf myform.IsDisposed Then
            myform = New FormCostCenterFamily
        End If
        Return myform
    End Function

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        AddHandler DoBackground.CallBack, AddressOf EventCallback 'DoBackGround Progress Report RaiseEvent CallBack
        AddHandler UCUser.RefreshDGV, AddressOf RefreshDGVCallback
    End Sub



    Private Sub RefreshToolStripButton_Click(sender As Object, e As EventArgs) Handles RefreshToolStripButton.Click, Me.Load
        LoadData()
    End Sub

    Private Sub LoadData()
        DoBackground1.doThread(AddressOf DoWork)
    End Sub

    Sub DoWork()
        myController = New CostCenterFamilyController
        Try
            DoBackground1.ProgressReport(1, "Loading...Please wait.")
            If myController.loaddata() Then
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

    Private Sub RefreshDGVCallback()
        DataGridView1.Invalidate()
    End Sub

    Private Sub AddToolStripButton_Click(sender As Object, e As EventArgs) Handles AddToolStripButton.Click
        'showTX(TxEnum.NewRecord)
        Dim drv As DataRowView = myController.GetNewRecord()
        drv.BeginEdit()
        drv.Row.Item("isactive") = True

    End Sub

    Private Sub CommitToolStripButton_Click(sender As Object, e As EventArgs) Handles CommitToolStripButton.Click
        Me.Validate()
        myController.save()
    End Sub

    Private Sub ToolStripTextBox1_TextChanged(sender As Object, e As EventArgs) Handles ToolStripTextBox1.TextChanged
        myController.ApplyFilter = ToolStripTextBox1.Text
    End Sub

    Private Sub DeleteToolStripButton_Click(sender As Object, e As EventArgs) Handles DeleteToolStripButton.Click
        myController.delete(DataGridView1)
    End Sub
End Class
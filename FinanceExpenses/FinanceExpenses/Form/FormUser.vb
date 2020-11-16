Imports System.Threading

Public Class FormUser
    Dim ProgressReportCallback1 As ProgressReportCallback = AddressOf myCallBack  'DoBackGround Progress Report ProgressReportCallback.Invoke
    Dim DoBackground1 As DoBackground = New DoBackground(Me, ProgressReportCallback1)

    Private Shared myform As FormUser
    Dim myController As UserController



    Public Shared Function getInstance()
        If myform Is Nothing Then
            myform = New FormUser
        ElseIf myform.IsDisposed Then
            myform = New FormUser
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

    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click
        Dim myform As New FormCostCenterAssignment
        myform.Show()
    End Sub

    Private Sub RefreshToolStripButton_Click(sender As Object, e As EventArgs) Handles RefreshToolStripButton.Click, Me.Load
        LoadData()
    End Sub

    Private Sub LoadData()
        DoBackground1.doThread(AddressOf DoWork)
    End Sub

    Sub DoWork()
        myController = New UserController
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

    Private Sub UpdateToolStripButton_Click(sender As Object, e As EventArgs) Handles UpdateToolStripButton.Click, DataGridView1.CellDoubleClick
        showTX(TxEnum.UpdateRecord)
    End Sub

    Private Sub showTX(txEnum As TxEnum)
        Dim drv As DataRowView = Nothing
        Dim helperbs As New BindingSource
        helperbs = myController.Model.GetUserBS
        Select Case txEnum
            Case FinanceExpenses.TxEnum.NewRecord
                drv = myController.GetNewRecord
                drv.Row.Item("isactive") = True
            Case FinanceExpenses.TxEnum.UpdateRecord
                drv = myController.GetCurrentRecord

        End Select
        drv.BeginEdit()

        Dim myform = New DialogAddUpdUser(drv, helperbs)
        myform.ShowDialog()

    End Sub

    Private Sub RefreshDGVCallback()
        DataGridView1.Invalidate()
    End Sub

    Private Sub AddToolStripButton_Click(sender As Object, e As EventArgs) Handles AddToolStripButton.Click
        showTX(TxEnum.NewRecord)
    End Sub

    Private Sub CommitToolStripButton_Click(sender As Object, e As EventArgs) Handles CommitToolStripButton.Click
        myController.save()
    End Sub

    Private Sub ToolStripTextBox1_TextChanged(sender As Object, e As EventArgs) Handles ToolStripTextBox1.TextChanged
        myController.ApplyFilter = ToolStripTextBox1.Text
    End Sub

    Private Sub DeleteToolStripButton_Click(sender As Object, e As EventArgs) Handles DeleteToolStripButton.Click
        myController.delete(DataGridView1)       
    End Sub

    Private Sub ToolStripTextBox1_Click(sender As Object, e As EventArgs) Handles ToolStripTextBox1.Click

    End Sub
End Class
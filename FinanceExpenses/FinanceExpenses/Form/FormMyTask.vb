Imports System.Threading
Public Class FormMyTask
    Private myController As New TaskController
    Delegate Sub ProgressReportDelegate(ByVal id As Integer, ByVal message As String)
    Dim myThread As New System.Threading.Thread(AddressOf DoWork)
    Dim MyTasksCriteria As String
    Dim HistoryCriteria As String
    Dim DS As New DataSet
    Dim MyTasksBS As BindingSource
    Dim HistoryBS As BindingSource


    Private Sub FormFindProductRequest_Load(sender As Object, e As EventArgs) Handles Me.Load
        loaddata()
    End Sub

    Private Sub loaddata()
        If Not myThread.IsAlive Then
            myThread = New Thread(AddressOf DoWork)
            myThread.Start()
        Else
            MessageBox.Show("Please wait until the current process is finished.")
        End If
    End Sub

    Sub DoWork()
        ProgressReport(6, "Marquee")
        ProgressReport(1, "Loading Data.")
        Dim FinanceCriteria As String = String.Empty
        If User.can("Validate For Finance") Then
            FinanceCriteria = String.Format("or (status = {0} and ndapprover isnull) or (status = {1})",
                                            Int(TaskStatusEnum.STATUS_VALIDATEDBYM1),
                                            Int(TaskStatusEnum.STATUS_VALIDATEDBYM2))
        End If
        If User.can("ViewAllTx") Then
            MyTasksCriteria = " where status > 0 and status < 8"
            HistoryCriteria = " where status >= 9"
            'ElseIf User.can("Validate For Finance") Then
            '    MyTasksCriteria = String.Format("where (status in({3},{4},{5},{6}) and attn = '{0}') or (status = {1} and ndapprover isnull) or (status = {2}) ",
            '                                    DirectCast(User.identity, UserController).email,
            '                                    Int(TaskStatusEnum.STATUS_VALIDATEDBYM1),
            '                                    Int(TaskStatusEnum.STATUS_VALIDATEDBYM2),
            '                                    Int(TaskStatusEnum.STATUS_NEW),
            '                                    Int(TaskStatusEnum.STATUS_REJECTEDBYM1),
            '                                    Int(TaskStatusEnum.STATUS_REJECTEDBYM2),
            '                                    Int(TaskStatusEnum.STATUS_REJECTEDBYFINANCE))
            '    HistoryCriteria = String.Format("where (status > 90) ")
        Else
            'Delegate
            'New                    Requester
            'ReSubmit               Requester
            'RejectedM1             Requester
            'RejectedM2             Requester
            'ValidatedbyRequester   M1
            'ValidatedbyM2          M2
            MyTasksCriteria = String.Format("where (status in({1},{2},{3},{6}) and attn = '{0}') or (status = {7} and forwardto = '{0}') or (status = {4} and u1.email = '{0}') or (status = {5} and u2.email = '{0}') or (status = {4} and ssc.getdelegator('{0}') ~u1.email) or (status = {5} and ssc.getdelegator('{0}') ~u2.email) {8}",
                                            DirectCast(User.identity, UserController).email,
                                            Int(TaskStatusEnum.STATUS_NEW),
                                            Int(TaskStatusEnum.STATUS_REJECTEDBYM1),
                                            Int(TaskStatusEnum.STATUS_REJECTEDBYM2),
                                            Int(TaskStatusEnum.STATUS_VALIDATEDBYREQUESTER),
                                            Int(TaskStatusEnum.STATUS_VALIDATEDBYM1),
                                            Int(TaskStatusEnum.STATUS_REJECTEDBYFINANCE),
                                            Int(TaskStatusEnum.STATUS_FORWARD),
                                            FinanceCriteria)
            HistoryCriteria = String.Format("where (status >= {2} and ssc.getdelegator('{0}') ~u1.email) or " &
                                            " (status >= {3} and ssc.getdelegator('{0}') ~u2.email) or ((attn = '{0}' or forwardto = '{0}') and status >= {1}) or (u1.email = '{0}' and status >= {2}) or (u2.email ='{0}' and status >= {3}) ",
                                            DirectCast(User.identity, UserController).email,
                                            Int(TaskStatusEnum.STATUS_VALIDATEDBYREQUESTER),
                                            Int(TaskStatusEnum.STATUS_VALIDATEDBYM1),
                                            Int(TaskStatusEnum.STATUS_VALIDATEDBYM2))
        End If
        Try
            DS = New DataSet
            If myController.MyTasksloaddata(DS, MyTasksCriteria, HistoryCriteria) Then
                ProgressReport(4, "InitData")
                ProgressReport(1, "Loading Data.Done!")
                ProgressReport(5, "Continuous")
            End If
        Catch ex As Exception
            ProgressReport(1, "Loading Data. Error::" & ex.Message)
            ProgressReport(5, "Continuous")
        End Try
    End Sub

    Private Sub ProgressReport(ByVal id As Integer, ByVal message As String)
        If Me.InvokeRequired Then
            Dim d As New ProgressReportDelegate(AddressOf ProgressReport)
            Me.Invoke(d, New Object() {id, message})
        Else
            Try
                Select Case id
                    Case 1
                        ToolStripStatusLabel1.Text = message
                    Case 2
                        ToolStripStatusLabel1.Text = message

                    Case 4
                        MyTasksBS = New BindingSource
                        MyTasksBS.DataSource = DS.Tables(0)
                        HistoryBS = New BindingSource
                        HistoryBS.DataSource = DS.Tables(1)
                        DataGridView1.AutoGenerateColumns = False
                        DataGridView1.DataSource = MyTasksBS
                        DataGridView2.AutoGenerateColumns = False
                        DataGridView2.DataSource = HistoryBS
                    Case 5
                        ToolStripProgressBar1.Style = ProgressBarStyle.Continuous
                    Case 6
                        ToolStripProgressBar1.Style = ProgressBarStyle.Marquee
                End Select
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try
        End If

    End Sub

    Private Sub RefreshToolStripButton_Click(sender As Object, e As EventArgs) Handles RefreshToolStripButton.Click
        loaddata()
    End Sub




    'Private Sub DataGridView1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellDoubleClick
    '    Dim drv = MyTasksBS.Current
    '    Dim myform = New FormProductRequest(drv.row.item("id"), TxEnum.ValidateRecord)
    '    myform.ShowDialog()
    '    If myform.IsModified Then
    '        loaddata()
    '    End If
    'End Sub
    'Private Sub DataGridView2_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView2.CellDoubleClick
    '    Dim drv = HistoryBS.Current
    '    Dim myform = New FormProductRequest(drv.row.item("id"), TxEnum.HistoryRecord)
    '    myform.ShowDialog()
    '    If myform.IsModified Then
    '        loaddata()
    '    End If
    'End Sub
    'Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    'End Sub

    'Private Sub DataGridView1_CellContentDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentDoubleClick
    '    Dim myform As New FormExpenses
    '    myform.Show()
    'End Sub

    Private Sub DataGridView1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellDoubleClick
        Dim drv = MyTasksBS.Current
        'Dim myform As New FormExpenses(drv.row.item("id"), TxEnum.UpdateRecord)
        Dim myForm = New FormExpenses(drv.row.item("id"), TxEnum.UpdateRecord)
        myform.Show()
        
    End Sub


    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.        
        AddHandler FormExpenses.myFormClosed, AddressOf Checkloaddata
    End Sub

    Private Sub Checkloaddata(ByVal isModified As Boolean)
        If isModified Then
            loaddata()
        End If
    End Sub

    Private Sub DataGridView2_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView2.CellDoubleClick
        Dim drv = HistoryBS.Current
        'Dim myform As New FormExpenses(drv.row.item("id"), TxEnum.UpdateRecord)
        Dim myForm = New FormExpenses(drv.row.item("id"), TxEnum.HistoryRecord)
        myForm.Show()
    End Sub
End Class
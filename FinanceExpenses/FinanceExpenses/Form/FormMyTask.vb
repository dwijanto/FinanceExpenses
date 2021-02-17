Imports System.Threading
Public Class FormMyTask

    Dim ProgressReportCallback1 As ProgressReportCallback = AddressOf myCallBack
    Dim DoBackground1 As DoBackground = New DoBackground(Me, ProgressReportCallback1)

    Private myController As New TaskController
    Delegate Sub ProgressReportDelegate(ByVal id As Integer, ByVal message As String)
    Dim myThread As New System.Threading.Thread(AddressOf DoWork)
    Dim MyTasksCriteria As String
    Dim HistoryCriteria As String
    Dim DS As New DataSet
    Dim MyTasksBS As BindingSource
    Dim HistoryBS As BindingSource
    Dim Startdate As Date = CDate(String.Format("{0}-{1}-{2}", Year(Today.Date) - 1, Month(Today.Date), Today.Date.Day))
    Dim Enddate As DateTime


    Private Sub FormFindProductRequest_Load(sender As Object, e As EventArgs) Handles Me.Load
        loaddata()
    End Sub

    'Private Sub loaddata1()
    '    If Not myThread.IsAlive Then
    '        myThread = New Thread(AddressOf DoWork)
    '        myThread.Start()
    '    Else
    '        MessageBox.Show("Please wait until the current process is finished.")
    '    End If
    'End Sub

    Private Sub loaddata()
        DoBackground1.doThread(AddressOf DoWork)
    End Sub

    Sub DoWork()
        DoBackground1.ProgressReport(5, "Marquee")
        DoBackground1.ProgressReport(1, "Loading Data.")
        Dim FinanceCriteria As String = String.Empty
        Dim FinanceCriteriaHistory As String = String.Empty
        Dim DateCriteria As String = String.Format(" and receiveddate >= '{0:yyyy-MM-dd}' and receiveddate <= '{1:yyyy-MM-dd}'", Startdate, Enddate.AddDays(1))

        If User.can("Validate For Finance") Then
            FinanceCriteria = String.Format("or (status = {0} and ndapprover isnull) or (status = {1})",
                                            Int(TaskStatusEnum.STATUS_VALIDATEDBYM1),
                                            Int(TaskStatusEnum.STATUS_VALIDATEDBYM2))
            FinanceCriteriaHistory = String.Format("or (status = {0})",
                                            Int(TaskStatusEnum.STATUS_COMPLETED))
        End If
        If User.can("ViewAllTx") Then
            MyTasksCriteria = " where status > 0 and status <= 10"
            HistoryCriteria = " where status > 10"
        Else
            MyTasksCriteria = String.Format("where (status in({1},{2},{3},{6}) and attn = '{0}' and forwardto isnull) or (status in ({7},{2},{3},{6}) and forwardto = '{0}') or (status in({4},{9}) and u1.email = '{0}') or (status = {5} and u2.email = '{0}') or (status in({4},{9}) and ssc.getdelegator('{0}') ~u1.email) or (status = {5} and ssc.getdelegator('{0}') ~u2.email) {8}",
                                            DirectCast(User.identity, UserController).email,
                                            Int(TaskStatusEnum.STATUS_NEW),
                                            Int(TaskStatusEnum.STATUS_REJECTEDBYM1),
                                            Int(TaskStatusEnum.STATUS_REJECTEDBYM2),
                                            Int(TaskStatusEnum.STATUS_VALIDATEDBYREQUESTER),
                                            Int(TaskStatusEnum.STATUS_VALIDATEDBYM1),
                                            Int(TaskStatusEnum.STATUS_REJECTEDBYFINANCE),
                                            Int(TaskStatusEnum.STATUS_FORWARD),
                                            FinanceCriteria,
                                            Int(TaskStatusEnum.STATUS_RE_SUBMIT))
            HistoryCriteria = String.Format("where ((status in ({4},{5},{6},{9}) and attn = '{0}' ) or (status >= {2} and ssc.getdelegator('{0}') ~u1.email) or " &
                                            " (status >= {3} and ssc.getdelegator('{0}') ~u2.email) or ((attn = '{0}' or forwardto = '{0}') and status >= {1}) or (u1.email = '{0}' and status >= {2}) or (u2.email ='{0}' and status >= {3}) {7}) {8} ",
                                            DirectCast(User.identity, UserController).email,
                                            Int(TaskStatusEnum.STATUS_VALIDATEDBYREQUESTER),
                                            Int(TaskStatusEnum.STATUS_VALIDATEDBYM1),
                                            Int(TaskStatusEnum.STATUS_VALIDATEDBYM2),
                                            Int(TaskStatusEnum.STATUS_FORWARD),
                                            Int(TaskStatusEnum.STATUS_COMPLETED),
                                            Int(TaskStatusEnum.STATUS_CANCELLED),
                                            FinanceCriteriaHistory,
                                            DateCriteria,
                                            Int(TaskStatusEnum.STATUS_RE_SUBMIT))
        End If
        Try
            DS = New DataSet
            If myController.MyTasksloaddata(DS, MyTasksCriteria, HistoryCriteria) Then
                DoBackground1.ProgressReport(4, "CallBack")
                DoBackground1.ProgressReport(1, "Loading Data.Done!")              
            End If
        Catch ex As Exception
            DoBackground1.ProgressReport(1, "Loading Data. Error::" & ex.Message)            
        Finally
            DoBackground1.ProgressReport(6, "Continuous")
        End Try
    End Sub


    Private Sub RefreshToolStripButton_Click(sender As Object, e As EventArgs) Handles RefreshToolStripButton.Click
        loaddata()
    End Sub

    Private Sub DataGridView1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellDoubleClick
        Dim drv = MyTasksBS.Current
        Dim myForm = New FormExpenses(drv.row.item("id"), TxEnum.UpdateRecord)
        If myForm.HasApprover Then
            myForm.Show()
        End If
    End Sub
    Private Sub DataGridView2_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView2.CellDoubleClick
        Dim drv = HistoryBS.Current
        Dim myForm = New FormExpenses(drv.row.item("id"), TxEnum.HistoryRecord)

        myForm.Show()


    End Sub

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.        
        AddHandler FormExpenses.myFormClosed, AddressOf Checkloaddata
        DateTimePicker1.Value = Startdate
        DateTimePicker2.Value = Enddate
    End Sub

    Private Sub Checkloaddata(ByVal isModified As Boolean)
        If isModified Then
            loaddata()
        End If
    End Sub

    Sub myCallBack()
        MyTasksBS = New BindingSource
        MyTasksBS.DataSource = DS.Tables(0)
        HistoryBS = New BindingSource
        HistoryBS.DataSource = DS.Tables(1)
        DataGridView1.AutoGenerateColumns = False
        DataGridView1.DataSource = MyTasksBS
        
        DataGridView2.AutoGenerateColumns = False
        DataGridView2.DataSource = HistoryBS
        If User.can("Validate For Finance") Then
            DataGridView1.Columns("financeremarksmytask").Visible = True
            DataGridView2.Columns("financeremarkshistory").Visible = True
            DataGridView2.ReadOnly = False
            ToolStripButton1.Visible = True
        End If
    End Sub

    Private Sub DateTimePicker1_ValueChanged(sender As Object, e As EventArgs) Handles DateTimePicker1.ValueChanged, DateTimePicker2.ValueChanged
        Startdate = DateTimePicker1.Value
        Enddate = DateTimePicker2.Value        
    End Sub

   
    Private Sub ToolStripTextBox1_TextChanged(sender As Object, e As EventArgs) Handles ToolStripTextBox1.TextChanged
        ApplyFilter()
    End Sub

    Private Sub ApplyFilter()

        Dim myfilter = String.Format("refnumber like '*{0}*' or statusname like '*{0}*' or emailsubject like '*{0}*' or sender like '*{0}*' or emailto like '*{0}*' or stapprovername like '*{0}*' or ndapprovername like '*{0}*' or financenumber like '*{0}*' or vendorcodetext like '*{0}*'  or vendorname like '*{0}*' ", ToolStripTextBox1.Text)
        MyTasksBS.Filter = myfilter
        HistoryBS.Filter = myfilter

    End Sub


    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click
        Me.Validate()
        MyTasksBS.EndEdit()
        HistoryBS.EndEdit()
        myController.SaveMyTask(DS)
    End Sub

    Private Sub ToolStripTextBox1_Click(sender As Object, e As EventArgs) Handles ToolStripTextBox1.Click

    End Sub
End Class
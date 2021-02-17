Imports System.Threading

Public Enum ApproverType
    RegularUser = 0
    FinalApprover = 1
End Enum
Public Class FormExpenses

    Dim ProgressReportCallback1 As ProgressReportCallback = AddressOf myCallBack
    Public DoBackground1 As DoBackground = New DoBackground(Me, ProgressReportCallback1)

    Dim ProgressReportCallback2 As ProgressReportCallback = AddressOf myCallBack
    Public DoBackground2 As DoBackground = New DoBackground(Me, ProgressReportCallback2)

    Dim FirstApproverUser As List(Of UserController)
    Dim SecondApproverUser As List(Of UserController)
    Dim _isModified As Boolean = False

    Private myController As New EmailController
    Private TxEnum As TxEnum
    Private Criteria As String

    Dim myParam = ParamAdapter.getInstance

    Dim Identity As UserController = User.getIdentity


    Private DRV As DataRowView      'HeaderRow
    Dim ApprovalDRV As DataRowView  'ApprovalRow


    Dim DTLBS As BindingSource
    Dim ActionBS As BindingSource
    Dim FinanceTxBS As BindingSource
    Dim ApprovalTXBS As BindingSource
    Dim VendorBS As BindingSource
    Dim COABS As BindingSource

    'Dim FirstApprover As String = String.Empty
    'Dim SecondApprover As String = String.Empty

    Dim FinanceEmailList As String
    Dim FinanceUserNameList As String

    'Dim FirstApproverEmail As String = String.Empty
    'Dim SecondApproverEmail As String = String.Empty
    Dim nLevelApproval As Long

    Public Shared Event myFormClosed(isModified As Boolean)
    Dim userid = DirectCast(User.identity, UserController).userid
    Dim _HasApprover As Boolean
    Dim myupload As UploadExpensesController


    Public Property HasApprover As Boolean
        Get
            Return _HasApprover
        End Get
        Set(value As Boolean)
            _HasApprover = value
        End Set
    End Property

    Public Sub myCallBack(ByRef sender As Object, ByRef e As EventArgs)

        DRV = myController.GetCurrentRecord
        DTLBS = myController.GetDTLBS
        ActionBS = myController.GetActionBS
        FinanceTxBS = myController.GetFinanceTxBS
        ApprovalTXBS = myController.GetApprovalTxBS
        VendorBS = myController.GetVendorBS
        COABS = myController.GetChartOfAccountBS
        UcFinanceExpenses1.BindingControl(DRV, DTLBS, FinanceTxBS, ApprovalTXBS, VendorBS, COABS, myController.DS, Me)
        DataGridView1.AutoGenerateColumns = False
        DataGridView1.DataSource = ActionBS

        ToolStripButtonCommit.Visible = False 'Commit
        ToolStripButtonSubmit.Visible = False 'submit
        ToolStripButtonReSubmit.Visible = False 'Re-Submit
        ToolStripButtonValidate.Visible = False 'Validate
        ToolStripButtonStsCancelled.Visible = False 'Cancel
        ToolStripButtonReject.Visible = False 'Reject
        ToolStripButtonComplete.Visible = False 'Complete
        ToolStripButtonForward.Visible = False 'Forward
        ToolStripButton1.Visible = False 'Download
        ToolStripButton2.Visible = False 'Upload
        Select Case TxEnum
            Case FinanceExpenses.TxEnum.UpdateRecord
                Select Case DRV.Row.Item("status")
                    Case TaskStatusEnum.STATUS_NEW
                        ToolStripButtonForward.Visible = True
                        ToolStripButtonValidate.Visible = True 'Validate
                        ToolStripButtonStsCancelled.Visible = True 'Cancel
                        ToolStripButton1.Visible = True
                        ToolStripButton2.Visible = True
                    Case TaskStatusEnum.STATUS_FORWARD
                        ToolStripButtonValidate.Visible = True 'Validate
                        ToolStripButtonStsCancelled.Visible = True 'Cancel
                        ToolStripButton1.Visible = True
                        ToolStripButton2.Visible = True
                    Case TaskStatusEnum.STATUS_REJECTEDBYM1, TaskStatusEnum.STATUS_REJECTEDBYM2, TaskStatusEnum.STATUS_REJECTEDBYFINANCE
                        ToolStripButtonValidate.Visible = True 'Validate
                        ToolStripButtonValidate.Text = "Re-Submit"
                        'DRV.Row.Item("status") = Int(TaskStatusEnum.STATUS_RE_SUBMIT)
                        ToolStripButtonStsCancelled.Visible = True 'Cancel
                    Case TaskStatusEnum.STATUS_VALIDATEDBYREQUESTER
                        ToolStripButtonValidate.Visible = True 'Validate
                        ToolStripButtonReject.Visible = True 'Reject
                    Case TaskStatusEnum.STATUS_VALIDATEDBYM1
                        Dim approvaldrv As DataRowView = myController.GetApprovalTxBS.Current
                        If Not IsDBNull(approvaldrv.Row.Item("ndapprover")) Then
                            ToolStripButtonValidate.Visible = True 'Validate
                            ToolStripButtonReject.Visible = True 'Reject
                        Else
                            ToolStripButtonComplete.Visible = True
                            ToolStripButtonReject.Visible = True 'Reject
                        End If
                    Case TaskStatusEnum.STATUS_VALIDATEDBYM2
                        ToolStripButtonComplete.Visible = True
                        ToolStripButtonReject.Visible = True 'Reject
                    Case TaskStatusEnum.STATUS_RE_SUBMIT
                        ToolStripButtonValidate.Visible = True 'Validate
                        ToolStripButtonReject.Visible = True 'Reject
                        ToolStripButton1.Visible = True
                        ToolStripButton2.Visible = True
                End Select
            Case FinanceExpenses.TxEnum.HistoryRecord
                UcFinanceExpenses1.DisabledContextMenu()
                If User.can("Validate For Finance") Then
                    ToolStripButtonCommit.Visible = True 'Commit
                End If
        End Select

        'Check Add or Update
        '                Select Case TxEnum
        '                    Case ProductRequestApp.TxEnum.NewRecord
        '                        DRV = myController.GetNewRecord
        '                        DRV.Row.Item("applicantname") = Identity.username
        '                        DRV.Row.Item("applicantdate") = Today.Date
        '                        DRV.Row.Item("deliverydate") = Today.Date
        '                        DRV.Row.Item("deptid") = Identity.deptid

        '                        DRV.Row.Item("sendto") = "Supply Chain"
        '                        DRV.Row.Item("createdby") = Identity.userid
        '                        DRV.Row.Item("createddate") = Now
        '                        DRV.Row.Item("status") = 0
        '                        DRV.EndEdit()
        '                    Case ProductRequestApp.TxEnum.UpdateRecord
        '                        DRV = myController.GetCurrentRecord
        '                    Case ProductRequestApp.TxEnum.ValidateRecord, ProductRequestApp.TxEnum.HistoryRecord
        '                        DRV = myController.GetCurrentRecord
        '                End Select

        '                DTLBS = myController.GetDTLBS

        '                'Binding Control
        '                Select Case TxEnum
        '                    Case ProductRequestApp.TxEnum.NewRecord, ProductRequestApp.TxEnum.UpdateRecord
        '                        If DRV.Row.Item("status") = ProductRequestStatusEnum.StatusDraft Then
        '                            'Enabled Submit & Commit
        '                            ToolStripButtonSubmit.Visible = True
        '                            ToolStripButtonCommit.Visible = True
        '                            ToolStripButtonStsCancelled.Visible = (TxEnum = ProductRequestApp.TxEnum.UpdateRecord)
        '                        ElseIf DRV.Row.Item("status") = ProductRequestStatusEnum.StatusNew Then
        '                            UcProductRequest1.GroupBox1.Enabled = False
        '                            UcProductRequest1.DataGridView1.ContextMenuStrip = Nothing
        '                            UcProductRequest1.RemoveHandlerDataGridView1CellDoubleClick()
        '                        End If
        '                    Case ProductRequestApp.TxEnum.ValidateRecord
        '                        'Check Status if rejected -> submit or cancel

        '                        If DRV.Row.Item("status") = ProductRequestStatusEnum.StatusRejectedbyDirector Or
        '                            DRV.Row.Item("status") = ProductRequestStatusEnum.StatusRejectedbyMDirector Then
        '                            ToolStripButtonReSubmit.Visible = True
        '                            ToolStripButtonStsCancelled.Visible = True
        '                        Else
        '                            UcProductRequest1.GroupBox1.Enabled = False
        '                            ToolStripButtonValidate.Visible = True
        '                            ToolStripButtonReject.Visible = True
        '                            UcProductRequest1.DataGridView1.ContextMenuStrip = Nothing
        '                            UcProductRequest1.RemoveHandlerDataGridView1CellDoubleClick()
        '                            If User.can("Validate Confirmed Qty") Then
        '                                UcProductRequest1.GroupBox1.Enabled = True
        '                                UcProductRequest1.DataGridView1.Columns(8).ReadOnly = False
        '                                For Each DRV As DataRowView In DTLBS.List
        '                                    DRV.Item("confirmedqty") = DRV.Item("qty")
        '                                Next
        '                                UcProductRequest1.EnabledDeliveryDate()
        '                            End If
        '                        End If



        '                        'Dim myRbac As New DbManager
        '                        'If myRbac.isAssignedto(User.getId, "Supply Chain HK") And (DRV.Item("status") = ProductRequestStatusEnum.StatusValidatedbyDirector Or DRV.Item("status") = ProductRequestStatusEnum.StatusValidatedbyMDirector) Then
        '                        '    'put default value for confirmation
        '                        '    For Each DRV As DataRowView In DTLBS.List
        '                        '        DRV.Item("confirmedqty") = DRV.Item("qty")
        '                        '    Next
        '                        'End If
        '                    Case ProductRequestApp.TxEnum.HistoryRecord
        '                        UcProductRequest1.GroupBox1.Enabled = False
        '                        UcProductRequest1.DataGridView1.ContextMenuStrip = Nothing
        '                        UcProductRequest1.RemoveHandlerDataGridView1CellDoubleClick()
        '                End Select
        '                Dim CMMFController1 As New CMMFController
        '                Dim BParnerController1 As New BPartnerController
        '                Dim ExpensesTypeController1 As New ExpensesTypeController
        '                Dim ExpensesCriteria As String = String.Format("where deptid = {0} and isactive", DRV.Row.Item("deptid"))

        '                'Check Userid Role and Status

        '                UcProductRequest1.BindingControl(DRV, DTLBS, CMMFController1.Model.GetCMMFBS, ExpensesTypeController1.Model.GetExpensesTypeBS(ExpensesCriteria), BParnerController1.Model.GetBPartnerBS)
        '                DataGridView1.AutoGenerateColumns = False
        '                DataGridView1.DataSource = myController.GetActionBS
    End Sub


    Public ReadOnly Property IsModified As Boolean
        Get
            Return _isModified
        End Get
    End Property

    Public Sub New(ByVal hdid As Long, ByVal txEnum As TxEnum)

        ' This call is required by the designer.
        InitializeComponent()
        RemoveHandler myController.IsModified, AddressOf setIsModified
        AddHandler myController.IsModified, AddressOf setIsModified

        AddHandler DoBackground.CallBack, AddressOf RaiseEventCallback

        ' Add any initialization after the InitializeComponent() call.
        Criteria = String.Format("where hd.id = {0}", hdid)
        Me.TxEnum = txEnum
        'Check User ApprovalLevel
        Dim myApproval As New ApprovalModel
        FinanceEmailList = myApproval.FinanceEmailList
        FinanceUserNameList = myApproval.FinanceUserNameList
        'Dim nLevelApproval = DirectCast(User.identity, UserController).nlevelapproval
        'If nLevelApproval = "" Then
        '    'NO Level Approval
        'Else
        '    If DirectCast(User.identity, UserController).nlevelapproval > 0 Then
        '        'FirstApprover based on UserLogin
        '        'FirstApprover = myApproval.getApprover(DirectCast(User.identity, UserController).employeenumber)
        '        'All Employee must have at least 1 approver, the approver can be First Approver or Final Approver.
        '        'First Approver has value nothing in nlevelapprover field
        '        'Final Approver has value 1 in nlevelapprover field
        '        FirstApproverUser = myApproval.getApprover(DirectCast(User.identity, UserController).employeenumber)
        '        'If nLevelApproval = 2 Then
        '        If FirstApproverUser(0).nlevelapproval = 1 Then
        '            'Final Approver, no need to find the second approver.
        '        ElseIf IsDBNull(FirstApproverUser(0).nlevelapproval) Then
        '            'First Approver. Find the Final Approver again.
        '            'SecondApprover = myApproval.getApprover(FirstApprover)
        '            'Get SecondApprover
        '            SecondApproverUser = myApproval.getApprover(FirstApproverUser(0).employeenumber)
        '        End If

        '    Else

        '    End If
        'End If

        'FirstApprover based on UserLogin
        'FirstApprover = myApproval.getApprover(DirectCast(User.identity, UserController).employeenumber)
        'All Employee must have at least 1 approver, the approver can be First Approver or Final Approver.
        'First Approver has value nothing in nlevelapprover field
        'Final Approver has value 1 in nlevelapprover field
        FirstApproverUser = myApproval.getApprover(DirectCast(User.identity, UserController).employeenumber)
        'If nLevelApproval = 2 Then
        HasApprover = True
        If FirstApproverUser.Count = 0 Then
            'Cannot Find Approver For this user
            'Show message
            MessageBox.Show("Sorry, you don't have any approver. Please contact Administrator!")
            HasApprover = False
        Else

            If FirstApproverUser(0).nlevelapproval = ApproverType.FinalApprover Then
                'Final Approver, no need to find the second approver.
            ElseIf FirstApproverUser(0).nlevelapproval = ApproverType.RegularUser Then
                'First Approver. Find the Final Approver again.
                'SecondApprover = myApproval.getApprover(FirstApprover)
                'Get SecondApprover
                SecondApproverUser = myApproval.getApprover(FirstApproverUser(0).employeenumber)
            End If

        End If
    End Sub

    Private Sub FormExpenses_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        RaiseEvent myFormClosed(IsModified)
    End Sub

    Private Sub FormExpenses_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If DoBackground1.myThread.IsAlive Then
            e.Cancel = True
            'DoBackground1.ProgressReport(1, "Please wait until the current process is finished.")
            MessageBox.Show("Please wait until the current process is finished.")
        End If
    End Sub


    Private Sub FormProductRequest_Load(sender As Object, e As EventArgs) Handles Me.Load
        loaddata()
    End Sub


    Private Sub loaddata()
        DoBackground1.doThread(AddressOf DoWork)
    End Sub
    Sub DoWork()
        DoBackground1.ProgressReport(5, "Marquee")
        DoBackground1.ProgressReport(1, "Loading Data.")

        Try
            If myController.loaddata(Criteria) Then
                DoBackground1.ProgressReport(4, "CallBack")
                DoBackground1.ProgressReport(1, "Loading Data.Done!")

            End If
        Catch ex As Exception
            DoBackground1.ProgressReport(1, "Loading Data.Error::" & ex.Message)

        Finally
            DoBackground1.ProgressReport(6, "Continuous")
        End Try
    End Sub

    'Private Sub ProgressReport(ByVal id As Integer, ByVal message As String)
    '    'If Me.InvokeRequired Then
    '    '    Dim d As New ProgressReportDelegate(AddressOf ProgressReport)
    '    '    Me.Invoke(d, New Object() {id, message})
    '    'Else
    '    '    Try
    '    '        Select Case id
    '    '            Case 1
    '    '                ToolStripStatusLabel1.Text = message
    '    '            Case 2
    '    '                ToolStripStatusLabel1.Text = message

    '    '            Case 4
    '    '                'Check Add or Update
    '    '                Select Case TxEnum
    '    '                    Case ProductRequestApp.TxEnum.NewRecord
    '    '                        DRV = myController.GetNewRecord
    '    '                        DRV.Row.Item("applicantname") = Identity.username
    '    '                        DRV.Row.Item("applicantdate") = Today.Date
    '    '                        DRV.Row.Item("deliverydate") = Today.Date
    '    '                        DRV.Row.Item("deptid") = Identity.deptid

    '    '                        DRV.Row.Item("sendto") = "Supply Chain"
    '    '                        DRV.Row.Item("createdby") = Identity.userid
    '    '                        DRV.Row.Item("createddate") = Now
    '    '                        DRV.Row.Item("status") = 0
    '    '                        DRV.EndEdit()
    '    '                    Case ProductRequestApp.TxEnum.UpdateRecord
    '    '                        DRV = myController.GetCurrentRecord
    '    '                    Case ProductRequestApp.TxEnum.ValidateRecord, ProductRequestApp.TxEnum.HistoryRecord
    '    '                        DRV = myController.GetCurrentRecord
    '    '                End Select

    '    '                DTLBS = myController.GetDTLBS

    '    '                'Binding Control
    '    '                Select Case TxEnum
    '    '                    Case ProductRequestApp.TxEnum.NewRecord, ProductRequestApp.TxEnum.UpdateRecord
    '    '                        If DRV.Row.Item("status") = ProductRequestStatusEnum.StatusDraft Then
    '    '                            'Enabled Submit & Commit
    '    '                            ToolStripButtonSubmit.Visible = True
    '    '                            ToolStripButtonCommit.Visible = True
    '    '                            ToolStripButtonStsCancelled.Visible = (TxEnum = ProductRequestApp.TxEnum.UpdateRecord)
    '    '                        ElseIf DRV.Row.Item("status") = ProductRequestStatusEnum.StatusNew Then
    '    '                            UcProductRequest1.GroupBox1.Enabled = False
    '    '                            UcProductRequest1.DataGridView1.ContextMenuStrip = Nothing
    '    '                            UcProductRequest1.RemoveHandlerDataGridView1CellDoubleClick()
    '    '                        End If
    '    '                    Case ProductRequestApp.TxEnum.ValidateRecord
    '    '                        'Check Status if rejected -> submit or cancel

    '    '                        If DRV.Row.Item("status") = ProductRequestStatusEnum.StatusRejectedbyDirector Or
    '    '                            DRV.Row.Item("status") = ProductRequestStatusEnum.StatusRejectedbyMDirector Then
    '    '                            ToolStripButtonReSubmit.Visible = True
    '    '                            ToolStripButtonStsCancelled.Visible = True
    '    '                        Else
    '    '                            UcProductRequest1.GroupBox1.Enabled = False
    '    '                            ToolStripButtonValidate.Visible = True
    '    '                            ToolStripButtonReject.Visible = True
    '    '                            UcProductRequest1.DataGridView1.ContextMenuStrip = Nothing
    '    '                            UcProductRequest1.RemoveHandlerDataGridView1CellDoubleClick()
    '    '                            If User.can("Validate Confirmed Qty") Then
    '    '                                UcProductRequest1.GroupBox1.Enabled = True
    '    '                                UcProductRequest1.DataGridView1.Columns(8).ReadOnly = False
    '    '                                For Each DRV As DataRowView In DTLBS.List
    '    '                                    DRV.Item("confirmedqty") = DRV.Item("qty")
    '    '                                Next
    '    '                                UcProductRequest1.EnabledDeliveryDate()
    '    '                            End If
    '    '                        End If



    '    '                        'Dim myRbac As New DbManager
    '    '                        'If myRbac.isAssignedto(User.getId, "Supply Chain HK") And (DRV.Item("status") = ProductRequestStatusEnum.StatusValidatedbyDirector Or DRV.Item("status") = ProductRequestStatusEnum.StatusValidatedbyMDirector) Then
    '    '                        '    'put default value for confirmation
    '    '                        '    For Each DRV As DataRowView In DTLBS.List
    '    '                        '        DRV.Item("confirmedqty") = DRV.Item("qty")
    '    '                        '    Next
    '    '                        'End If
    '    '                    Case ProductRequestApp.TxEnum.HistoryRecord
    '    '                        UcProductRequest1.GroupBox1.Enabled = False
    '    '                        UcProductRequest1.DataGridView1.ContextMenuStrip = Nothing
    '    '                        UcProductRequest1.RemoveHandlerDataGridView1CellDoubleClick()
    '    '                End Select
    '    '                Dim CMMFController1 As New CMMFController
    '    '                Dim BParnerController1 As New BPartnerController
    '    '                Dim ExpensesTypeController1 As New ExpensesTypeController
    '    '                Dim ExpensesCriteria As String = String.Format("where deptid = {0} and isactive", DRV.Row.Item("deptid"))

    '    '                'Check Userid Role and Status

    '    '                UcProductRequest1.BindingControl(DRV, DTLBS, CMMFController1.Model.GetCMMFBS, ExpensesTypeController1.Model.GetExpensesTypeBS(ExpensesCriteria), BParnerController1.Model.GetBPartnerBS)
    '    '                DataGridView1.AutoGenerateColumns = False
    '    '                DataGridView1.DataSource = myController.GetActionBS
    '    '            Case 5
    '    '                ToolStripProgressBar1.Style = ProgressBarStyle.Continuous
    '    '            Case 6
    '    '                ToolStripProgressBar1.Style = ProgressBarStyle.Marquee
    '    '        End Select
    '    '    Catch ex As Exception
    '    '        MessageBox.Show(ex.Message)
    '    '    End Try
    '    'End If

    'End Sub

    Private Sub ToolStripButton2_Click(sender As Object, e As EventArgs) Handles ToolStripButtonCommit.Click
        'If myController.save() Then
        '    UcProductRequest1.TextBox1.Text = DRV.Row.Item("refnumber")
        'End If
        myController.saveExpenses()
    End Sub

    Private Sub setIsModified()
        _isModified = True
    End Sub

    Private Sub ToolStripButtonSubmit_Click(sender As Object, e As EventArgs) Handles ToolStripButtonSubmit.Click
        'If Me.validate Then
        '    If DRV.Row.Item("status") = ProductRequestStatusEnum.StatusDraft Then  'DRV is header record
        '        If MessageBox.Show("Do you want to submit this record?", "Submit", System.Windows.Forms.MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = DialogResult.OK Then
        '            Dim userid = DirectCast(User.identity, UserController).userid
        '            DRV.Row.Item("status") = ProductRequestStatusEnum.StatusNew
        '            DRV.Row.Item("deptapproval") = DeptApproval.Name
        '            setApproval(DRV.Item("status"), "New", "")
        '        End If
        '    Else
        '        If DRV.Row.Item("status") = ProductRequestStatusEnum.StatusNew Then
        '            MessageBox.Show("Record already submitted.")
        '        ElseIf DRV.Row.Item("status") = ProductRequestStatusEnum.StatusCancelled Then
        '            MessageBox.Show("Record has been cancelled.")
        '        End If
        '    End If
        'End If

    End Sub

    Private Sub ToolStripButtonForward_Click(sender As Object, e As EventArgs) Handles ToolStripButtonForward.Click
        If Not DoBackground1.myThread.IsAlive Then
            If IsNothing(ApprovalDRV) Then
                If MessageBox.Show("Do you want to forward this record?", "Forward To", System.Windows.Forms.MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = DialogResult.OK Then
                    Dim myform As New DialogForwardTo
                    If myform.ShowDialog = Windows.Forms.DialogResult.OK Then
                        DRV.Row.Item("status") = TaskStatusEnum.STATUS_FORWARD
                        DRV.Row.Item("forwardto") = myform.getForwardTo
                        DRV.Row.Item("forwardtoname") = myform.getForwardToName
                        Dim remarks As String = myform.getRemark
                        Dim StatusName As String = String.Empty
                        StatusName = "Forward task"
                        setApproval(DRV.Item("status"), StatusName, remarks)
                    End If
                End If

            Else
                MessageBox.Show("Nothing todo.")
            End If
        End If
        


    End Sub

    Private Sub ToolStripButtonValidate_Click(sender As Object, e As EventArgs) Handles ToolStripButtonValidate.Click
        If Not DoBackground1.myThread.IsAlive Then
            If IsNothing(ApprovalDRV) Then
                If UcFinanceExpenses1.validate() And Me.validate Then
                    Dim ButtonText = ToolStripButtonValidate.Text
                    If MessageBox.Show("Do you want to validate this record?", ButtonText, System.Windows.Forms.MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = DialogResult.OK Then
                        Dim remarks As String = String.Empty 'InputBox("Please input some comment.")
                        Dim mydialog = New DialogInputText
                        If mydialog.ShowDialog = Windows.Forms.DialogResult.OK Then
                            remarks = mydialog.GetRemark
                            Dim StatusName As String = String.Empty
                            Select Case DRV.Row.Item("status")
                                Case TaskStatusEnum.STATUS_NEW, TaskStatusEnum.STATUS_FORWARD
                                    DRV.Row.Item("status") = TaskStatusEnum.STATUS_VALIDATEDBYREQUESTER
                                    StatusName = "Validated by Requester"
                                    'Create Approver
                                    Dim appdrv As DataRowView = myController.GetApprovalTxBS.AddNew
                                    appdrv.Row.Item("stapprover") = FirstApproverUser(0).employeenumber
                                    DRV.Row.Item("stemail") = FirstApproverUser(0).email
                                    DRV.Row.Item("stapprovername") = FirstApproverUser(0).username
                                    'If DirectCast(User.identity, UserController).nlevelapproval = 2 Then
                                    '    appdrv.Row.Item("ndapprover") = SecondApproverUser(0).employeenumber
                                    '    DRV.Row.Item("ndemail") = SecondApproverUser(0).email
                                    '    DRV.Row.Item("ndapprovername") = SecondApproverUser(0).username
                                    'End If
                                    If Not IsNothing(SecondApproverUser) Then
                                        appdrv.Row.Item("ndapprover") = SecondApproverUser(0).employeenumber
                                        DRV.Row.Item("ndemail") = SecondApproverUser(0).email
                                        DRV.Row.Item("ndapprovername") = SecondApproverUser(0).username
                                    End If
                                    appdrv.EndEdit()
                                Case TaskStatusEnum.STATUS_REJECTEDBYM1, TaskStatusEnum.STATUS_REJECTEDBYM2, TaskStatusEnum.STATUS_REJECTEDBYFINANCE
                                    'DRV.Row.Item("status") = TaskStatusEnum.STATUS_VALIDATEDBYREQUESTER
                                    'StatusName = "Validated by Requester"
                                    DRV.Row.Item("status") = TaskStatusEnum.STATUS_RE_SUBMIT
                                    StatusName = "Re-Submit"
                                Case TaskStatusEnum.STATUS_RE_SUBMIT
                                    'DRV.Row.Item("status") = TaskStatusEnum.STATUS_RE_SUBMIT
                                    'StatusName = "Re-Submit"
                                    DRV.Row.Item("status") = TaskStatusEnum.STATUS_VALIDATEDBYM1
                                    StatusName = "Validated by M+1"
                                Case TaskStatusEnum.STATUS_VALIDATEDBYREQUESTER
                                    DRV.Row.Item("status") = TaskStatusEnum.STATUS_VALIDATEDBYM1
                                    StatusName = "Validated by M+1"
                                    'Check Delegation
                                    Dim appdrv As DataRowView = ApprovalTXBS.Current
                                    If appdrv.Row.Item("stapprover") <> DirectCast(User.identity, UserController).employeenumber Then
                                        'Delegate Task
                                        appdrv.Row.Item("delegatestapprover") = DirectCast(User.identity, UserController).employeenumber
                                    End If
                                Case TaskStatusEnum.STATUS_VALIDATEDBYM1
                                    'Can be validate by Finance Team when
                                    Dim appdrv = ApprovalTXBS.Current
                                    If IsDBNull(appdrv.row.item("ndapprover")) Then
                                        DRV.Row.Item("status") = TaskStatusEnum.STATUS_VALIDATEDBYFINANCE
                                        StatusName = "Validated by Finance"
                                    Else
                                        DRV.Row.Item("status") = TaskStatusEnum.STATUS_VALIDATEDBYM2
                                        StatusName = "Validated by M+2"
                                    End If
                                    'Check Delegation
                                    If appdrv.Row.Item("ndapprover") <> DirectCast(User.identity, UserController).employeenumber Then
                                        'Delegate Task
                                        appdrv.Row.Item("delegatendapprover") = DirectCast(User.identity, UserController).employeenumber
                                    End If
                                Case TaskStatusEnum.STATUS_VALIDATEDBYM2
                                    DRV.Row.Item("status") = TaskStatusEnum.STATUS_VALIDATEDBYFINANCE
                                    StatusName = "Validated by Finance"
                            End Select
                            setApproval(DRV.Item("status"), StatusName, remarks)
                        End If
                    End If
                Else
                    MessageBox.Show("Please fix the error.")
                End If
            Else
                MessageBox.Show("Nothing todo.")
            End If
        End If
        
    End Sub

    Private Sub ToolStripButtonStsCancelled_Click(sender As Object, e As EventArgs) Handles ToolStripButtonStsCancelled.Click
        If Not DoBackground1.myThread.IsAlive Then


            If IsNothing(ApprovalDRV) Then
                If MessageBox.Show("Do you want to cancel this record?", "Cancel", System.Windows.Forms.MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = DialogResult.OK Then
                    Dim mydialog As New DialogInputText
                    If mydialog.ShowDialog = Windows.Forms.DialogResult.OK Then
                        Dim remarks As String = mydialog.GetRemark
                        If DRV.Row.Item("status") = TaskStatusEnum.STATUS_NEW Then
                            DRV.Row.Item("status") = TaskStatusEnum.STATUS_CANCELLED
                            setApproval(DRV.Item("status"), "Cancelled", remarks, False)
                        End If
                    End If
                End If
            Else
                MessageBox.Show("Nothing todo.")
            End If
        End If
    End Sub

    Private Sub ToolStripButtonReject_Click(sender As Object, e As EventArgs) Handles ToolStripButtonReject.Click
        If Not DoBackground1.myThread.IsAlive Then


            If IsNothing(ApprovalDRV) Then
                If MessageBox.Show("Do you want to reject this record?", "Reject", System.Windows.Forms.MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = DialogResult.OK Then
                    'Dim remarks As String = InputBox("Please input some comment.")
                    Dim mydialog As New DialogInputText
                    Dim remarks As String = String.Empty
                    If mydialog.ShowDialog = Windows.Forms.DialogResult.OK Then
                        remarks = mydialog.GetRemark
                        Dim StatusName As String = String.Empty
                        Select Case DRV.Row.Item("status")
                            Case TaskStatusEnum.STATUS_VALIDATEDBYREQUESTER
                                DRV.Row.Item("status") = TaskStatusEnum.STATUS_REJECTEDBYM1
                                StatusName = "Rejected by M+1"
                            Case TaskStatusEnum.STATUS_RE_SUBMIT
                                DRV.Row.Item("status") = TaskStatusEnum.STATUS_REJECTEDBYM1
                                StatusName = "Rejected by M+1"
                            Case TaskStatusEnum.STATUS_VALIDATEDBYM1
                                Dim appdrv = ApprovalTXBS.Current
                                If IsDBNull(appdrv.row.item("ndapprover")) Then
                                    DRV.Row.Item("status") = TaskStatusEnum.STATUS_REJECTEDBYFINANCE
                                    StatusName = "Rejected by Finance"
                                Else
                                    DRV.Row.Item("status") = TaskStatusEnum.STATUS_REJECTEDBYM2
                                    StatusName = "Rejected by M+2"
                                End If
                            Case TaskStatusEnum.STATUS_VALIDATEDBYM2
                                DRV.Row.Item("status") = TaskStatusEnum.STATUS_REJECTEDBYFINANCE
                                StatusName = "Rejected by Finance"
                        End Select
                        setApproval(DRV.Item("status"), StatusName, remarks)
                    Else

                    End If


                End If
            Else
                MessageBox.Show("Nothing todo.")
            End If
        End If
    End Sub

    Private Sub ToolStripButtonComplete_Click(sender As Object, e As EventArgs) Handles ToolStripButtonComplete.Click
        If Not DoBackground1.myThread.IsAlive Then


            If IsNothing(ApprovalDRV) Then
                If UcFinanceExpenses1.validate() Then
                    If MessageBox.Show("Do you want to complete this record?", "Complete", System.Windows.Forms.MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = DialogResult.OK Then
                        Dim remarks As String = String.Empty
                        Dim mydialog = New DialogInputText
                        If mydialog.ShowDialog = Windows.Forms.DialogResult.OK Then
                            remarks = mydialog.GetRemark
                            DRV.Row.Item("status") = TaskStatusEnum.STATUS_COMPLETED
                            setApproval(DRV.Item("status"), "Completed", remarks)
                        End If
                    End If
                End If

            Else
                MessageBox.Show("Nothing todo.")
            End If
        End If
    End Sub
    Private Sub setApproval(status As Integer, statusname As String, remarks As String, Optional send As Boolean = True)
        ApprovalDRV = myController.GetActionBS.AddNew
        ApprovalDRV.Row.Item("status") = status
        ApprovalDRV.Row.Item("statusname") = statusname
        ApprovalDRV.Row.Item("modifiedby") = userid
        ApprovalDRV.Row.Item("latestupdate") = Now
        ApprovalDRV.Row.Item("remark") = remarks

        ApprovalDRV.EndEdit()
        Logger.log(String.Format("** Submit {0} Id: {1}**", userid, DRV.Row.Item("id")))
        If myController.saveExpenses() Then
            If send Then
                SendEmail()
            End If
        End If
    End Sub
    Public Overloads Function validate() As Boolean
        Dim myret As Boolean = True
        If UcFinanceExpenses1.ErrorProvider1.GetError(UcFinanceExpenses1.TextBox9).Length > 0 Then
            myret = False
        End If
        If UcFinanceExpenses1.ErrorProvider1.GetError(UcFinanceExpenses1.TextBox5).Length > 0 Then
            myret = False
        End If
        If myController.DS.Tables("FinanceTx").HasErrors Then
            myret = False
        End If
        Return myret
    End Function

    Private Sub SendEmail()
        Dim myEmail As New SSCEmailTask
        Dim SendTo As String = ""
        Dim sendToName As String = ""
        Dim StatusName As String = ""
        Dim CC As String = String.Empty

        Select Case DRV.Item("status")
            Case TaskStatusEnum.STATUS_FORWARD
                'Check for Forwardto Email
                SendTo = DRV.Row.Item("forwardto")
                sendToName = DRV.Row.Item("forwardtoname")
                StatusName = "Forward"
            Case TaskStatusEnum.STATUS_VALIDATEDBYREQUESTER
                'Check Delegate
                SendTo = DRV.Row.Item("stemail")
                sendToName = DRV.Row.Item("stapprovername")
                StatusName = "Validated by Requester"
                'send to Delegate User 
                'If Not IsDBNull(DRV.Row.Item("delegatestemail")) Then
                '    CC = DRV.Row.Item("delegatestemail")
                'End If

                CC = myController.GetDelegateTo(SendTo)

            Case TaskStatusEnum.STATUS_RE_SUBMIT
                SendTo = DRV.Row.Item("stemail")
                sendToName = DRV.Row.Item("stapprovername")
                StatusName = "Re-Submit"
                'send to Delegate User 


                'If Not IsDBNull(DRV.Row.Item("delegatestemail")) Then
                '    CC = DRV.Row.Item("delegatestemail")
                'End If
                CC = myController.GetDelegateTo(SendTo)
            Case TaskStatusEnum.STATUS_REJECTEDBYFINANCE
                If IsDBNull(DRV.Row.Item("forwardto")) Then
                    SendTo = DRV.Row.Item("attn")
                    sendToName = DRV.Row.Item("attnname")
                Else
                    SendTo = DRV.Row.Item("forwardto")
                    sendToName = DRV.Row.Item("forwardto")                   
                End If
                StatusName = "Rejected by Finance"
            Case TaskStatusEnum.STATUS_REJECTEDBYM1
                If IsDBNull(DRV.Row.Item("forwardto")) Then
                    SendTo = DRV.Row.Item("attn")
                    sendToName = DRV.Row.Item("attnname")
                Else
                    SendTo = DRV.Row.Item("forwardto")
                    sendToName = DRV.Row.Item("forwardtoname")
                End If
                StatusName = "Rejected by M+1"
            Case TaskStatusEnum.STATUS_REJECTEDBYM2
                If IsDBNull(DRV.Row.Item("forwardto")) Then
                    SendTo = DRV.Row.Item("attn")
                    sendToName = DRV.Row.Item("attnname")
                Else
                    SendTo = DRV.Row.Item("forwardto")
                    sendToName = DRV.Row.Item("forwardto")
                End If
                StatusName = "Rejected by M+2"
            Case TaskStatusEnum.STATUS_VALIDATEDBYM1
                'Two Conditions, send to M+2 or Finance. If M+2 is blank, Send To Finance.
                If Not IsDBNull(DRV.Row.Item("ndemail")) Then
                    'M+2
                    SendTo = DRV.Item("ndemail")
                    sendToName = DRV.Item("ndapprovername")
                    'send to Delegate User 
                    'If Not IsDBNull(DRV.Row.Item("delegatendemail")) Then
                    '    CC = DRV.Row.Item("delegatendemail")
                    'End If
                    CC = myController.GetDelegateTo(SendTo)
                Else
                    'Finance
                    SendTo = FinanceEmailList
                    sendToName = FinanceUserNameList
                End If

                StatusName = "Validated by M+1"
            Case TaskStatusEnum.STATUS_VALIDATEDBYM2
                SendTo = FinanceEmailList
                sendToName = FinanceUserNameList
                StatusName = "Validated by M+2"
            Case TaskStatusEnum.STATUS_COMPLETED
                If IsDBNull(DRV.Row.Item("forwardto")) Then
                    SendTo = DRV.Row.Item("attn")
                    sendToName = DRV.Row.Item("attnname")
                Else
                    SendTo = DRV.Row.Item("forwardto")
                    sendToName = DRV.Row.Item("forwardto")
                End If
                StatusName = "Completed"
                'Case ProductRequestStatusEnum.StatusResubmit
                '    'Send to Dept Approval
                '    SendTo = DeptApproval.Email
                '    sendToName = DeptApproval.Name
                '    StatusName = "Resubmit"
                'Case ProductRequestStatusEnum.StatusValidatedbyDirector
                '    StatusName = "Validated by Dept"
                '    If Not IsDBNull(DRV.Item("mdapproval")) Then
                '        SendTo = MDApproval.Email
                '        sendToName = MDApproval.Name
                '    Else
                '        SendTo = SupplyChainTo
                '        CC = SupplyChainCC
                '        sendToName = "Supply Chain"
                '    End If
                'Case ProductRequestStatusEnum.StatusValidatedbyMDirector
                '    StatusName = "Validated by Managing Director"
                '    SendTo = SupplyChainTo
                '    CC = SupplyChainCC
                '    sendToName = "Supply Chain"
                'Case ProductRequestStatusEnum.StatusRejectedbyDirector
                '    'sendto creator
                '    StatusName = "Rejected by Director"
                '    SendTo = DRV.Row.Item("applicantemail")
                '    sendToName = DRV.Row.Item("applicantname")
                'Case ProductRequestStatusEnum.StatusRejectedbyMDirector
                '    'Send to Creator
                '    StatusName = "Rejected by Managing Director"
                '    SendTo = DRV.Row.Item("applicantemail")
                '    sendToName = DRV.Row.Item("applicantname")
                'Case ProductRequestStatusEnum.StatusCompleted
                '    'send to Creator
                '    StatusName = "Completed"
                '    SendTo = DRV.Row.Item("applicantemail") 'DirectCast(User.identity, UserController).email
                '    sendToName = DRV.Row.Item("applicantname") 'DirectCast(User.identity, UserController).username
                'Case ProductRequestStatusEnum.StatusCancelled
                '    'send to validator
                '    SendTo = DeptApproval.Email
                '    sendToName = DeptApproval.Name
                '    StatusName = "Cancelled"
        End Select



        Logger.log(String.Format("SendTo: {0}, SendTo Name: {1}, StatusName: {2} {3} CC: {4}", SendTo, sendToName, StatusName, DRV.Row.Item("id"), CC))
        myEmail.remark = ApprovalDRV.Row.Item("remark")
        If Not myEmail.Execute(SendTo, sendToName, StatusName, DRV, CC) Then
            Logger.log(String.Format("Error Message: {0}", myEmail.errormessage))
        Else
            Logger.log("Email Sent")
        End If
    End Sub



    Private Function ValidConfirmedQty() As Boolean
        Dim myret As Boolean = True
        For Each DRV As DataRowView In DTLBS.List
            DRV.Row.RowError = ""
            If IsDBNull(DRV.Row.Item("confirmedqty")) Then
                DRV.Row.RowError = "Confirmed qty cannot be null"
                myret = False
            Else
                'calculate total
            End If
        Next
        Return myret
    End Function

    Private Sub ToolStripButtonReSubmit_Click(sender As Object, e As EventArgs) Handles ToolStripButtonReSubmit.Click
        'If Me.validate Then
        '    If DRV.Row.Item("status") = ProductRequestStatusEnum.StatusRejectedbyDirector Or DRV.Row.Item("status") = ProductRequestStatusEnum.StatusRejectedbyMDirector Then  'DRV is header record
        '        If MessageBox.Show("Do you want to resubmit this record?", "reSubmit", System.Windows.Forms.MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = DialogResult.OK Then
        '            Dim userid = DirectCast(User.identity, UserController).userid
        '            DRV.Row.Item("status") = ProductRequestStatusEnum.StatusResubmit
        '            DRV.Row.Item("deptapproval") = DeptApproval.ID
        '            setApproval(DRV.Item("status"), "Resubmit", "")
        '        End If
        '    Else
        '        If DRV.Row.Item("status") = ProductRequestStatusEnum.StatusNew Then
        '            MessageBox.Show("Record already submitted.")
        '        ElseIf DRV.Row.Item("status") = ProductRequestStatusEnum.StatusCancelled Then
        '            MessageBox.Show("Record has been cancelled.")
        '        End If
        '    End If
        'End If
    End Sub

    'Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click
    '    Dim SaveFileDialog1 As New SaveFileDialog
    '    Dim ReportName As String = String.Format("{0:yyyyMMdd}_ProductRequestForm", Today.Date)
    '    SaveFileDialog1.FileName = ReportName
    '    SaveFileDialog1.DefaultExt = "xlsx"
    '    Dim FormatReportDelegate As FormatReportDelegate = AddressOf doFormat
    '    If SaveFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
    '        Dim myExcel = New ExportToExcelFile(Me, IO.Path.GetDirectoryName(SaveFileDialog1.FileName), IO.Path.GetFileName(SaveFileDialog1.FileName), FormatReportDelegate, 1, "\templates\ProductRequestForm.xltx")
    '        myExcel.CreateForm(Me, New System.EventArgs)
    '    End If


    'End Sub

    Private Sub doFormat(ByRef sender As Object, ByRef e As EventArgs)
        'Dim owb As Excel.Workbook = DirectCast(sender, Excel.Workbook)
        'Dim osheet As Excel.Worksheet = owb.Worksheets(1)
        'If Not IsDBNull(DRV.Row.Item("approvaldate")) Then
        '    osheet.Range("requestdate").Value = String.Format("{0:dd-MMM-yyyy}", DRV.Row.Item("approvaldate"))
        '    osheet.Range("approvaldate").Value = String.Format("Date: {0:dd-MMM-yyyy}", DRV.Row.Item("approvaldate"))
        'End If
        'If Not IsDBNull(DRV.Row.Item("refnumber")) Then
        '    osheet.Range("refnumber").Value = DRV.Row.Item("refnumber")
        'End If
        'Dim detailstartrow As Integer = osheet.Range("rowdetailstart").Row
        'Dim i As Integer = 1
        'For Each mydrv As DataRowView In DTLBS.List
        '    osheet.Cells(detailstartrow, 2).value = i
        '    osheet.Cells(detailstartrow, 3).value = mydrv.Row.Item("commercialcode")
        '    osheet.Cells(detailstartrow, 4).value = mydrv.Row.Item("cmmf")
        '    osheet.Cells(detailstartrow, 5).value = mydrv.Row.Item("localdescription")
        '    osheet.Cells(detailstartrow, 7).value = mydrv.Row.Item("confirmedqty")
        '    If Not IsDBNull(mydrv.Row.Item("confirmedqty")) And Not IsDBNull(mydrv.Row.Item("price")) Then
        '        osheet.Cells(detailstartrow, 8).value = mydrv.Row.Item("confirmedqty") * mydrv.Row.Item("price")
        '    End If
        '    'osheet.Cells(detailstartrow, 8).value = mydrv.Row.Item("confirmedqty") * mydrv.Row.Item("price")
        '    osheet.Cells(detailstartrow, 9).value = String.Format("{0} - {1}", mydrv.Row.Item("expensesacc"), mydrv.Row.Item("expensesname"))
        '    osheet.Cells(detailstartrow, 10).value = mydrv.Row.Item("remarks")
        '    detailstartrow += 1
        '    i += 1
        'Next
        'If Not IsDBNull(DRV.Row.Item("deliverydate")) Then
        '    osheet.Range("deliverydate").Value = DRV.Row.Item("deliverydate")
        'End If
        'If Not IsDBNull(DRV.Row.Item("bpartnerfullname")) Then
        '    osheet.Range("bpname").Value = DRV.Row.Item("bpartnerfullname")
        'End If
        'If Not IsDBNull(DRV.Row.Item("bpartnerfullname")) Then
        '    osheet.Range("bpname").Value = DRV.Row.Item("bpartnerfullname")
        'End If
        'If Not IsDBNull(DRV.Row.Item("bpartneraddress")) Then
        '    osheet.Range("address1").Value = DRV.Row.Item("bpartneraddress")
        'End If
        'osheet.Range("address2").Value = "" & DRV.Row.Item("region") & " " & DRV.Row.Item("country")
        'osheet.Range("otherinstruction").Value = "" & DRV.Row.Item("instruction")
        ''bpartneraddress
        'osheet.Range("applicantname").Value = "" & DRV.Row.Item("applicantname")
        'osheet.Range("applicantdate").Value = String.Format("Date: {0:dd-MMM-yyyy}", DRV.Row.Item("applicantdate"))
        'osheet.Range("deptapproval").Value = "" & DRV.Row.Item("approvalname")

        'osheet.Range("mdapproval").Value = "" & MDApproval.Name
    End Sub

    Private Sub BindingControl()

    End Sub

    Private Function GetDelegateTo(SendTo As String) As String
        Return myController.GetDelegateTo(SendTo)
    End Function

    Private Sub ToolStripButton2_Click_10(sender As Object, e As EventArgs)
        If Not DoBackground1.myThread.IsAlive Then
            myupload = New UploadExpensesController(Me, FinanceTxBS, myController)
            UcFinanceExpenses1.DoImport = True
            myupload.ImportData()
        Else
            MessageBox.Show("Please wait until the current process is finished.")
        End If



    End Sub
    Private Sub ToolStripButton2_Click_1(sender As Object, e As EventArgs) Handles ToolStripButton2.Click
        If Not DoBackground1.myThread.IsAlive Then
            myupload = New UploadExpensesController(Me, FinanceTxBS, myController)
            UcFinanceExpenses1.DoImport = True
            ImportData()
        Else
            MessageBox.Show("Please wait until the current process is finished.")
        End If



    End Sub

    Private Sub ImportData()
        Dim OpenFileDialog1 As New OpenFileDialog
        If Not DoBackground1.myThread.IsAlive Then
            ToolStripStatusLabel1.Text = ""
            If OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
                myupload.ImportFileName = OpenFileDialog1.FileName

                DoBackground2.doThread(AddressOf myupload.DoImport)
            Else
                'DoBackground1.ProgressReport(5, "Marquee")
                DoBackground2.ProgressReport(1, "Open file cancelled.")
            End If

        End If

    End Sub


    Public Sub RaiseEventCallback(sender As Object, e As EventArgs)
        If sender = 8 Then
            Try
                'DoBackground1.ProgressReport(1, "Start Raise Callback")
                UcFinanceExpenses1.RefreshDatagrid()

                'UcFinanceExpenses1.DataGridView1.DataSource = FinanceTxBS
                UcFinanceExpenses1.EnableContextMenu()

                UcFinanceExpenses1.getTotal()
                UcFinanceExpenses1.DoImport = False

                UcFinanceExpenses1.TextBox9.Text = myupload.VendorDesc
                UcFinanceExpenses1.TextBox5.Text = myupload.InvoiceNumber
            Catch ex As Exception
                DoBackground1.ProgressReport(1, ex.Message)
            End Try

            Try
                'MessageBox.Show(myupload.VendorCode)
                DRV.Row.BeginEdit()
                DRV.Row.Item("vendorcode") = myupload.VendorCode
                DRV.Row.Item("invoicenumber") = myupload.InvoiceNumber
                DRV.Row.EndEdit()
            Catch ex As Exception
                DoBackground1.ProgressReport(1, ex.Message)
            End Try
            'DoBackground1.ProgressReport(1, "End Raise Callback")
        ElseIf sender = 100 Then
            UcFinanceExpenses1.ErrorProvider1.SetError(UcFinanceExpenses1.TextBox9, "Vendor code is not available.")
        ElseIf sender = 101 Then
            UcFinanceExpenses1.ErrorProvider1.SetError(UcFinanceExpenses1.TextBox5, "Value cannot be blank.")
        End If
    End Sub

    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click
        If Not DoBackground1.myThread.IsAlive Then


            Dim savedialog1 As New SaveFileDialog
            savedialog1.FileName = "UploadExpanses.xlsx"
            If savedialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
                Dim filesource = String.Format("{0}\template\InvoiceUploadTemplate.xlsx", Application.StartupPath)
                Dim filedestination = savedialog1.FileName
                Try
                    FileCopy(filesource, filedestination)
                    MessageBox.Show("Done.")
                Catch ex As Exception
                    MessageBox.Show(ex.Message)
                End Try

            End If
        End If
    End Sub

   
End Class
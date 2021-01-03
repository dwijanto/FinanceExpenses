Imports Microsoft.Office.Interop
Imports System.Text

Public Class FormGenerateReport

    Dim RawDataCallback As GenerateReportDelegate = AddressOf FormattingRawData
    Dim GenerateReportCallback As GenerateReportDelegate = AddressOf FormattingReport
    Dim savefiledialog1 As New SaveFileDialog
    Dim doBackground1 As New DoBackground(Me, AddressOf doBackgroundCallback)
    Dim doBackground2 As New DoBackground(Me, AddressOf doBackground2Callback)
    Dim StartDate As Date
    Dim EndDate As Date

    Private _RefNumber As String
    Private _InvoiceNumber As String
    Private _ApprovalStatus As String
    Private _attn As String
    Private _stApprover As String
    Private _ndApprover As String
    Private _Vendorcode As String
    Dim myController As EmailController

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        StartDate = DateTimePicker1.Value
        EndDate = DateTimePicker2.Value
        Dim myfilter As New StringBuilder
        If TextBox1.TextLength > 0 Then
            If _RefNumber.ToString.Length > 0 Then
                myfilter.Append(String.Format(" and referencenumber = {0}", _RefNumber))
            End If
        End If
        If TextBox2.TextLength > 0 Then
            If _InvoiceNumber.ToString.Length > 0 Then
                myfilter.Append(String.Format(" and invoicenumber = '{0}'", _InvoiceNumber))
            End If
        End If
        If TextBox3.TextLength > 0 Then
            If _ApprovalStatus.ToString.Length > 0 Then
                myfilter.Append(String.Format(" and status = {0}", _ApprovalStatus))
            End If
        End If
        If TextBox4.TextLength > 0 Then
            If _attn.ToString.Length > 0 Then
                myfilter.Append(String.Format(" and attn = '{0}'", _attn))
            End If
        End If
        If TextBox5.TextLength > 0 Then
            If _stApprover.ToString.Length > 0 Then
                myfilter.Append(String.Format(" and stapprover = '{0}'", _stApprover))
            End If
        End If
        If TextBox6.TextLength > 0 Then
            If _ndApprover.ToString.Length > 0 Then
                myfilter.Append(String.Format(" and ndapprover = '{0}'", _ndApprover))
            End If
        End If
        If TextBox7.TextLength > 0 Then
            If _Vendorcode.ToString.Length > 0 Then
                myfilter.Append(String.Format(" and h.vendorcode = {0}", _Vendorcode))
            End If
        End If


        Dim criteria As String = String.Format(" where receiveddate >= '{0:yyyy-MM-dd}' and receiveddate <= '{1:yyyy-MM-dd}' {2}", StartDate, EndDate.AddDays(1), myfilter.ToString)
        'Dim Sqlstr As String = String.Format("select *,ssc.getstatusname(h.status) as statusname,date_part('Year',creationdate)::text || '-' || to_char(referencenumber,'FM000000') as refnumber from ssc.sscemailhd h" &
        '                       " left join ssc.sscemaildt d on d.hdid = h.id " &
        '                       " left join ssc.sscapprovaltx a on a.sscemailhdid = h.id" &
        '                       " left join ssc.sscfinancetx f on f.sscemailhdid = h.id" &
        '                       " {0} order by receiveddate asc;", criteria)
        Dim Sqlstr As String = String.Format("with lu as (select sscemailhdid,max(latestupdate) as latestaction from ssc.sscemailaction group by sscemailhdid order by sscemailhdid)" &
                                             " select h.*,d.*,a.*,f.*,ssc.getstatusname(h.status) as statusname,date_part('Year',creationdate)::text || '-' || to_char(referencenumber,'FM000000') as refnumber," &
                                             " v.vendorname,u1.username as requester,u2.username as forwardtoname,u3.username as staprovername," &
                                             " u4.username as ndapprovername,u5.username as delegatestapprovername,u6.username as delegatendapprovername,lu.latestaction" &
                                             " from ssc.sscemailhd h " &
                                             " left join ssc.sscemaildt d on d.hdid = h.id" &
                                             " left join ssc.sscapprovaltx a on a.sscemailhdid = h.id" &
                                             " left join ssc.sscfinancetx f on f.sscemailhdid = h.id " &
                                             " left join ssc.vendor v on v.vendorcode = h.vendorcode" &
                                             " left join ssc.user u1 on u1.email = h.attn" &
                                             " left join ssc.user u2 on u2.email = h.forwardto" &
                                             " left join ssc.user u3 on u3.employeenumber = a.stapprover" &
                                             " left join ssc.user u4 on u4.employeenumber = a.ndapprover" &
                                             " left join ssc.user u5 on u5.employeenumber = a.delegatestapprover" &
                                             " left join ssc.user u6 on u6.employeenumber =  a.delegatendapprover" &
                                             " left join lu on lu.sscemailhdid = h.id" &
                                             "{0} order by receiveddate asc;", criteria)
        savefiledialog1.FileName = String.Format("All_Invoice-{0:yyyyMMdd}.xlsx", Today.Date)
        If savefiledialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            Dim myReport = New GenerateReport(Me, Sqlstr, savefiledialog1.FileName, GenerateReportCallback, RawDataCallback, 1)
            myReport.Run()
        End If
    End Sub
    Public Sub ProgressReport(ByVal id As Integer, ByVal message As String)
        doBackground1.ProgressReport(id, message)
    End Sub
    Sub FormattingReport(ByRef sender As Object, ByRef e As EventArgs)

    End Sub

    Sub FormattingRawData(ByRef sender As Object, ByRef e As EventArgs)
        Dim osheet As Excel.Worksheet = DirectCast(sender, Excel.Worksheet)
        osheet.Columns("C:C").columnwidth = 70
        osheet.Columns("G:G").NumberFormat = "yyyy-MM-dd HH:mm:ss"
        osheet.Name = "RawData"

    End Sub

    Sub doBackgroundCallback()
        Debug.Print("Callback")
    End Sub


    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()
        initFilter()
        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub initFilter()
        doBackground2.doThread(AddressOf DoFilter)
    End Sub

    Sub doBackground2Callback()
        'Throw New NotImplementedException
    End Sub

    Sub DoFilter()
        myController = New EmailController
        Try
            doBackground2.ProgressReport(1, "Loading Filter...Please wait.")
            If myController.LoadFilter() Then
                doBackground2.ProgressReport(4, "CallBack")
            End If
            doBackground2.ProgressReport(1, String.Format("Loading Filter...Done."))
        Catch ex As Exception
            doBackground2.ProgressReport(1, ex.Message)
        End Try
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles ButtonRef.Click, ButtonInvoiceNumber.Click, ButtonApprovalStatus.Click, ButtonAttn.Click, ButtonstApprover.Click, ButtonndApprover.Click, ButtonVendor.Click
        Dim helperbs As New BindingSource
        Select Case DirectCast(sender, Button).Name
            Case "ButtonRef"
                helperbs = myController.GetReferencenumberFilterBS
            Case "ButtonInvoiceNumber"
                helperbs = myController.GetInvoiceNumberFilterBS
            Case "ButtonApprovalStatus"
                helperbs = myController.GetStatusFilterBS
            Case "ButtonAttn"
                helperbs = myController.GetAttnFilterBS
            Case "ButtonstApprover"
                helperbs = myController.GetstApproverFilterBS
            Case "ButtonndApprover"
                helperbs = myController.GetndApproverFilterBS
            Case "ButtonVendor"
                helperbs = myController.GetVendorFilterBS
        End Select

        helperbs.Filter = ""
        Dim myform = New FormHelper(helperbs)
        myform.Column1.Width = 400
        myform.Width = 600
        myform.DataGridView1.Columns(0).DataPropertyName = "description"

        myform.Filter = "[description] like '%{0}%'"
        If myform.ShowDialog() = DialogResult.OK Then
            Dim drvcurr As DataRowView = helperbs.Current
            Select Case DirectCast(sender, Button).Name
                Case "ButtonRef"
                    TextBox1.Text = drvcurr.Row.Item("description")
                    _RefNumber = drvcurr.Row.Item("referencenumber")
                Case "ButtonInvoiceNumber"
                    TextBox2.Text = drvcurr.Row.Item("description")
                    _InvoiceNumber = drvcurr.Row.Item("invoicenumber")
                Case "ButtonApprovalStatus"
                    TextBox3.Text = drvcurr.Row.Item("description")
                    _ApprovalStatus = drvcurr.Row.Item("status")
                Case "ButtonAttn"
                    TextBox4.Text = drvcurr.Row.Item("description")
                    _attn = drvcurr.Row.Item("attn")
                Case "ButtonstApprover"
                    TextBox5.Text = drvcurr.Row.Item("description")
                    _stApprover = drvcurr.Row.Item("stapprover")
                Case "ButtonndApprover"
                    TextBox6.Text = drvcurr.Row.Item("description")
                    _ndApprover = drvcurr.Row.Item("ndapprover")
                Case "ButtonVendor"
                    TextBox7.Text = drvcurr.Row.Item("description")
                    _Vendorcode = drvcurr.Row.Item("vendorcode")
            End Select
        Else

        End If

    End Sub
End Class
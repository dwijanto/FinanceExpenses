Imports System.Text
Imports System.Net.Mail
Imports System.Net.Mime

Public Enum TaskStatusEnum
    STATUS_DRAFT = 0
    STATUS_NEW = 1
    STATUS_RE_SUBMIT = 2
    STATUS_FORWARD = 3
    STATUS_VALIDATEDBYREQUESTER = 4
    STATUS_VALIDATEDBYM1 = 5
    STATUS_VALIDATEDBYM2 = 6
    STATUS_VALIDATEDBYFINANCE = 7
    STATUS_REJECTEDBYM1 = 8
    STATUS_REJECTEDBYM2 = 9
    STATUS_REJECTEDBYFINANCE = 10
    STATUS_CANCELLED = 98
    STATUS_COMPLETED = 99
End Enum
    

Public Class SSCEmailTask
    Inherits Email
    Dim AutoTask As Boolean = True
    Private EmailType As EmailTypeEnum
    Private DS As DataSet
    Private HDBS As BindingSource
    Public errormessage As String = String.Empty
    Enum EmailTypeEnum
        Valid = 0
        Invalid = 1
    End Enum


    Public Sub New()

    End Sub

    Public Sub New(ByVal EmailType As EmailTypeEnum)
        Me.EmailType = EmailType
    End Sub

    Private Property statusname As String
    Private Property sendtoname As String
    Public Property remark As String
    Private Property drv As DataRowView

    Private Sub Logg(ByVal mymessage As String)
        If AutoTask Then
            Logger.log(mymessage)
        End If
    End Sub

    Public Function Execute(ByVal sendto As String, ByVal sendtoname As String, ByVal statusname As String, ByVal drv As DataRowView, Optional ByVal cc As String = "") As Boolean
        Dim myret As Boolean = False
        Try
            'Prepare Email
            Me.statusname = statusname
            Me.sendtoname = sendtoname
            Me.drv = drv
            Me.sendto = Trim(sendto)
            If cc.Length > 0 Then
                Me.cc = Trim(cc)
            End If
            Me.subject = String.Format("Indirect Purchase Invoice Approval Task. (Date : {0:dd-MMM-yyyy}) ", Today.Date)
            Dim mycontent As String
            If Not IsNothing(Me.sendto) Then
                If statusname = "Completed" Then
                    mycontent = getBodyMessageCompleted()
                Else
                    mycontent = getBodyMessage()
                End If

                Dim htmlView As AlternateView = AlternateView.CreateAlternateViewFromString(String.Format("{0} <br>Or click the Indirect Purchase Invoice Approval icon on your desktop: <br><p> <a href=""https://sw07e601/RDWeb""><img src=cid:myLogo> </a><br></p><p>Indirect Purchase Invoice Approval System Administrator</p></body></html>", mycontent), Nothing, MediaTypeNames.Text.Html)
                Dim logo As New LinkedResource(Application.StartupPath & "\SSC.png")
                logo.ContentId = "myLogo"
                htmlView.LinkedResources.Add(logo)

                Me.htmlView = htmlView
                Me.isBodyHtml = True
                Me.sender = "no-reply@groupeseb.com"
                Me.body = mycontent
                If Not Me.send(errormessage) Then
                    Logger.log(errormessage)
                Else
                    myret = True
                End If
                'myret = Me.send(errormessage)

            End If
        Catch ex As Exception
            Logger.log(ex.Message)
            'MessageBox.Show(ex.Message)
            errormessage = ex.Message
        End Try

        Return myret
    End Function

    Private Function getBodyMessage() As String
        Dim sb As New StringBuilder
        sb.Append("<!DOCTYPE html><html><head><meta name=""description"" content=""[IndirectPurchaseInvoiceApproval]"" /><meta http-equiv=""Content-Type"" content=""text/html; charset=us-ascii""></head><style>  td,th {padding-left:5px;         padding-right:10px;         text-align:left;  }  th {background-color:red;    color:white}  .defaultfont{    font-size:11.0pt; font-family:""Calibri"",""sans-serif"";    }</style><body class=""defaultfont"">")
        sb.Append(String.Format("<p>Dear {0} </p><p>Please be informed that you have tasks that need to follow up.<br>Date: {1:dd-MMM-yyyy}<br><br><br>", sendtoname, Today.Date))
        sb.Append("    List of Tasks:</p>  <table border=1 cellspacing=0>    <tr><th>Status</th><th>Reference Number</th><th>Subject</th><th>From</th><th>Received Date</th><th>Remark</th></tr>")
        sb.Append(String.Format("<tr><td>{0}</td><td>{5}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td></tr>", statusname, drv.Item("emailsubject"), drv.Item("sender"), drv.Item("receiveddate"), remark, drv.Item("refnumber")))
        sb.Append("</table>  <br>  <p>Thank you.<br><br>You can access the system in RD WEB Access by below link:<br>   <a href=""https://sw07e601/RDWeb"">MyTask</a></p><br>")
        Return sb.ToString
    End Function
    Private Function getBodyMessageCompleted() As Object
        Dim sb As New StringBuilder
        sb.Append("<!DOCTYPE html><html><head><meta name=""description"" content=""[IndirectPurchaseInvoiceApproval]"" /><meta http-equiv=""Content-Type"" content=""text/html; charset=us-ascii""></head><style>  td,th {padding-left:5px;         padding-right:10px;         text-align:left;  }  th {background-color:red;    color:white}  .defaultfont{    font-size:11.0pt; font-family:""Calibri"",""sans-serif"";    }</style><body class=""defaultfont"">")
        sb.Append(String.Format("<p>Dear {0} </p><p>Please be informed that you request has been approved.<br>Date: {1:dd-MMM-yyyy}<br><br><br>", sendtoname, Today.Date))
        sb.Append("    List of Tasks:</p>  <table border=1 cellspacing=0>    <tr><th>Status</th><th>Reference Number</th><th>Subject</th><th>From</th><th>Received Date</th><th>Remark</th></tr>")
        sb.Append(String.Format("<tr><td>{0}</td><td>{5}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td></tr>", statusname, drv.Item("emailsubject"), drv.Item("sender"), drv.Item("receiveddate"), remark, drv.Item("refnumber")))
        sb.Append("</table>  <br>  <p>Thank you.<br><br>You can access the system in RD WEB Access by below link:<br>   <a href=""https://sw07e601/RDWeb"">MyTask</a></p><br>")
        Return sb.ToString

    End Function
    Public Function ValidateEmail() As Boolean
        'Get email with isvalid isnull
        Dim myret = False

        Dim Sqlstr = "select * from ssc.sscemailhd where isvalid isnull;"
        DS = New DataSet
        DS = DataAccess.GetDataSet(Sqlstr, CommandType.Text, Nothing)
        DS.Tables(0).TableName = "SSCEmailHD"
        For Each dr In DS.Tables(0).Rows
            CheckValidEmail(dr)
        Next
        Dim ds2 As DataSet = DS.GetChanges
        If Not IsNothing(ds2) Then
            Dim ra As Integer
            Dim mymessage = String.Empty

            Dim mye As New ContentBaseEventArgs(ds2, True, mymessage, ra, True)
            Dim myModel = New EmailModel
            myret = myModel.saveHD(Me, mye)
            If Not myret Then
                Logg(mye.message)
            End If
        End If
        Return myret
    End Function
    Public Function SendEmail1() As Boolean
        Dim myret = False

        Dim Sqlstr = "select hd.*,dt.attachmentname,u.username from ssc.sscemailhd hd left join ssc.sscemaildt dt on dt.hdid = hd.id" &
                     " left join ssc.user u on u.email = hd.attn" &
                     " where not isvalid isnull and sendisvalid isnull;" &
                     "select dt.* from ssc.paramdt dt" &
             " left join ssc.paramhd hd on hd.paramhdid = dt.paramhdid" &
             " where hd.paramname= 'ssc'" &
             " order by dt.ivalue;"

        DS = New DataSet
        DS = DataAccess.GetDataSet(Sqlstr, CommandType.Text, Nothing)

        Dim InvalidEmail = DS.Tables(1).Rows(6).Item("cvalue")

        DS.Tables(0).TableName = "SSCEmailHD"
        HDBS = New BindingSource
        HDBS.DataSource = DS.Tables("SSCEmailHD")
        Dim myEmail As Object = vbNull
        Select Case EmailType
            Case EmailTypeEnum.Valid
                myEmail = New SendValidEmail(HDBS)
            Case EmailTypeEnum.Invalid
                myEmail = New SendInvalidEmail(HDBS)
        End Select

        For Each n In myEmail.GetQuery
            Me.sendto = InvalidEmail
            If Not IsNothing(Me.sendto) Then
                Dim myContent = myEmail.getBodymessage(n.data)
                Dim htmlView As AlternateView = AlternateView.CreateAlternateViewFromString(String.Format("{0} <br>Or click the Indirect Purchase Invoice Approval icon on your desktop: <br><p> <img src=cid:myLogo> <br></p><p>Indirect Purchase Invoice Approval System Administrator</p></body></html>", myContent), Nothing, MediaTypeNames.Text.Html)
                Dim logo As New LinkedResource(Application.StartupPath & "\SSC.png")
                logo.ContentId = "myLogo"
                htmlView.LinkedResources.Add(logo)

                Me.htmlView = htmlView


                Me.sendto = Me.sendto '"dwijanto@yahoo.com"
                Me.isBodyHtml = True
                Me.sender = "no-reply@groupeseb.com"

                Me.subject = String.Format("Indirect Purchase Invoice Approval Task. (Date : {0:dd-MMM-yyyy}) ", Today.Date) '"***Do not reply to this e-mail.***" "Price CMMF Ex: Tasks status. " '"***Do not reply to this e-mail.***"
                Me.body = myContent 'roleTask.getbodymessage(n.data)
                Me.htmlView = htmlView
                If Not Me.send(errormessage) Then
                    Logger.log(errormessage)
                End If
            End If
        Next
        HDBS.EndEdit()
        Dim ds2 As DataSet = DS.GetChanges
        If Not IsNothing(ds2) Then
            Dim ra As Integer
            Dim mymessage = String.Empty

            Dim mye As New ContentBaseEventArgs(ds2, True, mymessage, ra, True)
            Dim myModel = New EmailModel
            myret = myModel.saveHD(Me, mye)
            If Not myret Then
                Logg(mye.message)
            End If
        End If
        Return myret
    End Function


    Public Function SendEmail() As Boolean
        Dim myret = False

        Dim myEmail As Object = vbNull
        Select Case EmailType
            Case EmailTypeEnum.Valid
                myEmail = New SendValidEmail()
            Case EmailTypeEnum.Invalid
                myEmail = New SendInvalidEmail()
        End Select

        DS = New DataSet
        DS = myEmail.LoadData
        Dim InvalidEmail As String = myEmail.InvalidEmail
        For Each n In myEmail.GetQuery
            If InvalidEmail.Length > 0 Then
                Me.sendto = InvalidEmail
            Else
                Me.sendto = n.key
            End If

            If Not IsNothing(Me.sendto) Then
                Dim myContent = myEmail.getBodymessage(n.data)
                Dim htmlView As AlternateView = AlternateView.CreateAlternateViewFromString(String.Format("{0} <br>Or click the Indirect Purchase Invoice Approval icon on your desktop: <br><p> <a href=""https://sw07e601/RDWeb""><img src=cid:myLogo> </a><br></p><p>Indirect Purchase Invoice Approval System Administrator</p></body></html>", myContent), Nothing, MediaTypeNames.Text.Html)
                Dim logo As New LinkedResource(Application.StartupPath & "\SSC.png")
                logo.ContentId = "myLogo"
                htmlView.LinkedResources.Add(logo)

                Me.htmlView = htmlView


                Me.sendto = Me.sendto '"dwijanto@yahoo.com"
                Me.isBodyHtml = True
                Me.sender = "no-reply@groupeseb.com"

                Me.subject = String.Format("Indirect Purchase Invoice Approval Task. (Date : {0:dd-MMM-yyyy}) ", Today.Date) '"***Do not reply to this e-mail.***" "Price CMMF Ex: Tasks status. " '"***Do not reply to this e-mail.***"
                Me.body = myContent 'roleTask.getbodymessage(n.data)
                Me.htmlView = htmlView
                If Not Me.send(errormessage) Then
                    Logger.log(errormessage)
                End If
            End If
        Next

        myEmail.HDBS.EndEdit()
        Dim ds2 As DataSet = DS.GetChanges
        If Not IsNothing(ds2) Then
            Dim ra As Integer
            Dim mymessage = String.Empty

            Dim mye As New ContentBaseEventArgs(ds2, True, mymessage, ra, True)
            Dim myModel = New EmailModel
            myret = myModel.saveHD(Me, mye)
            If Not myret Then
                Logg(mye.message)
            End If
        End If
        Return myret
    End Function

    Private Sub CheckValidEmail(dr As DataRow)
        dr.BeginEdit()
        Dim emailtoCount = dr.Item("emailto").ToString.Split(";").Count
        If emailtoCount = 2 Then
            If dr.Item("emailto").ToString.Contains("HONsscap@groupeseb.com") Then
                dr.Item("isvalid") = True
                dr.Item("attn") = dr.Item("emailto").ToString.Replace(";", "").Replace("HONsscap@groupeseb.com", "")
            Else
                dr.Item("isvalid") = False
                dr.Item("isvalidremark") = String.Format("No HONsscap@groupeseb.com in Email To.")
            End If
        Else
            dr.Item("isvalid") = False
            dr.Item("isvalidremark") = String.Format("EmailTo count is {0}. The correct one is 2", emailtoCount)
        End If
        dr.EndEdit()
    End Sub




End Class

Public Class SendValidEmail
    Private _HDBS As BindingSource
    Private _InvalidEmail As String = String.Empty

    Public Property InvalidEmail As String
        Get
            Return _InvalidEmail
        End Get
        Set(value As String)
            _InvalidEmail = value
        End Set
    End Property
    Sub New()
        ' TODO: Complete member initialization 
    End Sub

    Public Property HDBS As BindingSource
        Get
            Return _HDBS
        End Get
        Set(value As BindingSource)
            _HDBS = value
            ' HDBS.Filter = "isvalid"
        End Set
    End Property

    Public Sub New(ByVal HDBS As BindingSource)
        'Me.HDBS = HDBS
        'HDBS.Filter = "isvalid"
    End Sub

    Public Function GetQuery() As Object
        Return From n In HDBS.List
                          Group n By key = n.item("attn") Into Group
                          Select key, data = Group
    End Function

    Public Function GetSQLstr() As String
        Dim Sqlstr = "select hd.*,dt.attachmentname,u.username from ssc.sscemailhd hd left join ssc.sscemaildt dt on dt.hdid = hd.id" &
                    " left join ssc.user u on u.email = hd.attn" &
                    " where isvalid and sendisvalid isnull order by receiveddate desc;" &
                    "select dt.* from ssc.paramdt dt" &
            " left join ssc.paramhd hd on hd.paramhdid = dt.paramhdid" &
            " where hd.paramname= 'ssc'" &
            " order by dt.ivalue;"
        Return Sqlstr
    End Function

    Public Function LoadData() As DataSet
        Dim DS = DataAccess.GetDataSet(GetSQLstr, CommandType.Text, Nothing)
        InvalidEmail = "" ' DS.Tables(1).Rows(6).Item("cvalue")
        DS.Tables(0).TableName = "SSCEmailHD"
        HDBS = New BindingSource
        HDBS.DataSource = DS.Tables("SSCEmailHD")
        Return DS
    End Function

    Public Function GetBodyMessage(ByVal data As Object) As String
        Dim sb As New StringBuilder
        sb.Append("<!DOCTYPE html><html><head><meta name=""description"" content=""[IndirectPurchaseInvoiceApproval]"" /><meta http-equiv=""Content-Type"" content=""text/html; charset=us-ascii""></head><style>  td,th {padding-left:5px;         padding-right:10px;         text-align:left;  }  th {background-color:red;    color:white}  .defaultfont{    font-size:11.0pt;	font-family:""Calibri"",""sans-serif"";    }</style><body class=""defaultfont"">")
        sb.Append(String.Format("<p>Dear {0},</p><br><p>Please be informed that you have tasks that need to follow up.<br>Date: {1:dd-MMM-yyyy}<br><br><br>", data(0).row.item("username").ToString, Today.Date))
        sb.Append("    List of Tasks:</p>  <table border=1 cellspacing=0>    <tr>            <th>Subject</th>      <th>From</th>            <th>Received Date</th></tr>")
        For Each n As DataRowView In data
            sb.Append(String.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>", n.Item("emailsubject"), n.Item("sender"), n.Item("receiveddate")))
            n.Item("sendisvalid") = True
            n.Item("status") = TaskStatusEnum.STATUS_NEW
        Next
        sb.Append("</table>  <br>  <p>Thank you.<br><br>You can access the system in RD WEB Access by below link:<br>   <a href=""https://sw07e601/RDWeb"">MyTask</a></p><br>")
        Return sb.ToString
    End Function


End Class

Public Class SendInvalidEmail
    Private _HDBS As BindingSource

    Sub New()
        ' TODO: Complete member initialization 
    End Sub
    Dim _InvalidEmail As String = String.Empty

    Public Property InvalidEmail As String
        Get
            Return _InvalidEmail
        End Get
        Set(value As String)
            _InvalidEmail = value
        End Set
    End Property
    Public Property HDBS As BindingSource
        Get
            Return _HDBS
        End Get
        Set(value As BindingSource)
            _HDBS = value
        End Set
    End Property

    Public Sub New(ByVal HDBS As BindingSource)
        'Me.HDBS = HDBS
        'HDBS.Filter = "isvalid = false"
    End Sub

    Public Function GetQuery() As Object
        Return From n In HDBS.List
        Group n By key = n.item("isvalid") Into Group
        Select key, data = Group
    End Function

    Public Function GetSQLstr() As String
        Dim Sqlstr = "select hd.*,u.username from ssc.sscemailhd hd" &
                    " left join ssc.user u on u.email = hd.attn" &
                    " where not isvalid and sendisvalid isnull order by receiveddate desc;" &
                    "select dt.* from ssc.paramdt dt" &
            " left join ssc.paramhd hd on hd.paramhdid = dt.paramhdid" &
            " where hd.paramname= 'ssc'" &
            " order by dt.ivalue;"
        Return Sqlstr
    End Function

    Public Function LoadData() As DataSet
        Dim DS = DataAccess.GetDataSet(GetSQLstr, CommandType.Text, Nothing)
        InvalidEmail = DS.Tables(1).Rows(6).Item("cvalue")
        DS.Tables(0).TableName = "SSCEmailHD"
        HDBS = New BindingSource
        HDBS.DataSource = DS.Tables("SSCEmailHD")
        Return DS
    End Function

    Public Function GetBodyMessage(ByVal data As Object) As String
        Dim sb As New StringBuilder
        sb.Append("<!DOCTYPE html><html><head><meta name=""description"" content=""[IndirectPurchaseInvoiceApproval]"" /><meta http-equiv=""Content-Type"" content=""text/html; charset=us-ascii""></head><style>  td,th {padding-left:5px;         padding-right:10px;         text-align:left;  }  th {background-color:red;    color:white}  .defaultfont{    font-size:11.0pt;	font-family:""Calibri"",""sans-serif"";    }</style><body class=""defaultfont"">")
        sb.Append(String.Format("<p>Dear {0},</p><br><p>Please be informed that you have tasks that need to follow up.<br>Date: {1:dd-MMM-yyyy}<br><br><br>", data(0).row.item("username").ToString, Today.Date))
        sb.Append("    List of Tasks:</p>  <table border=1 cellspacing=0>    <tr>            <th>Subject</th>      <th>From</th><th>To</th>            <th>Received Date</th><th>Reason</th></tr>")
        For Each n As DataRowView In data
            sb.Append(String.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td></tr>", n.Item("emailsubject"), n.Item("sender"), n.Item("emailto"), n.Item("receiveddate"), n.Item("isvalidremark")))
            n.Item("sendisvalid") = True
        Next
        sb.Append("</table>  <br>  <p>Thank you.<br><br>You can access the system in RD WEB Access by below link:<br>   <a href=""https://sw07e601/RDWeb"">MyTask</a></p><br>")
        Return sb.ToString
    End Function


End Class


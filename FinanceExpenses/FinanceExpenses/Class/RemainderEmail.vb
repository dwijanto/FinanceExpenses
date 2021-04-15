Imports System.Text
Imports System.Net.Mail
Imports System.Net.Mime

Public Class RemainderEmail
    Inherits Email

    Dim AutoTask As Boolean = True
    Dim Limit As Decimal
    Dim Span As Integer
    Dim DS As DataSet
    Private SendToDict As New Dictionary(Of String, String)
    Private Enum EmailTypeEnum
        REGULARCHECK = 1
        MONTHEND = 2
    End Enum

    Dim EmailType As EmailTypeEnum
    Dim SendToName As String

    Public Sub New()
        Dim myParam As New ParamAdapter
        Limit = myParam.getLimit("MplusOneLimit")
        Span = myParam.getOverdueTask("ExpiredDateSpan")
    End Sub

    Public Function getDataSet(limit As Decimal, span As Integer) As DataSet
        Dim Sqlstr = String.Format("with txamount as (select sscemailhdid,sum(amount) as total,max(crcy) as crcy from ssc.sscfinancetx group by sscemailhdid)," &
            " txamountusd as (select sscemailhdid,total,total/pd.nvalue as amountinusd,pd.nvalue,pd.paramname,txamount.crcy " &
            " from txamount left join ssc.paramdt pd on pd.paramname = txamount.crcy)," &
            " data as (select hd.status as hdstatus,ea.status as eastatus,ssc.getstatusname(hd.status) as statusname,tu.amountinusd," &
            " coalesce(latestupdate,receiveddate) as lastaction,hd.receiveddate,ea.latestupdate" &
            " ,hd.*,u5.username as attnusername,u6.username as forwardtousername,atx.stapprover,u1.username as stusername,u1.email as stemail,atx.ndapprover,u2.username as ndusername,u2.email as ndemail,atx.delegatestapprover,u3.username as dstusername,u3.email as dstemail,atx.delegatendapprover,u4.username as dndusername,u4.email as dndemail" &
            " from ssc.sscemailhd hd " &
            " left join ssc.sscemailaction ea on ea.sscemailhdid = hd.id and ea.status = hd.status" &
            " left join ssc.sscapprovaltx atx on atx.sscemailhdid = hd.id " &
            " left join txamountusd tu on tu.sscemailhdid = hd.id" &
            " left join ssc.user u1 on u1.employeenumber = atx.stapprover" &
            " left join ssc.user u2 on u2.employeenumber = atx.ndapprover" &
            " left join ssc.user u3 on u3.employeenumber = atx.delegatestapprover" &
            " left join ssc.user u4 on u4.employeenumber = atx.delegatendapprover" &
            " left join ssc.user u5 on u5.email = hd.attn" &
            " left join ssc.user u6 on u6.email = hd.forwardto" &
            " where (hd.reminderdate <> current_date or hd.reminderstatus isnull) and not hd.status in ({0},{1},{2}) and not (hd.status = {3} and atx.ndapprover isnull) and not(hd.status = {3} and amountinusd < :limit) " &
            " order by hd.status),mycheck as (select id,current_date - lastaction::date as span,data.* from data order by span desc) " &
            " select * from mycheck where span > :span", Int(TaskStatusEnum.STATUS_CANCELLED),
                                                         Int(TaskStatusEnum.STATUS_COMPLETED),
                                                         Int(TaskStatusEnum.STATUS_VALIDATEDBYM2),
                                                         Int(TaskStatusEnum.STATUS_VALIDATEDBYM1))

        DS = New DataSet
        Dim myParams(1) As IDbDataParameter
        myParams(0) = DataAccess.factory.CreateParameter("limit", limit)
        myParams(1) = DataAccess.factory.CreateParameter("span", span)

        DS = DataAccess.GetDataSet(Sqlstr, CommandType.Text, myParams)
        DS.Tables(0).TableName = "Overdue"
        Return DS
    End Function

    Public Function SevenDaysOverdue() As Boolean
        'Get overdue records
        EmailType = EmailTypeEnum.REGULARCHECK
        Dim myret = False

        Dim ds As DataSet = getDataSet(Limit, Span)
        For Each dr In ds.Tables(0).Rows
            If Not IsDBNull(dr.item("reminderdate")) Then
                If dr.item("reminderdate") <> Today.Date Then
                    'Check last sending
                    Dim send As Boolean = False
                    If IsDBNull(dr.item("reminderstatus")) Then
                        If Date.Today.Subtract(dr.item("reminderdate")).TotalDays > 7 Then
                            send = True
                        End If
                    Else
                        If dr.item("reminderstatus") = dr.item("status") Then
                            If Date.Today.Subtract(dr.item("reminderdate")).TotalDays > 7 Then
                                send = True
                            End If
                        Else
                            send = True
                        End If
                    End If

                    If send Then
                        SendEmail(dr)
                        If IsDBNull(dr.item("reminderstatus")) Then
                            dr.item("reminderstatus") = dr.item("status")
                            dr.item("remindercount") = 1
                        Else
                            If dr.item("reminderstatus") = dr.item("status") Then
                                dr.item("remindercount") += dr.item("remindercount")
                            Else
                                dr.item("remindercount") = 1
                                dr.item("reminderstatus") = dr.item("status")
                            End If
                        End If
                        
                        dr.item("reminderdate") = Date.Today
                    End If
                    
                End If
            Else
                SendEmail(dr)
                dr.item("reminderdate") = Date.Today
                dr.item("reminderstatus") = dr.item("status")
                dr.item("remindercount") = 1
            End If
        Next
        myret = savedata()
        Return myret
    End Function

    Private Sub Logg(ByVal mymessage As String)
        If AutoTask Then
            Logger.log(mymessage)
        End If
    End Sub

    Public Function twodaysbeforeendofmonth() As Boolean
        EmailType = EmailTypeEnum.MONTHEND
        Dim myret = False
        Span = -1
        DS = getDataSet(Limit, Span)
        For Each dr In DS.Tables(0).Rows
            SendEmail(dr)
            dr.item("reminderdate") = Date.Today
            dr.item("reminderstatus") = dr.item("status")
        Next
        myret = savedata()
        Return myret
    End Function

    Private Sub SendEmail(dr As Object)
        Select Case dr.item("status")
            Case TaskStatusEnum.STATUS_VALIDATEDBYM1
                Me.sendto = dr.item("ndemail")
                Me.SendToName = dr.item("ndusername")
            Case TaskStatusEnum.STATUS_NEW, TaskStatusEnum.STATUS_REJECTEDBYM1, TaskStatusEnum.STATUS_REJECTEDBYM2, TaskStatusEnum.STATUS_REJECTEDBYFINANCE
                Me.sendto = dr.item("attn")
                If IsDBNull(dr.item("attnusername")) Then
                    Me.SendToName = "User / Approver"
                Else
                    Me.SendToName = dr.item("attnusername")
                End If

            Case TaskStatusEnum.STATUS_RE_SUBMIT, TaskStatusEnum.STATUS_VALIDATEDBYREQUESTER
                Me.sendto = dr.item("stemail")
                Me.SendToName = dr.item("stusername")
            Case TaskStatusEnum.STATUS_FORWARD
                Me.sendto = dr.item("forwardto")
                Me.SendToName = dr.item("forwardtousername")
        End Select

      


        Select Case EmailType
            Case EmailTypeEnum.REGULARCHECK
                Me.subject = "Reminder for tasks overdue 7 days"
                Me.body = getBodyMessageRegularCheck()
            Case EmailTypeEnum.MONTHEND
                Me.subject = "Reminder for tasks outstanding in the last 2 working days each month."
                Me.body = getBodyMessageMonthEnd()
        End Select



       


        Dim htmlView As AlternateView = AlternateView.CreateAlternateViewFromString(String.Format("{0} <br>Or click the Indirect Purchase Invoice Approval icon on your desktop: <br><p> <a href=""https://sw07e601/RDWeb""><img src=cid:myLogo> </a><br></p><p>Indirect Purchase Invoice Approval System Administrator</p></body></html>", Me.body), Nothing, MediaTypeNames.Text.Html)
        Dim logo As New LinkedResource(Application.StartupPath & "\SSC.png")
        logo.ContentId = "myLogo"
        htmlView.LinkedResources.Add(logo)

        Me.htmlView = htmlView
        Me.isBodyHtml = True
        Me.sender = "no-reply@groupeseb.com"
        Dim errormessage As String = String.Empty

        'Check Dictionary, if not available then send else exit sub
        If SendToDict.ContainsKey(Me.sendto.ToLower) Then
            Debug.Print("not send")
        Else
            SendToDict.Add(Me.sendto.ToLower, Me.sendto.ToLower)
            'Control Checking
            'Me.sendto = "dlie@groupeseb.com"

            If Not Me.send(errormessage) Then
                Logger.log(errormessage)
            End If
        End If
    End Sub

    Private Function savedata() As Boolean
        Dim myret As Boolean
        'Save modification 
        'ReminderDate, ReminderStatus, ReminderCount

        Dim ds2 As DataSet = DS.GetChanges
        If Not IsNothing(ds2) Then
            Dim ra As Integer
            Dim mymessage = String.Empty

            Dim mye As New ContentBaseEventArgs(ds2, True, mymessage, ra, True)
            Dim myModel = New EmailModel
            myret = myModel.saveReminderEmail(Me, mye)
            If Not myret Then
                Logg(mye.message)
            End If
        End If
        Return myret
    End Function

    Private Function getBodyMessageRegularCheck() As String
        Dim sb As New StringBuilder
        sb.Append("<!DOCTYPE html><html><head><meta name=""description"" content=""[IndirectPurchaseInvoiceReminder]"" /><meta http-equiv=""Content-Type"" content=""text/html; charset=us-ascii""></head><style>  td,th {padding-left:5px;         padding-right:10px;         text-align:left;  }  th {background-color:red;    color:white}  .defaultfont{    font-size:11.0pt; font-family:""Calibri"",""sans-serif"";    }</style><body class=""defaultfont"">")
        sb.Append(String.Format("<p>Dear {0}, </p><p>Please be reminded that the invoices in the &#8220;Indirect Purchase Platform&#8221;  waiting for you to process was 7 days overdue, please handle them as soon as possible</p>", SendToName))
        sb.Append(" <p>Thank you.<br><br>You can access the system in RD WEB Access by below link:<br>   <a href=""https://sw07e601/RDWeb"">MyTask</a></p><br>")
        Return sb.ToString
    End Function

    Private Function getBodyMessageMonthEnd() As String
        Dim sb As New StringBuilder
        sb.Append("<!DOCTYPE html><html><head><meta name=""description"" content=""[IndirectPurchaseInvoiceReminder]"" /><meta http-equiv=""Content-Type"" content=""text/html; charset=us-ascii""></head><style>  td,th {padding-left:5px;         padding-right:10px;         text-align:left;  }  th {background-color:red;    color:white}  .defaultfont{    font-size:11.0pt; font-family:""Calibri"",""sans-serif"";    }</style><body class=""defaultfont"">")
        sb.Append(String.Format("<p>Dear {0}, </p><p>Please be reminded that the cut-off date for posting invoices in &#8220;Indirect Purchase Platform&#8221; would due in 2 days.</p>", SendToName))
        sb.Append("<p>Invoices not properly approved on or before the last working day current month would be posted in SAP next month and the arrangement of payment may be affected</p>")
        sb.Append("</table>  <br>  <p>Thank you.<br><br>You can access the system in RD WEB Access by below link:<br>   <a href=""https://sw07e601/RDWeb"">MyTask</a></p><br>")
        Return sb.ToString
    End Function



End Class

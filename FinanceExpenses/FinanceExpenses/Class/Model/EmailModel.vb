Imports System.Text

Public Class EmailModel
    Implements IModel

    Dim _criteria As String
    Private _getDelegateTo As String

    Public Property emailsubject As String
    Public Property emailbody As String
    Public Property sender As String
    Public Property sendername As String
    Public Property emailto As String
    Public Property receiveddate As String
    Public Property attn As String
    Public Property status As String
    Public Property sapaccount As String
    Public Property sapcostcenter As String
    Public Property financenumber As String

    Public Property Criteria As String
        Get
            Return _criteria
        End Get
        Set(value As String)
            _criteria = value
        End Set
    End Property

    Private Function GetSqlstr(ByVal criteria) As String
        Dim sb As New StringBuilder
        sb.Append(String.Format("select u.* from {0} u left join ssc.sscemaildt dt on dt.hdid = u.id {1}", tablename, criteria))
        Return sb.ToString
    End Function

    Public Function LoadData(ByRef DS As DataSet) As Boolean Implements IModel.LoadData
        Dim sqlstr = GetSqlstr("")
        DS = DataAccess.GetDataSet(sqlstr, CommandType.Text, Nothing)
        DS.Tables(0).TableName = tablename
        Return True
    End Function

    Public Function LoadData(ByRef DS As DataSet, ByVal Criteria As String)
        Dim myret As Boolean = True
        Dim sb As New StringBuilder
        sb.Append(String.Format("select hd.*, ssc.getstatusname(status) as statusname,date_part('Year',creationdate)::text || '-' || to_char(referencenumber,'FM000000') as refnumber, requester.username as attnname," &
                                " forwarded.username as forwardtoname,stapp.email as stemail,stapp.username as stapprovername,ndapp.email as ndemail,ndapp.username as ndapprovername," &
                                " delegatestapp.email as delegatestemail,delegatestapp.username as delegatestapprovername,delegatendapp.email as delegatendemail," &
                                " delegatendapp.username as delegatendapprovername,case when v.vendorcode isnull then case when status = 1 then null else 'N/A' end else (v.vendorcode::text || ' - ' || v.vendorname::text) end as vendordesc from ssc.sscemailhd hd" &
                                " left join ssc.user requester on requester.email = hd.attn" &
                                " left join ssc.user forwarded on forwarded.email = hd.forwardto" &
                                " left join ssc.sscapprovaltx app on app.sscemailhdid = hd.id" &
                                " left join ssc.user stapp on stapp.employeenumber = app.stapprover" &
                                " left join ssc.user ndapp on ndapp.employeenumber = app.ndapprover" &
                                " left join ssc.user delegatestapp on delegatestapp.employeenumber = app.delegatestapprover" &
                                " left join ssc.vendor v on v.vendorcode = hd.vendorcode" &
                                " left join ssc.user delegatendapp on delegatendapp.employeenumber = app.delegatendapprover {0};", Criteria))
        sb.Append(String.Format("select dt.* from ssc.sscemaildt dt left join ssc.sscemailhd hd on hd.id = dt.hdid  {0};", Criteria))
        sb.Append(String.Format("select ssc.getstatusname(ac.status) as statusname,ac.* from ssc.sscemailaction ac left join ssc.sscemailhd hd on hd.id = ac.sscemailhdid  {0};", Criteria))
        sb.Append(String.Format("select fi.* from ssc.sscfinancetx fi left join ssc.sscemailhd hd on hd.id = fi.sscemailhdid  {0};", Criteria))
        sb.Append(String.Format("select ap.* from ssc.sscapprovaltx ap left join ssc.sscemailhd hd on hd.id = ap.sscemailhdid  {0};", Criteria))
        sb.Append(String.Format("select null as vendorcode,'N/A' as vendorname, 'N/A' as vendordesc union all (select vendorcode,vendorname::text, vendorcode::text || ' - ' || vendorname::text as vendordesc from ssc.vendor where isactive order by vendorname)"))
        DS = DataAccess.GetDataSet(sb.ToString, CommandType.Text, Nothing)

        DS.Tables(0).TableName = "SSCEmailHD"
        DS.Tables(1).TableName = "SSCEmailDT"
        DS.Tables(2).TableName = "ACTION"
        DS.Tables(3).TableName = "FINANCETX"
        DS.Tables(4).TableName = "APPROVALTX"
        DS.Tables(5).TableName = "VENDOR"

        Dim pk(0) As DataColumn
        pk(0) = DS.Tables(0).Columns("id")
        DS.Tables(0).PrimaryKey = pk
        DS.Tables(0).Columns("id").AutoIncrement = True
        DS.Tables(0).Columns("id").AutoIncrementSeed = -1
        DS.Tables(0).Columns("id").AutoIncrementStep = -1

        Dim pk1(0) As DataColumn
        pk1(0) = DS.Tables(1).Columns("id")
        DS.Tables(1).PrimaryKey = pk1
        DS.Tables(1).Columns("id").AutoIncrement = True
        DS.Tables(1).Columns("id").AutoIncrementSeed = -1
        DS.Tables(1).Columns("id").AutoIncrementStep = -1

        Dim pk2(0) As DataColumn
        pk2(0) = DS.Tables(2).Columns("id")
        DS.Tables(2).PrimaryKey = pk2
        DS.Tables(2).Columns("id").AutoIncrement = True
        DS.Tables(2).Columns("id").AutoIncrementSeed = -1
        DS.Tables(2).Columns("id").AutoIncrementStep = -1

        Dim pk3(0) As DataColumn
        pk3(0) = DS.Tables(3).Columns("id")
        DS.Tables(3).PrimaryKey = pk3
        DS.Tables(3).Columns("id").AutoIncrement = True
        DS.Tables(3).Columns("id").AutoIncrementSeed = -1
        DS.Tables(3).Columns("id").AutoIncrementStep = -1

        Dim pk4(0) As DataColumn
        pk4(0) = DS.Tables(4).Columns("id")
        DS.Tables(4).PrimaryKey = pk4
        DS.Tables(4).Columns("id").AutoIncrement = True
        DS.Tables(4).Columns("id").AutoIncrementSeed = -1
        DS.Tables(4).Columns("id").AutoIncrementStep = -1

        'Create Relation HD-DT
        Dim rel As DataRelation
        Dim hcol As DataColumn
        Dim dcol As DataColumn

        hcol = DS.Tables(0).Columns("id") 'id in table header
        dcol = DS.Tables(1).Columns("hdid") 'headerid in table detail
        rel = New DataRelation("hdrel", hcol, dcol)
        DS.Relations.Add(rel)

        hcol = DS.Tables(0).Columns("id") 'id in table header
        dcol = DS.Tables(2).Columns("sscemailhdid") 'headerid in table detail
        rel = New DataRelation("hdrel-action", hcol, dcol)
        DS.Relations.Add(rel)

        hcol = DS.Tables(0).Columns("id") 'id in table header
        dcol = DS.Tables(3).Columns("sscemailhdid") 'headerid in table detail
        rel = New DataRelation("hdrel-financetx", hcol, dcol)
        DS.Relations.Add(rel)

        hcol = DS.Tables(0).Columns("id") 'id in table header
        dcol = DS.Tables(4).Columns("sscemailhdid") 'headerid in table detail
        rel = New DataRelation("hdrel-approvaltx", hcol, dcol)
        DS.Relations.Add(rel)
        Return myret
    End Function

    Function FindLoadData(ByRef DS As DataSet) As Boolean
        Dim myret As Boolean = True
        Try
            Dim sb As New StringBuilder
            sb.Append(String.Format("select ssc.getstatusname(status) as statusname,hd.* from ssc.prhd hd {0};", Criteria))
            DS = DataAccess.GetDataSet(sb.ToString, CommandType.Text, Nothing)
            DS.Tables(0).TableName = tablename
        Catch ex As Exception
            myret = False
        End Try
        Return myret
    End Function

    Function MyTasksLoadData(ByRef DS As DataSet, MyTaskcriteria As String, Historycriteria As String) As Boolean
        Dim myret As Boolean = True
        'Try
        Dim sb As New StringBuilder
        sb.Append(String.Format("select ssc.getstatusname(status) as statusname,date_part('Year',creationdate)::text || '-' || to_char(referencenumber,'FM000000') as refnumber,hd.*,v.vendorcode::text as vendorcodetext,coalesce(v.vendorname,'N/A') as vendorname,ap.stapprover,ap.ndapprover,ap.delegatestapprover,ap.delegatendapprover,u1.username as stapprovername,u1.email as stapproveremail,u2.username as ndapprovername ,u2.email as ndapproveremail,u3.username as attnname from ssc.sscemailhd hd left join ssc.sscapprovaltx ap on ap.sscemailhdid = hd.id left join ssc.user u1 on u1.employeenumber = ap.stapprover left join ssc.user u2 on u2.employeenumber = ap.ndapprover left join ssc.vendor v on v.vendorcode = hd.vendorcode left join ssc.user u3 on u3.email = hd.attn {0} order by hd.id desc;", MyTaskcriteria))
        sb.Append(String.Format("select ssc.getstatusname(status) as statusname,date_part('Year',creationdate)::text || '-' || to_char(referencenumber,'FM000000') as refnumber,hd.*,v.vendorcode::text as vendorcodetext,coalesce(v.vendorname,'N/A') as vendorname,ap.stapprover,ap.ndapprover,ap.delegatestapprover,ap.delegatendapprover,u1.username as stapprovername,u1.email as stapproveremail,u2.username as ndapprovername,u2.email as ndapproveremail from ssc.sscemailhd hd left join ssc.sscapprovaltx ap on ap.sscemailhdid = hd.id left join ssc.user u1 on u1.employeenumber = ap.stapprover left join ssc.user u2 on u2.employeenumber = ap.ndapprover left join ssc.vendor v on v.vendorcode = hd.vendorcode {0} order by hd.id desc;", Historycriteria))
        DS = DataAccess.GetDataSet(sb.ToString, CommandType.Text, Nothing)

        'Catch ex As Exception
        'MessageBox.Show(ex.Message)
        ' myret = False
        'End Try

        Return myret
    End Function

    Function LoadFilter(ByRef DS As DataSet) As Boolean
        Dim myret As Boolean = True
        Try
            Dim sb As New StringBuilder
            sb.Append("select date_part('Year',creationdate)::text || '-' || to_char(referencenumber,'FM000000') as description,referencenumber from ssc.sscemailhd order by referencenumber desc;")
            sb.Append("select distinct invoicenumber as description,invoicenumber from ssc.sscemailhd where not invoicenumber isnull order by invoicenumber asc;")
            sb.Append("select distinct status,ssc.getstatusname(h.status) as description from ssc.sscemailhd h where not status isnull order by status;")
            sb.Append("select distinct u1.employeenumber || ' - ' || u1.username  ||  ' - ' || u1.email  as description , u1.userid, attn,u1.username,u1.employeenumber from ssc.sscemailhd h  left join ssc.user u1 on u1.email = h.attn where not attn isnull order by attn;")
            sb.Append("select distinct u1.employeenumber || ' - ' || u1.username  ||  ' - ' || u1.email  as description , u1.userid, stapprover,u1.username,u1.employeenumber from ssc.sscemailhd h left join ssc.sscapprovaltx a on a.sscemailhdid = h.id left join ssc.user u1 on u1.employeenumber = a.stapprover where not stapprover isnull order by stapprover;")
            sb.Append("select distinct u1.employeenumber || ' - ' || u1.username  ||  ' - ' || u1.email  as description , u1.userid, ndapprover,u1.username,u1.employeenumber from ssc.sscemailhd h left join ssc.sscapprovaltx a on a.sscemailhdid = h.id left join ssc.user u1 on u1.employeenumber = a.ndapprover where not ndapprover isnull order by ndapprover;")
            sb.Append("select distinct h.vendorcode,v.vendorname, h.vendorcode::text || ' - ' || v.vendorname as description  from ssc.sscemailhd h left join ssc.vendor v on v.vendorcode = h.vendorcode where not h.vendorcode isnull order by v.vendorname;")
            DS = DataAccess.GetDataSet(sb.ToString, CommandType.Text, Nothing)
        Catch ex As Exception
            myret = False
        End Try
        Return myret
    End Function


    Function saveExpenses(emailController As EmailController, mye As ContentBaseEventArgs) As Boolean
        Dim myret As Boolean = False
        Dim factory = DataAccess.factory
        Dim mytransaction As IDbTransaction
        Using conn As IDbConnection = factory.CreateConnection
            conn.Open()
            mytransaction = conn.BeginTransaction
            Dim dataadapter = factory.CreateAdapter
            Dim sqlstr As String = String.Empty

            sqlstr = "ssc.sp_insertsscemailhd"
            dataadapter.InsertCommand = factory.CreateCommand(sqlstr, conn)
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "emailsubject", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "emailbody", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "sender", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "sendername", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "emailto", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.DateTime, 0, "receiveddate", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "attn", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.Int32, 0, "status", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "sapaccount", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "sapcostcenter", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "financenumber", DataRowVersion.Current))            
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.Int64, 0, "id", ParameterDirection.InputOutput))
            dataadapter.InsertCommand.CommandType = CommandType.StoredProcedure

            sqlstr = "ssc.sp_updatesscemailhd"
            dataadapter.UpdateCommand = factory.CreateCommand(sqlstr, conn)
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.Int64, 0, "id", DataRowVersion.Original))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "emailsubject", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "emailbody", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "sender", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "sendername", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "emailto", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.DateTime, 0, "receiveddate", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "attn", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.Int32, 0, "status", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "sapaccount", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "sapcostcenter", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "financenumber", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "forwardto", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.Boolean, 0, "isvalid", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.Boolean, 0, "sendisvalid", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "isvalidremark", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "invoicenumber", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.Int32, 0, "doctype", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.Int64, 0, "vendorcode", DataRowVersion.Current))
            dataadapter.UpdateCommand.CommandType = CommandType.StoredProcedure

            sqlstr = "ssc.sp_deletesscemailhd"
            dataadapter.DeleteCommand = factory.CreateCommand(sqlstr, conn)
            dataadapter.DeleteCommand.Parameters.Add(factory.CreateParameter("", DbType.Int64, 0, "id", DataRowVersion.Original))
            dataadapter.DeleteCommand.CommandType = CommandType.StoredProcedure

            dataadapter.InsertCommand.Transaction = mytransaction
            dataadapter.UpdateCommand.Transaction = mytransaction
            dataadapter.DeleteCommand.Transaction = mytransaction

            mye.ra = factory.Update(mye.dataset.Tables("SSCEmailHD"))

            sqlstr = "ssc.sp_insertsscemaildt"
            dataadapter.InsertCommand = factory.CreateCommand(sqlstr, conn)
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.Int64, 0, "hdid", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "attachmentname", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.Int64, 0, "id", ParameterDirection.InputOutput))
            dataadapter.InsertCommand.CommandType = CommandType.StoredProcedure

            sqlstr = "ssc.sp_updatesscemaildt"
            dataadapter.UpdateCommand = factory.CreateCommand(sqlstr, conn)
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.Int64, 0, "id", DataRowVersion.Original))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.Int64, 0, "hdid", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "attachmentname", DataRowVersion.Current))
            dataadapter.UpdateCommand.CommandType = CommandType.StoredProcedure

            sqlstr = "ssc.sp_deletesscemaildt"
            dataadapter.DeleteCommand = factory.CreateCommand(sqlstr, conn)
            dataadapter.DeleteCommand.Parameters.Add(factory.CreateParameter("", DbType.Int64, 0, "id", DataRowVersion.Original))
            dataadapter.DeleteCommand.CommandType = CommandType.StoredProcedure

            dataadapter.InsertCommand.Transaction = mytransaction
            dataadapter.UpdateCommand.Transaction = mytransaction
            dataadapter.DeleteCommand.Transaction = mytransaction

            mye.ra = factory.Update(mye.dataset.Tables("SSCEmailDT"))

            'ACTION
            sqlstr = "ssc.sp_insertsscemailaction"
            dataadapter.InsertCommand = factory.CreateCommand(sqlstr, conn)
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.Int64, 0, "sscemailhdid", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.Int32, 0, "status", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "modifiedby", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.DateTime, 0, "latestupdate", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "remark", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.Int64, 0, "id", ParameterDirection.InputOutput))
            dataadapter.InsertCommand.CommandType = CommandType.StoredProcedure

            sqlstr = "ssc.sp_updatesscemailaction"
            dataadapter.UpdateCommand = factory.CreateCommand(sqlstr, conn)
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.Int64, 0, "id", DataRowVersion.Original))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.Int64, 0, "sscemailhdid", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.Int32, 0, "status", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "modifiedby", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.DateTime, 0, "latestupdate", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "remark", DataRowVersion.Current))
            dataadapter.UpdateCommand.CommandType = CommandType.StoredProcedure

            sqlstr = "ssc.sp_deletesscemailaction"
            dataadapter.DeleteCommand = factory.CreateCommand(sqlstr, conn)
            dataadapter.DeleteCommand.Parameters.Add(factory.CreateParameter("", DbType.Int64, 0, "id", DataRowVersion.Original))
            dataadapter.DeleteCommand.CommandType = CommandType.StoredProcedure

            dataadapter.InsertCommand.Transaction = mytransaction
            dataadapter.UpdateCommand.Transaction = mytransaction
            dataadapter.DeleteCommand.Transaction = mytransaction

            mye.ra = factory.Update(mye.dataset.Tables("ACTION"))

            'FINANCETX
            sqlstr = "ssc.sp_insertsscfinancetx"
            dataadapter.InsertCommand = factory.CreateCommand(sqlstr, conn)
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.Int64, 0, "sscemailhdid", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "glaccount", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "costcenter", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.Decimal, 0, "amount", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "remark", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "crcy", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "family", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.Int64, 0, "id", ParameterDirection.InputOutput))
            dataadapter.InsertCommand.CommandType = CommandType.StoredProcedure

            sqlstr = "ssc.sp_updatesscfinancetx"
            dataadapter.UpdateCommand = factory.CreateCommand(sqlstr, conn)
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.Int64, 0, "id", DataRowVersion.Original))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.Int64, 0, "sscemailhdid", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "glaccount", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "costcenter", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.Decimal, 0, "amount", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "crcy", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "family", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "remark", DataRowVersion.Current))
            dataadapter.UpdateCommand.CommandType = CommandType.StoredProcedure

            sqlstr = "ssc.sp_deletesscfinancetx"
            dataadapter.DeleteCommand = factory.CreateCommand(sqlstr, conn)
            dataadapter.DeleteCommand.Parameters.Add(factory.CreateParameter("", DbType.Int64, 0, "id", DataRowVersion.Original))
            dataadapter.DeleteCommand.CommandType = CommandType.StoredProcedure

            dataadapter.InsertCommand.Transaction = mytransaction
            dataadapter.UpdateCommand.Transaction = mytransaction
            dataadapter.DeleteCommand.Transaction = mytransaction

            'ApprovalTX

            mye.ra = factory.Update(mye.dataset.Tables("FINANCETX"))
            sqlstr = "ssc.sp_insertsscapprovaltx"
            dataadapter.InsertCommand = factory.CreateCommand(sqlstr, conn)
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.Int64, 0, "sscemailhdid", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "stapprover", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "delegatestapprover", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "ndapprover", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "delegatendapprover", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.Int64, 0, "id", ParameterDirection.InputOutput))
            dataadapter.InsertCommand.CommandType = CommandType.StoredProcedure

            sqlstr = "ssc.sp_updatesscapprovaltx"
            dataadapter.UpdateCommand = factory.CreateCommand(sqlstr, conn)
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.Int64, 0, "id", DataRowVersion.Original))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.Int64, 0, "sscemailhdid", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "stapprover", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "delegatestapprover", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "ndapprover", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "delegatendapprover", DataRowVersion.Current))
            dataadapter.UpdateCommand.CommandType = CommandType.StoredProcedure

            sqlstr = "ssc.sp_deletesscapprovaltx"
            dataadapter.DeleteCommand = factory.CreateCommand(sqlstr, conn)
            dataadapter.DeleteCommand.Parameters.Add(factory.CreateParameter("", DbType.Int64, 0, "id", DataRowVersion.Original))
            dataadapter.DeleteCommand.CommandType = CommandType.StoredProcedure

            dataadapter.InsertCommand.Transaction = mytransaction
            dataadapter.UpdateCommand.Transaction = mytransaction
            dataadapter.DeleteCommand.Transaction = mytransaction
            mye.ra = factory.Update(mye.dataset.Tables("APPROVALTX"))


            mytransaction.Commit()
            myret = True
        End Using
        Return myret
    End Function

    Public Function save(obj As Object, mye As ContentBaseEventArgs) As Boolean Implements IModel.save
        Dim myret As Boolean = False
        Dim factory = DataAccess.factory
        Dim mytransaction As IDbTransaction
        Using conn As IDbConnection = factory.CreateConnection
            conn.Open()
            mytransaction = conn.BeginTransaction
            Dim dataadapter = factory.CreateAdapter
            Dim sqlstr As String = String.Empty

            sqlstr = "ssc.sp_insertsscemailhd"
            dataadapter.InsertCommand = factory.CreateCommand(sqlstr, conn)
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "emailsubject", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "emailbody", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "sender", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "sendername", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "emailto", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.DateTime, 0, "receiveddate", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "attn", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.Int32, 0, "status", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "sapaccount", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "sapcostcenter", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "financenumber", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.Int64, 0, "id", ParameterDirection.InputOutput))
            dataadapter.InsertCommand.CommandType = CommandType.StoredProcedure

            sqlstr = "ssc.sp_updatesscemailhd"
            dataadapter.UpdateCommand = factory.CreateCommand(sqlstr, conn)
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.Int64, 0, "id", DataRowVersion.Original))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "emailsubject", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "emailbody", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "sender", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "sendername", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "emailto", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.DateTime, 0, "receiveddate", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "attn", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.Int32, 0, "status", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "sapaccount", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "sapcostcenter", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "financenumber", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "forwardto", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.Boolean, 0, "isvalid", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.Boolean, 0, "sendisvalid", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "isvalidremark", DataRowVersion.Current))
            dataadapter.UpdateCommand.CommandType = CommandType.StoredProcedure

            sqlstr = "ssc.sp_deletesscemailhd"
            dataadapter.DeleteCommand = factory.CreateCommand(sqlstr, conn)
            dataadapter.DeleteCommand.Parameters.Add(factory.CreateParameter("", DbType.Int64, 0, "id", DataRowVersion.Original))
            dataadapter.DeleteCommand.CommandType = CommandType.StoredProcedure

            dataadapter.InsertCommand.Transaction = mytransaction
            dataadapter.UpdateCommand.Transaction = mytransaction
            dataadapter.DeleteCommand.Transaction = mytransaction

            mye.ra = factory.Update(mye.dataset.Tables("SSCEmailHD"))

            sqlstr = "ssc.sp_insertsscemaildt"
            dataadapter.InsertCommand = factory.CreateCommand(sqlstr, conn)
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.Int64, 0, "hdid", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "attachmentname", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.Int64, 0, "id", ParameterDirection.InputOutput))
            dataadapter.InsertCommand.CommandType = CommandType.StoredProcedure

            sqlstr = "ssc.sp_updatesscemaildt"
            dataadapter.UpdateCommand = factory.CreateCommand(sqlstr, conn)
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.Int64, 0, "id", DataRowVersion.Original))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.Int64, 0, "hdid", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "attachmentname", DataRowVersion.Current))
            dataadapter.UpdateCommand.CommandType = CommandType.StoredProcedure

            sqlstr = "ssc.sp_deletesscemaildt"
            dataadapter.DeleteCommand = factory.CreateCommand(sqlstr, conn)
            dataadapter.DeleteCommand.Parameters.Add(factory.CreateParameter("", DbType.Int64, 0, "id", DataRowVersion.Original))
            dataadapter.DeleteCommand.CommandType = CommandType.StoredProcedure

            dataadapter.InsertCommand.Transaction = mytransaction
            dataadapter.UpdateCommand.Transaction = mytransaction
            dataadapter.DeleteCommand.Transaction = mytransaction

            mye.ra = factory.Update(mye.dataset.Tables("SSCEmailDT"))

            sqlstr = "ssc.sp_updatesscparameterdt"
            dataadapter.UpdateCommand = factory.CreateCommand(sqlstr, conn)
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.Int32, 0, "paramdtid", DataRowVersion.Original))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.Int64, 0, "paramhdid", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "paramname", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "cvalue", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.Date, 0, "dvalue", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.Int32, 0, "ivalue", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.Decimal, 0, "nvalue", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.DateTime, 0, "ts", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.Boolean, 0, "bvalue", DataRowVersion.Current))
            dataadapter.UpdateCommand.CommandType = CommandType.StoredProcedure
            mye.ra = factory.Update(mye.dataset.Tables("Param"))

            mytransaction.Commit()
            myret = True
        End Using
        Return myret
    End Function


    Public Function saveHD(obj As Object, mye As ContentBaseEventArgs) As Boolean
        Dim myret As Boolean = False
        Dim factory = DataAccess.factory
        Dim mytransaction As IDbTransaction
        Using conn As IDbConnection = factory.CreateConnection
            conn.Open()
            mytransaction = conn.BeginTransaction
            Dim dataadapter = factory.CreateAdapter
            Dim sqlstr As String = String.Empty

            sqlstr = "ssc.sp_insertsscemailhd"
            dataadapter.InsertCommand = factory.CreateCommand(sqlstr, conn)
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "emailsubject", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "emailbody", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "sender", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "sendername", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "emailto", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.DateTime, 0, "receiveddate", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "attn", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.Int32, 0, "status", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "sapaccount", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "sapcostcenter", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "financenumber", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.Int64, 0, "id", ParameterDirection.InputOutput))
            dataadapter.InsertCommand.CommandType = CommandType.StoredProcedure

            sqlstr = "ssc.sp_updatesscemailhd"
            dataadapter.UpdateCommand = factory.CreateCommand(sqlstr, conn)
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.Int64, 0, "id", DataRowVersion.Original))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "emailsubject", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "emailbody", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "sender", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "sendername", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "emailto", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.DateTime, 0, "receiveddate", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "attn", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.Int32, 0, "status", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "sapaccount", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "sapcostcenter", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "financenumber", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "forwardto", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.Boolean, 0, "isvalid", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.Boolean, 0, "sendisvalid", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "isvalidremark", DataRowVersion.Current))
            dataadapter.UpdateCommand.CommandType = CommandType.StoredProcedure

            sqlstr = "ssc.sp_deletesscemailhd"
            dataadapter.DeleteCommand = factory.CreateCommand(sqlstr, conn)
            dataadapter.DeleteCommand.Parameters.Add(factory.CreateParameter("", DbType.Int64, 0, "id", DataRowVersion.Original))
            dataadapter.DeleteCommand.CommandType = CommandType.StoredProcedure

            dataadapter.InsertCommand.Transaction = mytransaction
            dataadapter.UpdateCommand.Transaction = mytransaction
            dataadapter.DeleteCommand.Transaction = mytransaction

            mye.ra = factory.Update(mye.dataset.Tables("SSCEmailHD"))

            mytransaction.Commit()
            myret = True
        End Using
        Return myret
    End Function


    Public ReadOnly Property sortField As String Implements IModel.sortField
        Get
            Return "id"
        End Get
    End Property

    Public ReadOnly Property tablename As String Implements IModel.tablename
        Get
            Return "ssc.sscemailhd"
        End Get
    End Property


  
 

 

End Class

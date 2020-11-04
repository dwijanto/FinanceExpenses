Imports System.Text

Public Class EmailModel
    Implements IModel

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

    Private Function GetSqlstr(ByVal criteria) As String
        Dim sb As New StringBuilder
        sb.Append(String.Format("select u.* from {0} u left join ssc.sscemaildt dt on dt.hdid = u.id {1} ", tablename, criteria))
        Return sb.ToString
    End Function

    Public Function LoadData(ByRef DS As DataSet) As Boolean Implements IModel.LoadData
        Dim sqlstr = GetSqlstr("")
        DS = DataAccess.GetDataSet(sqlstr, CommandType.Text, Nothing)
        DS.Tables(0).TableName = tablename
        Return True
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
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.DateTime, 0, "emailto", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "receiveddate", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "attn", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "status", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "sapaccount", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "sapcostcenter", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "financenumber", DataRowVersion.Current))
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

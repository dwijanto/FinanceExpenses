Imports System.Text
Public Class DelegateModel
    Implements IModel



    Public Property staffocde As String
    Public Property startdate As String
    Public Property enddate As String
    Public Property delegateto As String
    Public Property isactive As String

    Public ReadOnly Property FilterField
        Get
            Return "[staffcode] like '%{0}%' "
        End Get
    End Property

    Public ReadOnly Property TableName As String Implements IModel.tablename
        Get
            Return "ssc.sscdelegate"
        End Get
    End Property

    Public ReadOnly Property SortField As String Implements IModel.sortField
        Get
            Return "startdate"
        End Get
    End Property


    Private Function GetSqlstr(ByVal criteria) As String
        Dim sb As New StringBuilder
        sb.Append(String.Format("select u.* from {0} u {1} ", TableName, criteria))
        Return sb.ToString
    End Function

    Public Function LoadData1(ByRef DS As DataSet) As Boolean Implements IModel.LoadData
        Return False
    End Function

    Public Function LoadData(ByRef DS As DataSet, ByVal criteria As String) As Boolean
        Dim sqlstr = GetSqlstr(criteria)
        DS = DataAccess.GetDataSet(sqlstr, CommandType.Text, Nothing)
        DS.Tables(0).TableName = TableName
        Return True
    End Function

    Public Function save(ByVal obj As Object, ByVal mye As ContentBaseEventArgs) As Boolean Implements IModel.save
        Dim myret As Boolean = False
        Dim factory = DataAccess.factory
        Dim mytransaction As IDbTransaction
        Using conn As IDbConnection = factory.CreateConnection
            conn.Open()
            mytransaction = conn.BeginTransaction
            Dim dataadapter = factory.CreateAdapter
            Dim sqlstr As String = String.Empty

            sqlstr = "ssc.sp_insertdelegate"
            dataadapter.InsertCommand = factory.CreateCommand(sqlstr, conn)
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "staffcode", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.Date, 0, "startdate", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.Date, 0, "enddate", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "delegateto", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.Boolean, 0, "isactive", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.Int64, 0, "id", ParameterDirection.InputOutput))

            dataadapter.InsertCommand.CommandType = CommandType.StoredProcedure

            sqlstr = "ssc.sp_updatedelegate"
            dataadapter.UpdateCommand = factory.CreateCommand(sqlstr, conn)
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.Int64, 0, "id", DataRowVersion.Original))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "staffcode", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.Date, 0, "startdate", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.Date, 0, "enddate", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "delegateto", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.Boolean, 0, "isactive", DataRowVersion.Current))
            dataadapter.UpdateCommand.CommandType = CommandType.StoredProcedure

            sqlstr = "ssc.sp_deletedelegate"
            dataadapter.DeleteCommand = factory.CreateCommand(sqlstr, conn)
            dataadapter.DeleteCommand.Parameters.Add(factory.CreateParameter("", DbType.Int64, 0, "id", DataRowVersion.Original))
            dataadapter.DeleteCommand.CommandType = CommandType.StoredProcedure

            dataadapter.InsertCommand.Transaction = mytransaction
            dataadapter.UpdateCommand.Transaction = mytransaction
            dataadapter.DeleteCommand.Transaction = mytransaction

            mye.ra = factory.Update(mye.dataset.Tables(TableName))
            mytransaction.Commit()
            myret = True
        End Using
        Return myret
    End Function



End Class

﻿Imports System.Text

Public Class GetEmailModel
    Implements IModel

    Public Property cmmf As String
    Public Property localdescription As String
    Public Property commercialcode As String
    Public Property brand As String
    Public Property pricehkd As String
    Public Property priceusd As String

    Public ReadOnly Property FilterField
        Get
            Return "[cmmfstring] like '%{0}%' or [localdescription] like '%{0}%' or [commercialcode] like '%{0}%'"
        End Get
    End Property

    Public ReadOnly Property TableName As String Implements IModel.tablename
        Get
            Return "ssc.cmmf"
        End Get
    End Property

    Public ReadOnly Property SortField As String Implements IModel.sortField
        Get
            Return "cmmf"
        End Get
    End Property


    Private Function GetSqlstr(ByVal criteria) As String
        Dim sb As New StringBuilder
        sb.Append(String.Format("select u.*,u.cmmf::character varying as cmmfstring,coalesce(cp.pricehkd,0) as price,coalesce(cp.priceusd,0) as priceusd from {0} {1} u left join ssc.cmmfprice cp on cp.cmmf = u.cmmf ", TableName, criteria))
        Return sb.ToString
    End Function

    Public Function LoadData1(ByRef DS As DataSet) As Boolean Implements IModel.LoadData
        Return False
    End Function


    Public Function GetCMMFBS() As BindingSource
        Dim ds As New DataSet
        Dim CMMFBS As New BindingSource
        Dim sqlstr = GetSqlstr("")
        ds = DataAccess.GetDataSet(sqlstr, CommandType.Text, Nothing)
        ds.Tables(0).TableName = TableName
        CMMFBS.DataSource = ds.Tables(0)
        Return CMMFBS
    End Function

    Public Function LoadData(ByRef DS As DataSet, ByVal criteria As String) As Boolean
        Dim sqlstr = GetSqlstr("")
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

            sqlstr = "sp_insertsscemailhd"
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
            'dataadapter.InsertCommand.Parameters.Add("", NpgsqlTypes.NpgsqlDbType.Bigint, 0, "docemailhdid").Direction = ParameterDirection.InputOutput
            dataadapter.InsertCommand.CommandType = CommandType.StoredProcedure


            'sqlstr = "sp_updatedocemailhd"
            'dataadapter.UpdateCommand = New NpgsqlCommand(sqlstr, conn)
            'dataadapter.UpdateCommand.Parameters.Add("", NpgsqlTypes.NpgsqlDbType.Varchar, 0, "sender").SourceVersion = DataRowVersion.Current
            'dataadapter.UpdateCommand.Parameters.Add("", NpgsqlTypes.NpgsqlDbType.Varchar, 0, "sendername").SourceVersion = DataRowVersion.Current
            'dataadapter.UpdateCommand.Parameters.Add("", NpgsqlTypes.NpgsqlDbType.Varchar, 0, "foldername").SourceVersion = DataRowVersion.Current
            'dataadapter.UpdateCommand.Parameters.Add("", NpgsqlTypes.NpgsqlDbType.Timestamp, 0, "receiveddate").SourceVersion = DataRowVersion.Current
            'dataadapter.UpdateCommand.Parameters.Add("", NpgsqlTypes.NpgsqlDbType.Bigint, 0, "docemailhdid").SourceVersion = DataRowVersion.Original
            'dataadapter.UpdateCommand.CommandType = CommandType.StoredProcedure


            'dataadapter.InsertCommand.Transaction = mytransaction
            'dataadapter.UpdateCommand.Transaction = mytransaction
            'mye.ra = dataadapter.Update(mye.dataset.Tables(1))


            'sqlstr = "sp_insertdocemaildt"
            'dataadapter.InsertCommand = New NpgsqlCommand(sqlstr, conn)
            'dataadapter.InsertCommand.Parameters.Add("", NpgsqlTypes.NpgsqlDbType.Bigint, 0, "docemailhdid").SourceVersion = DataRowVersion.Current
            'dataadapter.InsertCommand.Parameters.Add("", NpgsqlTypes.NpgsqlDbType.Varchar, 0, "docemaildtname").SourceVersion = DataRowVersion.Current
            'dataadapter.InsertCommand.CommandType = CommandType.StoredProcedure

            'dataadapter.InsertCommand.Transaction = mytransaction
            'mye.ra = dataadapter.Update(mye.dataset.Tables(2))
            myret = True
        End Using
        Return myret
    End Function
    Public Function save1(ByVal obj As Object, ByVal mye As ContentBaseEventArgs) As Boolean 'Implements IModel.save
        Dim myret As Boolean = False
        Dim factory = DataAccess.factory
        Dim mytransaction As IDbTransaction
        Using conn As IDbConnection = factory.CreateConnection
            conn.Open()
            mytransaction = conn.BeginTransaction
            Dim dataadapter = factory.CreateAdapter
            Dim sqlstr As String = String.Empty

            sqlstr = "ssc.sp_insertcmmf"

            dataadapter.InsertCommand = factory.CreateCommand(sqlstr, conn)
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.Int64, 0, "cmmf", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "commercialcode", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "localdescription", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "brand", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.Decimal, 0, "price", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.Decimal, 0, "priceusd", DataRowVersion.Current))

            dataadapter.InsertCommand.CommandType = CommandType.StoredProcedure

            sqlstr = "ssc.sp_updatecmmf"
            dataadapter.UpdateCommand = factory.CreateCommand(sqlstr, conn)
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.Int64, 0, "cmmf", DataRowVersion.Original))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.Int64, 0, "cmmf", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "commercialcode", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "localdescription", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "brand", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.Decimal, 0, "price", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.Decimal, 0, "priceusd", DataRowVersion.Current))
            dataadapter.UpdateCommand.CommandType = CommandType.StoredProcedure

            sqlstr = "ssc.sp_deletecmmf"
            dataadapter.DeleteCommand = factory.CreateCommand(sqlstr, conn)
            dataadapter.DeleteCommand.Parameters.Add(factory.CreateParameter("", DbType.Int64, 0, "cmmf", DataRowVersion.Original))
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

    'Function DocEmailTx(ByVal obj As Object, ByRef mye As ContentBaseEventArgs) As Boolean
    '    Dim myret As Boolean = False

    '    Dim sqlstr As String = String.Empty
    '    Dim DataAdapter As New NpgsqlDataAdapter
    '    Try
    '        Using conn As New NpgsqlConnection(Connectionstring)
    '            Try
    '                'select cmmf,sorg,plant,materialdesc,commref,familylv1,familylv2,sbu,brandid,rri,range from materialmaster
    '                conn.Open()
    '                myTransaction = conn.BeginTransaction

    '                sqlstr = "sp_insertdocemailhd"
    '                DataAdapter.InsertCommand = New NpgsqlCommand(sqlstr, conn)

    '                DataAdapter.InsertCommand.Parameters.Add("", NpgsqlTypes.NpgsqlDbType.Varchar, 0, "docemailname").SourceVersion = DataRowVersion.Current
    '                DataAdapter.InsertCommand.Parameters.Add("", NpgsqlTypes.NpgsqlDbType.Integer, 0, "docemailtype").SourceVersion = DataRowVersion.Current
    '                DataAdapter.InsertCommand.Parameters.Add("", NpgsqlTypes.NpgsqlDbType.Varchar, 0, "sender").SourceVersion = DataRowVersion.Current
    '                DataAdapter.InsertCommand.Parameters.Add("", NpgsqlTypes.NpgsqlDbType.Varchar, 0, "sendername").SourceVersion = DataRowVersion.Current
    '                DataAdapter.InsertCommand.Parameters.Add("", NpgsqlTypes.NpgsqlDbType.Varchar, 0, "foldername").SourceVersion = DataRowVersion.Current
    '                DataAdapter.InsertCommand.Parameters.Add("", NpgsqlTypes.NpgsqlDbType.Timestamp, 0, "receiveddate").SourceVersion = DataRowVersion.Current
    '                DataAdapter.InsertCommand.Parameters.Add("", NpgsqlTypes.NpgsqlDbType.Bigint, 0, "docemailhdid").Direction = ParameterDirection.InputOutput
    '                DataAdapter.InsertCommand.CommandType = CommandType.StoredProcedure


    '                sqlstr = "sp_updatedocemailhd"
    '                DataAdapter.UpdateCommand = New NpgsqlCommand(sqlstr, conn)
    '                DataAdapter.UpdateCommand.Parameters.Add("", NpgsqlTypes.NpgsqlDbType.Varchar, 0, "sender").SourceVersion = DataRowVersion.Current
    '                DataAdapter.UpdateCommand.Parameters.Add("", NpgsqlTypes.NpgsqlDbType.Varchar, 0, "sendername").SourceVersion = DataRowVersion.Current
    '                DataAdapter.UpdateCommand.Parameters.Add("", NpgsqlTypes.NpgsqlDbType.Varchar, 0, "foldername").SourceVersion = DataRowVersion.Current
    '                DataAdapter.UpdateCommand.Parameters.Add("", NpgsqlTypes.NpgsqlDbType.Timestamp, 0, "receiveddate").SourceVersion = DataRowVersion.Current
    '                DataAdapter.UpdateCommand.Parameters.Add("", NpgsqlTypes.NpgsqlDbType.Bigint, 0, "docemailhdid").SourceVersion = DataRowVersion.Original
    '                DataAdapter.UpdateCommand.CommandType = CommandType.StoredProcedure


    '                DataAdapter.InsertCommand.Transaction = myTransaction
    '                DataAdapter.UpdateCommand.Transaction = myTransaction
    '                mye.ra = DataAdapter.Update(mye.dataset.Tables(1))


    '                sqlstr = "sp_insertdocemaildt"
    '                DataAdapter.InsertCommand = New NpgsqlCommand(sqlstr, conn)
    '                DataAdapter.InsertCommand.Parameters.Add("", NpgsqlTypes.NpgsqlDbType.Bigint, 0, "docemailhdid").SourceVersion = DataRowVersion.Current
    '                DataAdapter.InsertCommand.Parameters.Add("", NpgsqlTypes.NpgsqlDbType.Varchar, 0, "docemaildtname").SourceVersion = DataRowVersion.Current
    '                DataAdapter.InsertCommand.CommandType = CommandType.StoredProcedure

    '                DataAdapter.InsertCommand.Transaction = myTransaction
    '                mye.ra = DataAdapter.Update(mye.dataset.Tables(2))


    '                myTransaction.Commit()
    '                myret = True
    '            Catch ex As Exception
    '                myTransaction.Rollback()
    '                mye.message = ex.Message
    '                Return False
    '            End Try
    '        End Using

    '    Catch ex As Exception
    '        mye.message = ex.Message
    '    End Try


    '    Return myret

    'End Function
End Class

Imports Npgsql
Imports System.Text

Public Class ParamAdapter
    Public Shared myInstance As ParamAdapter
    Dim factory = DataAccess.factory

    Public DS As DataSet
    Public BS As BindingSource
    Public BS2 As BindingSource
    Public BS3 As BindingSource
    Public BS4 As BindingSource
    Public BS5 As BindingSource
    Public BS6 As BindingSource
    Public BS7 As BindingSource
    Private ErrMessage As String = String.Empty

    Public ReadOnly Property getErrorMessage As String
        Get
            Return ErrMessage
        End Get
    End Property



    Public Shared Function getInstance() As ParamAdapter
        If myInstance Is Nothing Then
            myInstance = New ParamAdapter
        End If
        Return myInstance
    End Function

    Public Function GetParentid(ByVal paramName As String) As String
        Dim sqlstr = String.Format("select paramhdid from ssc.paramhd where paramname =:paramname", paramName)
        Dim myresult As String = String.Empty
        Dim myparam(0) As System.Data.IDbDataParameter
        myparam(0) = factory.CreateParameter("paramname", paramName)
        myresult = DataAccess.ExecuteScalar(sqlstr, CommandType.Text, myparam)
        Return myresult
    End Function
    Function getLimit(ByVal paramName) As Decimal
        Dim sqlstr = String.Format("select dt.nvalue,ph.* from ssc.paramdt dt left join ssc.paramhd ph on ph.paramhdid = dt.paramhdid where dt.paramname =:paramname and ph.paramname = 'DirectorLimit'", paramName)
        Dim myresult As String = String.Empty
        Dim myparam(0) As System.Data.IDbDataParameter
        myparam(0) = factory.CreateParameter("paramname", paramName)
        myresult = DataAccess.ExecuteScalar(sqlstr, CommandType.Text, myparam)
        Return myresult
    End Function
    Public Function GetApprovalName(ByVal paramdtId As Integer) As ApprovalInfo
        Dim myApprovalInfo As ApprovalInfo = Nothing
        Try
            Dim sqlstr = String.Format("select u.* from ssc.paramdt pd " &
                                   " left join ssc.user u on u.userid = pd.cvalue where pd.paramdtid =:id;", paramdtId)
            Dim myresult As String = String.Empty
            Dim myparam(0) As System.Data.IDbDataParameter
            myparam(0) = factory.CreateParameter("id", paramdtId)
            'myresult = DataAccess.ExecuteScalar(sqlstr, CommandType.Text, myparam)
            DS = DataAccess.GetDataSet(sqlstr, CommandType.Text, myparam)
            If DS.Tables(0).Rows.Count > 0 Then
                myApprovalInfo = New ApprovalInfo With {.ID = DS.Tables(0).Rows(0).Item("userid"),
                                                    .Name = DS.Tables(0).Rows(0).Item("username"),
                                                    .Email = DS.Tables(0).Rows(0).Item("email")
                                                    }
            End If
            
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
        Return myApprovalInfo
    End Function

    Public Function GetParamDetailCValue(ByVal paramname As String) As String
        Dim sqlstr = "select dt.cvalue from ssc.paramdt dt where dt.paramname =:paramname"
        Dim myresult As String = String.Empty
        Dim myparam(0) As System.Data.IDbDataParameter
        myparam(0) = factory.CreateParameter("paramname", paramname)
        myresult = DataAccess.ExecuteScalar(sqlstr, CommandType.Text, myparam)
        Return myresult
    End Function
    Public Function GetEmail(ByVal paramname As String) As String

        Dim sb As New StringBuilder
        Try
            Dim sqlstr = String.Format("select dt.cvalue from ssc.paramdt dt" &
                                       " left join ssc.paramhd ph on ph.paramhdid = dt.paramhdid " &
                                       " where ph.paramname = 'SupplyChainEmail' and dt.paramname =:paramname ", paramname)
            Dim myresult As String = String.Empty
            Dim myparam(0) As System.Data.IDbDataParameter
            myparam(0) = factory.CreateParameter("paramname", paramname)

            DS = DataAccess.GetDataSet(sqlstr, CommandType.Text, myparam)

            If DS.Tables(0).Rows.Count > 0 Then
                For Each drv As DataRow In DS.Tables(0).Rows
                    If sb.Length > 0 Then
                        sb.Append(";")
                    End If
                    sb.Append(drv.Item("cvalue"))
                Next
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
        Return sb.ToString
    End Function

    Public Function LoadData()
        Dim sb As New StringBuilder
        Dim myret As Boolean = False
        sb.Append(String.Format("select pd.* from ssc.paramdt pd  where pd.paramname = :emaillastreceived order by paramdtid;"))
        sb.Append(String.Format("select pd.* from ssc.paramdt pd  where pd.paramname = :url order by paramdtid;"))
        sb.Append(String.Format("select pd.* from ssc.paramdt pd  where pd.paramname = :username order by paramdtid; "))
        sb.Append(String.Format("select pd.* from ssc.paramdt pd  where pd.paramname = :password order by paramdtid; "))
        sb.Append(String.Format("select pd.* from ssc.paramdt pd  where pd.paramname = :basefolder order by paramdtid; "))
        sb.Append(String.Format("select pd.* from ssc.paramdt pd  where pd.paramname = :mailbox order by paramdtid; "))
        sb.Append(String.Format("select pd.* from ssc.paramdt pd  where pd.paramname = :notvalidemail order by paramdtid; "))
        Dim sqlstr = sb.ToString
        DS = New DataSet
        BS = New BindingSource
        BS2 = New BindingSource
        BS3 = New BindingSource
        BS4 = New BindingSource
        BS5 = New BindingSource
        BS6 = New BindingSource
        BS7 = New BindingSource

        Dim myparam(6) As System.Data.IDbDataParameter
        myparam(0) = factory.CreateParameter("emaillastreceived", "emaillastreceived")
        myparam(1) = factory.CreateParameter("url", "url")
        myparam(2) = factory.CreateParameter("username", "username")
        myparam(3) = factory.CreateParameter("password", "password")
        myparam(4) = factory.CreateParameter("basefolder", "basefolder")
        myparam(5) = factory.CreateParameter("mailbox", "mailbox")
        myparam(6) = factory.CreateParameter("notvalidemail", "NotValidEmail")
        Try
            DS = DataAccess.GetDataSet(sqlstr, CommandType.Text, myparam)
            Dim pk(0) As DataColumn
            pk(0) = DS.Tables(0).Columns("paramdtid")
            DS.Tables(0).PrimaryKey = pk
            BS.DataSource = DS.Tables(0)
            'DS.Tables(0).Columns("paramdtid").AutoIncrement = True
            'DS.Tables(0).Columns("paramdtid").AutoIncrementSeed = -1
            'DS.Tables(0).Columns("paramdtid").AutoIncrementStep = -1

            Dim pk2(0) As DataColumn
            pk2(0) = DS.Tables(1).Columns("paramdtid")
            DS.Tables(1).PrimaryKey = pk2
            BS2.DataSource = DS.Tables(1)
            'DS.Tables(1).Columns("paramdtid").AutoIncrement = True
            'DS.Tables(1).Columns("paramdtid").AutoIncrementSeed = -1
            'DS.Tables(1).Columns("paramdtid").AutoIncrementStep = -1

            Dim pk3(0) As DataColumn
            pk3(0) = DS.Tables(2).Columns("paramdtid")
            DS.Tables(2).PrimaryKey = pk3
            BS3.DataSource = DS.Tables(2)
            'DS.Tables(2).Columns("paramdtid").AutoIncrement = True
            'DS.Tables(2).Columns("paramdtid").AutoIncrementSeed = -1
            'DS.Tables(2).Columns("paramdtid").AutoIncrementStep = -1

            Dim pk4(0) As DataColumn
            pk4(0) = DS.Tables(3).Columns("paramdtid")
            DS.Tables(3).PrimaryKey = pk4
            BS4.DataSource = DS.Tables(3)
            'DS.Tables(3).Columns("paramdtid").AutoIncrement = True
            'DS.Tables(3).Columns("paramdtid").AutoIncrementSeed = -1
            'DS.Tables(3).Columns("paramdtid").AutoIncrementStep = -1

            Dim pk5(0) As DataColumn
            pk5(0) = DS.Tables(4).Columns("paramdtid")
            DS.Tables(4).PrimaryKey = pk5
            BS5.DataSource = DS.Tables(4)
            'DS.Tables(4).Columns("paramdtid").AutoIncrement = True
            'DS.Tables(4).Columns("paramdtid").AutoIncrementSeed = -1
            'DS.Tables(4).Columns("paramdtid").AutoIncrementStep = -1
            
            Dim pk6(0) As DataColumn
            pk6(0) = DS.Tables(5).Columns("paramdtid")
            DS.Tables(5).PrimaryKey = pk6
            BS6.DataSource = DS.Tables(5)
            'DS.Tables(5).Columns("paramdtid").AutoIncrement = True
            'DS.Tables(5).Columns("paramdtid").AutoIncrementSeed = -1
            'DS.Tables(5).Columns("paramdtid").AutoIncrementStep = -1

            Dim pk7(0) As DataColumn
            pk7(0) = DS.Tables(6).Columns("paramdtid")
            DS.Tables(6).PrimaryKey = pk7
            BS7.DataSource = DS.Tables(6)
            'DS.Tables(6).Columns("paramdtid").AutoIncrement = True
            'DS.Tables(6).Columns("paramdtid").AutoIncrementSeed = -1
            'DS.Tables(6).Columns("paramdtid").AutoIncrementStep = -1
            myret = True
        Catch ex As Exception
            ErrMessage = ex.Message
        End Try
        Return myret
    End Function

    Public Function save() As Boolean
        Dim myret As Boolean = False
        BS.EndEdit()
        BS2.EndEdit()
        BS3.EndEdit()
        BS4.EndEdit()
        BS5.EndEdit()
        BS6.EndEdit()
        BS7.EndEdit()

        Dim ds2 As DataSet = DS.GetChanges
        If Not IsNothing(ds2) Then
            Dim mymessage As String = String.Empty
            Dim ra As Integer
            Dim mye As New ContentBaseEventArgs(ds2, True, mymessage, ra, True)
            Try
                If save(mye) Then
                    DS.Merge(ds2)
                    DS.AcceptChanges()
                    MessageBox.Show("Saved.")
                    myret = True
                End If
            Catch ex As Exception
                MessageBox.Show(ex.Message)
                DS.Merge(ds2)
            End Try
        End If
        Return myret
    End Function

    Public Function Save(mye As ContentBaseEventArgs) As Boolean
        Dim myret As Boolean = False
        Dim factory = DataAccess.factory
        Dim mytransaction As IDbTransaction
        Using conn As IDbConnection = factory.CreateConnection
            conn.Open()
            mytransaction = conn.BeginTransaction
            Dim dataadapter = factory.CreateAdapter
            Dim sqlstr As String

            sqlstr = "ssc.sp_deleteparameter"
            dataadapter.DeleteCommand = factory.CreateCommand(sqlstr, conn)
            dataadapter.DeleteCommand.Parameters.Add(factory.CreateParameter("", NpgsqlTypes.NpgsqlDbType.Bigint, 0, "paramdtid"))
            dataadapter.DeleteCommand.CommandType = CommandType.StoredProcedure

            sqlstr = "ssc.sp_insertparameter"

            dataadapter.InsertCommand = factory.CreateCommand(sqlstr, conn)
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.Int64, 0, "paramhdid", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "paramname", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "cvalue", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.Date, 0, "dvalue", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.Int32, 0, "ivalue", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.Decimal, 0, "nvalue", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.DateTime, 0, "ts", DataRowVersion.Current))
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("", DbType.Int64, 0, "paramdtid", ParameterDirection.InputOutput))
            dataadapter.InsertCommand.CommandType = CommandType.StoredProcedure

            sqlstr = "ssc.sp_updateparameter"
            dataadapter.UpdateCommand = factory.CreateCommand(sqlstr, conn)
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.Int64, 0, "paramdtid", DataRowVersion.Original))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.Int64, 0, "paramhdid", DataRowVersion.Original))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "paramname", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.String, 0, "cvalue", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.Date, 0, "dvalue", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.Int32, 0, "ivalue", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.Decimal, 0, "nvalue", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.DateTime, 0, "ts", DataRowVersion.Current))
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("", DbType.Boolean, 0, "bvalue", DataRowVersion.Current))
            dataadapter.UpdateCommand.CommandType = CommandType.StoredProcedure

            dataadapter.InsertCommand.Transaction = mytransaction
            dataadapter.UpdateCommand.Transaction = mytransaction
            dataadapter.DeleteCommand.Transaction = mytransaction

            For Each dt In mye.dataset.Tables
                mye.ra = factory.Update(dt)
            Next
            'mye.ra = factory.Update(mye.dataset.Tables(0))
            'mye.ra = factory.Update(mye.dataset.Tables(1))
            'mye.ra = factory.Update(mye.dataset.Tables(2))
            mytransaction.Commit()
            myret = True
        End Using
        Return myret
    End Function




End Class
Public Class ApprovalInfo
    Public Property ID
    Public Property Name
    Public Property Email
End Class


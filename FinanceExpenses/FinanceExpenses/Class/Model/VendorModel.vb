'-- Table: vendor

'-- DROP TABLE vendor;

'CREATE TABLE vendor
'(
'  vendorname character varying,
'  vendorcode bigint NOT NULL,
'  shortname character varying,
'  CONSTRAINT vendorcode PRIMARY KEY (vendorcode),
')
'WITH (
'  OIDS=FALSE
');
'ALTER TABLE vendor
'  OWNER TO postgres;
'GRANT ALL ON TABLE vendor TO postgres;
'GRANT ALL ON TABLE vendor TO public;

<Serializable()> _
Public Class VendorModel
    Private DS As DataSet
    Public Property VendorCode As String
    Public Property VendorName As String
    Public Property isactive As String
    Private TableName As String = "Vendor"

    Public Sub New()

    End Sub


    Public Sub New(ByVal _vendorcode As String, ByVal _vendorname As String, ByVal _isactive As String)
        Me.VendorCode = _vendorcode
        Me.VendorName = _vendorname
        Me.isactive = _isactive
    End Sub

    'this function populate list of VendorModel based on public property of this model.
    Public Function GetVendors() As List(Of VendorModel)
        Return DataAccess.ExecuteReader(Of List(Of VendorModel))("select vendorcode::text,vendorname::text,isactive from ssc.vendor;",
                                                        CommandType.Text,
                                                        AddressOf DataAccess.OnReadAnyList(Of VendorModel),
                                                        Nothing)
    End Function

    'this function populate list of vendormodel based on custom fields
    Function GetVendorsCustom() As List(Of VendorModel)
        Return DataAccess.ExecuteReader(Of List(Of VendorModel))("select vendorcode::text,vendorname::text,isactive from ssc.vendor;",
                                                        CommandType.Text,
                                                        AddressOf onReadVendors,
                                                        Nothing)
    End Function

    Private Function onReadVendors(ByVal reader As IDataReader) As List(Of VendorModel)
        If (IsNothing(reader)) Then Return New List(Of VendorModel)
        Dim VendorModels As List(Of VendorModel) = New List(Of VendorModel)
        While (reader.Read)
            VendorModels.Add(onReadVendor(reader))
        End While
        Return VendorModels
    End Function

    Private Function onReadVendor(ByVal reader As IDataReader) As VendorModel
        Debug.Assert(Not IsNothing(reader))
        Return New VendorModel With {.VendorCode = DataAccess.SafeRead(Of String)(VendorCode, reader, "vendorcode"),
                                     .VendorName = DataAccess.SafeRead(Of String)(VendorName, reader, "vendorname"),
                                     .isactive = DataAccess.SafeRead(Of String)(isactive, reader, "isactive")}
    End Function

    Public Function GetDataSet(Optional ByVal Criteria As String = "") As DataSet
        Dim DS As DataSet = Nothing
        Dim sqlstr As String = String.Format("select vendorcode::text,vendorname::text,isactive::boolean from ssc.vendor {0};", Criteria)
        DS = DataAccess.GetDataSet(sqlstr, CommandType.Text, Nothing)        
        DS.Tables(0).TableName = TableName
        Return DS
    End Function


    Public Function save(obj As Object, mye As ContentBaseEventArgs) As Boolean
        Dim myret As Boolean = False
        Dim mytransaction As IDbTransaction
        Dim factory = DataAccess.factory
        Using conn As IDbConnection = factory.CreateConnection
            conn.Open()
            mytransaction = conn.BeginTransaction
            Dim dataadapter = factory.CreateAdapter
            Dim sqlstr As String
            sqlstr = "ssc.sp_deletevendortx"
            dataadapter.DeleteCommand = factory.CreateCommand(sqlstr, conn)
            dataadapter.DeleteCommand.Parameters.Add(factory.CreateParameter("ivendorcode", DbType.Int64, 0, "vendorcode"))
            dataadapter.DeleteCommand.Parameters(0).SourceVersion = DataRowVersion.Original
            dataadapter.DeleteCommand.CommandType = CommandType.StoredProcedure

            sqlstr = "ssc.sp_insertvendortx"
            dataadapter.InsertCommand = factory.CreateCommand(sqlstr, conn)
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("ivendorcode", DbType.Int64, 0, "vendorcode"))
            dataadapter.InsertCommand.Parameters(0).SourceVersion = DataRowVersion.Current
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("ivendorname", DbType.String, 0, "vendorname"))
            dataadapter.InsertCommand.Parameters(1).SourceVersion = DataRowVersion.Current
            dataadapter.InsertCommand.Parameters.Add(factory.CreateParameter("iisactive", DbType.Boolean, 0, "isactive"))
            dataadapter.InsertCommand.Parameters(2).SourceVersion = DataRowVersion.Current
            dataadapter.InsertCommand.Parameters(2).Direction = ParameterDirection.InputOutput
            dataadapter.InsertCommand.CommandType = CommandType.StoredProcedure

            sqlstr = "ssc.sp_updatevendor"
            dataadapter.UpdateCommand = factory.CreateCommand(sqlstr, conn)
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("ivendorcode", DbType.Int64, 0, "vendorcode"))
            dataadapter.UpdateCommand.Parameters(0).SourceVersion = DataRowVersion.Original
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("vendorcode", DbType.Int64, 0, "vendorcode"))
            dataadapter.UpdateCommand.Parameters(1).SourceVersion = DataRowVersion.Current
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("ivendorname", DbType.String, 0, "vendorname"))
            dataadapter.UpdateCommand.Parameters(2).SourceVersion = DataRowVersion.Current
            dataadapter.UpdateCommand.Parameters.Add(factory.CreateParameter("iisactive", DbType.Boolean, 0, "isactive"))
            dataadapter.UpdateCommand.Parameters(3).SourceVersion = DataRowVersion.Current
            dataadapter.UpdateCommand.CommandType = CommandType.StoredProcedure

            dataadapter.InsertCommand.Transaction = mytransaction
            dataadapter.UpdateCommand.Transaction = mytransaction
            dataadapter.DeleteCommand.Transaction = mytransaction
            'dataadapter.Update(mye.dataset)
            'Since IDataAdapter.Update only receive DataSet, we need to modify as below
            mye.ra = factory.Update(mye.dataset.Tables(0))


            mytransaction.Commit()
            myret = True
        End Using
        Return myret
    End Function
End Class





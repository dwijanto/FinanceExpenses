Imports System.Text

Public Class CMMFController
    Implements IController
    Implements IToolbarAction

    Public Model As New CMMFModel
    Public DS As DataSet

    Public WithEvents BS As BindingSource
    Dim _ErrorMessage As String
    Public myForm As Object
    Public FileNameFullPath As String


    Private CMMFSB As StringBuilder
    Private CMMFPriceSB As StringBuilder

    Public Property ErrorMessage As String
        Get
            Return _ErrorMessage
        End Get
        Set(value As String)
            _ErrorMessage = value
        End Set
    End Property

    'Public Sub New(ByVal Parent As Object, ByVal FileNameFullPath As String)
    '    Me.myForm = Parent
    '    Me.FileNameFullPath = FileNameFullPath
    'End Sub

    Public Sub New()
        MyBase.New()
    End Sub

    Public ReadOnly Property GetTable As DataTable Implements IController.GetTable
        Get
            Return DS.Tables(Model.TableName).Copy()
        End Get
    End Property

    Public ReadOnly Property GetBindingSource As BindingSource
        Get
            Dim BS As New BindingSource
            BS.DataSource = GetTable
            BS.Sort = Model.SortField
            Return BS
        End Get
    End Property

    Public Function loaddata() As Boolean Implements IController.loaddata
        Dim myret As Boolean = False
        Model = New CMMFModel
        DS = New DataSet
        If Model.LoadData(DS, "") Then
            Dim pk(0) As DataColumn
            pk(0) = DS.Tables(0).Columns("cmmf")
            DS.Tables(0).PrimaryKey = pk
            BS = New BindingSource
            BS.DataSource = DS.Tables(0)
            myret = True
        End If
        Return myret
    End Function


    'Public Function loaddata(ByVal criteria As String) As Boolean
    '    Dim myret As Boolean = False
    '    Model = New CMMFModel

    '    DS = New DataSet

    '    If Model.LoadData(DS, criteria) Then
    '        Dim pk(0) As DataColumn
    '        pk(0) = DS.Tables(0).Columns("cmmf")
    '        DS.Tables(0).PrimaryKey = pk

    '        Dim pk1(0) As DataColumn
    '        pk1(0) = DS.Tables(5).Columns("pcprojectid")
    '        DS.Tables(5).PrimaryKey = pk1
    '        DS.Tables(5).Columns("pcprojectid").AutoIncrement = True
    '        DS.Tables(5).Columns("pcprojectid").AutoIncrementSeed = -1
    '        DS.Tables(5).Columns("pcprojectid").AutoIncrementStep = -1


    '        Dim pk2(0) As DataColumn
    '        pk2(0) = DS.Tables(6).Columns("pcrangeid")
    '        DS.Tables(6).PrimaryKey = pk2
    '        DS.Tables(6).Columns("pcrangeid").AutoIncrement = True
    '        DS.Tables(6).Columns("pcrangeid").AutoIncrementSeed = -1
    '        DS.Tables(6).Columns("pcrangeid").AutoIncrementStep = -1

    '        myret = True

    '    End If
    '    Return myret
    'End Function

    Public Function save() As Boolean Implements IController.save
        Dim myret As Boolean = False

        BS.EndEdit()

        Dim ds2 As DataSet = DS.GetChanges()
        If Not IsNothing(ds2) Then
            Dim mymessage As String = String.Empty
            Dim ra As Integer
            Dim mye As New ContentBaseEventArgs(ds2, True, mymessage, ra, True)
            Try
                If save(mye) Then
                    DS.Merge(ds2)
                    'Don't use DS.AcceptChanges. Use the statement below.
                    'Reason: Only AcceptChanges for modified Table. if unmodified table use AcceptChanges -> the position is set to first Row (not correct)
                    For Each mytable As DataTable In ds2.Tables
                        If mytable.Rows.Count > 0 Then
                            DS.Tables(mytable.TableName).AcceptChanges()
                        End If
                    Next
                    MessageBox.Show("Saved.")
                    myret = True
                End If
            Catch ex As Exception
                MessageBox.Show(ex.Message)
                DS.Merge(ds2)
            End Try
        Else
            MessageBox.Show("Nothing to save.")
        End If

        Return myret
    End Function

    Public Function Save(ByVal mye As ContentBaseEventArgs) As Boolean Implements IToolbarAction.Save
        Dim myret As Boolean = False
        If Model.save(Me, mye) Then
            myret = True
        End If
        Return myret
    End Function

    Public Property ApplyFilter As String Implements IToolbarAction.ApplyFilter
        Get
            Return BS.Filter
        End Get
        Set(ByVal value As String)
            BS.Filter = String.Format(Model.FilterField, value)
        End Set
    End Property
    Public Function GetCurrentRecord() As DataRowView Implements IToolbarAction.GetCurrentRecord
        Return BS.Current
    End Function

    Public Function GetNewRecord() As DataRowView Implements IToolbarAction.GetNewRecord
        Return BS.AddNew
    End Function

    Public Sub RemoveAt(value As Integer) Implements IToolbarAction.RemoveAt
        BS.RemoveAt(value)
    End Sub

    Public Function DoImport() As Boolean
        If IsNothing(DS) Then
            myForm.ProgressReport(1, "Refresh Data first!.")
            Return False
        End If
        CMMFSB = New StringBuilder
        CMMFPriceSB = New StringBuilder

        myForm.ProgressReport(6, "Start")
        Dim myret As Boolean = False
        Dim sb As New StringBuilder
        Dim sbError = New StringBuilder
        Dim myList As New List(Of String())
        Dim myrecord() As String
        Dim sw As New Stopwatch
        sw.Start()
        Try
            Using objTFParser = New FileIO.TextFieldParser(FileNameFullPath)
                With objTFParser
                    .TextFieldType = FileIO.FieldType.Delimited
                    .SetDelimiters(",")
                    .HasFieldsEnclosedInQuotes = True
                    Dim count As Long = 0
                    myForm.ProgressReport(1, "Read Data")

                    Do Until .EndOfData
                        myrecord = .ReadFields
                        myList.Add(myrecord)
                    Loop

                    For i = 1 To myList.Count - 1

                        If myList(i).Length > 0 Then
                            'CMMF: Check Existing CMMF
                            Dim mydata As CMMFModel = New CMMFModel With {.cmmf = myList(i)(0),
                                                                          .localdescription = myList(i)(1),
                                                                          .commercialcode = myList(i)(2),
                                                                          .brand = myList(i)(3),
                                                                          .pricehkd = myList(i)(4),
                                                                          .priceusd = myList(i)(5)}
                            Dim pk1(0) As Object
                            pk1(0) = myList(i)(0)
                            Dim myresult As DataRow = DS.Tables(0).Rows.Find(pk1)

                            If IsNothing(myresult) Then
                                'Add New
                                CMMFSB.Append(mydata.cmmf & vbTab &
                                              mydata.localdescription & vbTab &
                                               mydata.commercialcode & vbTab &
                                                mydata.brand & vbCrLf)
                            Else
                                'Supposed to be updated
                            End If
                            CMMFPriceSB.Append(mydata.cmmf & vbTab &
                                              mydata.pricehkd & vbTab &
                                               mydata.priceusd & vbCrLf)

                        End If


                    Next
                    If CMMFSB.Length > 0 Then
                        Dim sqlstr As String = String.Format("copy marketing.cmmf(cmmf,localdescription,commercialcode,brand) from stdin with null as 'Null';", CMMFSB.ToString)
                        ErrorMessage = DataAccess.Copy(sqlstr, CMMFSB.ToString, myret)
                    End If
                    If CMMFPriceSB.Length > 0 Then
                        Dim sqlstr1 = String.Format("delete from marketing.cmmfprice;")
                        'PostgresqlDBAdapter1.ExecuteNonQuery(sqlstr1)
                        'copy
                        Dim sqlstr As String = String.Format("{0}copy marketing.cmmfprice(cmmf,pricehkd,priceusd) from stdin with null as 'Null';", sqlstr1)

                        ErrorMessage = ErrorMessage & DataAccess.Copy(sqlstr, CMMFPriceSB.ToString, myret)
                        sw.Stop()
                        If ErrorMessage.Length > 0 Then
                            myForm.ProgressReport(1, String.Format("Elapsed Time: {0}:{1}.{2} Done.", Format(sw.Elapsed.Minutes, "00"), Format(sw.Elapsed.Seconds, "00"), sw.Elapsed.Milliseconds.ToString))
                        Else
                            myForm.ProgressReport(1, ErrorMessage)

                        End If
                    Else
                        myForm.ProgressReport(1, "Nothing to import.")
                    End If
                    myForm.ProgressReport(8, "Refresh Data.")
                End With
            End Using
        Catch ex As Exception
            myForm.ProgressReport(1, ex.Message)
        Finally
            sw.Stop()           
            myForm.ProgressReport(5, "Stop")
        End Try
        Return myret
    End Function


End Class

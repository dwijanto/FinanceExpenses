Imports System.Threading
Imports Microsoft.Office.Interop
Imports System.Text

Public Class UploadExpensesController
    Private FinanceTxBS As BindingSource
    Public VendorCode As String
    Public VendorDesc As String
    Public Crcy As String
    Public InvoiceNumber As String
    Public Parent As FormExpenses
    Dim myThread As New System.Threading.Thread(AddressOf ImportData)
    Dim ImportFileName As String
    Dim myCallback As FormatReportDelegate
    Public ErrMessage As StringBuilder
    Public Property HasError As Boolean = False
    Dim myController As EmailController
    Dim ErrorProvider1 As New ErrorProvider

    Public Sub New(ByRef parent As FormExpenses, ByRef FinanceTxBS As BindingSource, ByRef myController As EmailController)
        Me.FinanceTxBS = FinanceTxBS
        Me.Parent = parent
        myCallback = AddressOf ReadExcel
        Me.myController = myController
    End Sub


    Public Sub ImportData()
        Dim OpenFileDialog1 As New OpenFileDialog
        If Not myThread.IsAlive Then
            Parent.ToolStripStatusLabel1.Text = ""
            If OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
                ImportFileName = OpenFileDialog1.FileName
                myThread = New Thread(AddressOf DoImport)
                myThread.SetApartmentState(ApartmentState.MTA)
                'myThread.Start()
                DoImport()
            Else
                'DoBackground1.ProgressReport(5, "Marquee")
                Parent.DoBackground1.ProgressReport(1, "Open file cancelled.")
            End If

        End If
    End Sub

    Public Sub DoImport()
        Dim myExcel As New ExportToExcelFile(Parent, ImportFileName, "", myCallback)
        Parent.DoBackground1.ProgressReport(1, "Working with template...")
        myExcel.DoReadExcel()
        Parent.DoBackground1.ProgressReport(1, "Populate Data...")
        PopulateData()
        'Parent.UcFinanceExpenses1.DataGridView1.Invalidate()
        'Parent.UcFinanceExpenses1.getTotal()
        Parent.DoBackground1.ProgressReport(1, "Done.")

        Parent.DoBackground1.ProgressReport(8, "Raise Callback.")
        Parent.DoBackground1.ProgressReport(1, "End Raise Callback. Done")

    End Sub

    Private Sub ReadExcel(ByRef sender As Object, ByRef e As EventArgs)

    End Sub

    Private Sub PopulateData()
        Dim myret As Boolean = True

        Dim myList As New List(Of String())
        Dim myrecord() As String

        'Read Text File
        Using objTFParser = New FileIO.TextFieldParser(ImportFileName.Replace(".xlsx", ".txt"), Encoding.UTF8) '1252 = ANSI
            With objTFParser
                .TextFieldType = FileIO.FieldType.Delimited
                .SetDelimiters(Chr(9))
                .HasFieldsEnclosedInQuotes = True
                Dim count As Long = 0
                Parent.DoBackground1.ProgressReport(1, "Read Data..")

                Do Until .EndOfData
                    myrecord = .ReadFields
                    myList.Add(myrecord)
                Loop
            End With
            VendorCode = myList(0)(1)
            InvoiceNumber = myList(1)(1)
            Crcy = myList(3)(1)

            

            For i = 5 To myList.Count - 1
                If myList(i)(1) = "" Then
                    Exit For
                End If
                Dim mymodel As UploadExpensesModel = New UploadExpensesModel With {.glaccount = myList(i)(0),
                                                                                   .costcenter = myList(i)(1),
                                                                                   .family = myList(i)(2),
                                                                                   .amount = myList(i)(3),
                                                                                   .remark = myList(i)(4),
                                                                                   .crcy = Crcy.ToString.ToUpper
                                                                               }

                'Check myModel entry

                Dim drv As DataRowView = FinanceTxBS.AddNew
                drv.BeginEdit()
                drv.Row.Item("glaccount") = mymodel.glaccount
                drv.Row.Item("costcenter") = mymodel.costcenter
                drv.Row.Item("family") = mymodel.family
                drv.Row.Item("amount") = mymodel.amount
                drv.Row.Item("remark") = mymodel.remark
                drv.Row.Item("crcy") = Crcy.ToString.ToUpper
                drv.EndEdit()
            Next
            HasError = Me.validate
        End Using
        FileSystem.Kill(ImportFileName.Replace(".xlsx", ".txt"))

    End Sub

    Private Function validate() As Boolean
        Dim myret As Boolean
        'Check Vendorcode
        Dim pk1(0) As Object
        pk1(0) = VendorCode
        Dim result As DataRow = myController.DS.Tables("MasterVendor").Rows.Find(pk1)
        ErrorProvider1.SetError(Parent.UcFinanceExpenses1.TextBox9, "") 'Vendor
        ErrorProvider1.SetError(Parent.UcFinanceExpenses1.TextBox5, "") 'Invoice Number
        If Not IsNothing(result) Then
            VendorDesc = result.Item("vendordesc")
            VendorCode = result.Item("vendorcode")
        Else
            VendorDesc = VendorCode
            'ErrorProvider1.SetError(Parent.UcFinanceExpenses1.TextBox9, "Vendor Code is not available.")
            Parent.DoBackground1.ProgressReport(100, "Vendor is not available")
            myret = False
        End If
        'Check InvoiceNumber
        If InvoiceNumber.Length = 0 Then
            'ErrorProvider1.SetError(Parent.UcFinanceExpenses1.TextBox5, "Value cannot be blank.")
            Parent.DoBackground1.ProgressReport(101, "Value cannot be blank")
            myret = False
        End If
        'Check Crcy
        Dim myErrorSB As StringBuilder
        Dim COATable As DataTable = myController.GetChartOfAccountBS.DataSource

        For Each drv As DataRowView In FinanceTxBS.List
            myErrorSB = New StringBuilder
            If drv.Row.Item("amount") = 0 Then
                myErrorSB.Append("Amount cannot be 0. ")
                myret = False
            End If
            If Crcy.Length = 0 Then
                myErrorSB.Append("Currency cannot be blank. ")
                myret = False
            End If
            'find SAPIndex
            Dim pk01(0) As Object
            pk01(0) = String.Format("{0}-{1}{2}", drv.Row.Item("glaccount"), drv.Row.Item("costcenter"), drv.Row.Item("family"))
            Dim myresult = COATable.Rows.Find(pk01)
            If IsNothing(myresult) Then
                myErrorSB.Append(String.Format("This account {0} - {1}{2} is not listed in Chart Of Account.", drv.Row.Item("glaccount"), drv.Row.Item("costcenter"), drv.Row.Item("family")))
                myret = False
            Else
                'drv.BeginEdit()
                drv.Row.Item("sapindexid") = myresult.Item("id")
                'drv.EndEdit()

            End If

            drv.Row.RowError = myErrorSB.ToString
        Next
        Return myret
    End Function

End Class

Public Class UploadExpensesModel
    Public glaccount As String
    Public costcenter As String
    Public family As String
    Public amount As String
    Public remark As String
    Public crcy As String
End Class


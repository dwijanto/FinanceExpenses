Public Class EmailController
    Implements IController
    Implements IToolbarAction

    Public Model As New EmailModel
    Public DS As DataSet

    Public BS As BindingSource
    Private DTLBS As BindingSource
    Private ActionBS As BindingSource
    Private FinanceTxBS As BindingSource
    Private ApprovalTXBS As BindingSource
    Private VendorBS As BindingSource
    Private ChartOfAccountBS As BindingSource

    Private ReferencenumberFilterBS As BindingSource
    Private InvoiceNumberFilterBS As BindingSource
    Private StatusFilterBS As BindingSource
    Private AttnFilterBS As BindingSource
    Private stApproverFilterBS As BindingSource
    Private ndApproverFilterBS As BindingSource
    Private VendorFilterBS As BindingSource

    Public Event IsModified()

    Public Sub New()
        MyBase.New()
    End Sub

    Public ReadOnly Property GetChartOfAccountBS As BindingSource
        Get
            Return ChartOfAccountBS
        End Get
    End Property

    Public ReadOnly Property GetReferencenumberFilterBS As BindingSource
        Get
            Return ReferencenumberFilterBS
        End Get
    End Property
    Public ReadOnly Property GetInvoiceNumberFilterBS As BindingSource
        Get
            Return InvoiceNumberFilterBS
        End Get
    End Property
    Public ReadOnly Property GetStatusFilterBS As BindingSource
        Get
            Return StatusFilterBS
        End Get
    End Property
    Public ReadOnly Property GetAttnFilterBS As BindingSource
        Get
            Return AttnFilterBS
        End Get
    End Property
    Public ReadOnly Property GetstApproverFilterBS As BindingSource
        Get
            Return stApproverFilterBS
        End Get
    End Property
    Public ReadOnly Property GetndApproverFilterBS As BindingSource
        Get
            Return ndApproverFilterBS
        End Get
    End Property

    Public ReadOnly Property GetVendorFilterBS As BindingSource
        Get
            Return VendorFilterBS
        End Get
    End Property

    Public ReadOnly Property GetTable As DataTable Implements IController.GetTable
        Get
            Return DS.Tables(Model.TableName).copy()
        End Get
    End Property

    Public ReadOnly Property GetBindingSource As BindingSource
        Get
            Dim BS As New BindingSource
            BS.DataSource = GetTable
            BS.Sort = Model.sortField
            Return BS
        End Get
    End Property

    Public Overloads Sub Delete(dgv As DataGridView)
        If Not IsNothing(GetCurrentRecord) Then
            If MessageBox.Show("Delete this record?", "Delete Record", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = DialogResult.OK Then
                For Each drv As DataGridViewRow In dgv.SelectedRows
                    RemoveAt(drv.Index)
                Next
            End If
        End If
    End Sub



    Public Function previewdoc(ByVal FullPathFileName As String, ByVal FileNameAlias As String)
        Dim myret As Boolean = False
        Dim FileName As String
        If FileNameAlias = "" Then
            FileName = IO.Path.GetFileName(FullPathFileName)
        Else
            FileName = FileNameAlias
        End If

        Try
            Dim filesource As String = FullPathFileName
            If FileIO.FileSystem.GetFileInfo(filesource).Length / 1048576 < 5 Then
                Dim mytemp = String.Format("{1}{0}", FileName, IO.Path.GetTempPath())
                FileIO.FileSystem.CopyFile(filesource, mytemp, True)
                Dim p As New System.Diagnostics.Process
                p.StartInfo.FileName = mytemp
                p.Start()
            Else
                MessageBox.Show("File too big.Please download.")
            End If

            myret = True
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
        Return myret
    End Function
    Public Function LoadSearchData() As Boolean
        Dim myret As Boolean = False
        Model = New EmailModel
        Dim DS = New DataSet
        If Model.LoadSearchData(DS) Then
            ReferencenumberFilterBS = New BindingSource            
            ReferencenumberFilterBS.DataSource = DS.Tables(0)           
            myret = True
        End If
        Return myret
    End Function

    Public Function loaddata() As Boolean Implements IController.loaddata
        Dim myret As Boolean = False
        Model = New EmailModel
        DS = New DataSet
        If Model.LoadData(DS) Then
            BS = New BindingSource
            BS.DataSource = DS.Tables(0)
            DTLBS.DataSource = BS
            DTLBS.DataMember = "hdrel"
            myret = True
        End If
        Return myret
    End Function

    Public Function loaddata(ByVal criteria As String) As Boolean
        Dim myret As Boolean = False
        Model = New EmailModel
        Model.Criteria = criteria
        DS = New DataSet
        If Model.LoadData(DS, criteria) Then
            BS = New BindingSource
            BS.DataSource = DS.Tables(0)
            DTLBS = New BindingSource
            DTLBS.DataSource = BS
            DTLBS.DataMember = "hdrel"
            ActionBS = New BindingSource
            ActionBS.DataSource = BS
            ActionBS.DataMember = "hdrel-action"
            FinanceTxBS = New BindingSource
            FinanceTxBS.DataSource = BS
            FinanceTxBS.DataMember = "hdrel-financetx"
            ApprovalTXBS = New BindingSource
            ApprovalTXBS.DataSource = BS
            ApprovalTXBS.DataMember = "hdrel-approvaltx"
            VendorBS = New BindingSource
            VendorBS.DataSource = DS.Tables("VENDOR")
            ChartOfAccountBS = New BindingSource
            ChartOfAccountBS.DataSource = DS.Tables("COA")
            myret = True
        End If
        Return myret
    End Function

    Public Function LoadFilter() As Boolean
        Dim myret As Boolean = False
        Model = New EmailModel
        Dim DS = New DataSet
        If Model.LoadFilter(DS) Then
            ReferencenumberFilterBS = New BindingSource
            InvoiceNumberFilterBS = New BindingSource
            StatusFilterBS = New BindingSource
            AttnFilterBS = New BindingSource
            stApproverFilterBS = New BindingSource
            ndApproverFilterBS = New BindingSource
            VendorFilterBS = New BindingSource

            ReferencenumberFilterBS.DataSource = DS.Tables(0)
            InvoiceNumberFilterBS.DataSource = DS.Tables(1)
            StatusFilterBS.DataSource = DS.Tables(2)
            AttnFilterBS.DataSource = DS.Tables(3)
            stApproverFilterBS.DataSource = DS.Tables(4)
            ndApproverFilterBS.DataSource = DS.Tables(5)
            VendorFilterBS.DataSource = DS.Tables(6)           
            myret = True
        End If
        Return myret

    End Function

    Public Function Findloaddata(ByVal criteria As String) As Boolean
        Dim myret As Boolean = False
        Model = New EmailModel
        Model.Criteria = criteria
        DS = New DataSet
        If Model.FindLoadData(DS) Then
            BS = New BindingSource
            BS.DataSource = DS.Tables(0)
            myret = True
        End If
        Return myret
    End Function

    Public Function MyTasksloaddata(ByRef DS As DataSet, ByVal MyTasksCriteria As String, ByVal HistoryCriteria As String) As Boolean
        Dim myret As Boolean = False
        Model = New EmailModel
        If Model.MyTasksLoadData(DS, MyTasksCriteria, HistoryCriteria) Then
            myret = True
        End If
        Return myret
    End Function

    Public Function saveExpenses() As Boolean
        Dim myret As Boolean = False
        BS.EndEdit()
        DTLBS.EndEdit()
        ActionBS.EndEdit()
        FinanceTxBS.EndEdit()
        ApprovalTXBS.EndEdit()

        Dim ds2 As DataSet = DS.GetChanges()
        If Not IsNothing(ds2) Then
            Dim mymessage As String = String.Empty
            Dim ra As Integer
            Dim mye As New ContentBaseEventArgs(ds2, True, mymessage, ra, True)
            Try
                If saveExpenses(mye) Then
                    DS.Merge(ds2)
                    'Don't use DS.AcceptChanges. Use the statement below.
                    'Reason: Only AcceptChanges for modified Table. if unmodified table use AcceptChanges -> the position is set to first Row (not correct)
                    For Each mytable As DataTable In ds2.Tables
                        If mytable.Rows.Count > 0 Then
                            DS.Tables(mytable.TableName).AcceptChanges()
                        End If
                    Next
                    MessageBox.Show("Saved.")
                    RaiseEvent IsModified()
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

    Public Function save() As Boolean Implements IController.save
        Dim myret As Boolean = False
        BS.EndEdit()
        DTLBS.EndEdit()
        ActionBS.EndEdit()
        FinanceTxBS.EndEdit()

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
                    RaiseEvent IsModified()
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

    Function saveExpenses(ByVal mye As ContentBaseEventArgs) As Boolean
        Dim myret As Boolean = False
        If Model.saveExpenses(Me, mye) Then
            myret = True
        End If
        Return myret
    End Function

    Public Property ApplyFilter As String Implements IToolbarAction.ApplyFilter
        Get
            Return BS.Filter
        End Get
        Set(ByVal value As String)
            BS.Filter = String.Format(value)
        End Set
    End Property
    Public Function GetCurrentRecord() As DataRowView Implements IToolbarAction.GetCurrentRecord
        Return BS.Current
    End Function

    Public Function GetNewRecord() As DataRowView Implements IToolbarAction.GetNewRecord
        Return BS.AddNew
    End Function

    Public Function GetDTLBS() As BindingSource
        Return DTLBS
    End Function

    Public Function GetActionBS() As BindingSource
        Return ActionBS
    End Function

    Public Function GetFinanceTxBS() As BindingSource
        Return FinanceTxBS
    End Function

    Public Function GetApprovalTxBS() As BindingSource
        Return ApprovalTXBS
    End Function

    Public Sub RemoveAt(value As Integer) Implements IToolbarAction.RemoveAt
        BS.RemoveAt(value)
    End Sub

    Public Function GetVendorBS() As BindingSource
        Return VendorBS
    End Function

    Function GetDelegateTo(staffemail As String) As String
        Dim factory = DataAccess.factory
        Dim sqlstr = String.Format("select delegateto from ssc.sscdelegate where staffcode =:paramname and isactive and startdate <= current_date and enddate >= current_date")
        Dim myresult As String = String.Empty
        Dim myparam(0) As System.Data.IDbDataParameter
        myparam(0) = factory.CreateParameter("paramname", staffemail)
        myresult = DataAccess.ExecuteScalar(sqlstr, CommandType.Text, myparam)
        If IsNothing(myresult) Then
            myresult = String.Empty
        End If
        Return myresult
    End Function


End Class

Imports System.ComponentModel

Public Class UCFinanceExpenses
    Implements INotifyPropertyChanged

    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged

    Private drv As DataRowView
    Private DtlBS As BindingSource
    Private WithEvents FinanceTxBS As BindingSource
    Private ApprovalTXBS As BindingSource
    Private VendorBS As BindingSource
    Private COABS As BindingSource
    Public DoImport As Boolean = False
    Public DS As DataSet
    Private CurrencyBS As BindingSource
    Public ExRate As Decimal
    Private Sub RadioButton_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged, RadioButton2.CheckedChanged
        onPropertyChanged("DocType")
    End Sub
    Private Sub onPropertyChanged(PropertyName As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(PropertyName))
    End Sub

    Public Function getExrate() As Decimal
        Dim mydrv As DataRowView = ComboBox1.SelectedItem
        If Not (IsNothing(mydrv)) Then
            ExRate = mydrv.Row.Item("nvalue")
        End If
        Return ExRate
    End Function

    Public Property DocType As Integer
        Get
            If RadioButton1.Checked Then
                Return 1
            Else
                Return 2
            End If
        End Get
        Set(value As Integer)
            If value = 1 Then
                RadioButton1.Checked = True
                Label6.Text = "Invoice number"
            ElseIf value = 2 Then
                RadioButton2.Checked = True
                Label6.Text = "Credit Note number"
            End If
        End Set
    End Property


    Public Sub DisabledContextMenu()
        DataGridView1.ContextMenuStrip = Nothing
        RadioButton1.Enabled = False
        RadioButton2.Enabled = False
        'TextBox5.Enabled = False
        TextBox5.ReadOnly = True
        ComboBox1.Enabled = False
    End Sub

    Public Sub BindingControl(ByRef drv As DataRowView, ByRef dtlbs As BindingSource, ByRef financetxbs As BindingSource, ByRef ApprovalTxBS As BindingSource, ByVal VendorBS As BindingSource, ByVal COABS As BindingSource, ByRef DS As DataSet, ByRef CurrencyBS As BindingSource, ByRef Parent As Object)
        Me.drv = drv
        Me.DtlBS = dtlbs
        Me.FinanceTxBS = financetxbs
        Me.ApprovalTXBS = ApprovalTxBS
        Me.VendorBS = VendorBS
        Me.COABS = COABS
        Me.DS = DS
        Me.CurrencyBS = CurrencyBS
        InitData()
        EnabledControl(drv.Row.Item("status"))
        EnableContextMenu()
        AddHandler DialogAddUpdCostCenter.RefreshDataGrid, AddressOf RefreshDatagrid
    End Sub

    Private Sub DataGridView1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellDoubleClick
        If Not IsNothing(FinanceTxBS.Current) Then
            If Not IsNothing(DataGridView1.ContextMenuStrip) Then
                If drv.Row.Item("status") = TaskStatusEnum.STATUS_NEW Or
                    drv.Row.Item("status") = TaskStatusEnum.STATUS_FORWARD Or
                    drv.Row.Item("status") = TaskStatusEnum.STATUS_RE_SUBMIT Or
                    drv.Row.Item("status") = TaskStatusEnum.STATUS_REJECTEDBYM1 Or
                    drv.Row.Item("status") = TaskStatusEnum.STATUS_REJECTEDBYM2 Or
                    drv.Row.Item("status") = TaskStatusEnum.STATUS_REJECTEDBYFINANCE Then

                    showDialog(TxEnum.UpdateRecord)
                End If

            End If
        End If
    End Sub

    Private Sub UpdateToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UpdateToolStripMenuItem.Click
        If Not IsNothing(FinanceTxBS) Then
            showDialog(TxEnum.UpdateRecord)
        Else
            MessageBox.Show("Please wait until the current process is finished.")
        End If
    End Sub
    Private Sub AddNewToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AddNewToolStripMenuItem.Click
        If Not IsNothing(FinanceTxBS) Then
            showDialog(TxEnum.NewRecord)
        Else
            MessageBox.Show("Please wait until the current process is finished.")
        End If

    End Sub
    Private Sub DeleteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteToolStripMenuItem.Click
        If Not IsNothing(FinanceTxBS) Then
            If Not IsNothing(FinanceTxBS.Current) Then
                If MessageBox.Show("Delete this record?", "Delete Record", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = DialogResult.OK Then
                    For Each drv As DataGridViewRow In DataGridView1.SelectedRows
                        FinanceTxBS.RemoveAt(drv.Index)
                    Next
                    getTotal()
                End If
            End If
        Else
            MessageBox.Show("Please wait until the current process is finished.")
        End If

    End Sub
    Public Overloads Function validate() As Boolean
        Dim myret As Boolean = True
        If Not CheckControl(TextBox5, "Value cannot be blank.") Then
            myret = False
        End If

        'If Not CheckControl(TextBoxSAP, "Value cannot be blank.") Then
        '    myret = False
        'End If

        ErrorProvider1.SetError(BtnVendor, "")
        If TextBox9.TextLength = 0 Then
            ErrorProvider1.SetError(BtnVendor, "Please click button to select vendor.")
            myret = False
        End If

        ErrorProvider1.SetError(ComboBox1, "")
        If ComboBox1.SelectedIndex < 0 Then
            ErrorProvider1.SetError(ComboBox1, "Please select currency from the list.")
            myret = False
        End If

        ErrorProvider1.SetError(DataGridView1, "")
        If FinanceTxBS.Count = 0 Then
            ErrorProvider1.SetError(DataGridView1, "You must add at least one record.")
            myret = False
        Else
            For Each dr As DataRow In DS.Tables("FinanceTx").Rows
                If IsDBNull(dr.Item("glaccount")) Then
                    dr.RowError = "Blank record. Please remove it."
                    myret = False
                End If
            Next
            If DS.Tables("FinanceTx").HasErrors Then
                ErrorProvider1.SetError(DataGridView1, "Error Found. Please check details.")
                myret = False
            End If
        End If

        Return myret
    End Function

    Public Function CheckControl(ByVal obj As Object, ByVal message As String) As Boolean
        Dim myret As Boolean = True
        If obj.Enabled Then
            ErrorProvider1.SetError(obj, "")
            If obj.TextLength = 0 Then
                ErrorProvider1.SetError(obj, message)
                myret = False
            End If
        End If
        Return myret
    End Function

    Private Sub showDialog(ByVal action As TxEnum)
        Dim mydrv As DataRowView = Nothing
        Select Case action
            Case TxEnum.NewRecord
                mydrv = FinanceTxBS.AddNew
            Case TxEnum.UpdateRecord
                mydrv = FinanceTxBS.Current
        End Select
        Dim myform As New DialogAddUpdCostCenter(mydrv, COABS)
        mydrv.BeginEdit()
        myform.ShowDialog()
        getTotal()

    End Sub


    Private Sub InitData()
        TextBox1.DataBindings.Clear()
        TextBox2.DataBindings.Clear()
        TextBox3.DataBindings.Clear()
        TextBox5.DataBindings.Clear()
        TextBoxSAP.DataBindings.Clear()
        TextBox7.DataBindings.Clear()
        TextBox8.DataBindings.Clear()
        TextBox9.DataBindings.Clear()
        ComboBox1.DataBindings.Clear()

        ComboBox1.DataSource = CurrencyBS
        ComboBox1.DisplayMember = "paramname"
        ComboBox1.ValueMember = "paramname"

        TextBox1.DataBindings.Add(New Binding("Text", drv, "sender", False, DataSourceUpdateMode.OnPropertyChanged))
        TextBox2.DataBindings.Add(New Binding("Text", drv, "receiveddate", True, DataSourceUpdateMode.OnPropertyChanged, "", "dd-MMM-yyyy hh:mm:ss tt"))
        TextBox3.DataBindings.Add(New Binding("Text", drv, "emailsubject", False, DataSourceUpdateMode.OnPropertyChanged))
        TextBox5.DataBindings.Add(New Binding("Text", drv, "invoicenumber", False, DataSourceUpdateMode.OnPropertyChanged))
        TextBoxSAP.DataBindings.Add(New Binding("Text", drv, "financenumber", False, DataSourceUpdateMode.OnPropertyChanged))
        TextBox7.DataBindings.Add(New Binding("Text", drv, "emailto", False, DataSourceUpdateMode.OnPropertyChanged))
        TextBox8.DataBindings.Add(New Binding("Text", drv, "refnumber", False, DataSourceUpdateMode.OnPropertyChanged))
        TextBox9.DataBindings.Add(New Binding("Text", drv, "vendordesc", False, DataSourceUpdateMode.OnPropertyChanged))
        ComboBox1.DataBindings.Add(New Binding("selectedvalue", drv, "crcy", False, DataSourceUpdateMode.OnPropertyChanged))

        Me.DataBindings.Add(New Binding("DocType", drv, "doctype", False, DataSourceUpdateMode.OnPropertyChanged))


        For Each drv As DataRowView In DtlBS.List
            ListBox1.Items.Add(drv.Row.Item("attachmentname"))
        Next

        DataGridView1.AutoGenerateColumns = False
        DataGridView1.DataSource = FinanceTxBS
        getTotal()
        WebBrowser1.Refresh()
        WebBrowser1.DocumentText = "" & drv.Row("emailbody")
        If ListBox1.Items.Count > 0 Then
            ListBox1.SelectedIndex = 0
        End If
    End Sub

    Public Sub getTotal()
        Dim total As Decimal = 0
        For Each drv As DataRowView In FinanceTxBS.List
            If Not IsDBNull(drv.Row.Item("amount")) Then
                total = total + drv.Row.Item("amount")
            End If
        Next
        TextBox4.Text = String.Format("{0:#,##0.00}", total)
    End Sub

    Public Sub EnabledControl(Status As FinanceExpenses.TaskStatusEnum)
        'TextBox5.Enabled = False
        TextBox5.ReadOnly = True
        ComboBox1.Enabled = False
        TextBoxSAP.Enabled = False
        RadioButton1.Enabled = False
        RadioButton2.Enabled = False
        DataGridView1.ContextMenuStrip = Nothing
        ToolTip1.SetToolTip(DataGridView1, "")
        BtnVendor.Enabled = False
        Select Case Status
            Case TaskStatusEnum.STATUS_NEW, TaskStatusEnum.STATUS_FORWARD, TaskStatusEnum.STATUS_REJECTEDBYFINANCE, TaskStatusEnum.STATUS_REJECTEDBYM1, TaskStatusEnum.STATUS_REJECTEDBYM2
                'TextBox5.Enabled = True
                TextBox5.ReadOnly = False
                ComboBox1.Enabled = True
                RadioButton1.Enabled = True
                RadioButton2.Enabled = True
                DataGridView1.ContextMenuStrip = ContextMenuStrip1
                BtnVendor.Enabled = True
                'ToolTip1.ToolTipTitle = "Right Click to activate context menu"
                ToolTip1.SetToolTip(DataGridView1, "Right Click to activate context menu")

            Case TaskStatusEnum.STATUS_VALIDATEDBYREQUESTER
            Case TaskStatusEnum.STATUS_RE_SUBMIT

            Case TaskStatusEnum.STATUS_VALIDATEDBYM1
                Dim appdrv As DataRowView = ApprovalTXBS.Current
                If IsDBNull(appdrv.Row.Item("ndapprover")) Then
                    TextBoxSAP.Enabled = True
                End If
            Case TaskStatusEnum.STATUS_VALIDATEDBYM2
                TextBoxSAP.Enabled = True
            Case TaskStatusEnum.STATUS_COMPLETED
                If User.can("Validate For Finance") Then
                    TextBoxSAP.Enabled = True
                End If
            Case Else

        End Select
    End Sub

    Public Sub EnableContextMenu()
        Try
            UpdateToolStripMenuItem.Visible = FinanceTxBS.Count > 0
            DeleteToolStripMenuItem.Visible = FinanceTxBS.Count > 0
        Catch ex As Exception

        End Try

    End Sub

    Public Sub RefreshDatagrid()
        DataGridView1.ScrollBars = ScrollBars.None
        DataGridView1.Enabled = True
        DataGridView1.ScrollBars = ScrollBars.Both
        DataGridView1.PerformLayout()
        DataGridView1.Invalidate()
        'Dim MI As New MethodInvoker(AddressOf ToBeInvoke)
        'Invoke(MI)
        'ToBeInvoke()
    End Sub
    Private Sub ToBeInvoke()
        DataGridView1.ScrollBars = ScrollBars.None
        DataGridView1.Enabled = True
        DataGridView1.ScrollBars = ScrollBars.Both
        DataGridView1.PerformLayout()
        DataGridView1.Invalidate()
    End Sub



    Private Sub FinanceTxBS_ListChanged(sender As Object, e As System.ComponentModel.ListChangedEventArgs) Handles FinanceTxBS.ListChanged
        If Not DoImport Then
            EnableContextMenu()
        End If


    End Sub


    Private Sub ListBox1_DoubleClick(sender As Object, e As EventArgs) Handles ListBox1.DoubleClick, Button1.Click
        ViewDocument()
    End Sub

    Private Sub ViewDocument()
        Dim selecteditem = ListBox1.SelectedItem
        If Not IsNothing(selecteditem) Then
            Dim myParamAdapter = ParamAdapter.getInstance
            Dim myFileController As New EmailController
            Dim FullPathFileName = String.Format("{0}\{1:yyyyMM}\{1:ddHHmmss}{2}", myParamAdapter.GetParamDetailCValue("basefolder"), CDate(TextBox2.Text), selecteditem)


            myFileController.previewdoc(FullPathFileName, selecteditem)
        Else
            MessageBox.Show("Select item from the list.")
        End If

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim selecteditem = ListBox1.SelectedItem
        If Not IsNothing(selecteditem) Then
            Dim myParamAdapter = ParamAdapter.getInstance
            Dim myFileController As New EmailController
            Dim FullPathFileName = String.Format("{0}\{1:yyyyMM}\{1:ddHHmmss}{2}", myParamAdapter.GetParamDetailCValue("basefolder"), CDate(TextBox2.Text), selecteditem)
            Dim mydialog As New SaveFileDialog
            mydialog.FileName = selecteditem
            If mydialog.ShowDialog = DialogResult.OK Then
                FileIO.FileSystem.CopyFile(FullPathFileName, mydialog.FileName, True)
                Dim p As New System.Diagnostics.Process
                If MessageBox.Show("Open the file?", "Question", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.OK Then
                    p.StartInfo.FileName = mydialog.FileName
                    p.Start()
                End If

            End If
        Else
            MessageBox.Show("Select item from the list.")
        End If


    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles BtnVendor.Click

        If drv.Row.Item("status") = TaskStatusEnum.STATUS_NEW Or drv.Row.Item("status") = TaskStatusEnum.STATUS_FORWARD Or
            drv.Row.Item("status") = TaskStatusEnum.STATUS_RE_SUBMIT Or
            drv.Row.Item("status") = TaskStatusEnum.STATUS_REJECTEDBYFINANCE Or
            drv.Row.Item("status") = TaskStatusEnum.STATUS_REJECTEDBYM1 Or
            drv.Row.Item("status") = TaskStatusEnum.STATUS_REJECTEDBYM2 Then

            Dim helperbs As New BindingSource
            Dim mycontroller As New UserController
            helperbs = VendorBS
            helperbs.Filter = ""
            Dim myform = New FormHelper(helperbs)
            myform.Column1.Width = 400
            myform.Width = 600
            myform.DataGridView1.Columns(0).DataPropertyName = "vendordesc"

            myform.Filter = "[vendordesc] like '%{0}%'"
            If myform.ShowDialog() = DialogResult.OK Then
                Dim drvcurr As DataRowView = helperbs.Current
                TextBox9.Text = drvcurr.Row.Item("vendordesc")
                drv.Row.Item("vendorcode") = drvcurr.Row.Item("vendorcode")
                ErrorProvider1.SetError(TextBox9, "")
            End If
        End If
        
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged

    End Sub

    Private Sub DataGridView1_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles DataGridView1.DataError

    End Sub

    Private Sub ComboBox1_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles ComboBox1.SelectionChangeCommitted
        getExrate()
    End Sub
End Class

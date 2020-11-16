Public Class UCFinanceExpenses
    Private drv As DataRowView
    Private DtlBS As BindingSource
    Private WithEvents FinanceTxBS As BindingSource
    Private ApprovalTXBS As BindingSource
    Public Sub BindingControl(ByRef drv As DataRowView, ByRef dtlbs As BindingSource, ByRef financetxbs As BindingSource, ByRef ApprovalTxBS As BindingSource)
        Me.drv = drv
        Me.DtlBS = dtlbs
        Me.FinanceTxBS = financetxbs
        Me.ApprovalTXBS = ApprovalTxBS
        InitData()
        EnabledControl(drv.Row.Item("status"))
        EnableContextMenu()
        AddHandler DialogAddUpdCostCenter.RefreshDataGrid, AddressOf RefreshDatagrid
    End Sub

    Private Sub UpdateToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UpdateToolStripMenuItem.Click, DataGridView1.CellDoubleClick
        showDialog(TxEnum.UpdateRecord)
    End Sub
    Private Sub AddNewToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AddNewToolStripMenuItem.Click
        showDialog(TxEnum.NewRecord)
    End Sub

    Public Overloads Function validate() As Boolean
        Dim myret As Boolean = True
        If Not CheckControl(TextBox5, "Value cannot be blank.") Then
            myret = False
        End If
        If Not CheckControl(TextBox6, "Value cannot be blank.") Then
            myret = False
        End If
        ErrorProvider1.SetError(DataGridView1, "")
        If FinanceTxBS.Count = 0 Then
            ErrorProvider1.SetError(DataGridView1, "You must add at least one record.")
            myret = False
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
        Dim myform As New DialogAddUpdCostCenter(mydrv)
        mydrv.BeginEdit()
        myform.ShowDialog()
        getTotal()

    End Sub


    Private Sub InitData()
        TextBox1.DataBindings.Clear()
        TextBox2.DataBindings.Clear()
        TextBox3.DataBindings.Clear()
        TextBox5.DataBindings.Clear()
        TextBox6.DataBindings.Clear()

        TextBox1.DataBindings.Add(New Binding("Text", drv, "sender", False, DataSourceUpdateMode.OnPropertyChanged))
        TextBox2.DataBindings.Add(New Binding("Text", drv, "receiveddate", True, DataSourceUpdateMode.OnPropertyChanged, "", "dd-MMM-yyyy hh:mm:ss tt"))
        TextBox3.DataBindings.Add(New Binding("Text", drv, "emailsubject", False, DataSourceUpdateMode.OnPropertyChanged))
        TextBox5.DataBindings.Add(New Binding("Text", drv, "invoicenumber", False, DataSourceUpdateMode.OnPropertyChanged))
        TextBox6.DataBindings.Add(New Binding("Text", drv, "financenumber", False, DataSourceUpdateMode.OnPropertyChanged))

        For Each drv As DataRowView In DtlBS.List
            ListBox1.Items.Add(drv.Row.Item("attachmentname"))
        Next

        DataGridView1.AutoGenerateColumns = False
        DataGridView1.DataSource = FinanceTxBS
        getTotal()
        WebBrowser1.Refresh()
        WebBrowser1.DocumentText = drv.Row("emailbody")
        If ListBox1.Items.Count > 0 Then
            ListBox1.SelectedIndex = 0
        End If
    End Sub

    Private Sub getTotal()
        Dim total = 0
        For Each drv As DataRowView In FinanceTxBS.List
            total = total + drv.Row.Item("amount")
        Next
        TextBox4.Text = String.Format("{0:#,##0.00}", total)
    End Sub

    Public Sub EnabledControl(Status As FinanceExpenses.TaskStatusEnum)
        TextBox5.Enabled = False
        TextBox6.Enabled = False
        Select Case Status
            Case TaskStatusEnum.STATUS_NEW, TaskStatusEnum.STATUS_FORWARD
                TextBox5.Enabled = True
            Case TaskStatusEnum.STATUS_VALIDATEDBYREQUESTER

            Case TaskStatusEnum.STATUS_VALIDATEDBYM1
                Dim appdrv As DataRowView = ApprovalTXBS.Current
                If IsDBNull(appdrv.Row.Item("ndapprover")) Then
                    TextBox6.Enabled = True
                End If
            Case TaskStatusEnum.STATUS_VALIDATEDBYM2
                TextBox6.Enabled = True
        End Select
    End Sub

    Private Sub EnableContextMenu()
        UpdateToolStripMenuItem.Visible = FinanceTxBS.Count > 0
        DeleteToolStripMenuItem.Visible = FinanceTxBS.Count > 0
    End Sub

    Private Sub RefreshDatagrid()
        DataGridView1.Invalidate()
    End Sub

    Private Sub DeleteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteToolStripMenuItem.Click
        If Not IsNothing(FinanceTxBS.Current) Then
            If MessageBox.Show("Delete this record?", "Delete Record", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = DialogResult.OK Then
                For Each drv As DataGridViewRow In DataGridView1.SelectedRows
                    FinanceTxBS.RemoveAt(drv.Index)
                Next
                getTotal()
            End If
        End If        
    End Sub


    Private Sub FinanceTxBS_ListChanged(sender As Object, e As System.ComponentModel.ListChangedEventArgs) Handles FinanceTxBS.ListChanged
        EnableContextMenu()

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
End Class

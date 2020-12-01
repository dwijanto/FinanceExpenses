﻿Imports System.ComponentModel

Public Class UCFinanceExpenses
    Implements INotifyPropertyChanged

    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged

    Private drv As DataRowView
    Private DtlBS As BindingSource
    Private WithEvents FinanceTxBS As BindingSource
    Private ApprovalTXBS As BindingSource

    Private Sub RadioButton_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged, RadioButton2.CheckedChanged
        onPropertyChanged("DocType")
    End Sub
    Private Sub onPropertyChanged(PropertyName As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(PropertyName))
    End Sub

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
        TextBox5.Enabled = False
    End Sub

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

    Private Sub DataGridView1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellDoubleClick
        If Not IsNothing(DataGridView1.ContextMenuStrip) Then
            showDialog(TxEnum.UpdateRecord)
        End If
    End Sub

    Private Sub UpdateToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UpdateToolStripMenuItem.Click
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
        TextBox7.DataBindings.Clear()
        TextBox8.DataBindings.Clear()

        TextBox1.DataBindings.Add(New Binding("Text", drv, "sender", False, DataSourceUpdateMode.OnPropertyChanged))
        TextBox2.DataBindings.Add(New Binding("Text", drv, "receiveddate", True, DataSourceUpdateMode.OnPropertyChanged, "", "dd-MMM-yyyy hh:mm:ss tt"))
        TextBox3.DataBindings.Add(New Binding("Text", drv, "emailsubject", False, DataSourceUpdateMode.OnPropertyChanged))
        TextBox5.DataBindings.Add(New Binding("Text", drv, "invoicenumber", False, DataSourceUpdateMode.OnPropertyChanged))
        TextBox6.DataBindings.Add(New Binding("Text", drv, "financenumber", False, DataSourceUpdateMode.OnPropertyChanged))
        TextBox7.DataBindings.Add(New Binding("Text", drv, "emailto", False, DataSourceUpdateMode.OnPropertyChanged))
        TextBox8.DataBindings.Add(New Binding("Text", drv, "refnumber", False, DataSourceUpdateMode.OnPropertyChanged))
        Me.DataBindings.Add(New Binding("DocType", drv, "doctype", False, DataSourceUpdateMode.OnPropertyChanged))

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
        Dim total As Decimal = 0
        For Each drv As DataRowView In FinanceTxBS.List
            If Not IsDBNull(drv.Row.Item("amount")) Then
                total = total + drv.Row.Item("amount")
            End If
        Next
        TextBox4.Text = String.Format("{0:#,##0.00}", total)
    End Sub

    Public Sub EnabledControl(Status As FinanceExpenses.TaskStatusEnum)
        TextBox5.Enabled = False
        TextBox6.Enabled = False
        RadioButton1.Enabled = False
        RadioButton2.Enabled = False
        Select Case Status
            Case TaskStatusEnum.STATUS_NEW, TaskStatusEnum.STATUS_FORWARD, TaskStatusEnum.STATUS_REJECTEDBYFINANCE, TaskStatusEnum.STATUS_REJECTEDBYM1, TaskStatusEnum.STATUS_REJECTEDBYM2
                TextBox5.Enabled = True
                RadioButton1.Enabled = True
                RadioButton2.Enabled = True
            Case TaskStatusEnum.STATUS_VALIDATEDBYREQUESTER

            Case TaskStatusEnum.STATUS_VALIDATEDBYM1
                Dim appdrv As DataRowView = ApprovalTXBS.Current
                If IsDBNull(appdrv.Row.Item("ndapprover")) Then
                    TextBox6.Enabled = True
                End If
            Case TaskStatusEnum.STATUS_VALIDATEDBYM2
                TextBox6.Enabled = True
            Case TaskStatusEnum.STATUS_COMPLETED
                If User.can("Validate For Finance") Then
                    TextBox6.Enabled = True
                End If
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

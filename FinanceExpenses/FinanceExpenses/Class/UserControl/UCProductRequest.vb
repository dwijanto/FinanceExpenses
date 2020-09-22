Public Class UCProductRequest

    Public HDDRV As DataRowView
    Public DTLBS As BindingSource
    Private CMMFBS As BindingSource
    Private ExpensesTypeBS As BindingSource
    Private BPartnerBS As BindingSource
    Private _total As Decimal

    Public ReadOnly Property getTotal
        Get
            Return _total
        End Get
    End Property


    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        RemoveHandler DataGridView1.CellDoubleClick, AddressOf DataGridView1_CellDoubleClick
        AddHandler DataGridView1.CellDoubleClick, AddressOf DataGridView1_CellDoubleClick
    End Sub

    Public Sub AddHandlerDataGridView1CellDoubleClick()
        AddHandler DataGridView1.CellDoubleClick, AddressOf DataGridView1_CellDoubleClick
    End Sub
    Public Sub RemoveHandlerDataGridView1CellDoubleClick()
        RemoveHandler DataGridView1.CellDoubleClick, AddressOf DataGridView1_CellDoubleClick
    End Sub

    Private Sub AddToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AddToolStripMenuItem.Click
        Dim DTLDrv As DataRowView = DTLBS.AddNew
        DTLDrv.Item("prhdid") = HDDRV.Item("id")
        DTLDrv.Item("createdby") = DirectCast(User.identity, UserController).username
        DTLDrv.Item("createddate") = Now


        'Dim myform = New DialogAddUpdProductRequest(DTLDrv, CMMFBS, ExpensesTypeBS)
        'RemoveHandler myform.RefreshDataGridView, AddressOf RefreshDataGridView
        'AddHandler myform.RefreshDataGridView, AddressOf RefreshDataGridView

        'If myform.ShowDialog() = DialogResult.OK Then
        '    DTLDrv.Item("total") = DTLDrv.Item("qty") * DTLDrv.Item("price")
        '    calculateTotal()
        'End If


    End Sub

    Public Sub EnabledDeliveryDate()
        Me.Enabled = True
        For Each ctrl As Control In Me.Controls
            If ctrl.GetType Is GroupBox1.GetType Then
                For Each o As Control In ctrl.Controls
                    o.Enabled = False
                Next
            End If
            'ctrl.Enabled = False
        Next
        DateTimePicker2.Enabled = True
    End Sub


    Public Overloads Function validate() As Boolean
        DataGridView1.EndEdit()

        Dim myret As Boolean = True
        ErrorProvider1.SetError(Button1, "")
        If TextBox9.Text.Length = 0 Then
            ErrorProvider1.SetError(Button1, "Field cannot be blank. Press button for help.")
            myret = False
        End If
        ErrorProvider1.SetError(DateTimePicker2, "")
        If (DateTimePicker2.Value - DateTimePicker1.Value).Days < 3 Then
            ErrorProvider1.SetError(DateTimePicker2, "D+3 / day of request + 3 working days after approval.")
            myret = False
        End If

        For Each drv As DataRowView In DTLBS.List
            If Not IsDBNull(drv.Row.Item("confirmedqty")) Then
                drv.Row.RowError = ""
                If drv.Row.Item("confirmedqty") > drv.Row.Item("qty") Then
                    drv.Row.RowError = "Confirmed Qty is bigger then Qty."
                    myret = False
                End If
            End If
        Next
        Return myret
    End Function

    Private Sub DataGridView1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) 'Handles DataGridView1.CellDoubleClick
        Dim dtldrv As DataRowView = DTLBS.Current
        'Dim myform = New DialogAddUpdProductRequest(dtldrv, CMMFBS, ExpensesTypeBS)
        'RemoveHandler myform.RefreshDataGridView, AddressOf RefreshDataGridView
        'AddHandler myform.RefreshDataGridView, AddressOf RefreshDataGridView

        'If myform.ShowDialog() = DialogResult.OK Then
        '    dtldrv.Item("total") = dtldrv.Item("qty") * dtldrv.Item("price")
        '    calculateTotal()

        'End If
    End Sub

    Public Sub DisabledContextMenuStrip()
        DataGridView1.ContextMenuStrip = Nothing
    End Sub

    Public Sub BindingControl(ByVal hddrv As DataRowView, ByVal DTLBS As BindingSource, ByVal CMMFBS As BindingSource, ExpensesTypeBS As BindingSource, ByVal BPartnerBS As BindingSource)
        Try
            Me.DTLBS = DTLBS
            Me.HDDRV = hddrv            
            Me.CMMFBS = CMMFBS
            Me.BPartnerBS = BPartnerBS
            Me.ExpensesTypeBS = ExpensesTypeBS
            TextBox1.DataBindings.Clear()
            TextBox2.DataBindings.Clear()
            TextBox3.DataBindings.Clear()
            TextBox4.DataBindings.Clear()
            TextBox5.DataBindings.Clear()
            TextBox6.DataBindings.Clear()
            TextBox7.DataBindings.Clear()
            TextBox8.DataBindings.Clear()
            TextBox9.DataBindings.Clear()



            DataGridView1.AutoGenerateColumns = False
            DataGridView1.DataSource = DTLBS

            TextBox1.DataBindings.Add(New Binding("Text", hddrv, "refnumber", True, DataSourceUpdateMode.OnPropertyChanged, ""))
            TextBox2.DataBindings.Add(New Binding("Text", hddrv, "applicantname", True, DataSourceUpdateMode.OnPropertyChanged, ""))
            TextBox3.DataBindings.Add(New Binding("Text", hddrv, "reason", True, DataSourceUpdateMode.OnPropertyChanged, ""))
            TextBox4.DataBindings.Add(New Binding("Text", hddrv, "bpartneraddress", True, DataSourceUpdateMode.OnPropertyChanged, ""))
            TextBox9.DataBindings.Add(New Binding("Text", hddrv, "bpartnerfullname", True, DataSourceUpdateMode.OnPropertyChanged, ""))

            TextBox5.DataBindings.Add(New Binding("Text", hddrv, "instruction", True, DataSourceUpdateMode.OnPropertyChanged, ""))
            TextBox6.DataBindings.Add(New Binding("Text", hddrv, "sendto", True, DataSourceUpdateMode.OnPropertyChanged, ""))


            DateTimePicker1.DataBindings.Add(New Binding("Text", hddrv, "applicantdate", True, DataSourceUpdateMode.OnPropertyChanged, ""))
            DateTimePicker2.DataBindings.Add(New Binding("Text", hddrv, "deliverydate", True, DataSourceUpdateMode.OnPropertyChanged, ""))

            calculateTotal()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Private Sub DeleteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteToolStripMenuItem.Click
        If Not IsNothing(DTLBS) Then
            If MessageBox.Show("Delete this record?", "Delete Record", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = DialogResult.OK Then
                For Each drv As DataGridViewRow In DataGridView1.SelectedRows
                    DTLBS.RemoveAt(drv.Index)
                Next
            End If
        End If
    End Sub

    Private Sub RefreshDataGridView()
        DataGridView1.Invalidate()
    End Sub

    Private Sub calculateTotal()        
        Dim total2 As Decimal
        _total = 0
        For Each dtldrv As DataRowView In DTLBS.List
            _total = _total + (dtldrv.Item("qty") * dtldrv.Item("price"))
            Dim confirmedqty = IIf(IsDBNull(dtldrv.Item("confirmedqty")), 0, dtldrv.Item("confirmedqty"))
            total2 = total2 + (confirmedqty * dtldrv.Item("price"))
        Next
        TextBox7.Text = String.Format("{0:#,##0.00}", _total)
        TextBox8.Text = String.Format("{0:#,##0.00}", total2)
        DataGridView1.Invalidate()
    End Sub


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'Dim myhelper As New DialogHelper(BPartnerBS)
        'myhelper.DataGridView1.Columns(0).DataPropertyName = "bpartnerfullname"
        'myhelper.DataGridView1.Columns(0).HeaderText = "Business Partner"
        'myhelper.DataGridView1.Columns(0).Width = 300
        'myhelper.DataGridView1.Columns(1).DataPropertyName = "bpartneraddress"
        'myhelper.DataGridView1.Columns(1).HeaderText = "Address"
        'myhelper.DataGridView1.Columns(1).Width = 400
        'myhelper.DataGridView1.Columns(2).Visible = False
        'myhelper.DataGridView1.Columns(3).Visible = False
        'myhelper.Size = New Point(800, 500)
        'myhelper.Filter = "bpartnerfullname like '%{0}%' or bpartneraddress like '%{0}%'"
        'myhelper.ShowDialog()
        'Dim mydrv = BPartnerBS.Current
        'TextBox9.Text = mydrv.item("bpartnerfullname")
        'TextBox4.Text = mydrv.item("bpartneraddress")
        'HDDRV.Item("bpartnerid") = mydrv.item("id")
        'HDDRV.Item("bpartneraddress") = mydrv.item("bpartneraddress")
        'HDDRV.Item("bpartnername") = mydrv.item("bpartnerfullname")
        'HDDRV.Item("bpartnerfullname") = mydrv.item("bpartnerfullname")
        'HDDRV.Item("region") = mydrv.item("region")
        'HDDRV.Item("country") = mydrv.item("country")
    End Sub
End Class

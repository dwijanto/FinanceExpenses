Public Class UCUser
    Public Shared Event RefreshDGV()
    Private HelperBS As BindingSource
    Dim DRV As DataRowView

    Dim nLevelapprovalBS As BindingSource
    Dim nlevelApprovals As New List(Of nLevelApproval)
    Public Overloads Function validate() As Boolean
        Dim myret As Boolean = True
        myret = CheckErrorTextBox(TextBox1, "Field cannot be blank.") And
                CheckErrorTextBox(TextBox2, "Field cannot be blank.") And
                CheckErrorTextBox(TextBox3, "Field cannot be blank.") And
                CheckErrorTextBox(TextBox4, "Field cannot be blank.") And
                CheckErrorTextBox(TextBox6, "Field cannot be blank.") And
        CheckErrorTextBox(TextBox7, "Field cannot be blank.")
        'CheckErrorTextBox(TextBox5, "Field cannot be blank.") And   
        Return myret
    End Function

    Private Function CheckErrorTextBox(ByRef obj As TextBox, ByVal errmsg As String) As Boolean
        ErrorProvider1.SetError(obj, "")
        Dim myret As Boolean = True
        If obj.Text.Length = 0 Then
            ErrorProvider1.SetError(obj, errmsg)
            myret = False
        End If
        Return myret
    End Function


    Public Sub BindingControl(ByRef drv As DataRowView, ByRef HelperBS As BindingSource)
        Me.HelperBS = HelperBS
        Me.DRV = drv
        TextBox1.DataBindings.Clear()
        TextBox2.DataBindings.Clear()
        TextBox3.DataBindings.Clear()
        TextBox4.DataBindings.Clear()
        'TextBox5.DataBindings.Clear()
        TextBox6.DataBindings.Clear()
        TextBox7.DataBindings.Clear()
        CheckBox1.DataBindings.Clear()
        ComboBox1.DataSource = nLevelapprovalBS
        ComboBox1.ValueMember = "id"
        ComboBox1.DisplayMember = "description"

        TextBox1.DataBindings.Add(New Binding("Text", drv, "userid", True, DataSourceUpdateMode.OnPropertyChanged))
        TextBox2.DataBindings.Add(New Binding("Text", drv, "username", True, DataSourceUpdateMode.OnPropertyChanged))
        TextBox3.DataBindings.Add(New Binding("Text", drv, "employeenumber", True, DataSourceUpdateMode.OnPropertyChanged))
        TextBox4.DataBindings.Add(New Binding("Text", drv, "email", True, DataSourceUpdateMode.OnPropertyChanged))
        'TextBox5.DataBindings.Add(New Binding("Text", drv, "nlevelapproval", True, DataSourceUpdateMode.OnPropertyChanged))
        ComboBox1.DataBindings.Add(New Binding("selectedvalue", drv, "nlevelapproval", True, DataSourceUpdateMode.OnPropertyChanged))

        TextBox6.DataBindings.Add(New Binding("Text", drv, "approvercode", True, DataSourceUpdateMode.OnPropertyChanged))
        TextBox7.DataBindings.Add(New Binding("Text", drv, "approvername", True, DataSourceUpdateMode.OnPropertyChanged))
        CheckBox1.DataBindings.Add(New Binding("checked", drv, "isactive", True, DataSourceUpdateMode.OnPropertyChanged))
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged, TextBox2.TextChanged, TextBox3.TextChanged, TextBox4.TextChanged, TextBox6.TextChanged, TextBox7.TextChanged, CheckBox1.CheckedChanged
        RefreshDataGridView()
    End Sub

    Public Sub RefreshDataGridView()
        RaiseEvent RefreshDGV()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        HelperBS.Filter = ""
        Dim myform = New FormHelper(HelperBS)

        myform.DataGridView1.Columns(0).DataPropertyName = "description"
        myform.Filter = "[description] like '%{0}%'"
        If myform.ShowDialog() = DialogResult.OK Then
            Dim drvcurr As DataRowView = HelperBS.Current           
            TextBox6.Text = drvcurr.Row.Item("employeenumber")
            TextBox7.Text = drvcurr.Row.Item("username")
        Else

        End If

    End Sub

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        nLevelapprovalBS = New BindingSource

        nlevelApprovals.Add(New nLevelApproval With {.id = 0, .description = "Regular User"})
        nlevelApprovals.Add(New nLevelApproval With {.id = 1, .description = "Final Approver"})
        nLevelapprovalBS.DataSource = nlevelApprovals

    End Sub

    Private Sub ComboBox1_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles ComboBox1.SelectionChangeCommitted
        Dim myselected As nLevelApproval = ComboBox1.SelectedItem
        DRV.Row.Item("nlevelapprovalname") = myselected.description
    End Sub
End Class

Public Class nLevelApproval
    Public Property id
    Public Property description

    Public Overrides Function tostring() As String
        Return description
    End Function
End Class

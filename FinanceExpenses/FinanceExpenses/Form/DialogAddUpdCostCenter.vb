Imports System.Windows.Forms

Public Class DialogAddUpdCostCenter
    Private drv As DataRowView
    Public Shared Event RefreshDataGrid()

    Public Sub New(ByRef drv As DataRowView)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.drv = drv
    End Sub


    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If Validate() Then
            drv.EndEdit()
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Close()
        End If
        
    End Sub

    Public Overloads Function validate() As Boolean
        Dim myret As Boolean = True
        ErrorProvider1.SetError(TextBox1, "")
        If TextBox1.TextLength = 0 Then
            ErrorProvider1.SetError(TextBox1, "Value cannot be blank.")
            myret = False
        End If
        ErrorProvider1.SetError(TextBox2, "")
        If TextBox2.TextLength = 0 Then
            ErrorProvider1.SetError(TextBox2, "Value cannot be blank.")
            myret = False
        End If
        ErrorProvider1.SetError(TextBox3, "")
        If TextBox3.TextLength = 0 Then
            ErrorProvider1.SetError(TextBox3, "Value cannot be blank.")
            myret = False
        End If
        ErrorProvider1.SetError(TextBox4, "")
        If TextBox4.TextLength = 0 Then
            ErrorProvider1.SetError(TextBox4, "Value cannot be blank.")
            myret = False
        End If
        ErrorProvider1.SetError(TextBox5, "")
        If TextBox5.TextLength = 0 Then
            ErrorProvider1.SetError(TextBox5, "Value cannot be blank.")
            myret = False
        End If
        Return myret
    End Function

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        drv.CancelEdit()
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub DialogAddUpdCostCenter_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        InitData()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click, Button2.Click
        'Dim myform As New FormHelper(helperbs)
        'myform.ShowDialog()
    End Sub

    Private Sub InitData()
        TextBox1.DataBindings.Clear()
        TextBox2.DataBindings.Clear()
        TextBox3.DataBindings.Clear()
        TextBox4.DataBindings.Clear()
        TextBox5.DataBindings.Clear()

        TextBox1.DataBindings.Add(New Binding("Text", drv, "glaccount", False, DataSourceUpdateMode.OnPropertyChanged))
        TextBox2.DataBindings.Add(New Binding("Text", drv, "costcenter", False, DataSourceUpdateMode.OnPropertyChanged))
        TextBox3.DataBindings.Add(New Binding("Text", drv, "amount", True, DataSourceUpdateMode.OnPropertyChanged, "", "#,##0.00"))
        TextBox4.DataBindings.Add(New Binding("Text", drv, "remark", False, DataSourceUpdateMode.OnPropertyChanged))
        TextBox5.DataBindings.Add(New Binding("Text", drv, "crcy", False, DataSourceUpdateMode.OnPropertyChanged))
    End Sub


    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged, TextBox2.TextChanged, TextBox3.TextChanged, TextBox4.TextChanged
        RaiseEvent RefreshDataGrid()
    End Sub
End Class

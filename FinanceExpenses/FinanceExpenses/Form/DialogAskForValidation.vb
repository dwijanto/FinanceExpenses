Imports System.Windows.Forms

Public Class DialogAskForValidation

    Public Property getAskForValidationEmail As String
    Public Property getAskForValidationEmployeeNumber As String
    Public Property getAskForValidationName As String
    Public Property getNlevelApproval As Integer
    Public Property getRemark As String

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If validate() Then
            getAskForValidationName = TextBox1.Text
            getRemark = TextBox2.Text
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Close()
        End If

    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Public Overloads Function validate() As Boolean
        Dim myret = True
        ErrorProvider1.SetError(TextBox1, "")
        If TextBox1.TextLength = 0 Then
            ErrorProvider1.SetError(TextBox1, "Value cannot be blank.")
            myret = False
        End If
        Return myret
    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim helperbs As New BindingSource
        Dim mycontroller As New UserController
        helperbs = mycontroller.Model.GetUserBS
        helperbs.Filter = ""
        Dim myform = New FormHelper(helperbs)
        myform.Column1.Width = 400
        myform.Width = 600
        myform.DataGridView1.Columns(0).DataPropertyName = "description"

        myform.Filter = "[description] like '%{0}%'"
        If myform.ShowDialog() = DialogResult.OK Then
            Dim drvcurr As DataRowView = helperbs.Current
            TextBox1.Text = drvcurr.Row.Item("username")
            getAskForValidationName = drvcurr.Row.Item("username")
            getAskForValidationEmployeeNumber = drvcurr.Row.Item("employeenumber")
            getAskForValidationEmail = drvcurr.Row.Item("email")
            getNlevelApproval = drvcurr.Row.Item("nlevelapproval")
        Else

        End If

    End Sub

    'Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
    '    Me.DialogResult = System.Windows.Forms.DialogResult.OK
    '    Me.Close()
    'End Sub

    'Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
    '    Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
    '    Me.Close()
    'End Sub

End Class

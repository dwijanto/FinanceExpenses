Imports System.Windows.Forms

Public Class DialogForwardTo
    Public Property getForwardTo As String
    Public Property getRemark As String

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If validate() Then
            getForwardTo = TextBox1.Text
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

End Class

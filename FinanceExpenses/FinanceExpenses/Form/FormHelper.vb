Public Class FormHelper
    Dim bs As New BindingSource
    Public Filter As String
    Public Sub New(bs As BindingSource)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        DataGridView1.AutoGenerateColumns = False
        DataGridView1.DataSource = bs
        Me.bs = bs
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub


    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        Dim myfilter As String = String.Empty
        If TextBox1.Text.Length <> 0 Then
            myfilter = String.Format(Filter, TextBox1.Text)
        End If
        bs.Filter = myfilter
    End Sub
End Class
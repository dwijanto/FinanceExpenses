Public Class UCFinanceExpenses

    Private Sub AddNewToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AddNewToolStripMenuItem.Click
        Dim myform As New DialogAddUpdCostCenter
        myform.ShowDialog()
    End Sub
End Class

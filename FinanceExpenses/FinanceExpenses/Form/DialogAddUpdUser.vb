Imports System.Windows.Forms

Public Class DialogAddUpdUser
    Dim Drv As DataRowView
    Dim HelperBS As BindingSource

    Public Sub New(drv As DataRowView, Helperbs As BindingSource)
        InitializeComponent()
        Me.Drv = drv
        Me.HelperBS = Helperbs
    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If UcUser1.validate Then
            Drv.EndEdit()
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Drv.CancelEdit()
        UcUser1.RefreshDataGridView()
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
        
    End Sub

    Private Sub DialogAddUpdUser_Load(sender As Object, e As EventArgs) Handles Me.Load
        UcUser1.BindingControl(Drv, HelperBS)
    End Sub

End Class

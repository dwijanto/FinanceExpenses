Imports System.ComponentModel

Public Class UCParams
    Dim myController As ParamAdapter

    Public Sub BindingControl(ByRef myController As ParamAdapter)
        Me.myController = myController
        InitData()
    End Sub

   

    Private Sub InitData()

        TB1EmailLastReceived.DataBindings.Clear()
        TB2Url.DataBindings.Clear()
        TB3UserName.DataBindings.Clear()
        TB4Password.DataBindings.Clear()
        TB5BaseFolder.DataBindings.Clear()
        TB6Mailbox.DataBindings.Clear()
        TB7NotValidEmail.DataBindings.Clear()
        TB8FinanceTeam.DataBindings.Clear()

        'TB1EmailLastReceived.DataBindings.Add(New Binding("Text", myController.BS, "ts", True, DataSourceUpdateMode.OnPropertyChanged, "dd-MMM-yyyy HH:mm:ss"))
        TB1EmailLastReceived.DataBindings.Add(New Binding("Text", myController.BS, "ts", True, DataSourceUpdateMode.OnPropertyChanged, "", "dd-MMM-yyyy HH:mm:ss"))
        TB2Url.DataBindings.Add(New Binding("Text", myController.BS2, "cvalue", False, DataSourceUpdateMode.OnPropertyChanged))
        TB3UserName.DataBindings.Add(New Binding("Text", myController.BS3, "cvalue", False, DataSourceUpdateMode.OnPropertyChanged))
        TB4Password.DataBindings.Add(New Binding("Text", myController.BS4, "cvalue", False, DataSourceUpdateMode.OnPropertyChanged))
        TB5BaseFolder.DataBindings.Add(New Binding("Text", myController.BS5, "cvalue", False, DataSourceUpdateMode.OnPropertyChanged))
        TB6Mailbox.DataBindings.Add(New Binding("Text", myController.BS6, "cvalue", False, DataSourceUpdateMode.OnPropertyChanged))
        TB7NotValidEmail.DataBindings.Add(New Binding("Text", myController.BS7, "cvalue", False, DataSourceUpdateMode.OnPropertyChanged))
        TB8FinanceTeam.DataBindings.Add(New Binding("Text", myController.BS8, "cvalue", False, DataSourceUpdateMode.OnPropertyChanged))
    End Sub

    Private Sub UCParams_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class

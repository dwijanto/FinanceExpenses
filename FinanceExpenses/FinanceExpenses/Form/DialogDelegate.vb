Imports System.Windows.Forms

Public Class DialogDelegate
    Dim ProgressReportCallback1 As ProgressReportCallback = AddressOf myCallBack  'DoBackGround Progress Report ProgressReportCallback.Invoke
    Dim DoBackground1 As DoBackground = New DoBackground(Me, ProgressReportCallback1)

    Private Shared myform As DialogDelegate
    Dim myController As DelegateController
    Private DRV As DataRowView


    Public Shared Function getInstance()
        If myform Is Nothing Then
            myform = New DialogDelegate
        ElseIf myform.IsDisposed Then
            myform = New DialogDelegate
        End If
        Return myform
    End Function

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        AddHandler DoBackground.CallBack, AddressOf EventCallback 'DoBackGround Progress Report RaiseEvent CallBack

    End Sub


    Private Sub RefreshToolStripButton_Click(sender As Object, e As EventArgs) Handles Me.Load
        LoadData()
    End Sub

    Private Sub LoadData()
        DoBackground1.doThread(AddressOf DoWork)
    End Sub

    Sub DoWork()
        myController = New DelegateController
        Try
            Dim criteria = String.Format(" where staffcode = '{0}' ", DirectCast(User.identity, UserController).email)
            DoBackground1.ProgressReport(1, "Loading...Please wait.")
            If myController.loaddata(criteria) Then
                DoBackground1.ProgressReport(4, "CallBack")
            End If
            DoBackground1.ProgressReport(1, String.Format("Loading...Done. Records {0}", myController.BS.Count))
        Catch ex As Exception
            DoBackground1.ProgressReport(1, ex.Message)
        End Try
    End Sub

    Sub myCallBack()        
        If myController.BS.Count = 0 Then
            DRV = myController.GetNewRecord
            DRV.Row.Item("staffcode") = DirectCast(User.identity, UserController).email
            DRV.Row.Item("isactive") = True
        Else
            DRV = myController.GetCurrentRecord
        End If
        InitData()
    End Sub

    Private Sub EventCallback(sender As Object, e As EventArgs)
        Debug.Print(sender)
    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If Me.validate Then
            If myController.save() Then
                Me.DialogResult = System.Windows.Forms.DialogResult.OK
                Me.Close()
            Else
                If myController.ErrorMsg.Length = 0 Then
                    Me.Close()
                End If
            End If
        End If
    End Sub

    Public Overloads Function validate() As Boolean
        Dim myret As Boolean = True
        ErrorProvider1.SetError(TextBox1, "")
        If TextBox1.TextLength = 0 Then
            ErrorProvider1.SetError(TextBox1, "Value cannot be blank.")
            myret = False
        End If
        ErrorProvider1.SetError(DateTimePicker1, "")
        If Not DateTimePicker1.Checked Then
            ErrorProvider1.SetError(DateTimePicker1, "Date Time Picker must be enabled.")
            myret = False
        End If
        ErrorProvider1.SetError(DateTimePicker2, "")
        If Not DateTimePicker2.Checked Then
            ErrorProvider1.SetError(DateTimePicker2, "Date Time Picker must be enabled.")
            myret = False
        End If
        Return myret
    End Function

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        DRV.CancelEdit()
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub InitData()       
        TextBox1.DataBindings.Clear()       
        CheckBox1.DataBindings.Clear()
        DateTimePicker1.DataBindings.Clear()
        DateTimePicker2.DataBindings.Clear()

        TextBox1.DataBindings.Add(New Binding("Text", DRV, "delegateto", True, DataSourceUpdateMode.OnPropertyChanged))
        DateTimePicker1.DataBindings.Add(New Binding("Text", DRV, "startdate", True, DataSourceUpdateMode.OnPropertyChanged))
        DateTimePicker2.DataBindings.Add(New Binding("Text", DRV, "enddate", True, DataSourceUpdateMode.OnPropertyChanged))
        CheckBox1.DataBindings.Add(New Binding("checked", DRV, "isactive", True, DataSourceUpdateMode.OnPropertyChanged))
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim helperbs As New BindingSource
        Dim mycontroller As New UserController
        helperbs = myController.Model.GetUserBS
        HelperBS.Filter = ""
        Dim myform = New FormHelper(HelperBS)
        myform.Column1.Width = 400
        myform.Width = 600
        myform.DataGridView1.Columns(0).DataPropertyName = "description"

        myform.Filter = "[description] like '%{0}%'"
        If myform.ShowDialog() = DialogResult.OK Then
            Dim drvcurr As DataRowView = HelperBS.Current
            TextBox1.Text = drvcurr.Row.Item("email")
        Else

        End If

    End Sub
End Class

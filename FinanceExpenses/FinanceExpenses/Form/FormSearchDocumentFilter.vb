Public Class FormSearchDocumentFilter
    Dim ProgressReportCallback1 As ProgressReportCallback = AddressOf myCallBack  'DoBackGround Progress Report ProgressReportCallback.Invoke
    Dim DoBackground1 As DoBackground = New DoBackground(Me, ProgressReportCallback1)

    Private Shared myform As FormSearchDocumentFilter
    Dim myController As EmailController
    Dim Criteria As String
    Dim SelectedFolder As String
    Dim myParam As ParamAdapter = ParamAdapter.getInstance
    Dim basefolder As String

    Public Shared Function getInstance()
        If myform Is Nothing Then
            myform = New FormSearchDocumentFilter
        ElseIf myform.IsDisposed Then
            myform = New FormSearchDocumentFilter
        End If
        Return myform
    End Function

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        AddHandler DoBackground.CallBack, AddressOf EventCallback 'DoBackGround Progress Report RaiseEvent CallBack
    End Sub



    Private Sub RefreshToolStripButton_Click(sender As Object, e As EventArgs) Handles RefreshToolStripButton.Click
        LoadData()
    End Sub

    Private Sub LoadData()
        If TextBox1.Text.Length = 0 Then
            MessageBox.Show("Year cannot be empty.")
            Exit Sub
        End If
        Criteria = TextBox1.Text
        DoBackground1.doThread(AddressOf DoWork)
    End Sub

    Sub DoWork()
        myController = New EmailController
        Try
            DoBackground1.ProgressReport(1, "Loading...Please wait.")
            basefolder = myParam.GetParamDetailCValue("basefolder")
            If myController.LoadSearchData(Criteria) Then
                DoBackground1.ProgressReport(4, "CallBack")
            End If
            DoBackground1.ProgressReport(1, String.Format("Loading...Done. Records {0}", myController.BS.Count))
        Catch ex As Exception
            DoBackground1.ProgressReport(1, ex.Message)
        End Try
    End Sub

    Sub myCallBack()
        DataGridView1.AutoGenerateColumns = False
        DataGridView1.DataSource = myController.BS
        ToolStripTextBox1.Enabled = myController.BS.Count > 0
        EnabledButton()
    End Sub

    Private Sub EventCallback(sender As Object, e As EventArgs)
        Debug.Print(sender)
    End Sub


    Private Sub ToolStripTextBox1_TextChanged(sender As Object, e As EventArgs) Handles ToolStripTextBox1.TextChanged
        myController.ApplyFilter = String.Format("billref like '%{0}%' or financenumber like '%{0}%' or vendorcode like '%{0}%' or vendorname like '%{0}%'", ToolStripTextBox1.Text)
        EnabledButton()
    End Sub

    Private Sub EnabledButton()
        Button1.Enabled = myController.BS.Count > 0
        Button2.Enabled = myController.BS.Count > 0
        CheckBox1.Enabled = myController.BS.Count > 0
    End Sub

   
   
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Validate()
        If Not DoBackground1.myThread.IsAlive Then
            'get Folder

            Dim myfolder = New FolderBrowserDialog
            If myfolder.ShowDialog = Windows.Forms.DialogResult.OK Then
                myform.ToolStripStatusLabel1.Text = ""
                SelectedFolder = myfolder.SelectedPath
                DoBackground1.doThread(AddressOf DoDownload)
            End If
        Else
        MessageBox.Show("Please wait until the current process is finished.")
        End If
    End Sub

    Private Sub DoDownload()
        Dim i As Integer = 0
        Dim selectedItems As Integer
        For Each drv As DataRowView In myController.BS.List
            If drv.Row.Item("isselected") Then
                selectedItems += 1
            End If
        Next
        For Each drv As DataRowView In myController.BS.List
            If drv.Row.Item("isselected") Then
                i = i + 1
                DoBackground1.ProgressReport(1, String.Format("Downloading ::{0} of {1} {2}", i, selectedItems, drv.Row.Item("attachmentname")))
                Try
                    Dim FullPathSourceFile = String.Format("{0}\{1:yyyyMM}\{1:ddHHmmss}{2}", basefolder, drv.Row.Item("receiveddate"), drv.Row.Item("attachmentname"))
                    Dim Targetfolder As String = String.Format("{0}\{1}", SelectedFolder, drv.Row.Item("billref"))
                    Dim TargetFile As String = String.Format("{0}\{1}", Targetfolder, drv.Row.Item("attachmentname"))
                    DoBackground1.ProgressReport(1, String.Format("Copy in progress {0} [{1} of {2}]", TargetFile, i, selectedItems))
                    If Not IO.Directory.Exists(SelectedFolder) Then
                        IO.Directory.CreateDirectory(SelectedFolder)
                    End If
                    FileIO.FileSystem.CopyFile(FullPathSourceFile, TargetFile, True)
                Catch ex As Exception
                End Try
            End If
        Next
        DoBackground1.ProgressReport(1, "Done. Please check your folder ::" & SelectedFolder)
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged

        If Not IsNothing(myController.BS) Then
            For Each drv As DataRowView In myController.BS.List
                drv.Row.Item("isselected") = CheckBox1.Checked
            Next
            myController.BS.EndEdit()
        End If


    End Sub


    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        ViewDocument()
    End Sub
    Private Sub ViewDocument()
        Dim drv As DataRowView = myController.BS.Current
        If Not IsNothing(drv) Then
            Dim myParamAdapter = ParamAdapter.getInstance
            Dim myFileController As New EmailController
            Dim FullPathFileName = String.Format("{0}\{1:yyyyMM}\{1:ddHHmmss}{2}", myParamAdapter.GetParamDetailCValue("basefolder"), drv.Row.Item("receiveddate"), drv.Row.Item("attachmentname"))
            myFileController.previewdoc(FullPathFileName, drv.Row.Item("attachmentname"))
        End If

    End Sub
End Class
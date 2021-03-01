Imports System.Text

Public Class FormSearchDocument
    Dim myController As EmailController
    Dim doBackground1 As New DoBackground(Me, AddressOf doBackgroundCallback)
    Dim doBackground2 As New DoBackground(Me, AddressOf doBackground2Callback)

    Private Shared myform As FormSearchDocument

    Dim myParam As ParamAdapter = ParamAdapter.getInstance
    Dim basefolder As String
    Dim sb As New StringBuilder

    Private selectedFolder As String

    Public Shared Function getInstance()
        If myform Is Nothing Then
            myform = New FormSearchDocument
        ElseIf myform.IsDisposed Then
            myform = New FormSearchDocument
        End If
        Return myform
    End Function


    Sub doBackgroundCallback()

    End Sub

    Sub doBackground2Callback()
        Throw New NotImplementedException
    End Sub

    Private Sub FormSearchDocument_Load(sender As Object, e As EventArgs) Handles Me.Load
        loaddata()
    End Sub

    Private Sub loaddata()
        doBackground1.doThread(AddressOf doWork1)
    End Sub
    Private Sub doWork1()
        basefolder = myParam.GetParamDetailCValue("basefolder")
    End Sub

    Private Sub doWork()
        myController = New EmailController
        Try
            doBackground1.ProgressReport(1, "Loading...Please wait.")
            basefolder = myParam.GetParamDetailCValue("basefolder")
            If myController.LoadSearchData() Then
                doBackground1.ProgressReport(4, "CallBack")
            End If
            doBackground1.ProgressReport(1, String.Format("Loading...Done. Records {0}", myController.GetReferencenumberFilterBS.Count))
        Catch ex As Exception
            doBackground1.ProgressReport(1, ex.Message)
        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)
        Try
            Dim helperbs As New BindingSource
            helperbs.Filter = ""
            Dim myform = New FormHelper(myController.GetReferencenumberFilterBS)
            myform.Column1.Width = 400
            myform.Width = 600
            myform.DataGridView1.Columns(0).DataPropertyName = "description"
            myform.Filter = "[description] like '%{0}%'"
            Dim myList As String()
            If myform.ShowDialog() = DialogResult.OK Then
                ReDim myList(myform.DataGridView1.SelectedCells.Count - 1)
                For i = 0 To myform.DataGridView1.SelectedCells.Count - 1
                    myList(i) = myform.DataGridView1.SelectedCells(i).Value
                Next
                ListBox1.Items.AddRange(myList)
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        ListBox1.Items.Clear()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If Not IsNothing(ListBox1.SelectedItem) Then
            If MessageBox.Show(String.Format("do you want to delete {0}?", ListBox1.SelectedItem.ToString), "Delete Item", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.OK Then
                ListBox1.Items.Remove(ListBox1.SelectedItem)
            End If
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If ListBox1.Items.Count > 0 Then
            'Get Folder
            'Copy Item
            Dim folderbrowserdialog1 As New FolderBrowserDialog
            sb.Clear()
            If folderbrowserdialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
                selectedFolder = folderbrowserdialog1.SelectedPath
                For Each Item In ListBox1.Items
                    If sb.Length > 0 Then
                        sb.Append(",")
                    End If
                    sb.Append(String.Format("'{0}'", Item))
                Next
            End If

            doBackground1.doThread(AddressOf GetRecord)
        End If
    End Sub

    Private Sub GetRecord()
        Dim sqlstr = String.Format("select date_part('Year',creationdate)::text || '-' || to_char(referencenumber,'FM000000') as description, receiveddate,dt.attachmentname,financenumber  from ssc.sscemailhd hd" &
            " left join ssc.sscemaildt dt on dt.hdid = hd.id where financenumber in ({0}) and status = 99", sb.ToString)
        Dim ds As DataSet = DataAccess.GetDataSet(sqlstr, CommandType.Text, Nothing)
        If ds.Tables(0).Rows.Count > 0 Then
            Dim i As Integer = 0
            For Each dr As DataRow In ds.Tables(0).Rows
                'Find Folder if not exist then create
                'After that copy to that folder
                i += 1
                Dim Targetfolder As String = String.Format("{0}\{1}-{2}", selectedFolder, dr.Item("description"), dr.Item("financenumber"))
                Dim TargetFile As String = String.Format("{0}\{1}", Targetfolder, dr.Item("attachmentname"))
                doBackground1.ProgressReport(1, String.Format("Copy in progress {0} [{1} of {2}]", TargetFile, i, ds.Tables(0).Rows.Count))
                Try
                    Dim FullPathSourceFile = String.Format("{0}\{1:yyyyMM}\{1:ddHHmmss}{2}", basefolder, dr.Item("receiveddate"), dr.Item("attachmentname"))

                    If Not IO.Directory.Exists(selectedFolder) Then
                        IO.Directory.CreateDirectory(selectedFolder)
                    End If
                    FileIO.FileSystem.CopyFile(FullPathSourceFile, TargetFile, True)
                Catch ex As Exception
                    MessageBox.Show(ex.Message)
                End Try

            Next
            doBackground1.ProgressReport(1, String.Format("Done"))
        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Dim mydata As String = My.Computer.Clipboard.GetText()
        mydata = mydata.Replace(vbLf, "")

        Dim arrdata() = mydata.Split(vbCr)
        'ListBox1.Items.AddRange(arrdata)
        For Each Item In arrdata
            If Item.Length > 0 Then
                ListBox1.Items.Add(Item)
            End If
        Next
    End Sub

   
End Class


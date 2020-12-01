Imports System.Threading

Public Enum AutoGenerateEnum
    Auto = 0
    Not_Auto = 1
End Enum

Public Class FormAutoGetEmail
    Delegate Sub ProgressReportDelegate(ByVal id As Integer, ByRef message As String)
    Public Property AutoGenerate As AutoGenerateEnum
    Dim myThread As New System.Threading.Thread(AddressOf doWork)
    Private Sub FormAutoGetEmail_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label1.Text = String.Format("Server = {0};", My.Settings.HOST)
        If AutoGenerate = AutoGenerateEnum.Auto Then
            Me.WindowState = FormWindowState.Minimized
            LoadMe()
        End If
    End Sub

    Private Sub LoadMe()
        'Label1.Text = String.Format("Server = {0};", My.Settings.HOST)
        If Not myThread.IsAlive Then
            Try
                'ToolStripStatusLabel1.Text = ""
                myThread = New System.Threading.Thread(AddressOf doWork)
                myThread.TrySetApartmentState(ApartmentState.MTA)
                myThread.Start()
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try
        End If


    End Sub

    Sub doWork()
        Logger.log("----Start ---")
        Logger.log("----Check Email ---")
        Dim myform As New FormGetEmail
        myform.DoWork()
        Logger.log("----Validate Email ---")
        Dim myValidTask As New SSCEmailTask
        myValidTask.ValidateEmail()
        Logger.log("----Send Valid Email ---")
        Dim ValidTask As New SSCEmailTask(SSCEmailTask.EmailTypeEnum.Valid)
        ValidTask.SendEmail()
        Logger.log("----Send inValid Email ---")
        Dim InValidTask As New SSCEmailTask(SSCEmailTask.EmailTypeEnum.Invalid)
        InValidTask.SendEmail()
        ProgressReport(4, "Close")
        Logger.log("----End ---")
    End Sub

    Private Sub ProgressReport(ByVal id As Integer, ByRef message As String)
        If Me.InvokeRequired Then
            Dim d As New ProgressReportDelegate(AddressOf ProgressReport)
            Me.Invoke(d, New Object() {id, message})
        Else
            Select Case id
                Case 1
                    Me.ToolStripStatusLabel1.Text = message
                Case 2

                Case 4
                    ' Me.Label4.Text = message
                    Me.Close()
                Case 5
                    'ToolStripProgressBar1.Style = ProgressBarStyle.Continuous
                Case 6
                    'ToolStripProgressBar1.Style = ProgressBarStyle.Marquee
                Case 7
                    'Dim myvalue = message.ToString.Split(",")
                    'ToolStripProgressBar1.Minimum = 1
                    'ToolStripProgressBar1.Value = myvalue(0)
                    'ToolStripProgressBar1.Maximum = myvalue(1)
            End Select

        End If

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If Not myThread.IsAlive Then
            Me.ToolStripStatusLabel1.Text = ("Processing..Please wait.")
            Me.ToolStripProgressBar1.Style = ProgressBarStyle.Marquee
            LoadMe()
        Else
            Me.ToolStripStatusLabel1.Text = ("Please wait until the current process is finished.")
        End If

    End Sub
End Class
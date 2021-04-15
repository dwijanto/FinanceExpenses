Imports System.Threading
Public Class FormReminder

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
        Dim mytask As New RemainderEmail

        If (Date.DaysInMonth(Today.Year, Today.Month)) - 2 = Today.Day Then
            Logger.log("----2 Days before end of month ---")
            mytask.twodaysbeforeendofmonth()
        Else
            Logger.log("----Daily Checking for 7 days overdue ---")
            mytask.SevenDaysOverdue()
        End If
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
                    Me.Close()
                Case 5
                    'ToolStripProgressBar1.Style = ProgressBarStyle.Continuous
                Case 6
                    'ToolStripProgressBar1.Style = ProgressBarStyle.Marquee
                Case 7
                   
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
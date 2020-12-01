<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UCParams
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TB1EmailLastReceived = New System.Windows.Forms.TextBox()
        Me.TB2Url = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TB3UserName = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TB4Password = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.TB5BaseFolder = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.TB6Mailbox = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.TB7NotValidEmail = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(40, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(95, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Email last received"
        '
        'TB1EmailLastReceived
        '
        Me.TB1EmailLastReceived.Location = New System.Drawing.Point(141, 13)
        Me.TB1EmailLastReceived.Name = "TB1EmailLastReceived"
        Me.TB1EmailLastReceived.Size = New System.Drawing.Size(298, 20)
        Me.TB1EmailLastReceived.TabIndex = 1
        '
        'TB2Url
        '
        Me.TB2Url.Location = New System.Drawing.Point(141, 39)
        Me.TB2Url.Name = "TB2Url"
        Me.TB2Url.Size = New System.Drawing.Size(298, 20)
        Me.TB2Url.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(117, 42)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(18, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "url"
        '
        'TB3UserName
        '
        Me.TB3UserName.Location = New System.Drawing.Point(141, 65)
        Me.TB3UserName.Name = "TB3UserName"
        Me.TB3UserName.Size = New System.Drawing.Size(298, 20)
        Me.TB3UserName.TabIndex = 5
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(75, 68)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(60, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "User Name"
        '
        'TB4Password
        '
        Me.TB4Password.Location = New System.Drawing.Point(141, 91)
        Me.TB4Password.Name = "TB4Password"
        Me.TB4Password.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.TB4Password.Size = New System.Drawing.Size(298, 20)
        Me.TB4Password.TabIndex = 7
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(82, 94)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(53, 13)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Password"
        '
        'TB5BaseFolder
        '
        Me.TB5BaseFolder.Location = New System.Drawing.Point(141, 117)
        Me.TB5BaseFolder.Name = "TB5BaseFolder"
        Me.TB5BaseFolder.Size = New System.Drawing.Size(298, 20)
        Me.TB5BaseFolder.TabIndex = 9
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(72, 120)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(63, 13)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "Base Folder"
        '
        'TB6Mailbox
        '
        Me.TB6Mailbox.Location = New System.Drawing.Point(141, 143)
        Me.TB6Mailbox.Name = "TB6Mailbox"
        Me.TB6Mailbox.Size = New System.Drawing.Size(298, 20)
        Me.TB6Mailbox.TabIndex = 11
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(92, 146)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(43, 13)
        Me.Label6.TabIndex = 10
        Me.Label6.Text = "Mailbox"
        '
        'TB7NotValidEmail
        '
        Me.TB7NotValidEmail.Location = New System.Drawing.Point(141, 169)
        Me.TB7NotValidEmail.Name = "TB7NotValidEmail"
        Me.TB7NotValidEmail.Size = New System.Drawing.Size(298, 20)
        Me.TB7NotValidEmail.TabIndex = 13
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(19, 172)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(116, 13)
        Me.Label7.TabIndex = 12
        Me.Label7.Text = "Not Valid Email send to"
        '
        'UCParams
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.TB7NotValidEmail)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.TB6Mailbox)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.TB5BaseFolder)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.TB4Password)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.TB3UserName)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.TB2Url)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.TB1EmailLastReceived)
        Me.Controls.Add(Me.Label1)
        Me.Name = "UCParams"
        Me.Size = New System.Drawing.Size(481, 209)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TB1EmailLastReceived As System.Windows.Forms.TextBox
    Friend WithEvents TB2Url As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TB3UserName As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents TB4Password As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents TB5BaseFolder As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents TB6Mailbox As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents TB7NotValidEmail As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label

End Class

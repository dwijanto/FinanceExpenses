<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormGenerateReport
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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
        Me.Button1 = New System.Windows.Forms.Button()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabel2 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripProgressBar1 = New System.Windows.Forms.ToolStripProgressBar()
        Me.DateTimePicker1 = New System.Windows.Forms.DateTimePicker()
        Me.DateTimePicker2 = New System.Windows.Forms.DateTimePicker()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.ButtonRef = New System.Windows.Forms.Button()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.ButtonInvoiceNumber = New System.Windows.Forms.Button()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.ButtonApprovalStatus = New System.Windows.Forms.Button()
        Me.TextBox3 = New System.Windows.Forms.TextBox()
        Me.ButtonAttn = New System.Windows.Forms.Button()
        Me.TextBox4 = New System.Windows.Forms.TextBox()
        Me.ButtonstApprover = New System.Windows.Forms.Button()
        Me.TextBox5 = New System.Windows.Forms.TextBox()
        Me.ButtonndApprover = New System.Windows.Forms.Button()
        Me.TextBox6 = New System.Windows.Forms.TextBox()
        Me.ButtonVendor = New System.Windows.Forms.Button()
        Me.TextBox7 = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(192, 256)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(94, 31)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "Generate"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1, Me.ToolStripStatusLabel2, Me.ToolStripProgressBar1})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 301)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(631, 22)
        Me.StatusStrip1.TabIndex = 1
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(0, 17)
        '
        'ToolStripStatusLabel2
        '
        Me.ToolStripStatusLabel2.Name = "ToolStripStatusLabel2"
        Me.ToolStripStatusLabel2.Size = New System.Drawing.Size(514, 17)
        Me.ToolStripStatusLabel2.Spring = True
        '
        'ToolStripProgressBar1
        '
        Me.ToolStripProgressBar1.Name = "ToolStripProgressBar1"
        Me.ToolStripProgressBar1.Size = New System.Drawing.Size(100, 16)
        '
        'DateTimePicker1
        '
        Me.DateTimePicker1.CustomFormat = "dd-MMM-yyyy"
        Me.DateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTimePicker1.Location = New System.Drawing.Point(192, 22)
        Me.DateTimePicker1.Name = "DateTimePicker1"
        Me.DateTimePicker1.Size = New System.Drawing.Size(129, 20)
        Me.DateTimePicker1.TabIndex = 2
        '
        'DateTimePicker2
        '
        Me.DateTimePicker2.CustomFormat = "dd-MMM-yyyy"
        Me.DateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTimePicker2.Location = New System.Drawing.Point(192, 48)
        Me.DateTimePicker2.Name = "DateTimePicker2"
        Me.DateTimePicker2.Size = New System.Drawing.Size(129, 20)
        Me.DateTimePicker2.TabIndex = 3
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(131, 28)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(55, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Start Date"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(134, 54)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(52, 13)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "End Date"
        '
        'ButtonRef
        '
        Me.ButtonRef.Location = New System.Drawing.Point(478, 71)
        Me.ButtonRef.Name = "ButtonRef"
        Me.ButtonRef.Size = New System.Drawing.Size(29, 23)
        Me.ButtonRef.TabIndex = 7
        Me.ButtonRef.Text = "..."
        Me.ButtonRef.UseVisualStyleBackColor = True
        '
        'TextBox1
        '
        Me.TextBox1.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.TextBox1.Location = New System.Drawing.Point(192, 74)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ReadOnly = True
        Me.TextBox1.Size = New System.Drawing.Size(280, 20)
        Me.TextBox1.TabIndex = 6
        '
        'ButtonInvoiceNumber
        '
        Me.ButtonInvoiceNumber.Location = New System.Drawing.Point(478, 97)
        Me.ButtonInvoiceNumber.Name = "ButtonInvoiceNumber"
        Me.ButtonInvoiceNumber.Size = New System.Drawing.Size(29, 23)
        Me.ButtonInvoiceNumber.TabIndex = 9
        Me.ButtonInvoiceNumber.Text = "..."
        Me.ButtonInvoiceNumber.UseVisualStyleBackColor = True
        '
        'TextBox2
        '
        Me.TextBox2.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.TextBox2.Location = New System.Drawing.Point(192, 100)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.ReadOnly = True
        Me.TextBox2.Size = New System.Drawing.Size(280, 20)
        Me.TextBox2.TabIndex = 8
        '
        'ButtonApprovalStatus
        '
        Me.ButtonApprovalStatus.Location = New System.Drawing.Point(478, 123)
        Me.ButtonApprovalStatus.Name = "ButtonApprovalStatus"
        Me.ButtonApprovalStatus.Size = New System.Drawing.Size(29, 23)
        Me.ButtonApprovalStatus.TabIndex = 11
        Me.ButtonApprovalStatus.Text = "..."
        Me.ButtonApprovalStatus.UseVisualStyleBackColor = True
        '
        'TextBox3
        '
        Me.TextBox3.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.TextBox3.Location = New System.Drawing.Point(192, 126)
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.ReadOnly = True
        Me.TextBox3.Size = New System.Drawing.Size(280, 20)
        Me.TextBox3.TabIndex = 10
        '
        'ButtonAttn
        '
        Me.ButtonAttn.Location = New System.Drawing.Point(478, 149)
        Me.ButtonAttn.Name = "ButtonAttn"
        Me.ButtonAttn.Size = New System.Drawing.Size(29, 23)
        Me.ButtonAttn.TabIndex = 13
        Me.ButtonAttn.Text = "..."
        Me.ButtonAttn.UseVisualStyleBackColor = True
        '
        'TextBox4
        '
        Me.TextBox4.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.TextBox4.Location = New System.Drawing.Point(192, 152)
        Me.TextBox4.Name = "TextBox4"
        Me.TextBox4.ReadOnly = True
        Me.TextBox4.Size = New System.Drawing.Size(280, 20)
        Me.TextBox4.TabIndex = 12
        '
        'ButtonstApprover
        '
        Me.ButtonstApprover.Location = New System.Drawing.Point(478, 175)
        Me.ButtonstApprover.Name = "ButtonstApprover"
        Me.ButtonstApprover.Size = New System.Drawing.Size(29, 23)
        Me.ButtonstApprover.TabIndex = 15
        Me.ButtonstApprover.Text = "..."
        Me.ButtonstApprover.UseVisualStyleBackColor = True
        '
        'TextBox5
        '
        Me.TextBox5.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.TextBox5.Location = New System.Drawing.Point(192, 178)
        Me.TextBox5.Name = "TextBox5"
        Me.TextBox5.ReadOnly = True
        Me.TextBox5.Size = New System.Drawing.Size(280, 20)
        Me.TextBox5.TabIndex = 14
        '
        'ButtonndApprover
        '
        Me.ButtonndApprover.Location = New System.Drawing.Point(478, 201)
        Me.ButtonndApprover.Name = "ButtonndApprover"
        Me.ButtonndApprover.Size = New System.Drawing.Size(29, 23)
        Me.ButtonndApprover.TabIndex = 17
        Me.ButtonndApprover.Text = "..."
        Me.ButtonndApprover.UseVisualStyleBackColor = True
        '
        'TextBox6
        '
        Me.TextBox6.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.TextBox6.Location = New System.Drawing.Point(192, 204)
        Me.TextBox6.Name = "TextBox6"
        Me.TextBox6.ReadOnly = True
        Me.TextBox6.Size = New System.Drawing.Size(280, 20)
        Me.TextBox6.TabIndex = 16
        '
        'ButtonVendor
        '
        Me.ButtonVendor.Location = New System.Drawing.Point(478, 227)
        Me.ButtonVendor.Name = "ButtonVendor"
        Me.ButtonVendor.Size = New System.Drawing.Size(29, 23)
        Me.ButtonVendor.TabIndex = 19
        Me.ButtonVendor.Text = "..."
        Me.ButtonVendor.UseVisualStyleBackColor = True
        '
        'TextBox7
        '
        Me.TextBox7.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.TextBox7.Location = New System.Drawing.Point(192, 230)
        Me.TextBox7.Name = "TextBox7"
        Me.TextBox7.ReadOnly = True
        Me.TextBox7.Size = New System.Drawing.Size(280, 20)
        Me.TextBox7.TabIndex = 18
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(89, 77)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(97, 13)
        Me.Label3.TabIndex = 20
        Me.Label3.Text = "Reference Number"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(104, 103)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(82, 13)
        Me.Label4.TabIndex = 21
        Me.Label4.Text = "Invoice Number"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(104, 129)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(82, 13)
        Me.Label5.TabIndex = 22
        Me.Label5.Text = "Approval Status"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(160, 155)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(26, 13)
        Me.Label6.TabIndex = 23
        Me.Label6.Text = "Attn"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(119, 181)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(67, 13)
        Me.Label7.TabIndex = 24
        Me.Label7.Text = "1st Approver"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(115, 207)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(71, 13)
        Me.Label8.TabIndex = 25
        Me.Label8.Text = "2nd Approver"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(117, 233)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(69, 13)
        Me.Label9.TabIndex = 26
        Me.Label9.Text = "Vendor Code"
        '
        'FormGenerateReport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(631, 323)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.ButtonVendor)
        Me.Controls.Add(Me.TextBox7)
        Me.Controls.Add(Me.ButtonndApprover)
        Me.Controls.Add(Me.TextBox6)
        Me.Controls.Add(Me.ButtonstApprover)
        Me.Controls.Add(Me.TextBox5)
        Me.Controls.Add(Me.ButtonAttn)
        Me.Controls.Add(Me.TextBox4)
        Me.Controls.Add(Me.ButtonApprovalStatus)
        Me.Controls.Add(Me.TextBox3)
        Me.Controls.Add(Me.ButtonInvoiceNumber)
        Me.Controls.Add(Me.TextBox2)
        Me.Controls.Add(Me.ButtonRef)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.DateTimePicker2)
        Me.Controls.Add(Me.DateTimePicker1)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.Button1)
        Me.Name = "FormGenerateReport"
        Me.Text = "FormGenerateReport"
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Public WithEvents ToolStripStatusLabel1 As System.Windows.Forms.ToolStripStatusLabel
    Public WithEvents ToolStripStatusLabel2 As System.Windows.Forms.ToolStripStatusLabel
    Public WithEvents ToolStripProgressBar1 As System.Windows.Forms.ToolStripProgressBar
    Friend WithEvents DateTimePicker1 As System.Windows.Forms.DateTimePicker
    Friend WithEvents DateTimePicker2 As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ButtonRef As System.Windows.Forms.Button
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents ButtonInvoiceNumber As System.Windows.Forms.Button
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents ButtonApprovalStatus As System.Windows.Forms.Button
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    Friend WithEvents ButtonAttn As System.Windows.Forms.Button
    Friend WithEvents TextBox4 As System.Windows.Forms.TextBox
    Friend WithEvents ButtonstApprover As System.Windows.Forms.Button
    Friend WithEvents TextBox5 As System.Windows.Forms.TextBox
    Friend WithEvents ButtonndApprover As System.Windows.Forms.Button
    Friend WithEvents TextBox6 As System.Windows.Forms.TextBox
    Friend WithEvents ButtonVendor As System.Windows.Forms.Button
    Friend WithEvents TextBox7 As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
End Class

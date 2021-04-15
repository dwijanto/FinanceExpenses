<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormMenu
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormMenu))
        Me.ToolStripContainer1 = New System.Windows.Forms.ToolStripContainer()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.ActionsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ProductRequestApprovalToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DelegateToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FinanceToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ParameterToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ChartOfAccountToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CostCenterFamilyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SAPIndexToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.UserToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MasterVendorToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DelegateStatusToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.SearchDocumentToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SearchDocumentFilterToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AdminToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RBACToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GetEmailFromServerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ReportToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ProductRequestToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.UserManualToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MasterToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.VendorToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.UcCutoffDate1 = New FinanceExpenses.UCCutoffDate()
        Me.SendReminderToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripContainer1.BottomToolStripPanel.SuspendLayout()
        Me.ToolStripContainer1.ContentPanel.SuspendLayout()
        Me.ToolStripContainer1.TopToolStripPanel.SuspendLayout()
        Me.ToolStripContainer1.SuspendLayout()
        Me.FlowLayoutPanel1.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ToolStripContainer1
        '
        '
        'ToolStripContainer1.BottomToolStripPanel
        '
        Me.ToolStripContainer1.BottomToolStripPanel.Controls.Add(Me.StatusStrip1)
        '
        'ToolStripContainer1.ContentPanel
        '
        Me.ToolStripContainer1.ContentPanel.Controls.Add(Me.FlowLayoutPanel1)
        Me.ToolStripContainer1.ContentPanel.Size = New System.Drawing.Size(718, 158)
        Me.ToolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ToolStripContainer1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStripContainer1.Name = "ToolStripContainer1"
        Me.ToolStripContainer1.Size = New System.Drawing.Size(718, 204)
        Me.ToolStripContainer1.TabIndex = 0
        Me.ToolStripContainer1.Text = "ToolStripContainer1"
        '
        'ToolStripContainer1.TopToolStripPanel
        '
        Me.ToolStripContainer1.TopToolStripPanel.Controls.Add(Me.MenuStrip1)
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Dock = System.Windows.Forms.DockStyle.None
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 0)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(718, 22)
        Me.StatusStrip1.TabIndex = 0
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.AutoScroll = True
        Me.FlowLayoutPanel1.Controls.Add(Me.UcCutoffDate1)
        Me.FlowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(718, 158)
        Me.FlowLayoutPanel1.TabIndex = 0
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Dock = System.Windows.Forms.DockStyle.None
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ActionsToolStripMenuItem, Me.FinanceToolStripMenuItem, Me.AdminToolStripMenuItem, Me.ReportToolStripMenuItem, Me.HelpToolStripMenuItem, Me.MasterToolStripMenuItem1})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(718, 24)
        Me.MenuStrip1.TabIndex = 0
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'ActionsToolStripMenuItem
        '
        Me.ActionsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ProductRequestApprovalToolStripMenuItem, Me.DelegateToolStripMenuItem})
        Me.ActionsToolStripMenuItem.Name = "ActionsToolStripMenuItem"
        Me.ActionsToolStripMenuItem.Size = New System.Drawing.Size(59, 20)
        Me.ActionsToolStripMenuItem.Text = "Actions"
        '
        'ProductRequestApprovalToolStripMenuItem
        '
        Me.ProductRequestApprovalToolStripMenuItem.Name = "ProductRequestApprovalToolStripMenuItem"
        Me.ProductRequestApprovalToolStripMenuItem.Size = New System.Drawing.Size(218, 22)
        Me.ProductRequestApprovalToolStripMenuItem.Text = "Expenses Request Approval"
        '
        'DelegateToolStripMenuItem
        '
        Me.DelegateToolStripMenuItem.Name = "DelegateToolStripMenuItem"
        Me.DelegateToolStripMenuItem.Size = New System.Drawing.Size(218, 22)
        Me.DelegateToolStripMenuItem.Text = "Delegate"
        '
        'FinanceToolStripMenuItem
        '
        Me.FinanceToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ParameterToolStripMenuItem, Me.ChartOfAccountToolStripMenuItem, Me.CostCenterFamilyToolStripMenuItem, Me.SAPIndexToolStripMenuItem, Me.UserToolStripMenuItem, Me.MasterVendorToolStripMenuItem, Me.DelegateStatusToolStripMenuItem1, Me.SearchDocumentToolStripMenuItem, Me.SearchDocumentFilterToolStripMenuItem})
        Me.FinanceToolStripMenuItem.Name = "FinanceToolStripMenuItem"
        Me.FinanceToolStripMenuItem.Size = New System.Drawing.Size(60, 20)
        Me.FinanceToolStripMenuItem.Text = "Finance"
        '
        'ParameterToolStripMenuItem
        '
        Me.ParameterToolStripMenuItem.Name = "ParameterToolStripMenuItem"
        Me.ParameterToolStripMenuItem.Size = New System.Drawing.Size(197, 22)
        Me.ParameterToolStripMenuItem.Tag = "FormParameters"
        Me.ParameterToolStripMenuItem.Text = "Parameter"
        '
        'ChartOfAccountToolStripMenuItem
        '
        Me.ChartOfAccountToolStripMenuItem.Name = "ChartOfAccountToolStripMenuItem"
        Me.ChartOfAccountToolStripMenuItem.Size = New System.Drawing.Size(197, 22)
        Me.ChartOfAccountToolStripMenuItem.Text = "Chart Of Account"
        '
        'CostCenterFamilyToolStripMenuItem
        '
        Me.CostCenterFamilyToolStripMenuItem.Name = "CostCenterFamilyToolStripMenuItem"
        Me.CostCenterFamilyToolStripMenuItem.Size = New System.Drawing.Size(197, 22)
        Me.CostCenterFamilyToolStripMenuItem.Text = "Cost Center Family"
        '
        'SAPIndexToolStripMenuItem
        '
        Me.SAPIndexToolStripMenuItem.Name = "SAPIndexToolStripMenuItem"
        Me.SAPIndexToolStripMenuItem.Size = New System.Drawing.Size(197, 22)
        Me.SAPIndexToolStripMenuItem.Text = "SAP Index"
        '
        'UserToolStripMenuItem
        '
        Me.UserToolStripMenuItem.Name = "UserToolStripMenuItem"
        Me.UserToolStripMenuItem.Size = New System.Drawing.Size(197, 22)
        Me.UserToolStripMenuItem.Tag = "FormUser"
        Me.UserToolStripMenuItem.Text = "User"
        '
        'MasterVendorToolStripMenuItem
        '
        Me.MasterVendorToolStripMenuItem.Name = "MasterVendorToolStripMenuItem"
        Me.MasterVendorToolStripMenuItem.Size = New System.Drawing.Size(197, 22)
        Me.MasterVendorToolStripMenuItem.Tag = "FormMasterVendor"
        Me.MasterVendorToolStripMenuItem.Text = "Master Vendor"
        '
        'DelegateStatusToolStripMenuItem1
        '
        Me.DelegateStatusToolStripMenuItem1.Name = "DelegateStatusToolStripMenuItem1"
        Me.DelegateStatusToolStripMenuItem1.Size = New System.Drawing.Size(197, 22)
        Me.DelegateStatusToolStripMenuItem1.Tag = "FormDelegateStatus"
        Me.DelegateStatusToolStripMenuItem1.Text = "Delegate Status"
        '
        'SearchDocumentToolStripMenuItem
        '
        Me.SearchDocumentToolStripMenuItem.Name = "SearchDocumentToolStripMenuItem"
        Me.SearchDocumentToolStripMenuItem.Size = New System.Drawing.Size(197, 22)
        Me.SearchDocumentToolStripMenuItem.Tag = "FormSearchDocument"
        Me.SearchDocumentToolStripMenuItem.Text = "Search Document"
        '
        'SearchDocumentFilterToolStripMenuItem
        '
        Me.SearchDocumentFilterToolStripMenuItem.Name = "SearchDocumentFilterToolStripMenuItem"
        Me.SearchDocumentFilterToolStripMenuItem.Size = New System.Drawing.Size(197, 22)
        Me.SearchDocumentFilterToolStripMenuItem.Tag = "FormSearchDocumentFilter"
        Me.SearchDocumentFilterToolStripMenuItem.Text = "Search Document Filter"
        '
        'AdminToolStripMenuItem
        '
        Me.AdminToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.RBACToolStripMenuItem, Me.GetEmailFromServerToolStripMenuItem, Me.SendReminderToolStripMenuItem})
        Me.AdminToolStripMenuItem.Name = "AdminToolStripMenuItem"
        Me.AdminToolStripMenuItem.Size = New System.Drawing.Size(55, 20)
        Me.AdminToolStripMenuItem.Text = "Admin"
        '
        'RBACToolStripMenuItem
        '
        Me.RBACToolStripMenuItem.Name = "RBACToolStripMenuItem"
        Me.RBACToolStripMenuItem.Size = New System.Drawing.Size(181, 22)
        Me.RBACToolStripMenuItem.Tag = "FormRBAC"
        Me.RBACToolStripMenuItem.Text = "RBAC"
        '
        'GetEmailFromServerToolStripMenuItem
        '
        Me.GetEmailFromServerToolStripMenuItem.Name = "GetEmailFromServerToolStripMenuItem"
        Me.GetEmailFromServerToolStripMenuItem.Size = New System.Drawing.Size(181, 22)
        Me.GetEmailFromServerToolStripMenuItem.Text = "GetEmailFromServer"
        '
        'ReportToolStripMenuItem
        '
        Me.ReportToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ProductRequestToolStripMenuItem})
        Me.ReportToolStripMenuItem.Name = "ReportToolStripMenuItem"
        Me.ReportToolStripMenuItem.Size = New System.Drawing.Size(54, 20)
        Me.ReportToolStripMenuItem.Text = "Report"
        '
        'ProductRequestToolStripMenuItem
        '
        Me.ProductRequestToolStripMenuItem.Name = "ProductRequestToolStripMenuItem"
        Me.ProductRequestToolStripMenuItem.Size = New System.Drawing.Size(160, 22)
        Me.ProductRequestToolStripMenuItem.Text = "Expenses Report"
        '
        'HelpToolStripMenuItem
        '
        Me.HelpToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.UserManualToolStripMenuItem})
        Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(44, 20)
        Me.HelpToolStripMenuItem.Text = "Help"
        '
        'UserManualToolStripMenuItem
        '
        Me.UserManualToolStripMenuItem.Name = "UserManualToolStripMenuItem"
        Me.UserManualToolStripMenuItem.Size = New System.Drawing.Size(140, 22)
        Me.UserManualToolStripMenuItem.Text = "User Manual"
        '
        'MasterToolStripMenuItem1
        '
        Me.MasterToolStripMenuItem1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.VendorToolStripMenuItem})
        Me.MasterToolStripMenuItem1.Name = "MasterToolStripMenuItem1"
        Me.MasterToolStripMenuItem1.Size = New System.Drawing.Size(55, 20)
        Me.MasterToolStripMenuItem1.Text = "Master"
        '
        'VendorToolStripMenuItem
        '
        Me.VendorToolStripMenuItem.Name = "VendorToolStripMenuItem"
        Me.VendorToolStripMenuItem.Size = New System.Drawing.Size(150, 22)
        Me.VendorToolStripMenuItem.Text = "Master Vendor"
        '
        'UcCutoffDate1
        '
        Me.UcCutoffDate1.Location = New System.Drawing.Point(3, 3)
        Me.UcCutoffDate1.Name = "UcCutoffDate1"
        Me.UcCutoffDate1.Size = New System.Drawing.Size(537, 147)
        Me.UcCutoffDate1.TabIndex = 0
        '
        'SendReminderToolStripMenuItem
        '
        Me.SendReminderToolStripMenuItem.Name = "SendReminderToolStripMenuItem"
        Me.SendReminderToolStripMenuItem.Size = New System.Drawing.Size(181, 22)
        Me.SendReminderToolStripMenuItem.Text = "Send Reminder"
        '
        'FormMenu
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(718, 204)
        Me.Controls.Add(Me.ToolStripContainer1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "FormMenu"
        Me.Text = "FormMenu"
        Me.ToolStripContainer1.BottomToolStripPanel.ResumeLayout(False)
        Me.ToolStripContainer1.BottomToolStripPanel.PerformLayout()
        Me.ToolStripContainer1.ContentPanel.ResumeLayout(False)
        Me.ToolStripContainer1.TopToolStripPanel.ResumeLayout(False)
        Me.ToolStripContainer1.TopToolStripPanel.PerformLayout()
        Me.ToolStripContainer1.ResumeLayout(False)
        Me.ToolStripContainer1.PerformLayout()
        Me.FlowLayoutPanel1.ResumeLayout(False)
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ToolStripContainer1 As System.Windows.Forms.ToolStripContainer
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents ActionsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FinanceToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ParameterToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AdminToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RBACToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents UserToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ProductRequestApprovalToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ReportToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ProductRequestToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HelpToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents UserManualToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GetEmailFromServerToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ChartOfAccountToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DelegateToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MasterVendorToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CostCenterFamilyToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SAPIndexToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DelegateStatusToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MasterToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents VendorToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SearchDocumentToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SearchDocumentFilterToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FlowLayoutPanel1 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents UcCutoffDate1 As FinanceExpenses.UCCutoffDate
    Friend WithEvents SendReminderToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem

End Class

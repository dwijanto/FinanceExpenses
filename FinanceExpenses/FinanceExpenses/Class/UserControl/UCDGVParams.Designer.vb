﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UCDGVParams
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
        Me.components = New System.ComponentModel.Container()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewRecordToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DeleteRecordToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        CType(Me.DataGridView1,System.ComponentModel.ISupportInitialize).BeginInit
        Me.ContextMenuStrip1.SuspendLayout
        Me.SuspendLayout
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = false
        Me.DataGridView1.AllowUserToDeleteRows = false
        Me.DataGridView1.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight
        Me.DataGridView1.ColumnHeadersHeight = 35
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column2})
        Me.DataGridView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataGridView1.Location = New System.Drawing.Point(0, 0)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DataGridView1.Size = New System.Drawing.Size(629, 303)
        Me.DataGridView1.TabIndex = 0
        '
        'Column1
        '
        Me.Column1.DataPropertyName = "approvaltype"
        Me.Column1.HeaderText = "Param Name"
        Me.Column1.Name = "Column1"
        Me.Column1.ReadOnly = true
        Me.Column1.Width = 150
        '
        'Column2
        '
        Me.Column2.DataPropertyName = "approvalname"
        Me.Column2.HeaderText = "Param Value"
        Me.Column2.Name = "Column2"
        Me.Column2.Width = 200
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewRecordToolStripMenuItem, Me.DeleteRecordToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(153, 70)
        '
        'NewRecordToolStripMenuItem
        '
        Me.NewRecordToolStripMenuItem.Name = "NewRecordToolStripMenuItem"
        Me.NewRecordToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.NewRecordToolStripMenuItem.Text = "New Record"
        '
        'DeleteRecordToolStripMenuItem
        '
        Me.DeleteRecordToolStripMenuItem.Name = "DeleteRecordToolStripMenuItem"
        Me.DeleteRecordToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.DeleteRecordToolStripMenuItem.Text = "Delete Record"
        '
        'UCDGVParams
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.DataGridView1)
        Me.Name = "UCDGVParams"
        Me.Size = New System.Drawing.Size(629, 303)
        CType(Me.DataGridView1,System.ComponentModel.ISupportInitialize).EndInit
        Me.ContextMenuStrip1.ResumeLayout(false)
        Me.ResumeLayout(false)

End Sub
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewRecordToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DeleteRecordToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem

End Class

Imports System.Reflection
Public Enum TxEnum
    NewRecord = 1
    CopyRecord = 2
    UpdateRecord = 3
    HistoryRecord = 4
    ValidateRecord = 5
End Enum
Public Class FormMenu

    Private UserInfo1 As UserInfo = UserInfo.getInstance
    Dim HasError As Boolean = True
    Private userid As String
    Private myuser As UserController
    Dim myRbac As New DbManager

    Public Sub New()
        myuser = New UserController
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.

        Try
            UserInfo1.Userid = Environment.UserDomainName & "\" & Environment.UserName
            'UserInfo1.Userid = "AS\cchan"
            'UserInfo1.Userid = "AS\btam"
            'UserInfo1.Userid = "AS\mto"
            'UserInfo1.Userid = "AS\pbarnes"
            'UserInfo1.Userid = "AS\kliang"
            UserInfo1.computerName = My.Computer.Name
            UserInfo1.ApplicationName = "Finance Expenses Apps"
            UserInfo1.Username = Environment.UserDomainName & "\" & Environment.UserName
            UserInfo1.isAuthenticate = False
            UserInfo1.isAdmin = DataAccess.isAdmin(UserInfo1.Userid)
            HasError = False
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub
    Private Sub FormMenu_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If HasError Then
            Me.Close()
            Exit Sub
        End If

        Try
            userid = UserInfo1.Userid

            Dim myAD = New ADPrincipalContext
            Dim UserInfo As List(Of ADPrincipalContext) = New List(Of ADPrincipalContext)
            If myAD.GetInfo(userid) Then
                myuser.Model.ADDUPDUserManager(ADPrincipalContext.ADPrincipalContexts)
            Else
                MessageBox.Show(myAD.ErrorMessage)
                Me.Close()
                Exit Sub
            End If
            Dim mydata As DataSet = myuser.findByUserid(userid.ToLower)
            If mydata.tables(0).rows.count > 0 Then
                Dim identity = myuser.findIdentity(mydata.Tables(0).rows(0).item("id"))
                User.setIdentity(identity)
                User.login(identity)
                User.IdentityClass = myuser
                DataAccess.LogLogin(UserInfo1)
                Me.Text = GetMenuDesc()
                Me.Location = New Point(300, 10)
                MenuHandles()
            Else
                'disable menubar
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message)
            Me.Close()
        End Try

    End Sub
   
    Public Function GetMenuDesc() As String
        Return "App.Version: " & My.Application.Info.Version.ToString & " :: Server: " & DataAccess.GetHostName & ", Database: " & DataAccess.GetDataBaseName & ", Userid: " & UserInfo1.Userid 'HelperClass1.UserId
    End Function

    Private Sub MenuHandles()
        AddHandler RBACToolStripMenuItem.Click, AddressOf ToolStripMenuItem_Click
        'AddHandler UserToolStripMenuItem.Click, AddressOf ToolStripMenuItem_Click
        'AddHandler ParameterToolStripMenuItem.Click, AddressOf ToolStripMenuItem_Click
        'AddHandler CMMFToolStripMenuItem.Click, AddressOf ToolStripMenuItem_Click
        'AddHandler BPartnerToolStripMenuItem.Click, AddressOf ToolStripMenuItem_Click
        'AddHandler BPartnerAddressToolStripMenuItem.Click, AddressOf ToolStripMenuItem_Click

        'ActionsToolStripMenuItem.Visible = User.can("View Actions")
        'FindProductRequestToolStripMenuItem.Visible = User.can("Create Product Request") And (DirectCast(User.identity, UserController).deptid = DeptEnum.MarketingHK Or DirectCast(User.identity, UserController).deptid = DeptEnum.SalesHK) 'User with deptid in Sales HK and Marketing HK
        'CreateProductRequestToolStripMenuItem.Visible = User.can("Create Product Request") And (DirectCast(User.identity, UserController).deptid = DeptEnum.MarketingHK Or DirectCast(User.identity, UserController).deptid = DeptEnum.SalesHK) 'User with deptid in Sales HK and Marketing HK
        'ProductRequestApprovalToolStripMenuItem.Visible = User.can("View Product Request Approval")
        'ParameterToolStripMenuItem.Visible = User.can("View Master")
        AddHandler DelegateStatusToolStripMenuItem1.Click, AddressOf ToolStripMenuItem_Click
        AddHandler SearchDocumentToolStripMenuItem.Click, AddressOf ToolStripMenuItem_Click
        ReportToolStripMenuItem.Visible = User.can("Validate For Finance")
        FinanceToolStripMenuItem.Visible = User.can("View Master")
        MasterToolStripMenuItem1.Visible = User.can("Update Vendor")
        AdminToolStripMenuItem.Visible = User.can("View Admin")
    End Sub

    Private Sub ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim ctrl As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
        Dim assembly1 As Assembly = Assembly.GetAssembly(GetType(FormMenu))
        Dim frm As Object = CType(assembly1.CreateInstance(assembly1.GetName.Name.ToString & "." & ctrl.Tag.ToString, True), Form)
        Dim myform = frm.GetInstance
        myform.show()
        myform.windowstate = FormWindowState.Normal
        myform.activate()
    End Sub

    Private Sub ProductRequestApprovalToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ProductRequestApprovalToolStripMenuItem.Click
        Dim myform = New FormMyTask
        myform.Show()
    End Sub

    Private Sub ParameterToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ParameterToolStripMenuItem.Click
        Dim myform As New FormParameter
        myform.Show()
    End Sub


    Private Sub UserManualToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UserManualToolStripMenuItem.Click
        Dim p As New System.Diagnostics.Process        
        p.StartInfo.FileName = Application.StartupPath & "\Help\ProductRequestHelp.pdf"
        Try
            p.Start()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Private Sub BPartnerToolStripMenuItem_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub BPartnerAddressToolStripMenuItem_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub GetEmailFromServerToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GetEmailFromServerToolStripMenuItem.Click
        'Dim myform As New FormGetEmail
        'myform.Show()
        Dim myform As New FormAutoGetEmail
        myform.AutoGenerate = AutoGenerateEnum.Not_Auto
        myform.Show()
    End Sub

    Private Sub UserToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UserToolStripMenuItem.Click
        Dim myform As New FormUser
        myform.Show()
    End Sub

    Private Sub ChartOfAccountToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ChartOfAccountToolStripMenuItem.Click
        Dim myform As New FormCOA
        myform.Show()
    End Sub

    Private Sub DelegateToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DelegateToolStripMenuItem.Click
        Dim myform As New DialogDelegate
        myform.ShowDialog()
    End Sub

    Private Sub RBACToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RBACToolStripMenuItem.Click
        
    End Sub

    Private Sub ProductRequestToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ProductRequestToolStripMenuItem.Click
        Dim myform As New FormGenerateReport
        myform.ShowDialog()
    End Sub

    Private Sub MasterVendorToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MasterVendorToolStripMenuItem.Click, VendorToolStripMenuItem.Click
        Dim myform As New FormMasterVendor
        myform.ShowDialog()
    End Sub

    Private Sub CostCenterFamilyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CostCenterFamilyToolStripMenuItem.Click
        Dim myform As New FormCostCenterFamily
        myform.Show()
    End Sub

    Private Sub SAPIndexToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SAPIndexToolStripMenuItem.Click
        Dim myform As New FormSAPIndex
        myform.Show()
    End Sub

    Private Sub DelegateStatusToolStripMenuItem_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub DelegateStatusToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles DelegateStatusToolStripMenuItem1.Click

    End Sub

    Private Sub VendorToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles VendorToolStripMenuItem.Click

    End Sub

    Private Sub SearchDocumentToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SearchDocumentToolStripMenuItem.Click

    End Sub
End Class

Imports System.Text

Public Class ApprovalModel
    Private FinanceEmailSB As New StringBuilder
    Private FinanceUserNameListSB As New StringBuilder

    Public ReadOnly Property FinanceEmailList As String
        Get
            Return FinanceEmailSB.ToString
        End Get
    End Property

    Public ReadOnly Property FinanceUserNameList As String
        Get
            Return FinanceUserNameListSB.ToString
        End Get
    End Property

    Public ReadOnly Property tablename As String
        Get
            Return "ssc.sscapproval"
        End Get

    End Property

    Public Sub New()
        InitFinanceApprover()
    End Sub

    'Public Function getApprover(employeenumber As String) As String
    '    Dim Sqlstr = String.Format("select approvercode from {0} where staffcode = '{1}';", tablename, employeenumber)
    '    Return DataAccess.ExecuteScalar(Sqlstr, CommandType.Text, Nothing)
    'End Function

    Public Function getApprover(employeenumber As String) As List(Of UserController)
        Dim Sqlstr = String.Format("select u.* from {0} a left join ssc.user u on u.employeenumber = a.approvercode where staffcode = '{1}';", tablename, employeenumber)
        Return DataAccess.ExecuteReader(Of List(Of UserController))(Sqlstr, CommandType.Text, AddressOf DataAccess.OnReadAnyList(Of UserController), Nothing)
    End Function

    Public Sub InitFinanceApprover()        
        Dim sqlstr = String.Format("select u.* from ssc.auth_assignment a left join ssc.user u on u.id = a.user_id::bigint where item_name = 'Finance'")
        Dim FinanceApprover As List(Of UserController) = DataAccess.ExecuteReader(Of List(Of UserController))(sqlstr, CommandType.Text, AddressOf DataAccess.OnReadAnyList(Of UserController), Nothing)
        FinanceEmailSB = New StringBuilder
        FinanceUserNameListSB = New StringBuilder
        For Each Item In FinanceApprover
            If FinanceEmailSB.Length > 0 Then
                FinanceEmailSB.Append(";")
                FinanceUserNameListSB.Append(";")
            End If
            FinanceEmailSB.Append(Item.email)
            FinanceUserNameListSB.Append(Item.username)
        Next
    End Sub


End Class

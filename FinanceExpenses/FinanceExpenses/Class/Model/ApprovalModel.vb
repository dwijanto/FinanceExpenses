Public Class ApprovalModel
    Public Property employeenumber As String
    Public Property approver As String
    Public ReadOnly Property tablename As String
        Get
            Return "ssc.sscapproval"
        End Get

    End Property

    Public Sub New()

    End Sub

    Public Function getApprover(employeenumber As String) As String
        Dim Sqlstr = String.Format("select approvercode from {0} where staffcode = '{1}';", tablename, employeenumber)
        Return DataAccess.ExecuteScalar(Sqlstr, CommandType.Text, Nothing)
    End Function
End Class

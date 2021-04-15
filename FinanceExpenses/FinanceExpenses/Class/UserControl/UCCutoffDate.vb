Public Class UCCutoffDate
    Dim myparam As ParamAdapter = ParamAdapter.getInstance

    Public Sub DisplayMessage()
        Dim mydate As Date = myparam.GetDateValue("cutoffdate")
        Label1.Text = String.Format("Invoice posting CUT-OFF date is {0:dddd} {2}{1} {0:MMMM yyyy}", mydate, getSuffix(mydate), mydate.Day)

    End Sub

    Private Function getSuffix(ByVal value As Date) As String
        Dim suffix As String
        If value.Day Mod 10 = 1 And value.Day Mod 100 <> 11 Then
            suffix = "st"
        ElseIf value.Day Mod 10 = 2 And value.Day Mod 100 <> 12 Then
            suffix = "nd"
        ElseIf value.Day Mod 10 = 3 And value.Day Mod 100 <> 13 Then
            suffix = "rd"
        Else
            suffix = "th"
        End If
        Return suffix
    End Function
End Class

Public NotInheritable Class Check
    Public Shared Function IsUserSignedIn(Page As Page) As Boolean
        If (Page.Session("UserId") Is Nothing) Then
            Return False
        Else
            Return True
        End If
    End Function




End Class

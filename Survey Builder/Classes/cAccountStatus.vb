Public Class cAccountStatus
    Public Enum Status
        Authorised
        Invalid
        Disabled
    End Enum

    Public Property Message As String
    Public Property Code As Status
End Class

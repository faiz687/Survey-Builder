Public Interface ILoginHandler
    Function ValidateADCredentials(ByVal Username As String, ByVal Password As String, Domain As String) As cAccountStatus
    Function CheckSystemAccess(ByVal Username As String) As cAccountStatus
End Interface

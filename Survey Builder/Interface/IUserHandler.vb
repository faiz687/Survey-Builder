Public Interface IUserHandler
    Function GetAllUsers(ByVal Name As cUser_SearchOptions) As List(Of cUser)
    Function GetUserById(ByVal UserId As Integer) As cUser
    Function DeleteUser(ByVal UserId As Integer) As Boolean
    Function GetActiveDirecotryUsers() As List(Of cUser)
    Function GetActiveDirectoryUserBySID(ByVal SID As String) As cUser
    Function UpdateUser(ByVal User As cUser) As Boolean
    Function AddUser(ByVal User As cUser) As Boolean

End Interface


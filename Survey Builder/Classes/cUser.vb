Public Class cUser
    Public Property SID As String
    Public Property UserId As Integer
    Public Property UserName As String
    Public Property Forename As String
    Public Property Surname As String
    Public Property Email As String
    Public Property Locked As Boolean
    Public Property Deleted As Boolean
    Public Property CreatedDateTime As DateTime
    Public Property Permission As cPermission
    Public ReadOnly Property FullName As String
        Get
            Return String.Format("{0} {1}", Forename, Surname)
        End Get
    End Property
    Public ReadOnly Property SearchName As String
        Get
            Return String.Format("{0} {1} -  {2}", Forename, Surname, Email)
        End Get
    End Property

End Class

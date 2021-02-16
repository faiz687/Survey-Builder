Imports System.Globalization

Public Class cSurvey
    Public Property SurveyId As Integer
    Public Property SurveyName As String
    Public Property Enabled As Boolean
    Public Property UserId As cUser
    Public Property LastModifiedDateTime As DateTime
    Public Property SurveyCreatedDateTime As DateTime
    Public Property SurveyVersion As Integer
    Public ReadOnly Property Disabled As String
        Get
            If Enabled = False Then Return True
            Return False
        End Get
    End Property

    Public ReadOnly Property LastModifiedDate As String
        Get
            Return LastModifiedDateTime.ToString("dd/MM/yyyy")
        End Get
    End Property
    Public ReadOnly Property SurveyCreatedDate As String
        Get
            Return SurveyCreatedDateTime.ToString("dd/MM/yyyy")
        End Get
    End Property

End Class

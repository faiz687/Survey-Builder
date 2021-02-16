Public Class cSurveyQuestions
    Public Property QuestionId As Integer
    Public Property Question As String
    Public Property QuestionDescription As String
    Public Property Mandatory As Boolean
    Public ReadOnly Property MandatoryCss As String
        Get
            If Mandatory = True Then Return "QuestionMandatory"
            Return ""
        End Get
    End Property
    Public Property Control As cControl
    Public Property QuestionCreatedDateTime As DateTime
    Public Property QuestionLastModifiedDateTime As DateTime
    Public Property Survey As cSurvey
    Public Property DisplayId As Integer
    Public Property Enabled As Boolean

End Class

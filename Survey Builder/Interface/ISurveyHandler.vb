Public Interface ISurveyHandler
    Function DoesSurveyExists(ByVal Survey As cSurvey) As Boolean
    Function CreateSurvey(ByVal Survey As cSurvey) As Integer
    Function GetAllSurvey(ByVal SurveyName As String) As List(Of cSurvey)
    Function GetSurveyById(ByVal SurveyId As Integer) As cSurvey
    Function UpdateSurveyName(Survey As cSurvey) As Boolean
    Function UpdateSurveyLastModifiedDate(Survey As cSurvey) As Boolean
    Function UpdateSurveyStatus(Survey As cSurvey) As Boolean
    Function DeleteSurveyById(Survey As cSurvey) As Boolean
    Function UpdateSurveyVersion(Survey As cSurvey) As Boolean
    Function GetSurveyVersion(ByVal SurveyId As Integer) As Integer

    Function GetAllSurveyEnabledDisabled(ByVal SurveyName As String) As List(Of cSurvey)

End Interface

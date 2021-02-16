Public Interface ISurveyQuestion
    Function GetAllQuesitons(ByVal SurveyID As Integer) As List(Of cSurveyQuestions)
    Function AddQuestionToSurvey(Question As cSurveyQuestions) As Integer
    Function UpdateQuestionById(Question As cSurveyQuestions) As Boolean
    Function UpdateQuestionDisplayID(Question As cSurveyQuestions) As Boolean
    Function UpdateAllDisplayIdsFromQuestionId(QuestionId As Integer, SurveyId As Integer) As Boolean
    Function DeleteQuestionById(QuestionId As Integer) As Boolean
    Function DeleteAllQuestionById(SurveyId As Integer) As Boolean

End Interface

Public Interface IResponsesHandler
    Function GetAllResponses(ByVal QuestionId As Integer) As List(Of cResponses)
    Function GetResponsesByQuestionIdAndControlId(ByVal QuestionId As Integer, ByVal ControlId As Integer) As List(Of cResponses)
    Function AddResponseForQuestion(Response As cResponses) As Boolean
    Function DeleteQuestionResponsesByQuestionID(QuestionId As Integer) As Boolean
    Function DeleteAllQuestionResponsesBySurveyId(SurveyId As Integer) As Boolean
End Interface

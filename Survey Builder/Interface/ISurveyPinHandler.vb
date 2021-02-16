Public Interface ISurveyPinHandler
    Function InsertPin(Pin As cSurveysPin) As Boolean
    Function GetSurveyPin(Pin As cSurveysPin) As cSurveysPin
    Function UpdatePinUsedGeneratedDateTime(Pin As cSurveysPin)
    Function DeleteSurveyPin(Pin As cSurveysPin)
End Interface

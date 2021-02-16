using SurveyBuilderApp.Classes;
using System;
using System.Collections.Generic;
using System.Text;
public interface ISurveyPinHandler
{
    string InsertPin(cSurveysPin Pin);
    string GetSurveyPin(cSurveysPin Pin);
    string UpdatePinUsedGeneratedDateTime(cSurveysPin Pin);
    string DeleteSurveyPin(cSurveysPin Pin);

}

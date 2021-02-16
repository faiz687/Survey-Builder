using SurveyBuilderApp.Classes;
using System;
using System.Collections.Generic;

namespace SurveyBuilderApp.Interfaces
{
    interface ISurveyHandler
    {
        string GetAllSurvey(string SurveyName);
        string GetUpdatedSurveyListFromServer(List<SurveyVersions> SurveyIdVersionList);
        String GetSurveyFromServer(UpdatedSurveys Survey);

    }
}

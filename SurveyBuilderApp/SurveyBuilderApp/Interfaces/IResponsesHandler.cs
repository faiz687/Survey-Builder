using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Numerics;
using System.Text;

namespace SurveyBuilderApp.Interfaces
{
    interface IResponsesHandler
    {      
        string GetAllResponses(int QuestionId);
    }
}

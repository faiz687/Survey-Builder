using SurveyBuilderApp.Classes;
using System;
using System.Collections.Generic;

namespace SurveyBuilderApp.Interfaces
{
    public interface IDatabaseService 
    {
        List<cSliderNumbersRange> GetSliderProperties (int QuestionId);
        void InsertSliderToDevice(cSliderNumbersRange SliderNumber);
        void DataConnection(string dbFilename, string dbLicense, string dbPassword);
        void CreateTableSurveys();
        void CreateResponsesTable();
        void CreateSliderTable();
        Boolean IsSurveyTableGreaterThanZero();
        void DropResponsesTable();
        void DropSliderNumbersTable();
        void DropSurveyTable();
        void DropSurveyQuestionsTable();
        List<SurveyVersions> GetAllSurveyIdWithVersions();
        List<cSurveyQuestions> GetAllQuestionBySurveyId(int SurveyId);
        List<cResponses> GetAllResponsesByQuestionId(int QuestionId);
        void DeleteOldOrRemovedSurveys(List<UpdatedSurveys> ListOfProdutsToUpdate);
        void DeleteSurveyQuestionBySurveyId(int SurveyId);
        void InsertSurveyToDevice(cSurvey Survey);
        void InsertResponsesToDevice(cResponses Response);
        List<cSurvey> GetAllSurvey();
        void InsertSurveyQuestionToDevice(cSurveyQuestions SurveyQuestion);
        void CreateQuestionsTable();
        void DeleteResponsesByQuestionId(int QuestionId);
        void CreateTableSurveyAnswers();
        void CreateTableAnswers();
        void DropTableSurveyAnswers();
        void DropTableAnswers();
    }
}

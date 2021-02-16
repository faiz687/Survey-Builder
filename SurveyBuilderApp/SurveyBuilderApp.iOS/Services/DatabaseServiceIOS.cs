using SurveyBuilderApp.iOS.Services;
using SQLite;
using SurveyBuilderApp.Interfaces;
using System;
using SurveyBuilderApp.Classes;
using System.Collections.Generic;
using System.Diagnostics;

[assembly: Xamarin.Forms.Dependency(typeof(DatabaseServiceIOS))]
namespace SurveyBuilderApp.iOS.Services
{
    public class DatabaseServiceIOS : IDatabaseService
    {
        SQLiteConnection mConnection;
        public string Filename { get; set; }
        public string CipherVersion { get; set; }
        public string CipherFipsStatus { get; set; }
        public DatabaseServiceIOS()
        {
            DataConnection(App.DBPath, App.DBLisenceKey, App.DBPassword);
        }
        public void DataConnection(string dbFilename, string dbLicense, string dbPassword)
        {
            mConnection = new SQLiteConnection(new SQLiteConnectionString(dbFilename, true, dbPassword));
            mConnection.ExecuteScalar<int>(String.Format("", dbLicense)); // removed for security.

            // Database Version for if we need it
            CipherVersion = mConnection.ExecuteScalar<string>(""); // removed for security.
            CipherFipsStatus = mConnection.ExecuteScalar<string>(""); // removed for security.
        }
        public void CreateTableSurveys()
        {
           mConnection.CreateTable<cSurvey>();
        }
        public bool IsSurveyTableGreaterThanZero()
        {
            if (mConnection.Table<cSurvey>().Count() > 0)
            {
                return true;
            }
            return false;
        }
        public void DropSurveyTable()
        {
            mConnection.DropTable<cSurvey>();
        }
        public List<SurveyVersions> GetAllSurveyIdWithVersions()
        {
            List<cSurvey> SurveyIdWithVersion = mConnection.Query<cSurvey>("SELECT SurveyId , SurveyVersion FROM cSurvey");
            List<SurveyVersions> NewSurveyIdWithVersionList = new List<SurveyVersions>();
            if ((SurveyIdWithVersion.Count) > 0)
            {
                foreach (cSurvey Survey in SurveyIdWithVersion)
                {
                    NewSurveyIdWithVersionList.Add(new SurveyVersions { SurveyId = Survey.SurveyId, SurveyVersion = Survey.SurveyVersion });
                }
                return NewSurveyIdWithVersionList;
            }
            else
            {
                return NewSurveyIdWithVersionList;
            }
        }
        public void DeleteOldOrRemovedSurveys(List<UpdatedSurveys> ListOfSurveyToUpdate)
        {
            
            foreach (UpdatedSurveys Survey in ListOfSurveyToUpdate)
            {
                if (Survey.SurveyUpdatedOrNot == true)
                {
                    mConnection.Execute("DELETE FROM cSurvey WHERE [SurveyId]=?", Survey.SurveyId);
                    var ListSurveyQuestion = GetAllQuestionBySurveyId(Survey.SurveyId);
                    if (ListSurveyQuestion.Count > 0)
                    {
                        foreach (cSurveyQuestions Question in ListSurveyQuestion)
                        {
                            DeleteResponsesByQuestionId(Question.QuestionId);
                        }
                    }
                    DeleteSurveyQuestionBySurveyId(Survey.SurveyId);
                }
            }
        }
        public void InsertSurveyToDevice(cSurvey Survey)
        {
            mConnection.Insert(Survey);
        }
        public List<cSurvey> GetAllSurvey()
        {
           return mConnection.Query<cSurvey>("SELECT SurveyId , SurveyName  FROM cSurvey");
        }
        public void InsertSurveyQuestionToDevice(cSurveyQuestions SurveyQuestion)
        {
            mConnection.Insert(SurveyQuestion);
        }
        public void CreateQuestionsTable()
        {
            mConnection.CreateTable<cSurveyQuestions>();
        }
        public List<cSurveyQuestions> GetAllQuestionBySurveyId(int SurveyId)
        {
          return mConnection.Query<cSurveyQuestions>("SELECT * FROM cSurveyQuestions WHERE [SurveyId]='" + SurveyId + "'");
        }
        public void DropSurveyQuestionsTable()
        {
            mConnection.DropTable<cSurveyQuestions>();
        }
        public void DeleteSurveyQuestionBySurveyId(int SurveyId)
        {
            mConnection.Execute("DELETE FROM cSurveyQuestions WHERE [SurveyId]=?", SurveyId);
        }
        public void CreateResponsesTable()
        {
            mConnection.CreateTable<cResponses>();
        }
        public void InsertResponsesToDevice(cResponses Response)
        {
            mConnection.Insert(Response);
        }
        public void CreateSliderTable()
        {
            mConnection.CreateTable<cSliderNumbersRange>();
        }
        public void InsertSliderToDevice(cSliderNumbersRange SliderNumber)
        {
            mConnection.Insert(SliderNumber);
        }
        public List<cResponses> GetAllResponsesByQuestionId(int QuestionId)
        {
            return mConnection.Query<cResponses>("SELECT * FROM cResponses WHERE [QuestionId]='" + QuestionId + "'");
        }
        public List<cSliderNumbersRange> GetSliderProperties(int QuestionId)
        {
            return mConnection.Query<cSliderNumbersRange>("SELECT * FROM cSliderNumbersRange WHERE [QuestionId]='" + QuestionId + "'");
        }
        public void DropResponsesTable()
        {
            mConnection.DropTable<cResponses>();
        }
        public void DropSliderNumbersTable()
        {
            mConnection.DropTable<cSliderNumbersRange>();
        }
        public void DeleteResponsesByQuestionId(int QuestionId)
        {
            mConnection.Execute("DELETE FROM cResponses WHERE [QuestionId]=?", QuestionId);
        }
        public void CreateTableSurveyAnswers()
        {
            mConnection.CreateTable<cSurveyAnswer>();
        }
        public void CreateTableAnswers()
        {
            mConnection.CreateTable<cAnswer>();
        }
        public void DropTableSurveyAnswers()
        {
            mConnection.DropTable<cSurveyAnswer>();
        }
        public void DropTableAnswers()
        {
            mConnection.DropTable<cAnswer>();
        }

       
    }
}
using Newtonsoft.Json;
using SurveyBuilderApp.AppPages;
using SurveyBuilderApp.Classes;
using SurveyBuilderApp.Interfaces;
using SurveyBuilderApp.Services;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SurveyBuilderApp.AppPagesCodeBehind
{
    public class VersionCheckPage : INotifyPropertyChanged
    {
        private bool _IsLoading;

        private bool _AppIconImage;

        private bool _VersionMessageLabel;

        private string _ActivityStatus;

        private string AppVersion = App.Version;
        public ICommand PageAppearingCommand { private set; get; }
        public bool IsLoading { get => _IsLoading; set { _IsLoading = value; RaisePropertyChanged("IsLoading"); } }
        public bool AppIconImage { get => _AppIconImage; set { _AppIconImage = value; RaisePropertyChanged("AppIconImage"); } }
        public bool VersionMessageLabel { get => _VersionMessageLabel; set { _VersionMessageLabel = value; RaisePropertyChanged("VersionMessageLabel"); } }
        public string ActivityStatus { get => _ActivityStatus; set { _ActivityStatus = value; RaisePropertyChanged("ActivityStatus"); } }

        IDatabaseService dbHandler = DependencyService.Get<IDatabaseService>();

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
        public VersionCheckPage()
        {
            PageAppearingCommand = new Command(OnAppearing);
        }

        public async void OnAppearing()
        {
            if (Reachability.IsHostReachable("http://www.google.co.uk"))
            {
                {
                    IsLoading = true;
                    ActivityStatus = "Loading...";
                    IVersionHandler Version = new VersionHandler();
                    string Response = "";
                    string latestVersion = await Task.Run(() => Response = Version.GetLatestVersion());
                    IsLoading = false;
                    cVersion jsonObj = JsonConvert.DeserializeObject<cVersion>(latestVersion.ToString());
                    string VersionWCF = jsonObj.VersionName;
                    if (!string.IsNullOrWhiteSpace(VersionWCF))
                    {
                        if (VersionWCF == AppVersion)
                        {
                            ISurveyHandler _Survey = new SurveyHandler();
                            dbHandler.DropResponsesTable();
                            dbHandler.DropSliderNumbersTable();
                            dbHandler.DropSurveyTable();
                            dbHandler.DropSurveyQuestionsTable();
                            dbHandler.DropTableSurveyAnswers();
                            dbHandler.DropTableAnswers();
                            dbHandler.CreateTableSurveyAnswers();
                            dbHandler.CreateTableAnswers();
                            dbHandler.CreateTableSurveys();
                            dbHandler.CreateQuestionsTable();
                            dbHandler.CreateResponsesTable();
                            dbHandler.CreateSliderTable();
                            string SurveyResponse = "";
                            List<SurveyVersions> ProductIdAndVersionList = dbHandler.GetAllSurveyIdWithVersions();
                            await Task.Run(() => SurveyResponse = _Survey.GetUpdatedSurveyListFromServer(ProductIdAndVersionList));
                            List<UpdatedSurveys> ListOfProdutsToUpdate = JsonConvert.DeserializeObject<List<UpdatedSurveys>>(SurveyResponse);                                             
                            if (ListOfProdutsToUpdate.Count > 0)
                            {
                                dbHandler.DeleteOldOrRemovedSurveys(ListOfProdutsToUpdate);
                                await Task.Run(() => DownloadNewOrUpdatedProducts(ListOfProdutsToUpdate));
                            }
                            App.Current.MainPage = new MainTabbedPage();
                        }
                        else
                        {
                            AppIconImage = true;
                            VersionMessageLabel = true;
                        }
                    }
                }
            }
            else
            {
                App.Current.MainPage = new MainTabbedPage();
            }
        }
        private async Task DownloadNewOrUpdatedProducts(List<UpdatedSurveys> listOfProdutsToUpdate)
        {
            ISurveyQuestion SurveyQuestionHandle = new SurveyQuestionsHandler();
            ISurveyHandler _SurveyHandle = new SurveyHandler();
            IResponsesHandler _ResponsesHandle = new ResponsesHandler();
            ISliderNumbersRange _Sliderhandle = new SliderNumbersRangeHandler();
            string SurevyResponse = "";
            string SurevyQuestionResponse = "";
            cSurvey _Survey = new cSurvey();
            List<cSurveyQuestions> _SurveyQuestionList = new List<cSurveyQuestions>();
            foreach (UpdatedSurveys Survey in listOfProdutsToUpdate)
                if (Survey.SurveyUpdatedOrNot == false)
                {
                    await Task.Run(() => SurevyResponse = _SurveyHandle.GetSurveyFromServer(Survey));
                    _Survey = JsonConvert.DeserializeObject<cSurvey>(SurevyResponse);
                    dbHandler.InsertSurveyToDevice(_Survey);
                      
                    await Task.Run(() => SurevyQuestionResponse = SurveyQuestionHandle.GetAllQuesitons(Survey.SurveyId));
                    if (!(SurevyQuestionResponse == "Unauthorized" || SurevyQuestionResponse == "No Content"))
                    {
                        _SurveyQuestionList = JsonConvert.DeserializeObject<List<cSurveyQuestions>>(SurevyQuestionResponse);
                        foreach (cSurveyQuestions Question in _SurveyQuestionList)
                        {
                            dbHandler.InsertSurveyQuestionToDevice(Question);
                            if (Question.Control == 4 || Question.Control == 5 )
                            {
                                string QuestionResponse = "";
                                await Task.Run(() => QuestionResponse = _ResponsesHandle.GetAllResponses(Question.QuestionId));
                                List<cResponses> AllResponses = JsonConvert.DeserializeObject<List<cResponses>>(QuestionResponse.ToString());
                                foreach (cResponses Response in AllResponses)
                                {
                                    dbHandler.InsertResponsesToDevice(Response);
                                }
                            }
                            else if (Question.Control == 3)
                            {
                                string QuestionResponse = "";
                                await Task.Run(() => QuestionResponse = _ResponsesHandle.GetAllResponses(Question.QuestionId));
                                if (!(QuestionResponse == "NoContent"))
                                {
                                    List<cResponses> AllResponses = JsonConvert.DeserializeObject<List<cResponses>>(QuestionResponse.ToString());
                                    foreach (cResponses Response in AllResponses)
                                    {
                                        dbHandler.InsertResponsesToDevice(Response);
                                    }
                                }
                                else
                                {
                                    string QuestionSliderResponse = "";
                                    await Task.Run(() => QuestionSliderResponse = _Sliderhandle.GetPropertiesForSlider(Question.QuestionId));
                                    cSliderNumbersRange SliderProperties = JsonConvert.DeserializeObject<cSliderNumbersRange>(QuestionSliderResponse.ToString());
                                    dbHandler.InsertSliderToDevice(SliderProperties);
                                }
                            }
                        }
                    }
                }
        }
    }

}

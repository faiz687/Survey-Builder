using Newtonsoft.Json;
using SurveyBuilderApp.AppPages;
using SurveyBuilderApp.Classes;
using SurveyBuilderApp.Interfaces;
using SurveyBuilderApp.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SurveyBuilderApp.AppPagesViewModel
{
    public class HomePage :  INotifyPropertyChanged
    {
        private string _SearchedText;

        private bool _IsLoading;

        private bool _ShowAllSurveys;

        private ObservableCollection<cSurvey> AllSurveyMain;

        IDatabaseService dbHandler = DependencyService.Get<IDatabaseService>();

        private IEnumerable<cSurvey> _AllSurveys;
        public  IEnumerable<cSurvey> AllSurveys{ get => _AllSurveys;  set { _AllSurveys = value; RaisePropertyChanged("AllSurveys"); } }
        public string SearchedText{get => _SearchedText; 
            set
            {
                _SearchedText = value;
                RaisePropertyChanged("SearchedText") ; 
                SearchSurveyName();
            }
        }
        public bool IsLoading { get => _IsLoading; set { _IsLoading = value; RaisePropertyChanged("IsLoading"); } }
        public bool ShowAllSurveys { get => _ShowAllSurveys; set { _ShowAllSurveys = value; RaisePropertyChanged("ShowAllSurveys"); } }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
        public ICommand SurveyNameClickedCommand { private set; get; }
        public ICommand PageAppearingCommand { private set; get; }

        public HomePage()
        {
            SurveyNameClickedCommand = new Command<CollectionView>((SurveysCollectionView) => SurveyNameClicked(SurveysCollectionView));
            PageAppearingCommand = new Command(OnAppearing); ;
        }

        private async void OnAppearing()
        {
            if (Reachability.IsHostReachable("http://www.google.co.uk"))
            {
                {
                    ShowAllSurveys = false;
                    IsLoading = true;
                    string Response = "";
                    ISurveyHandler Survey = new SurveyHandler();
                    string AllSurveysResponse = await Task.Run(() => Response = Survey.GetAllSurvey(""));
                    AllSurveyMain = JsonConvert.DeserializeObject<ObservableCollection<cSurvey>>(AllSurveysResponse.ToString());
                    AllSurveys = AllSurveyMain;
                    IsLoading = false;
                    ShowAllSurveys = true;
                }
            }
            else
            {
                var SurevysList = dbHandler.GetAllSurvey();
                AllSurveyMain = new ObservableCollection<cSurvey>(SurevysList);
                AllSurveys = AllSurveyMain;
                ShowAllSurveys = true;
            }
        }
        private void SearchSurveyName()
        {
            if (SearchedText != null)
            {
                AllSurveys = AllSurveyMain.Where(Survey => Survey.SurveyName.Contains(SearchedText));
                return;
            }
            AllSurveys = AllSurveyMain;
        }
        private void SurveyNameClicked(CollectionView SurveysCollectionView)
        {
           int SurveyId = (SurveysCollectionView.SelectedItem as cSurvey).SurveyId;
           string SurveyName = (SurveysCollectionView.SelectedItem as cSurvey).SurveyName;
           int SurveyVersion = (SurveysCollectionView.SelectedItem as cSurvey).SurveyVersion;
           var Properties = App.Current.Properties;
           if (!Properties.ContainsKey("SelectedSurveyId"))
           {
                Properties.Add("SelectedSurveyVersion", SurveyVersion);
                Properties.Add("SelectedSurveyName", SurveyName);
                Properties.Add("SelectedSurveyId", SurveyId);
           }
           else
            {
                Properties["SelectedSurveyVersion"] = SurveyVersion;
                Properties["SelectedSurveyId"] = SurveyId;
                Properties["SelectedSurveyName"] = SurveyName;
            }
            App.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new AppPages.SurveyPage()));
        }
    }    
}

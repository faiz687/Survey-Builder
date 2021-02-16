using Foundation;
using Newtonsoft.Json;
using SurveyBuilderApp.Classes;
using SurveyBuilderApp.Interfaces;
using SurveyBuilderApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SurveyBuilderApp.AppPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PinManagementPage : ContentPage
    {
        public PinManagementPage()
        {
            InitializeComponent();
            OnAppearing();
        }

        protected async override void OnAppearing()
        {
            if (Reachability.IsHostReachable("http://www.google.co.uk"))
            {
                {

                    PinLabel.IsVisible = false;
                    PinPickerLayout.IsVisible = false;
                    PinActivityIndicator.IsVisible = true;
                    string Response = "";
                    ISurveyHandler Survey = new SurveyHandler();
                    string AllSurveysResponse = await Task.Run(() => Response = Survey.GetAllSurvey(""));
                    SurveyPicker.ItemsSource  = JsonConvert.DeserializeObject<ObservableCollection<cSurvey>>(AllSurveysResponse.ToString());
                    PinPickerLayout.IsVisible = true;
                    PinActivityIndicator.IsVisible = false;
                }
            }

        }
        private async void GenerateButton_Clicked_1(object sender, EventArgs e)
        {
            if (SurveyPicker.SelectedIndex != -1)
            {
                ISurveyPinHandler _SurveyPin = new SurveyPinHandler();
                bool PinExists = true;
                string NewSurveyPin = "";
                PinPickerLayout.IsVisible = false;
                PinActivityIndicator.IsVisible = true;
                while (PinExists)
                {
                    string Response = "";
                    NewSurveyPin = Guid.NewGuid().ToString().Substring(0, 5);
                    string SurveyPinResponse = await Task.Run(() => Response = _SurveyPin.GetSurveyPin(new cSurveysPin { PinNumber = NewSurveyPin, SurveyId = (SurveyPicker.SelectedItem as cSurvey).SurveyId, PinGeneratedDatetime = DateTime.Now, PinUsedDateTime = DateTime.Now }));
                    cSurveysPin _Pin = JsonConvert.DeserializeObject<cSurveysPin>(Response.ToString());
                    if (_Pin.SurveyId == 0)
                    {
                        PinExists = false;
                    }
                }
                string PinResponse = "";
                await Task.Run(() => PinResponse = _SurveyPin.InsertPin(new cSurveysPin { PinNumber = NewSurveyPin, PinGeneratedDatetime = DateTime.Now, SurveyId = (SurveyPicker.SelectedItem as cSurvey).SurveyId }));
                PinPickerLayout.IsVisible = true;
                PinActivityIndicator.IsVisible = false;
                if (PinResponse == "false")
                {
                    PinLabel.Text = PinResponse = "Sorry ,Pin cannot be generated at this time.";
                }
                else
                {
                    PinLabel.Text = NewSurveyPin;
                }
                PinLabel.IsVisible = true;

            }
            else
            {
                await DisplayAlert("Message", "Please select a survey first.", "OK");
            }
        }
    }
}
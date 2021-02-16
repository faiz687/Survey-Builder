using Foundation;
using SurveyBuilderApp.AppPages;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SurveyBuilderApp
{
    public partial class App : Application
    {
        public static readonly string DBLisenceKey = ""; // removed for security.
        public static readonly string DBPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "SurveyBuilder.db");
        public static readonly string DBPassword = ""; // removed for security.
        public static readonly string Version  = NSBundle.MainBundle.ObjectForInfoDictionary("CFBundleShortVersionString").ToString();
       
        public App()
        {       
            InitializeComponent();
            MainPage = new VersionCheckPage();
        }

        protected override void OnStart()
        {

        }

        protected override void OnSleep()
        {

        }

        protected override void OnResume()
        {

        }
    }
}

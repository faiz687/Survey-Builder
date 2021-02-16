using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SurveyBuilderApp.AppPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VersionCheckPage : ContentPage
    {

        public VersionCheckPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}
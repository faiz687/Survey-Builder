using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SurveyBuilderApp.AppPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainTabbedPage : TabbedPage
    {
        public MainTabbedPage()
        {
            InitializeComponent();
            NavigationPage HomePage = new NavigationPage(new HomePage()) { Title = "Home" , IconImageSource="HomeIcon" };
            Children.Add(HomePage);
            Children.Add(new PinManagementPage());
        }
    }
}
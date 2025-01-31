using SRTS_Telemetry.ViewModels;

namespace SRTS_Telemetry
{
    public partial class App : Application
    {
        public static MainViewModel ViewModel = new MainViewModel();
        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }
    }
}

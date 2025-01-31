using SRTS_Telemetry.ViewModels;

namespace SRTS_Telemetry
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = App.ViewModel;
        }
    }
}

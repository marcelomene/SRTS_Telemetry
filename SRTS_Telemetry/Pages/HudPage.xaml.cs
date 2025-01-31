using SRTS_Telemetry.ViewModels;

namespace SRTS_Telemetry.Pages;

public partial class HudPage : ContentPage
{

    public HudPage()
	{
		InitializeComponent();
		BindingContext = App.ViewModel;
		App.ViewModel.SetControls(this.throttleBoxview, this.brakeBoxview);
	}
}
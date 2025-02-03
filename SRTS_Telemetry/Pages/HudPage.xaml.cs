using SRTS_Telemetry.ViewModels;

namespace SRTS_Telemetry.Pages;

public partial class HudPage : ContentPage
{

    public HudPage()
	{
		InitializeComponent();
		BindingContext = App.ViewModel;
		App.ViewModel.SetControls(this.throttleBoxview, this.brakeBoxview);
        DeviceDisplay.Current.MainDisplayInfoChanged += Current_MainDisplayInfoChanged;
	}

    private void Current_MainDisplayInfoChanged(object? sender, DisplayInfoChangedEventArgs e)
    {
        var displayInfo = e.DisplayInfo;

        if (displayInfo.Orientation == DisplayOrientation.Landscape)
        {
            Shell.SetTabBarIsVisible(this, false);
            Shell.SetNavBarIsVisible(this, false);

            foreach (var item in MainGrid.RowDefinitions)
            {
                if(item != null)
                    item.Height = GridLength.Auto;
            }
        }
        else if (displayInfo.Orientation == DisplayOrientation.Portrait)
        {
            Shell.SetTabBarIsVisible(this, true);
            Shell.SetNavBarIsVisible(this, true);

            foreach (var item in MainGrid.RowDefinitions)
            {
                if(item != null)
                    item.Height = GridLength.Star;
            }
        }
    }
}
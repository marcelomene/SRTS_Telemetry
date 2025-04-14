namespace SRTS_Telemetry.Pages;

public partial class HudPage2 : ContentPage
{
    List<BoxView> rpmBoxViews;
    public HudPage2()
	{
		InitializeComponent();
        BindingContext = App.ViewModel;
        rpmBoxViews = new() { rpm1, rpm2, rpm3, rpm4, rpm5, rpm6, rpm7, rpm8, rpm9, rpm10, rpm11, rpm12 };
        App.ViewModel.SetControls(this.throttleBoxview, this.brakeBoxview, rpmBoxViews);
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
                if (item != null)
                    item.Height = GridLength.Auto;
            }
        }
        else if (displayInfo.Orientation == DisplayOrientation.Portrait)
        {
            Shell.SetTabBarIsVisible(this, true);
            Shell.SetNavBarIsVisible(this, true);

            foreach (var item in MainGrid.RowDefinitions)
            {
                if (item != null)
                    item.Height = GridLength.Star;
            }
        }
    }
}
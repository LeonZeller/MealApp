using System.Xml.Serialization;

namespace MealApp.Views;

public partial class AppearanceSettingsPage : ContentPage
{
	public AppearanceSettingsPage()
	{
		InitializeComponent();
		loadCurrentTheme();
	}

	private void loadCurrentTheme()
	{
		var saved = Preferences.Get("AppTheme", "System");
		switch (saved)
		{
			case "Light":
				LightRadio.IsChecked = true;
				break;
			case "Dark":
				DarkRadio.IsChecked = true;
				break;
			default:
				SystemRadio.IsChecked = true;
				break;
        }
    }

	private void OnThemeChanged(object sender, CheckedChangedEventArgs e)
	{
		if (!e.Value) return;

		var radio = (RadioButton)sender;
		string theme = "System";

		if (radio == LightRadio) theme = "Light";
		else if (radio == DarkRadio) theme = "Dark";

		Preferences.Set("AppTheme", theme);

		Application.Current!.UserAppTheme = theme switch
		{
			"Light" => AppTheme.Light,
			"Dark" => AppTheme.Dark,
			_ => AppTheme.Unspecified
        };
    }
}
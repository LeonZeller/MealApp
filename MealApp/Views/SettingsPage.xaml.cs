using MealApp.Services;
using System.Threading.Tasks;

namespace MealApp.Views;

public partial class SettingsPage : ContentPage
{
	public SettingsPage(BackupSettingsPage backupSettingsPage)
	{
		InitializeComponent();
        this.backupSettingsPage = backupSettingsPage;
    }

    private async void OnAppearanceTapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new AppearanceSettingsPage());
    }

    private async void OnPeopleTapped(object sender, TappedEventArgs e)
    {
        await PopupService.ShowInfoAsync("Bald verfügbar", "Muss noch gemacht werden...", "??");
    }

    private async void OnBackupTapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(backupSettingsPage);
    }   

    private async void OnAboutTapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new AboutPage());
    }

    private BackupSettingsPage backupSettingsPage;
}
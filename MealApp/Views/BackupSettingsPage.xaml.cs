using MealApp.Services;

namespace MealApp.Views;

public partial class BackupSettingsPage : ContentPage
{
	public BackupSettingsPage()
	{
		InitializeComponent();
	}

	private async void OnExportClicked(object sender, EventArgs e)
	{
        await PopupService.ShowInfoAsync("Bald verfügbar", "Muss noch", "ℹ️");
    }

	private async void OnImportClicked(object sender, EventArgs e)
	{
		await PopupService.ShowInfoAsync("Bald verfügbar", "Muss noch", "ℹ️");
    }
}
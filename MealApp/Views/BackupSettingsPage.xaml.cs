using MealApp.Services;

namespace MealApp.Views;

public partial class BackupSettingsPage : ContentPage
{
	public BackupSettingsPage(DatabaseService databaseService)
	{
		InitializeComponent();
		this.databaseService = databaseService;
		
	}

	private async void OnExportClicked(object sender, EventArgs e)
	{
		try
		{
			await databaseService.ExportAsync();
		}
		catch (Exception ex)
		{

			await PopupService.ShowInfoAsync("Fehler", "Export fehlgeschlagen: " + ex.Message, "ℹ️");
		}
    }

	private async void OnImportClicked(object sender, EventArgs e)
	{
		try
		{
			var result = await FilePicker.Default.PickAsync(new PickOptions
			{
				PickerTitle = "Backup auswählen",
			});

			await databaseService.ImportAsync(result.FullPath);
        }
		catch (Exception ex)
		{

            await PopupService.ShowInfoAsync("Fehler", "Import fehlgeschlagen: " + ex.Message, "ℹ️");
        }
    }

	private readonly DatabaseService databaseService;
}
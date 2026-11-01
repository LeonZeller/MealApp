using MealApp.Models;
using MealApp.Services;
using MealApp.ViewModels;
using MealApp.Popups;
using MealApp.Controls;
namespace MealApp.Views;

public partial class CalendarPage : ContentPage
{
    public CalendarPage(MealServices mealServices, CalendarService calendarService, 
        ShoppingService shoppingService)
    {
        InitializeComponent();
        viewModel = new CalendarViewModel(mealServices, calendarService, shoppingService);
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await viewModel.loadEntriesCommand.ExecuteAsync(null);

        if (!viewModel.IsCalendarView)
        {
            viewModel.ResetToToday();
        }
    }

    private async void OnAddMealClicked(object sender, EventArgs e)
    {
        if (viewModel.SelectedDate == null) return;

        var popup = new TimePickerPopup(new TimeSpan(18, 0, 0));
        await Navigation.PushModalAsync(popup);
        var result = await popup.ResultTask.Task;

        if (result == null) return;
        await Task.Delay(200);

        var mealNames = viewModel.GetMealNames();
        if (mealNames.Length == 0)
        {
            await PopupService.ShowErrorAsync("Hinweis", "Keine Gerichte vorhanden.");
            return;
        }

        var options = mealNames.Select(name => (name, name, false)).ToList();
        var selectedMealName = await PopupService.ShowAsync(
            "Gericht auswählen", null, "🍽️", options);

        if (selectedMealName == "Abbrechen" || selectedMealName == null) return;

        var meal = viewModel.GetMealByName(selectedMealName);
        if (meal == null) return;

        await viewModel.SaveEntry(meal, result.Time, result.Persons);

        await PopupService.ShowInfoAsync(Title,
            viewModel.GetSavedAlertMessage(meal, result.Time, new CalendarEntry
            {
                Date = viewModel.SelectedDate!.Value.Date,
                ServingTime = result.Time,
                Meal = meal
                
            }),
            "✅");
    }

    private void OnCalendarTabClicked(object sender, EventArgs e)
        => viewModel.showCalendarCommand.Execute(null);

    private void OnDayTabClicked(object sender, EventArgs e)
    {
        viewModel.ResetToToday();
        viewModel.showDayViewCommand.Execute(null);
    }

    

    private void OnDateTapped(object sender, DateTime date)
    {
        viewModel.SelectedDate = date;
    }

    private void OnDateDoubleTapped(object sender, DateTime date)
    {
        viewModel.SelectedDate = date;
        viewModel.GoToDate(date);
        viewModel.showDayViewCommand.Execute(null);
    }

    

    private async void OnEntryTapped(object sender, CalendarEntryTappedEventArgs e)
    {
        var entry = e.Entry;
        if (entry.Meal == null) return;

        var action = await DisplayActionSheet(
            $"{entry.Meal.Name}",
            "Abbrechen",
            null,
            "Kochen starten",
            "Gericht anzeigen",
            "Entfernen"
        );

        if (action == "Kochen starten")
        {
            await Navigation.PushAsync(new CookingPage(entry.Meal));
        }
        else if (action == "Gericht anzeigen")
        {
            await Navigation.PushAsync(new MealDetailPage(entry.Meal));
        }
        else if (action == "Entfernen")
        {
            var confirm = await PopupService.ConfirmAsync(
                "Eintrag entfernen",
                $"{entry.Meal.Name} aus dem Kalender entfernen?",
                "Ja", "Nein", " 🗑️");
            if (confirm)
            {
                await viewModel.DeleteEntry(entry);
            }
        }
    }

    
    private async void OnSettingsClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(SettingsPage));
    }

    private void OnPreviousDayClicked(object sender, EventArgs e)
    {
        viewModel.GoToPreviousDay();
    }

    private void OnNextDayClicked(object sender, EventArgs e)
    {
        viewModel.GoToNextDay();
    }

    private readonly CalendarViewModel viewModel;
}
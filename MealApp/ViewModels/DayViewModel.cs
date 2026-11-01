using CommunityToolkit.Mvvm.ComponentModel;
using MealApp.Models;
using MealApp.Services;
using Plugin.Maui.CalendarStore;

namespace MealApp.ViewModels
{
    public partial class DayViewModel : ObservableObject
    {
        public string DayLabel => date.ToString("dddd, dd.MM.yyyy");


        public DayViewModel(DateTime date, CalendarService calendarService)
        {
            this.date = date;
            this.calendarService = calendarService;
            LoadEntries();
        }

        

        private void LoadEntries()
        {
            Entries = calendarService.Entries.
                Where(e => e.Date.Date == Date.Date).ToList();
        }

        [ObservableProperty]
        private DateTime date;

        [ObservableProperty]
        private List<CalendarEntry> entries = new();

        [ObservableProperty]
        private List<CalendarEvent> systemEvents = new();

        private readonly CalendarService calendarService;
    }
}

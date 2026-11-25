using MealApp.Models;
using System.Collections.ObjectModel;


namespace MealApp.Services
{
    public interface ICalendarService
    {
        ObservableCollection<CalendarEntry> Entries { get; }

        Task LoadEntriesAsync();
        Task AddEntryAsync(CalendarEntry entry);
        Task DeleteEntryAsync(CalendarEntry entry);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using MealApp.Models;


namespace MealApp.Services
{
    public class CalendarService
    {
        public CalendarService(DatabaseService databaseService)
        {
            this.databaseService = databaseService;
        }

        public ObservableCollection<CalendarEntry> Entries { get; set; } = new();

        public async Task LoadEntriesAsync()
        {
            var entries = await databaseService.GetCalendarEntriesAsync();
            Entries.Clear();
            foreach (var entry in entries)
            {
                Entries.Add(entry);
            }
        }

        public async Task AddEntryAsync(CalendarEntry entry)
        {
            await databaseService.AddCalendarEntryAsync(entry);
            Entries.Add(entry);
        }

        public async Task DeleteEntryAsync(CalendarEntry entry)
        {
            await databaseService.DeleteCalendarEntryAsync(entry);
            Entries.Remove(entry);
        }







        private readonly DatabaseService databaseService;

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MealApp.Services;
using MealApp.Models;
using System.Collections.ObjectModel;

namespace MealApp.Tests.Fakes
{
    public class FakeCalendarService : ICalendarService
    {
        public ObservableCollection<CalendarEntry> Entries { get; } = new();

        public Task AddEntryAsync(CalendarEntry entry)
        {
            return Task.CompletedTask;
        }

        public Task DeleteEntryAsync(CalendarEntry entry)
        {
            return Task.CompletedTask;
        }

        public Task LoadEntriesAsync()
        {
            return Task.CompletedTask;
        }
    }
}

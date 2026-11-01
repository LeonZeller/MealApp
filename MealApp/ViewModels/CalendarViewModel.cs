using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MealApp.Models;
using MealApp.Services;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace MealApp.ViewModels
{
    public partial class CalendarViewModel : BaseViewModel
    {
        

        partial void OnSelectedDateChanged(DateTime? value)
        {
            IsAddButtonEnabled = value.HasValue;
        }

        

        public CalendarViewModel(MealServices mealServices, CalendarService calendarService,
            ShoppingService shoppingService)
        {
            this.mealServices = mealServices;
            this.calendarService = calendarService;
            this.shoppingService = shoppingService;
            Title = "Kalender";
        }

        [ObservableProperty]
        private Dictionary<DateTime, List<CalendarDot>> calendarDots = new();

        public DayViewModel CurrentDay => new DayViewModel(currentDate, calendarService);

        public void GoToDate(DateTime date)
        {
            currentDate = date.Date;
            OnPropertyChanged(nameof(CurrentDay));
        }

        public void GoToNextDay()
        {
            currentDate = currentDate.AddDays(1);
            OnPropertyChanged(nameof(CurrentDay));
        }

        public void GoToPreviousDay()
        {
            currentDate = currentDate.AddDays(-1);
            OnPropertyChanged(nameof(CurrentDay));
        }

        public void ResetToToday()
        {
            currentDate = DateTime.Today;
            OnPropertyChanged(nameof(CurrentDay));
        }

        public string[] GetMealNames()
        {
            return mealServices.Meals.Select(m => m.Name).ToArray();
        }

        public async Task SaveEntry(Meal meal, TimeSpan servingTime, int persons)
        {
            if (SelectedDate == null) return;

            var mealTime = getMealTime(servingTime);
            var entry = new CalendarEntry
            {
                Date = SelectedDate.Value.Date,
                ServingTime = servingTime,
                MealId = meal.Id,
                Meal = meal,
                MealTime = mealTime,
                Persons = persons
            };

            await calendarService.AddEntryAsync(entry);
            rebuildCalendarDots();
        }

        public async Task DeleteEntry(CalendarEntry entry)
        {
            OnPropertyChanged(nameof(CurrentDay));
        }

        public string GetSavedAlertMessage(Meal meal, TimeSpan servingTime, CalendarEntry entry)
        {
            var mealTime = getMealTime(servingTime);
            return $"{mealTimeIcon(mealTime)} {meal.Name} am {entry.Date:dd.MM.yyyy}\nFertig um {servingTime:hh\\:mm}\nKochen ab {entry.StartCookingTime:HH:mm}";
        }

        public Meal? GetMealByName(string name)
        {
            return mealServices.Meals.FirstOrDefault(m => m.Name == name);
        }

        public Color GetDominantColor(DateTime date)
        {
            var entriesForDay = calendarService.Entries
                .Where(e => e.Date.Date == date.Date)
                .ToList();

            if (entriesForDay.Count == 0)
                return Colors.Gray;

            
            var longest = entriesForDay
                .Where(e => e.Meal != null)
                .OrderByDescending(e => e.Meal!.PreperationTime)
                .FirstOrDefault();

            return longest != null ? getMealTimeColor(longest.MealTime) : Colors.Gray;
        }

        [RelayCommand]
        private async Task loadEntries()
        {
            OnPropertyChanged(nameof(CurrentDay));
        }

        [RelayCommand]
        private async Task addEntry(string timeInput)
        {
            if (SelectedDate == null) return;

            if (!TimeSpan.TryParseExact(timeInput, @"hh\:mm", null, out TimeSpan servingTime))
            {
                await Shell.Current.DisplayAlert("Fehler", "Bitte eine gültige Uhrzeit eingeben (HH:MM).", "OK");
                return;
            }

            var mealNames = mealServices.Meals.Select(m => m.Name).ToArray();
            if (mealNames.Length == 0)
            {
                await Shell.Current.DisplayAlert("Hinweis", "Keine Gerichte vorhanden.", "OK");
                return;
            }

            return;
        }

        private void rebuildCalendarDots()
        {
            var dots = new Dictionary<DateTime, List<CalendarDot>>();

            var groupedByDate = calendarService.Entries.GroupBy(e => e.Date.Date);

            foreach (var group in groupedByDate)
            {
                
                var dotsForDay = group
                    .Where(e => e.Meal != null)
                    .GroupBy(e => e.MealTime)
                    .OrderBy(g => (int)g.Key)
                    .Select(g => new CalendarDot { Color = getMealTimeColor(g.Key) })
                    .ToList();

                if (dotsForDay.Count > 0)
                {
                    dots[group.Key] = dotsForDay;
                }
            }

            CalendarDots = dots;
        }

        private static MealTime getMealTime(TimeSpan time)
        {
            if (time.Hours >= 5 && time.Hours < 11)
                return MealTime.Frühstück;
            if (time.Hours >= 11 && time.Hours < 16)
                return MealTime.Mittagessen;
            return MealTime.Abendessen;
        }

        private static Color getMealTimeColor(MealTime mealTime) => mealTime switch
        {
            MealTime.Frühstück => Colors.Yellow,
            MealTime.Mittagessen => Colors.Green,
            MealTime.Abendessen => Colors.Red,
            _ => Colors.Gray
        };

        private static string mealTimeIcon(MealTime mealTime) => mealTime switch
        {
            MealTime.Frühstück => "🌅",
            MealTime.Mittagessen => "☀️",
            MealTime.Abendessen => "🌙",
            _ => ""
        };

        

        [RelayCommand]
        private void showCalendar() => IsCalendarView = true;

        [RelayCommand]
        private void showDayView() => IsCalendarView = false;

        [ObservableProperty]
        private DateTime? selectedDate;

        [ObservableProperty]
        private bool isAddButtonEnabled;

        private readonly MealServices mealServices;
        private readonly CalendarService calendarService;

        [ObservableProperty]
        private bool isCalendarView = true;

        private readonly ShoppingService shoppingService;
        private DateTime currentDate = DateTime.Today;
    }
}
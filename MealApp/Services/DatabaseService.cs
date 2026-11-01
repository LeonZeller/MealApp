using MealApp.Models;
using SQLite;

namespace MealApp.Services
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection? database;

        public async Task InitializeAsync()
        {
            if (database != null) return;

            try
            {
                var dbPath = Path.Combine(FileSystem.AppDataDirectory, "meals.db");

                database = new SQLiteAsyncConnection(dbPath);
                await database.CreateTableAsync<Meal>();
                await database.CreateTableAsync<CalendarEntry>();
                await database.CreateTableAsync<CustomShoppingItem>();
            }
            catch (Exception ex)
            {

                System.Diagnostics.Debug.WriteLine($"DB Init Fehler: {ex.Message}");
                await Shell.Current.DisplayAlert("Fehler",
                    "Die Datenbank konnte nicht initialisiert werden. Bitte App neu starten.", "OK");    

            }
        }

        public async Task<List<Meal>> GetMealsAsync()
        {
            await InitializeAsync();
            return await database!.Table<Meal>().ToListAsync();
        }

        public async Task<List<CalendarEntry>> GetCalendarEntriesAsync()
        {
            await InitializeAsync();
            var entries = await database!.Table<CalendarEntry>().ToListAsync();

            foreach (var entry in entries)
            {
                entry.Meal = await database.FindAsync<Meal>(entry.MealId);
            }
            return entries;
        }

        public async Task AddCalendarEntryAsync(CalendarEntry entry)
        {
            await InitializeAsync();
            await database!.InsertAsync(entry);
        }

        public async Task DeleteCalendarEntryAsync(CalendarEntry entry)
        {
            await InitializeAsync();
            await database!.DeleteAsync(entry);
        }

        public async Task AddMealAsync(Meal meal)
        {
            await InitializeAsync();
            await database!.InsertAsync(meal);
        }

        public async Task DeleteMealAsync(Meal meal)
        {
            await InitializeAsync();
            await database!.DeleteAsync(meal);
        }

        public async Task UpdateMealAsync(Meal meal)
        {
            await InitializeAsync();
            await database!.UpdateAsync(meal);
        }

        public async Task<List<CustomShoppingItem>> GetCustomShoppingItemsAsync()
        {
            await InitializeAsync();
            return await database!.Table<CustomShoppingItem>().ToListAsync();
        }

        public async Task AddCustomShoppingItemAsync(CustomShoppingItem item)
        {
            await InitializeAsync();
            await database!.InsertAsync(item);
        }

        public async Task UpdateCustomShoppingItemAsync(CustomShoppingItem item)
        {
            await InitializeAsync();
            await database!.UpdateAsync(item);
        }

        public async Task DeleteCustomShoppingItemAsync(CustomShoppingItem item)
        {
            await InitializeAsync();
            await database!.DeleteAsync(item);
            
        }
    }
}
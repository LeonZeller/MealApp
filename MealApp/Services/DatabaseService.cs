using MealApp.Models;
using SQLite;

namespace MealApp.Services
{
    public class DatabaseService
    {
        

        public async Task InitializeAsync()
        {


            if (database != null) return;            

            try
            {                
                //if (File.Exists(dbPath)) File.Delete(dbPath); // TEMPORÄR: Datenbank leeren, danach wieder entfernen!                

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

        public async Task ExportAsync()
        {
            await InitializeAsync();

            await database.ExecuteScalarAsync<int>("PRAGMA wal_checkpoint(TRUNCATE)");

            string fileName = $"MealApp-Backup-{DateTime.Now:yyyy-MM-dd}.db3";
            string backupPath = Path.Combine(FileSystem.CacheDirectory, fileName);

            File.Copy(dbPath, backupPath, true);

            await Share.Default.RequestAsync(new ShareFileRequest
            {
                Title = "MealApp-Backup speichern",
                File = new ShareFile(backupPath)
            });
        }

        public async Task ImportAsync(string importFilePath)
        {
            await InitializeAsync();
            if (!File.Exists(importFilePath))
            {
                throw new FileNotFoundException("Die Importdatei wurde nicht gefunden.", importFilePath);
            }
            // Close the current database connection
            await database.CloseAsync();
            // Replace the current database file with the imported file
            File.Copy(importFilePath, dbPath, true);
            // Re-initialize the database connection
            database = new SQLiteAsyncConnection(dbPath);
        }

        private SQLiteAsyncConnection? database;
        private string dbPath = Path.Combine(FileSystem.AppDataDirectory, "meals.db");
    }
}
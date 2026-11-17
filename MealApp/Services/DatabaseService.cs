using MealApp.Models;
using SQLite;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

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
                await database.CreateTableAsync<Meal>().ConfigureAwait(false);
                await database.CreateTableAsync<CalendarEntry>().ConfigureAwait(false);
                await database.CreateTableAsync<CustomShoppingItem>().ConfigureAwait(false);
            }
            catch (Exception ex)
            {

                System.Diagnostics.Debug.WriteLine($"DB Init Fehler: {ex.Message}");                
                database = null;
                throw;
            }
        }

        

        public async Task<List<Meal>> GetMealsAsync()
        {
            await InitializeAsync().ConfigureAwait(false);
            return await database!.Table<Meal>().ToListAsync().ConfigureAwait(false);
        }

        public async Task<List<CalendarEntry>> GetCalendarEntriesAsync()
        {
            await InitializeAsync().ConfigureAwait(false);
            var entries = await database!.Table<CalendarEntry>().ToListAsync().ConfigureAwait(false);
            List<Meal> meals = await database.Table<Meal>().ToListAsync().ConfigureAwait(false);

            Dictionary<int, Meal> mealDictionary = meals.ToDictionary(m => m.Id);

            foreach (var entry in entries)
            {
                mealDictionary.TryGetValue(entry.MealId, out Meal? meal);
                entry.Meal = meal;
                
            }
            return entries;
        }

        public async Task AddCalendarEntryAsync(CalendarEntry entry)
        {
            await InitializeAsync().ConfigureAwait(false);
            await database!.InsertAsync(entry).ConfigureAwait(false);
        }

        public async Task DeleteCalendarEntryAsync(CalendarEntry entry)
        {
            await InitializeAsync().ConfigureAwait(false);
            await database!.DeleteAsync(entry).ConfigureAwait(false);
        }

        public async Task AddMealAsync(Meal meal)
        {
            await InitializeAsync().ConfigureAwait(false);
            await database!.InsertAsync(meal).ConfigureAwait(false);
        }

        public async Task DeleteMealAsync(Meal meal)
        {
            await InitializeAsync().ConfigureAwait(false);
            await database!.DeleteAsync(meal).ConfigureAwait(false);
        }

        public async Task UpdateMealAsync(Meal meal)
        {
            await InitializeAsync().ConfigureAwait(false);
            await database!.UpdateAsync(meal).ConfigureAwait(false);
        }

        public async Task<List<CustomShoppingItem>> GetCustomShoppingItemsAsync()
        {
            await InitializeAsync().ConfigureAwait(false);
            return await database!.Table<CustomShoppingItem>().ToListAsync().ConfigureAwait(false);
        }

        public async Task AddCustomShoppingItemAsync(CustomShoppingItem item)
        {
            await InitializeAsync().ConfigureAwait(false);
            await database!.InsertAsync(item).ConfigureAwait(false);
        }

        public async Task UpdateCustomShoppingItemAsync(CustomShoppingItem item)
        {
            await InitializeAsync().ConfigureAwait(false);
            await database!.UpdateAsync(item).ConfigureAwait(false);
        }

        public async Task DeleteCustomShoppingItemAsync(CustomShoppingItem item)
        {
            await InitializeAsync().ConfigureAwait(false);
            await database!.DeleteAsync(item).ConfigureAwait(false);
            
        }

        public async Task<string> ExportAsync()
        {
            await InitializeAsync().ConfigureAwait(false);

            await database!.ExecuteScalarAsync<int>("PRAGMA wal_checkpoint(TRUNCATE)").ConfigureAwait(false);

            string fileName = $"MealApp-Backup-{DateTime.Now:yyyy-MM-dd}.db3";
            string backupPath = Path.Combine(FileSystem.CacheDirectory, fileName);

            File.Copy(dbPath, backupPath, true);

            return backupPath;
        }

        public async Task ImportAsync(string importFilePath)
        {
            await InitializeAsync().ConfigureAwait(false);
            if (!File.Exists(importFilePath))
            {
                throw new FileNotFoundException("Die Importdatei wurde nicht gefunden.", importFilePath);
            }

            if (database != null)
            {
                await database.CloseAsync().ConfigureAwait(false); 
                database = null;
            }

            deleteIfExists(dbPath + ".shm");
            deleteIfExists(dbPath + ".wal");

            File.Copy(importFilePath, dbPath, true);

            database = new SQLiteAsyncConnection(dbPath);
        }

        private void deleteIfExists(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        private SQLiteAsyncConnection? database;
        private string dbPath = Path.Combine(FileSystem.AppDataDirectory, "meals.db");
    }
}
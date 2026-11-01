using MealApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealApp.Services
{
    public class MealServices
    {
        
        public ObservableCollection<Meal> Meals { get; set; } = new ObservableCollection<Meal>();

        public MealServices(DatabaseService databaseService)
        {
            this.databaseService = databaseService;
        }

        public async Task LoadMealsAsync()
        {
            var meals = await databaseService.GetMealsAsync();
            Meals.Clear();
            foreach (var meal in meals)
            {
                Meals.Add(meal);
            }
        }

        public async Task AddMealAsync(Meal meal)
        {
            await databaseService.AddMealAsync(meal);
            Meals.Add(meal);
        }

        public async Task UpdateMealAsync(Meal meal)
        {
            try
            {
                await databaseService.UpdateMealAsync(meal);

                var existing = Meals.FirstOrDefault(m => m.Id == meal.Id);
                if (existing != null)
                {
                    return;
                }

                var index = Meals.IndexOf(existing);
                Meals[index] = meal;
            }
            catch (Exception ex)
            {

                System.Diagnostics.Debug.WriteLine($"UpdateMeal Fehler: {ex.Message}");
            }
        }

        public async Task DeleteMealAsync(Meal meal)
        {
            await databaseService.DeleteMealAsync(meal);
            Meals.Remove(meal);
        }

        private readonly DatabaseService databaseService;
    }
    
}

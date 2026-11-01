using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MealApp.Models;
using System.Collections.ObjectModel;

namespace MealApp.Services
{
    public class ShoppingService
    {
        public ObservableCollection<CustomShoppingItem> CustomItems { get; } = new();

        public ShoppingService(DatabaseService databaseService)
        {
            this.databaseService = databaseService;
        }

        public async Task LoadAsync()
        {
            var items = await databaseService.GetCustomShoppingItemsAsync();
            CustomItems.Clear();
            foreach (var item in items)
            {
                CustomItems.Add(item);
            }
        }

        public async Task AddCustomItemAsync(CustomShoppingItem item)
        {
            await databaseService.AddCustomShoppingItemAsync(item);
            CustomItems.Add(item);
        }

        public async Task UpdateCustomItemAsync(CustomShoppingItem item)
        {
            await databaseService.UpdateCustomShoppingItemAsync(item);
        }

        public async Task DeleteCustomItemAsync(CustomShoppingItem item)
        {
            await databaseService.DeleteCustomShoppingItemAsync(item);
            CustomItems.Remove(item);
        }

        private readonly DatabaseService databaseService;
    }
}

using MealApp.Models;
using MealApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealApp.Tests.Fakes
{
    public class FakeShoppingService : IShoppingService
    {
        public ObservableCollection<CustomShoppingItem> CustomItems { get; } = new();

        public Task AddCustomItemAsync(CustomShoppingItem item)
        {
            return Task.CompletedTask;
        }

        public Task DeleteCustomItemAsync(CustomShoppingItem item)
        {
            return Task.CompletedTask;
        }

        public Task LoadAsync()
        {
            return Task.CompletedTask;
        }

        public Task UpdateCustomItemAsync(CustomShoppingItem item)
        {
            return Task.CompletedTask;
        }
    }
}

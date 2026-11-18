using System.Collections.ObjectModel;
using MealApp.Models;

namespace MealApp.Services
{
    public interface IShoppingService
    {
        ObservableCollection<CustomShoppingItem> CustomItems { get; }

        Task LoadAsync();
        Task AddCustomItemAsync(CustomShoppingItem item);
        Task UpdateCustomItemAsync(CustomShoppingItem item);
        Task DeleteCustomItemAsync(CustomShoppingItem item);
    }
}

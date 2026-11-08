using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MealApp.Models;
using MealApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace MealApp.ViewModels
{
    public partial class ShoppingListViewModel : BaseViewModel
    {
        

        public ObservableCollection<ShoppingItem> ShoppingItems { get; } = new();

        public bool IsEmpty => !ShoppingItems.Any();

        public ShoppingListViewModel(CalendarService calendarService, ShoppingService shoppingService)
        {
            this.calendarService = calendarService;
            this.shoppingService = shoppingService;
            Title = "EinkaufsListe";

            ShoppingItems.CollectionChanged += (s, e) => OnPropertyChanged(nameof(IsEmpty));
        }


        public async Task CheckItem(ShoppingItem item)
        {
            item.IsChecked = true;

            if (item.IsCustom && item.CustomItem != null)
            {
                item.CustomItem.LastBoughtDate = DateTime.Today;
                await shoppingService.UpdateCustomItemAsync(item.CustomItem);

                if (item.CustomItem.IntervalWeeks == 0)
                {
                    await shoppingService.DeleteCustomItemAsync(item.CustomItem);
                }
            }
            buildShoppingList();
        }

        public async Task DeleteCustomItem(CustomShoppingItem item)
        {
            await shoppingService.DeleteCustomItemAsync(item);
            buildShoppingList();
        }

        [RelayCommand]
        private async Task Load()
        {
            await calendarService.LoadEntriesAsync();
            await shoppingService.LoadAsync();
            weekOffset = 0;
            buildShoppingList();
        }
        private void buildShoppingList()
        {
            var today = DateTime.Today;
            int diff = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
            var monday = today.AddDays(-diff).AddDays(weekOffset * 7);
            var sunday = monday.AddDays(6);

            int weekNumber = ISOWeek.GetWeekOfYear(monday);
            WeekLabel = $"KW {weekNumber}: {monday:dd.MM} - {sunday:dd.MM.yyyy}";

            ShoppingItems.Clear();

            var weekEntries = calendarService.Entries
                .Where(e => e.Date.Date >= monday && e.Date.Date <= sunday)
                .ToList();

            var mealIngredients = weekEntries
                .Where(e => e.Meal != null)
                .SelectMany(e => e.Meal!.Ingredients.Select(i => new
                {
                    i.Name,
                    Amount = i.Amount * e.Persons,
                    i.Unit
                }))
                .GroupBy(i => new { Name = i.Name.ToLower().Trim(), i.Unit })
                .Select(g => new ShoppingItem
                {
                    Name = g.First().Name,
                    Amount = g.Sum(i => i.Amount),
                    Unit = g.Key.Unit,
                    IsCustom = false
                })
                .OrderBy(i => i.Name);

            var customItems = shoppingService.CustomItems
                .Where(i => i.ShouldAppearThisWeek(monday))
                .Select(i => new ShoppingItem
                {
                    Name = i.Name,
                    Amount = i.Amount,
                    Unit = i.Unit,
                    IsCustom = true,
                    CustomItem = i
                })
                .OrderBy(i => i.Name);

            foreach (var item in mealIngredients)
            {
                ShoppingItems.Add(item);
            }

            foreach (var item in customItems)
            {
                ShoppingItems.Add(item);
            }
        }

        [RelayCommand]
        private void ChangeWeek(int offset)
        {
            weekOffset += offset;
            buildShoppingList();
        }

        [ObservableProperty]
        private string weekLabel = string.Empty;

        private readonly CalendarService calendarService;
        private readonly ShoppingService shoppingService;
        private int weekOffset = 0;

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MealApp.Models;
using MealApp.Services;

namespace MealApp.ViewModels
{
    public partial class ShoppingItemDetailViewModel : BaseViewModel
    {
        public List<string> Units { get; } = new List<string> { "", "g", "kg", "ml", "l", 
            "Stück", "Packung", "Rolle" };
        public List<IntervalOption> IntervalOptions { get; } = new()
        {
            new IntervalOption { Label = "Einmalig", Weeks = 0 },
            new IntervalOption { Label = "Jede Woche", Weeks = 1 },
            new IntervalOption { Label = "Alle 2 Wochen", Weeks = 2 },
            new IntervalOption { Label = "Alle 3 Wochen", Weeks = 3 },
            new IntervalOption { Label = "Alle 4 Wochen ", Weeks = 4 },
            new IntervalOption { Label = "Alle 6 Wochen", Weeks = 6 },
            new IntervalOption { Label = "Alle 8 Wochen", Weeks = 8 }
        };

        public ShoppingItemDetailViewModel(ShoppingService shoppingService, CustomShoppingItem? itemToEdit = null)
        {
            this.shoppingService = shoppingService;
            this.itemToEdit = itemToEdit;
            Title = itemToEdit == null ? "Neuer Artikel" : "Artikel bearbeiten";

            if (itemToEdit != null)
            {
                Name = itemToEdit.Name;
                Amount = itemToEdit.Amount.ToString(System.Globalization.CultureInfo.InvariantCulture);
                SelectedUnit = itemToEdit.Unit;
                SelectedInterval = IntervalOptions.FirstOrDefault(io => io.Weeks == itemToEdit.IntervalWeeks)
                    ?? IntervalOptions[0];
            }
            else
            {
                SelectedInterval = IntervalOptions[0];
            }
        }

        [RelayCommand]
        private async Task Save()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                await PopupService.ShowInfoAsync("Fehler", "Bitte geben Sie einen Namen ein.", "❌");
                return;
            }

            double amount = double.TryParse(Amount, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double parsedAmount)
                ? parsedAmount
                : 0;

            if (itemToEdit != null)
            {
                itemToEdit.Name = Name.Trim();
                itemToEdit.Amount = amount;
                itemToEdit.Unit = SelectedUnit ?? "";
                itemToEdit.IntervalWeeks = SelectedInterval?.Weeks ?? 0;

                await shoppingService.UpdateCustomItemAsync(itemToEdit);
            }
            else
            {
                var newItem = new CustomShoppingItem
                {
                    Name = Name.Trim(),
                    Amount = amount,
                    Unit = SelectedUnit ?? "",
                    IntervalWeeks = SelectedInterval?.Weeks ?? 0
                };
                await shoppingService.AddCustomItemAsync(newItem);
            }
            await Shell.Current.Navigation.PopAsync();
        }

        [ObservableProperty]
        private string name = string.Empty;
        [ObservableProperty]
        private string amount = string.Empty;
        [ObservableProperty]
        private string? selectedUnit;
        [ObservableProperty]
        private IntervalOption? selectedInterval;

        private readonly ShoppingService shoppingService;
        private CustomShoppingItem itemToEdit;
    }

    public class IntervalOption
    {
        public string Label { get; set; } = string.Empty;
        public int Weeks { get; set; }
    }
}

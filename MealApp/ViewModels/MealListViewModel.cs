using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MealApp.Models;
using MealApp.Services;
using System.Collections.ObjectModel;

namespace MealApp.ViewModels
{
    public partial class MealListViewModel : BaseViewModel
    {
        public ObservableCollection<Meal> FilteredMeals { get; } = new();

        public bool IsEmpty => !FilteredMeals.Any();

        public MealListViewModel(MealServices mealServices)
        {
            this.mealServices = mealServices;
            Title = "Gerichte";
            MaxPrepTime = 180;

            FilteredMeals.CollectionChanged += (s, e) => OnPropertyChanged(nameof(IsEmpty));

            mealServices.Meals.CollectionChanged += (s, e) => applyFilter();
        }

        partial void OnIsComponentViewChanged(bool value)
        {
            applyFilter();
        }

        partial void OnSearchTextChanged(string value)
        {
            applyFilter();
        }

        partial void OnIngredientTextChanged(string value)
        {
            applyFilter();
        }

        partial void OnOnlyVeganChanged(bool value)
        {
            applyFilter();
        }

        partial void OnMaxPrepTimeChanged(int value)
        {
            applyFilter();
        }

        [RelayCommand]
        private async Task LoadMeals()
        {
            await mealServices.LoadMealsAsync();
            applyFilter();
        }

        [RelayCommand]
        private async Task DeleteMeal(Meal meal)
        {
            await mealServices.DeleteMealAsync(meal);
            applyFilter();
        }

        [RelayCommand]
        private void ClearFilters()
        {
            SearchText = string.Empty;
            IngredientText = string.Empty;
            OnlyVegan = false;
            MaxPrepTime = 180;
            
        }

        [RelayCommand]
        private void ToggleSearch()
        {
            IsSearchVisible = !IsSearchVisible;            
        }

        [RelayCommand]
        private void ShowMeals()
        {
            IsComponentView = false;
            Title = "Gerichte";
        }

        [RelayCommand]
        private void ShowComponents()
        {
            IsComponentView = true;
            Title = "Komponenten";            
        }

        private void applyFilter()
        {
            FilteredMeals.Clear();

            var filtered = mealServices.Meals
                .Where(m => m.IsComponent == IsComponentView)
                .AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                filtered = filtered.Where(m => m.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(IngredientText))
            {
                filtered = filtered.Where(m => m.Ingredients.Any(i => i.Name.Contains(IngredientText, StringComparison.OrdinalIgnoreCase)));
            }

            if (OnlyVegan)
            {
                filtered = filtered.Where(m => m.Vegan);
            }

            if (MaxPrepTime < 180)
            {
                filtered = filtered.Where(m => m.PreperationTime <= MaxPrepTime);
            }

            foreach (var meal in filtered)
            {
                FilteredMeals.Add(meal);
            }

            HasActiveFilters = !string.IsNullOrWhiteSpace(SearchText)
                || !string.IsNullOrWhiteSpace(IngredientText)
                || OnlyVegan
                || MaxPrepTime < 180;

            ActiveFiltersLabel = buildActiveFiltersLabel();
        }

        private string buildActiveFiltersLabel()
        {
            var parts = new List<string>();
            if (!string.IsNullOrWhiteSpace(SearchText)) parts.Add($"„{SearchText}\"");
            if (!string.IsNullOrWhiteSpace(IngredientText)) parts.Add($"enthält {IngredientText}");
            if (OnlyVegan) parts.Add("🌱");
            if (MaxPrepTime < 180) parts.Add($"≤ {MaxPrepTime} min");
            return string.Join(" • ", parts);
        }

        [ObservableProperty]
        private string searchText = string.Empty;
        [ObservableProperty]
        private string ingredientText = string.Empty;
        [ObservableProperty]
        private bool onlyVegan;
        [ObservableProperty]
        private int maxPrepTime;
        [ObservableProperty]
        private bool isSearchVisible;
        [ObservableProperty]
        private bool hasActiveFilters;
        [ObservableProperty]
        private string activeFiltersLabel = string.Empty;
        [ObservableProperty]
        private bool isComponentView;

        private readonly MealServices mealServices;
    }
}

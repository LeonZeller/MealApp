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
    public partial class AddMealViewModel : BaseViewModel
    {
        
        public List<string> Units { get; } = new List<string> { "g", "kg", "ml", "l", "Stück", "TL", "EL", "Prise" };

        public AddMealViewModel(MealServices mealService, Meal? mealToEdit = null)
        {
            this.mealService = mealService;
            this.mealToEdit = mealToEdit;
            Title = mealToEdit == null ? "Neues Gericht" : "Gericht bearbeiten";

            if (mealToEdit != null)
            {
                Name = mealToEdit.Name;
                PrepTime = mealToEdit.PreperationTime.ToString();
                Steps = string.Join(Environment.NewLine, mealToEdit.PreperationSteps);
                Vegan = mealToEdit.Vegan;
                ImagePath = mealToEdit.ImagePath;
                IsComponent = mealToEdit.IsComponent;
            }
        }

        public List<string> AllIngredientNames =>
            mealService.Meals
                .SelectMany(m => m.Ingredients)
                .Select(i => i.Name)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(n => n)
                .ToList();

        public List<string> ComponentNames =>
            mealService.Meals
                .Where(m => m.IsComponent)
                .Select(m => m.Name)                
                .OrderBy(n => n)
                .ToList();

        public bool HasImage => !string.IsNullOrWhiteSpace(ImagePath);

        partial void OnImagePathChanged(string value)
        {
            OnPropertyChanged(nameof(HasImage));
        }

        [RelayCommand]
        private async Task Save(List<(string Name, double Amount, string Unit)> ingredients)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                await Shell.Current.DisplayAlert("Fehler", "Bitte einen Namen eingeben.", "OK");
                return;
            }

            if (!int.TryParse(PrepTime, out int prepTime))
            {
                await Shell.Current.DisplayAlert("Fehler", "Bitte eine gültige Vorbereitungszeit eingeben.", "OK");
                return;
            }

            var ingredientList = ingredients
                .Where(i => !string.IsNullOrWhiteSpace(i.Name))
                .Select(i => new Ingredient
                {
                    Name = i.Name.Trim(),
                    Amount = i.Amount,
                    Unit = i.Unit
                })
                .ToList();

            var stepList = Steps
                .Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim())
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .ToList();

            if (mealToEdit != null)
            {
                mealToEdit.Name = Name.Trim();
                mealToEdit.PreperationTime = prepTime;
                mealToEdit.Vegan = Vegan;
                mealToEdit.ImagePath = ImagePath;
                mealToEdit.Ingredients = ingredientList;
                mealToEdit.PreperationSteps = stepList;
                mealToEdit.IsComponent = IsComponent;


                await mealService.UpdateMealAsync(mealToEdit);
            }
            else
            {
                var meal = new Meal(
                    name: Name.Trim(),
                    ingredients: ingredientList,
                    preperationTime: prepTime,
                    preperationSteps: stepList,
                    vegan: Vegan,
                    imagePath: ImagePath,
                    isComponent: IsComponent

                );  
                await mealService.AddMealAsync(meal);
            }
            await Shell.Current.GoToAsync("..");
        }

        private readonly MealServices mealService;
        private readonly Meal? mealToEdit;

        [ObservableProperty]
        private string name = string.Empty;

        [ObservableProperty]
        private string prepTime = string.Empty;

        [ObservableProperty]
        private string steps = string.Empty;

        [ObservableProperty]
        private bool vegan;

        [ObservableProperty]
        private string imagePath = string.Empty;

        [ObservableProperty]
        private bool isComponent;
        
    }
}

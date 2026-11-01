using MealApp.Models;
using MealApp.Services;
using MealApp.ViewModels;
using MealApp.Views;

namespace MealApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MealServices mealServices)
        {
            InitializeComponent();
            this.mealServices = mealServices;
            viewModel = new MealListViewModel(mealServices);
            BindingContext = viewModel;
            BindableLayout.SetItemsSource(MealsContainer, viewModel.FilteredMeals);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await viewModel.LoadMealsCommand.ExecuteAsync(null);
        }

        private void OnSearchClicked(object sender, EventArgs e)
        {
            viewModel.ToggleSearchCommand.Execute(null);
        }

        private async void OnMealButtonClicked(object sender, EventArgs e)
        {
            var border = (Border)sender;
            var meal = (Meal)border.BindingContext;
            await Navigation.PushAsync(new MealDetailPage(meal));
        }

        private async void OnAddClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddMealPage(mealServices, null, viewModel.IsComponentView));
        }

        private async void OnDeleteSwiped(object sender, EventArgs e)
        {
            var swipeItem = (SwipeItem)sender;
            var meal = (Meal)swipeItem.BindingContext;
            bool confirm = await PopupService.ConfirmAsync("Löschen", $"{meal.Name} wirklich löschen?", "Ja", "Nein", "🗑️");
            if (confirm)
                await viewModel.DeleteMealCommand.ExecuteAsync(meal);
        }

        private async void OnEditSwiped(object sender, EventArgs e)
        {
            var swipeItem = (SwipeItem)sender;
            var meal = (Meal)swipeItem.BindingContext;
            await Navigation.PushAsync(new AddMealPage(mealServices, meal));
        }

        private async void OnSettingsClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(SettingsPage));
        }

        private readonly MealListViewModel viewModel;
        private readonly MealServices mealServices;
        
    }


}
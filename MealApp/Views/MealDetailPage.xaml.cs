using MealApp.Models;
using MealApp.ViewModels;

namespace MealApp.Views;

public partial class MealDetailPage : ContentPage
{
    public MealDetailPage(Meal meal)
    {
        InitializeComponent();
        var viewModel = new MealDetailViewModel(meal);
        BindingContext = viewModel;

        BindableLayout.SetItemsSource(IngredientsContainer, meal.Ingredients);

        for (int i = 0; i < meal.PreperationSteps.Count; i++)
        {
            var grid = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(30) },
                    new ColumnDefinition { Width = GridLength.Star }
                },
                ColumnSpacing = 10
            };

            grid.Add(new Label
            {
                Text = $"{i + 1}.",
                FontAttributes = FontAttributes.Bold,
                VerticalOptions = LayoutOptions.Start
            }, 0, 0);

            grid.Add(new Label
            {
                Text = meal.PreperationSteps[i],
                VerticalOptions = LayoutOptions.Start
            }, 1, 0);

            StepsContainer.Children.Add(grid);
        }
    }

    private async void OnStartCookingClicked(object sender, EventArgs e)
    {
        var meal = ((MealDetailViewModel)BindingContext).Meal;
        await Navigation.PushAsync(new CookingPage(meal));
    }
}
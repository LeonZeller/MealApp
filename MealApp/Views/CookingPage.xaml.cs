using MealApp.Models;
using MealApp.ViewModels;
using System.Threading.Tasks;

namespace MealApp.Views;

public partial class CookingPage : ContentPage
{
	public CookingPage(Meal meal)
	{
		InitializeComponent();
        viewModel = new CookingViewModel(meal);
        BindingContext = viewModel;
    }

    private async void OnPreviousClicked(object sender, EventArgs e)
    {
        viewModel.PreviousStep();
    }

    private async void OnNextClicked(object sender, EventArgs e)
    {
        if (viewModel.IsLastStep)
        {
            await Navigation.PopAsync();
        }
        else
        {
            viewModel.NextStep();
        }
    }

    private readonly CookingViewModel viewModel;
}
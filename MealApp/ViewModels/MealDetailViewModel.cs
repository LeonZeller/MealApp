using CommunityToolkit.Mvvm.ComponentModel;
using MealApp.Models;

namespace MealApp.ViewModels
{
    public partial class MealDetailViewModel : BaseViewModel
    {
        [ObservableProperty]
        private Meal meal;

        public MealDetailViewModel(Meal meal)
        {
            this.meal = meal;
            Title = meal.Name;
        }
    }
}

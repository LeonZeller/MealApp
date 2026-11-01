using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MealApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;


namespace MealApp.ViewModels
{
    public partial class CookingViewModel : BaseViewModel
    {
        public string MealName { get; }

        public CookingViewModel(Meal meal)
        {
            this.meal = meal;
            MealName = meal.Name;
            Title = "Kochen";
            updateView();
        }

        public void NextStep()
        {
            if (currentStepIndex < meal.PreperationSteps.Count - 1)
            {
                currentStepIndex++;
                updateView();
            }
        }

        public void PreviousStep()
        {
            if (currentStepIndex > 0)
            {
                currentStepIndex--;
                updateView();
            }
        }

        public bool IsLastStep => currentStepIndex == meal.PreperationSteps.Count - 1;

        private void updateView()
        {
            CurrentStepNumber = currentStepIndex + 1;
            CurrentStepText = meal.PreperationSteps[currentStepIndex];
            StepProgress = $"Schritt {currentStepIndex + 1} von {meal.PreperationSteps.Count}";
            ProgressValue = (double)(currentStepIndex + 1) / meal.PreperationSteps.Count;
            CanGoPrevious = currentStepIndex > 0;
            NextButtonText = IsLastStep ? "Fertig" : "Weiter";
        }

        [ObservableProperty]
        private int currentStepNumber;
        [ObservableProperty]
        private string currentStepText = string.Empty;
        [ObservableProperty]
        private string stepProgress = string.Empty;
        [ObservableProperty]
        private double progressValue;
        [ObservableProperty]
        private bool canGoPrevious;
        [ObservableProperty]
        private string nextButtonText = "Weiter";

        private readonly Meal meal;
        private int currentStepIndex = 0;
    }
}

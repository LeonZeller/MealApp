using MealApp.Models;
using MealApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealApp.Tests.Fakes
{
    public class MealFilterTests
    {
        

        [Fact]
        public void Apply_OnlyVegan_ReturnsOnlyVeganMeals()
        {
            //Arrange
            var meals = new List<Meal>
            {
                meal("Bolognese"),
                meal("Gemüsecurry", vegan: true)
            };

            var options = new MealFilterOptions { OnlyVegan = true };
            var filter = new MealFilter();

            var result = filter.Apply(meals, options).ToList();

            //Assert
            var single = Assert.Single(result);
            Assert.Equal("Gemüsecurry", single.Name);
        }

        [Fact]
        public void Apply_PrepTime_ReturnsOnlyMealsWithinPrepTime()
        {
            //Arrange
            var meals = new List<Meal>
            {
                meal("Bolognese", prepTime: 20),
                meal("Gemüsecurry", prepTime: 60)
            };
            var options = new MealFilterOptions { MaxPrepTime = 45 };
            var filter = new MealFilter();
            var result = filter.Apply(meals, options).ToList();
            //Assert
            var single = Assert.Single(result);
            Assert.Equal("Bolognese", single.Name);
        }

        [Fact]
        public void Apply_SearchText_ReturnsOnlyMealsWithSearchText()
        {
            //Arrange
            var meals = new List<Meal>
            {
                meal("Spaghetti"),
                meal("Gemüsecurry")
            };
            var options = new MealFilterOptions { SearchText = "spag" };
            var filter = new MealFilter();
            var result = filter.Apply(meals, options).ToList();
            //Assert
            var single = Assert.Single(result);
            Assert.Equal("Spaghetti", single.Name);
        }

        [Fact]
        public void Apply_ComponentViewFalse_ReturnsNoComponents()
        {
            // Arrange
            var meals = new List<Meal>
            {
                meal("Bolognese"),
                meal("Tomatensoße", isComponent: true)
            };
            var options = new MealFilterOptions { IsComponentView = false };
            var filter = new MealFilter();

            // Act
            var result = filter.Apply(meals, options).ToList();

            // Assert
            var single = Assert.Single(result);
            Assert.Equal("Bolognese", single.Name);
        }

        [Fact]
        public void Apply_PrepTimeExactlyMax_MealIsIncluded()
        {
            // Arrange
            var meals = new List<Meal>
            {
                meal("Bolognese", prepTime: 45)
            };
            var options = new MealFilterOptions { MaxPrepTime = 45 };
            var filter = new MealFilter();

            // Act
            var result = filter.Apply(meals, options).ToList();

            // Assert
            Assert.Single(result);
        }

        [Fact]
        public void Apply_NoTimeLimit_LongMealIsIncluded()
        {
            // Arrange
            var meals = new List<Meal>
            {
                meal("Schmorbraten", prepTime: 200)
            };
            var options = new MealFilterOptions { MaxPrepTime = MealFilterOptions.NoTimeLimit };
            var filter = new MealFilter();

            // Act
            var result = filter.Apply(meals, options).ToList();

            // Assert
            Assert.Single(result);
        }

        private static Meal meal(string name, bool vegan = false, int prepTime = 30, bool isComponent = false)
            => new Meal { Name = name, Vegan = vegan, PreperationTime = prepTime, IsComponent = isComponent };
    }
}

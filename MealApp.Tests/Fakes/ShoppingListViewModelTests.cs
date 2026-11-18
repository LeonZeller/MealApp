using MealApp.Models;
using MealApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealApp.Tests.Fakes
{
    public class ShoppingListViewModelTests
    {
        [Fact]
        public async Task Load_EintragDieseWoche_ZutatenMitPersonenMultipliziert()
        {
            var fakeCalendarService = new FakeCalendarService();
            var fakeShoppingService = new FakeShoppingService();

            var meal = new Meal
            {
                Name = "Nudeln mit Soße",
                Ingredients = new List<Ingredient>
                {
                    new Ingredient { Name = "Nudeln", Amount = 100, Unit = "g" },                 
                }
            };

            fakeCalendarService.Entries.Add(new CalendarEntry
            {
                Meal = meal,
                Date = DateTime.Today,
                Persons = 2
            });

            var viewModel = new ShoppingListViewModel(fakeCalendarService, fakeShoppingService);

            await viewModel.LoadCommand.ExecuteAsync(null);

            var item = Assert.Single(viewModel.ShoppingItems);
            Assert.Equal("Nudeln", item.Name);
            Assert.Equal(200, item.Amount);
        }
    }
}

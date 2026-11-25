using MealApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealApp.Services
{
    public class MealFilter
    {
        public IEnumerable<Meal> Apply(IEnumerable<Meal> meals, MealFilterOptions options)
        {
            var filtered = meals
                .Where(m => m.IsComponent == options.IsComponentView);

            if (!string.IsNullOrWhiteSpace(options.SearchText))
            {
                filtered = filtered.Where(m => m.Name.Contains(options.SearchText, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(options.IngredientText))
            {
                filtered = filtered.Where(m => m.Ingredients.Any(i => i.Name.Contains(options.IngredientText, StringComparison.OrdinalIgnoreCase)));
            }

            if (options.OnlyVegan)
            {
                filtered = filtered.Where(m => m.Vegan);
            }

            if (options.MaxPrepTime < 180)
            {
                filtered = filtered.Where(m => m.PreperationTime <= options.MaxPrepTime);
            }

            return filtered;
        }
    }
}

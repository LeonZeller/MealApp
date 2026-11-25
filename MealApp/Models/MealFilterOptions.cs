using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealApp.Models
{
    public class MealFilterOptions
    {
        public bool IsComponentView { get; set; }
        public string SearchText { get; set; } = string.Empty;
        public string IngredientText { get; set; } = string.Empty;
        public bool OnlyVegan { get; set; }
        public int MaxPrepTime { get; set; } = NoTimeLimit;

        public const int NoTimeLimit = 180;

        
    }
}

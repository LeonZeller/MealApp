using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MealApp.Models
{
    public class ShoppingItem
    {
        public string Name { get; set; } = string.Empty;
        public double Amount { get; set; }
        public string Unit { get; set; } = string.Empty;
        public bool IsChecked { get; set; }
        public bool IsCustom { get; set; }
        public CustomShoppingItem? CustomItem { get; set; }

        public string DisplayAmount => Amount > 0 ? $"{Amount} {Unit}".Trim() : Unit;
    }
}

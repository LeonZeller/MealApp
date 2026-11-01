using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
namespace MealApp.Models
{
    [Table("CustomShoppingItems")]
    public class CustomShoppingItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Amount { get; set; }
        public string Unit { get; set; } = string.Empty;
        public int IntervalWeeks { get; set; }
        public DateTime? LastBoughtDate { get; set; }

        public bool ShouldAppearThisWeek(DateTime weekStart)
        {
            if (IntervalWeeks == 0)
            {
                return LastBoughtDate == null;
            }

            if (LastBoughtDate == null)
            {
                return true;
            }

            var weeksSinceLastBought = (int)((weekStart - LastBoughtDate.Value).TotalDays / 7);
            return weeksSinceLastBought >= IntervalWeeks;
        }
    }
}

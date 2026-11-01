using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace MealApp.Models
{
    [Table("CalendarEntries")]
    public class CalendarEntry
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan ServingTime { get; set; }
        public int MealId { get; set; }
        public MealTime MealTime { get; set; }
        public int Persons { get; set; }

        [Ignore]
        public Meal? Meal { get; set; }

        public DateTime StartCookingTime => 
            Date.Date + ServingTime - TimeSpan.FromMinutes(Meal?.PreperationTime ?? 0);

    }
}

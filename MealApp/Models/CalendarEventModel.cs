using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealApp.Models
{
    public class CalendarEventModel
    {
        public string Name { get; set; } = string.Empty;
        public MealTime MealTime { get; set; }
    }
}

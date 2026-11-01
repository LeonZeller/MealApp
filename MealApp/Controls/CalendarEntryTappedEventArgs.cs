using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MealApp.Models;

namespace MealApp.Controls
{
    public class CalendarEntryTappedEventArgs : EventArgs
    {
        public CalendarEntry Entry { get; set; } = null;
    }
}

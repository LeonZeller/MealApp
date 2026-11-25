using MealApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealApp.Tests.Fakes
{
    public class CustomShoppingItemTests
    {
        [Fact]
        public void ShouldAppearThisWeek_OneTimeItemNeverBought_ReturnsTrue()
        {
            // Arrange
            var item = new CustomShoppingItem
            {                
                IntervalWeeks = 0, // One-time item
                LastBoughtDate = null // Never bought
            };
            var monday = new DateTime(2026, 11, 9); // A Monday

            // Act
            var result = item.ShouldAppearThisWeek(monday);
            // Assert
            Assert.True(result);
        }   
    }
}

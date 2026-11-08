using MealApp.Models;

namespace MealApp.Tests
{
    public class CustomShoppingItemTests
    {
        [Fact]
        public void ShouldAppearThisWeek_OneTimeItemNeverBought_ReturnsTrue()
        {
            // Arrange: einen Artikel mit passenden Werten anlegen
            var item = new CustomShoppingItem
            {                
                IntervalWeeks = 0,
                LastBoughtDate = null
            };

            var monday = new DateTime(2026, 11, 9);

            // Act: die Methode aufrufen
            bool result = item.ShouldAppearThisWeek(monday);

            // Assert: prüfen, ob true bzw. false herauskommt
            Assert.True(result);
        }

        [Fact]
        public void ShouldAppearThisWeek_OneTimeItemBought_ReturnsFalse()
        {
            // Arrange: einen Artikel mit passenden Werten anlegen
            var item = new CustomShoppingItem
            {
                IntervalWeeks = 0,
                LastBoughtDate = new DateTime(2026, 11, 2)
            };

            var monday = new DateTime(2026, 11, 9);

            // Act: die Methode aufrufen
            bool result = item.ShouldAppearThisWeek(monday);

            // Assert: prüfen, ob true bzw. false herauskommt
            Assert.False(result);
        }

        [Fact]
        public void ShouldAppearThisWeek_ItemNeverBought_ReturnsTrue()
        {
            // Arrange: einen Artikel mit passenden Werten anlegen
            var item = new CustomShoppingItem
            {
                IntervalWeeks = 2,
                LastBoughtDate = null
            };

            var monday = new DateTime(2026, 11, 9);

            // Act: die Methode aufrufen
            bool result = item.ShouldAppearThisWeek(monday);

            // Assert: prüfen, ob true bzw. false herauskommt
            Assert.True(result);
        }

        [Fact]
        public void ShouldAppearThisWeek_IntervalExactlyReached_ReturnsTrue()
        {
            // Arrange: einen Artikel mit passenden Werten anlegen
            var item = new CustomShoppingItem
            {
                IntervalWeeks = 2,
                LastBoughtDate = new DateTime(2026, 10, 26)
            };

            var monday = new DateTime(2026, 11, 9);

            // Act: die Methode aufrufen
            bool result = item.ShouldAppearThisWeek(monday);

            // Assert: prüfen, ob true bzw. false herauskommt
            Assert.True(result);
        }

        [Fact]
        public void ShouldAppearThisWeek_TwoTimeItemBought_ReturnsFalse()
        {
            // Arrange: einen Artikel mit passenden Werten anlegen
            var item = new CustomShoppingItem
            {
                IntervalWeeks = 2,
                LastBoughtDate = new DateTime(2026, 11, 2)
            };

            var monday = new DateTime(2026, 11, 9);

            // Act: die Methode aufrufen
            bool result = item.ShouldAppearThisWeek(monday);

            // Assert: prüfen, ob true bzw. false herauskommt
            Assert.False(result);
        }

        [Fact]
        public void ShouldAppearThisWeek_WeeklyItemBoughtLastMidWeek_ReturnsTrue()
        {
            // Arrange: einen Artikel mit passenden Werten anlegen
            var item = new CustomShoppingItem
            {
                IntervalWeeks = 1,
                LastBoughtDate = new DateTime(2026, 11, 4)
            };

            var monday = new DateTime(2026, 11, 9);

            // Act: die Methode aufrufen
            bool result = item.ShouldAppearThisWeek(monday);

            // Assert: prüfen, ob true bzw. false herauskommt
            Assert.True(result);
        }

        
    }
}
using CanteenBillingSystem.Domain.Entities;
using CanteenBillingSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanteenBillingSystem.Domain.UnitTests
{
    public class MealTests
    {
        [Theory]
        [ClassData(typeof(MealTestCases))]
        public void GetTotal_ShouldCalculateCorrectTotal(
            List<MealItem> items,
            decimal expectedTotal)
        {
            // Arrange
            var meal = new Meal(items);

            // Act
            var total = meal.GetTotal();

            // Assert
            Assert.Equal(expectedTotal, total);
        }
    }
}
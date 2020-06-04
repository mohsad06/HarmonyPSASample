using HarmonyPSASample.Helper;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace HarmonyPSASample.Test
{
    public class TotalCostCalculatorTest
    {
        private readonly Mock<IAccountDetails> _moqAccountDetails;

        public TotalCostCalculatorTest()
        {
            _moqAccountDetails = new Mock<IAccountDetails>();
        }

        [Fact]
        public void GetTotalCostByDifferentBandsTest()
        {
            var customerId = Guid.NewGuid();

            _moqAccountDetails.Setup(x => x.NumberOfTextMessagesSentInMonth(customerId, 1, 1)).Returns(700);
            _moqAccountDetails.Setup(x => x.GetAccountPriceBands(customerId)).Returns(new List<PriceBand>
            {
                new PriceBand{QtyFrom=1, QtyTo=200, PricePerTextMessage=0.10m },
                new PriceBand{QtyFrom=201, QtyTo=500, PricePerTextMessage=0.08m },
                new PriceBand{QtyFrom=501, QtyTo=1000, PricePerTextMessage=0.06m },
                new PriceBand{QtyFrom=1001, PricePerTextMessage=0.03m }
            });

            var costCalculator = new TotalCostCalculator(_moqAccountDetails.Object);
            var totalCost = costCalculator.CalculateCost(customerId, 1, 1);

            Assert.Equal(56.00m, totalCost);
        }

        [Fact]
        public void GetTotalCostByOneBandTest()
        {
            var customerId = Guid.NewGuid();

            _moqAccountDetails.Setup(x => x.NumberOfTextMessagesSentInMonth(customerId, 1, 1)).Returns(700);
            _moqAccountDetails.Setup(x => x.GetAccountPriceBands(customerId)).Returns(new List<PriceBand>
            {
                new PriceBand{QtyFrom=1, PricePerTextMessage=0.10m }
            });

            var costCalculator = new TotalCostCalculator(_moqAccountDetails.Object);
            var totalCost = costCalculator.CalculateCost(customerId, 1, 1);

            Assert.Equal(70.00m, totalCost);
        }
    }
}

using HarmonyPSASample.Helper;
using System;

namespace HarmonyPSASample
{
    public class TotalCostCalculator : ITotalCostCalculator
    {
        private IAccountDetails _accountDetails;

        public TotalCostCalculator(IAccountDetails accountDetails)
        {
            _accountDetails = accountDetails;
        }

        public decimal CalculateCost(Guid customerAccount, int monthNo, int yearNo)
        {
            // Get the total number of messages using the helper interface
            var messageCount = _accountDetails.NumberOfTextMessagesSentInMonth(customerAccount, monthNo, yearNo);

            // Get all price bands using helper interface
            var priceBands = _accountDetails.GetAccountPriceBands(customerAccount);

            // The total cost of all messages based on message count and price bands
            decimal? totalCost = 0;

            // Using this variable we track the remaining message count that we need to apply in the calculation in case we run out of bands or the current band has a blank QtyTo value
            int? remainingMessageCount = messageCount;

            foreach (var priceBand in priceBands)
            {
                // Based on requirements this condition only happens when we're at the end of price bands list or there's only one price band
                if (priceBand.QtyTo == null)
                {
                    totalCost += priceBand.PricePerTextMessage * remainingMessageCount;
                    break;
                }
                else
                {
                    // When message count does not exceed the current band QtyTo that means we need to apply the remaining count to the calculation and finish up
                    if (messageCount < priceBand.QtyTo)
                    {
                        totalCost += priceBand.PricePerTextMessage * remainingMessageCount;
                        break;
                    }
                    // Message count exceeds the current band QtyTo, so we need to apply the whole range to the calculation and update remaining count
                    else
                    {
                        totalCost += priceBand.PricePerTextMessage * (priceBand.QtyTo - priceBand.QtyFrom + 1);
                        remainingMessageCount -= priceBand.QtyTo - priceBand.QtyFrom + 1;
                    }
                }
            }

            return totalCost.Value;
        }
    }
}

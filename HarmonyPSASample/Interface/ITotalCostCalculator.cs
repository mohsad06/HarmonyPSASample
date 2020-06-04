using System;

namespace HarmonyPSASample
{
    public interface ITotalCostCalculator
    {
        /// <summary>
        /// Calculates the total cost for the month from text message usage
        /// </summary>
        /// <param name="customerAccount">The identifier of the customer account</param>
        /// <param name="monthNo">Number of the month</param>
        /// <param name="yearNo">Number of the year</param>
        /// <returns>Total cost of message usage in the specified month</returns>
        decimal CalculateCost(Guid customerAccount, int monthNo, int yearNo);
    }
}

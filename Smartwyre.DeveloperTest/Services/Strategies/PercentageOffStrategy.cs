using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services.Strategies;

/// <summary>
/// Example of a new incentive type that could be easily added to the system.
/// This strategy calculates a percentage discount off the total product price.
/// </summary>
public class PercentageOffStrategy : IIncentiveCalculationStrategy
{
    public IncentiveType IncentiveType => IncentiveType.PercentageOff;

    public bool CanCalculate(Rebate rebate, Product product, CalculateRebateRequest request)
    {
        return rebate != null &&
               product != null &&
               product.SupportedIncentives.HasFlag(SupportedIncentiveType.PercentageOff) &&
               rebate.Percentage > 0 &&
               rebate.Percentage <= 1 && // Percentage must be between 0 and 1
               product.Price > 0 &&
               request.Volume > 0;
    }

    public decimal Calculate(Rebate rebate, Product product, CalculateRebateRequest request)
    {
        var totalPrice = product.Price * request.Volume;
        return totalPrice * rebate.Percentage;
    }
}

using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services.Strategies;

public class FixedRateRebateStrategy : IIncentiveCalculationStrategy
{
    public IncentiveType IncentiveType => IncentiveType.FixedRateRebate;

    public bool CanCalculate(Rebate rebate, Product product, CalculateRebateRequest request)
    {
        return rebate != null &&
               product != null &&
               product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedRateRebate) &&
               rebate.Percentage > 0 &&
               product.Price > 0 &&
               request.Volume > 0;
    }

    public decimal Calculate(Rebate rebate, Product product, CalculateRebateRequest request)
    {
        return product.Price * rebate.Percentage * request.Volume;
    }
}

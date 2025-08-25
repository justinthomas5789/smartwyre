using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services.Strategies;

public class FixedCashAmountStrategy : IIncentiveCalculationStrategy
{
    public IncentiveType IncentiveType => IncentiveType.FixedCashAmount;

    public bool CanCalculate(Rebate rebate, Product product, CalculateRebateRequest request)
    {
        return rebate != null &&
               product != null &&
               product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedCashAmount) &&
               rebate.Amount > 0;
    }

    public decimal Calculate(Rebate rebate, Product product, CalculateRebateRequest request)
    {
        return rebate.Amount;
    }
}

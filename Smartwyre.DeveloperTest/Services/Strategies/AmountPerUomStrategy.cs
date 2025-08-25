using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services.Strategies;

public class AmountPerUomStrategy : IIncentiveCalculationStrategy
{
    public IncentiveType IncentiveType => IncentiveType.AmountPerUom;

    public bool CanCalculate(Rebate rebate, Product product, CalculateRebateRequest request)
    {
        return rebate != null &&
               product != null &&
               product.SupportedIncentives.HasFlag(SupportedIncentiveType.AmountPerUom) &&
               rebate.Amount > 0 &&
               request.Volume > 0;
    }

    public decimal Calculate(Rebate rebate, Product product, CalculateRebateRequest request)
    {
        return rebate.Amount * request.Volume;
    }
}

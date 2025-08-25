using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Data;

public class RebateDataStore : IRebateDataStore
{
    public Rebate GetRebate(string rebateIdentifier)
    {
        return new Rebate
        {
            Incentive = IncentiveType.FixedCashAmount,
            Amount = 100,
            Percentage = 0.1m
        };
    }

    public void StoreCalculationResult(Rebate rebate, decimal rebateAmount)
    {
        Console.WriteLine($"Stored rebate calculation: {rebateAmount:C} for incentive type: {rebate.Incentive}");
    }
}

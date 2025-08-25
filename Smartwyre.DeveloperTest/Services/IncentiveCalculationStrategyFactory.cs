using System;
using System.Collections.Generic;
using Smartwyre.DeveloperTest.Services.Strategies;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services;

public class IncentiveCalculationStrategyFactory : IIncentiveCalculationStrategyFactory
{
    private readonly Dictionary<IncentiveType, IIncentiveCalculationStrategy> _strategies;

    public IncentiveCalculationStrategyFactory()
    {
        _strategies = new Dictionary<IncentiveType, IIncentiveCalculationStrategy>
        {
            { IncentiveType.FixedCashAmount, new FixedCashAmountStrategy() },
            { IncentiveType.FixedRateRebate, new FixedRateRebateStrategy() },
            { IncentiveType.AmountPerUom, new AmountPerUomStrategy() },
            { IncentiveType.PercentageOff, new PercentageOffStrategy() }
        };
    }

    public IIncentiveCalculationStrategy GetStrategy(IncentiveType incentiveType)
    {
        if (_strategies.TryGetValue(incentiveType, out var strategy))
        {
            return strategy;
        }

        throw new ArgumentException($"No strategy found for incentive type: {incentiveType}", nameof(incentiveType));
    }
}

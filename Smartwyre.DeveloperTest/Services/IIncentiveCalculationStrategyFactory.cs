using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services;

public interface IIncentiveCalculationStrategyFactory
{
    IIncentiveCalculationStrategy GetStrategy(IncentiveType incentiveType);
}

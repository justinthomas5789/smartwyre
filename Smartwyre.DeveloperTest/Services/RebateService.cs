using System;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services;

public class RebateService : IRebateService
{
    private readonly IRebateDataStore _rebateDataStore;
    private readonly IProductDataStore _productDataStore;
    private readonly IIncentiveCalculationStrategyFactory _strategyFactory;

    public RebateService(
        IRebateDataStore rebateDataStore,
        IProductDataStore productDataStore,
        IIncentiveCalculationStrategyFactory strategyFactory)
    {
        _rebateDataStore = rebateDataStore ?? throw new ArgumentNullException(nameof(rebateDataStore));
        _productDataStore = productDataStore ?? throw new ArgumentNullException(nameof(productDataStore));
        _strategyFactory = strategyFactory ?? throw new ArgumentNullException(nameof(strategyFactory));
    }

    public CalculateRebateResult Calculate(CalculateRebateRequest request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        // Lookup the rebate and product
        var rebate = _rebateDataStore.GetRebate(request.RebateIdentifier);
        var product = _productDataStore.GetProduct(request.ProductIdentifier);

        var result = new CalculateRebateResult();

        try
        {
            // Get the appropriate strategy for the incentive type
            var strategy = _strategyFactory.GetStrategy(rebate.Incentive);

            // Check if the strategy can calculate the rebate
            if (strategy.CanCalculate(rebate, product, request))
            {
                // Calculate the rebate amount
                var rebateAmount = strategy.Calculate(rebate, product, request);

                // Store the calculation result
                _rebateDataStore.StoreCalculationResult(rebate, rebateAmount);

                result.Success = true;
            }
            else
            {
                result.Success = false;
            }
        }
        catch (ArgumentException)
        {
            // Invalid incentive type
            result.Success = false;
        }

        return result;
    }
}

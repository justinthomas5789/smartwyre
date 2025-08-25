using Smartwyre.DeveloperTest.Services.Strategies;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests;

public class StrategyTests
{
    [Fact]
    public void FixedCashAmountStrategy_WithValidData_CanCalculateReturnsTrue()
    {
        // Arrange
        var strategy = new FixedCashAmountStrategy();
        var rebate = new Rebate { Amount = 100 };
        var product = new Product { SupportedIncentives = SupportedIncentiveType.FixedCashAmount };
        var request = new CalculateRebateRequest { Volume = 1 };

        // Act
        var canCalculate = strategy.CanCalculate(rebate, product, request);

        // Assert
        Assert.True(canCalculate);
    }

    [Fact]
    public void FixedCashAmountStrategy_WithInvalidData_CanCalculateReturnsFalse()
    {
        // Arrange
        var strategy = new FixedCashAmountStrategy();
        var rebate = new Rebate { Amount = 0 }; // Invalid amount
        var product = new Product { SupportedIncentives = SupportedIncentiveType.FixedCashAmount };
        var request = new CalculateRebateRequest { Volume = 1 };

        // Act
        var canCalculate = strategy.CanCalculate(rebate, product, request);

        // Assert
        Assert.False(canCalculate);
    }

    [Fact]
    public void FixedCashAmountStrategy_Calculate_ReturnsCorrectAmount()
    {
        // Arrange
        var strategy = new FixedCashAmountStrategy();
        var rebate = new Rebate { Amount = 150 };
        var product = new Product();
        var request = new CalculateRebateRequest { Volume = 5 };

        // Act
        var result = strategy.Calculate(rebate, product, request);

        // Assert
        Assert.Equal(150, result); // Should return rebate amount regardless of volume
    }

    [Fact]
    public void FixedRateRebateStrategy_WithValidData_CanCalculateReturnsTrue()
    {
        // Arrange
        var strategy = new FixedRateRebateStrategy();
        var rebate = new Rebate { Percentage = 0.1m };
        var product = new Product { SupportedIncentives = SupportedIncentiveType.FixedRateRebate, Price = 100 };
        var request = new CalculateRebateRequest { Volume = 2 };

        // Act
        var canCalculate = strategy.CanCalculate(rebate, product, request);

        // Assert
        Assert.True(canCalculate);
    }

    [Fact]
    public void FixedRateRebateStrategy_Calculate_ReturnsCorrectAmount()
    {
        // Arrange
        var strategy = new FixedRateRebateStrategy();
        var rebate = new Rebate { Percentage = 0.15m };
        var product = new Product { Price = 200 };
        var request = new CalculateRebateRequest { Volume = 3 };

        // Act
        var result = strategy.Calculate(rebate, product, request);

        // Assert
        Assert.Equal(90, result); // 200 * 0.15 * 3 = 90
    }

    [Fact]
    public void AmountPerUomStrategy_WithValidData_CanCalculateReturnsTrue()
    {
        // Arrange
        var strategy = new AmountPerUomStrategy();
        var rebate = new Rebate { Amount = 25 };
        var product = new Product { SupportedIncentives = SupportedIncentiveType.AmountPerUom };
        var request = new CalculateRebateRequest { Volume = 4 };

        // Act
        var canCalculate = strategy.CanCalculate(rebate, product, request);

        // Assert
        Assert.True(canCalculate);
    }

    [Fact]
    public void AmountPerUomStrategy_Calculate_ReturnsCorrectAmount()
    {
        // Arrange
        var strategy = new AmountPerUomStrategy();
        var rebate = new Rebate { Amount = 30 };
        var product = new Product();
        var request = new CalculateRebateRequest { Volume = 5 };

        // Act
        var result = strategy.Calculate(rebate, product, request);

        // Assert
        Assert.Equal(150, result); // 30 * 5 = 150
    }

    [Fact]
    public void PercentageOffStrategy_WithValidData_CanCalculateReturnsTrue()
    {
        // Arrange
        var strategy = new PercentageOffStrategy();
        var rebate = new Rebate { Percentage = 0.2m };
        var product = new Product { SupportedIncentives = SupportedIncentiveType.PercentageOff, Price = 100 };
        var request = new CalculateRebateRequest { Volume = 2 };

        // Act
        var canCalculate = strategy.CanCalculate(rebate, product, request);

        // Assert
        Assert.True(canCalculate);
    }

    [Fact]
    public void PercentageOffStrategy_Calculate_ReturnsCorrectAmount()
    {
        // Arrange
        var strategy = new PercentageOffStrategy();
        var rebate = new Rebate { Percentage = 0.25m };
        var product = new Product { Price = 80 };
        var request = new CalculateRebateRequest { Volume = 3 };

        // Act
        var result = strategy.Calculate(rebate, product, request);

        // Assert
        Assert.Equal(60, result); // (80 * 3) * 0.25 = 240 * 0.25 = 60
    }
}

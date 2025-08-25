using Moq;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Services.Strategies;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests;

public class RebateServiceTests
{
    private readonly Mock<IRebateDataStore> _mockRebateDataStore;
    private readonly Mock<IProductDataStore> _mockProductDataStore;
    private readonly Mock<IIncentiveCalculationStrategyFactory> _mockStrategyFactory;
    private readonly RebateService _rebateService;

    public RebateServiceTests()
    {
        _mockRebateDataStore = new Mock<IRebateDataStore>();
        _mockProductDataStore = new Mock<IProductDataStore>();
        _mockStrategyFactory = new Mock<IIncentiveCalculationStrategyFactory>();
        _rebateService = new RebateService(_mockRebateDataStore.Object, _mockProductDataStore.Object, _mockStrategyFactory.Object);
    }

    [Fact]
    public void Calculate_WithValidFixedCashAmount_ReturnsSuccess()
    {
        // Arrange
        var request = new CalculateRebateRequest { RebateIdentifier = "REB001", ProductIdentifier = "PROD001", Volume = 1 };
        var rebate = new Rebate { Incentive = IncentiveType.FixedCashAmount, Amount = 100 };
        var product = new Product { SupportedIncentives = SupportedIncentiveType.FixedCashAmount };
        var strategy = new FixedCashAmountStrategy();

        _mockRebateDataStore.Setup(x => x.GetRebate("REB001")).Returns(rebate);
        _mockProductDataStore.Setup(x => x.GetProduct("PROD001")).Returns(product);
        _mockStrategyFactory.Setup(x => x.GetStrategy(IncentiveType.FixedCashAmount)).Returns(strategy);

        // Act
        var result = _rebateService.Calculate(request);

        // Assert
        Assert.True(result.Success);
        _mockRebateDataStore.Verify(x => x.StoreCalculationResult(rebate, 100), Times.Once);
    }

    [Fact]
    public void Calculate_WithValidFixedRateRebate_ReturnsSuccess()
    {
        // Arrange
        var request = new CalculateRebateRequest { RebateIdentifier = "REB001", ProductIdentifier = "PROD001", Volume = 2 };
        var rebate = new Rebate { Incentive = IncentiveType.FixedRateRebate, Percentage = 0.1m };
        var product = new Product { SupportedIncentives = SupportedIncentiveType.FixedRateRebate, Price = 50 };
        var strategy = new FixedRateRebateStrategy();

        _mockRebateDataStore.Setup(x => x.GetRebate("REB001")).Returns(rebate);
        _mockProductDataStore.Setup(x => x.GetProduct("PROD001")).Returns(product);
        _mockStrategyFactory.Setup(x => x.GetStrategy(IncentiveType.FixedRateRebate)).Returns(strategy);

        // Act
        var result = _rebateService.Calculate(request);

        // Assert
        Assert.True(result.Success);
        _mockRebateDataStore.Verify(x => x.StoreCalculationResult(rebate, 10), Times.Once); // 50 * 0.1 * 2 = 10
    }

    [Fact]
    public void Calculate_WithValidAmountPerUom_ReturnsSuccess()
    {
        // Arrange
        var request = new CalculateRebateRequest { RebateIdentifier = "REB001", ProductIdentifier = "PROD001", Volume = 3 };
        var rebate = new Rebate { Incentive = IncentiveType.AmountPerUom, Amount = 25 };
        var product = new Product { SupportedIncentives = SupportedIncentiveType.AmountPerUom };
        var strategy = new AmountPerUomStrategy();

        _mockRebateDataStore.Setup(x => x.GetRebate("REB001")).Returns(rebate);
        _mockProductDataStore.Setup(x => x.GetProduct("PROD001")).Returns(product);
        _mockStrategyFactory.Setup(x => x.GetStrategy(IncentiveType.AmountPerUom)).Returns(strategy);

        // Act
        var result = _rebateService.Calculate(request);

        // Assert
        Assert.True(result.Success);
        _mockRebateDataStore.Verify(x => x.StoreCalculationResult(rebate, 75), Times.Once); // 25 * 3 = 75
    }

    [Fact]
    public void Calculate_WithUnsupportedIncentive_ReturnsFailure()
    {
        // Arrange
        var request = new CalculateRebateRequest { RebateIdentifier = "REB001", ProductIdentifier = "PROD001", Volume = 1 };
        var rebate = new Rebate { Incentive = IncentiveType.FixedCashAmount, Amount = 100 };
        var product = new Product { SupportedIncentives = SupportedIncentiveType.FixedRateRebate }; // Wrong incentive type
        var strategy = new FixedCashAmountStrategy();

        _mockRebateDataStore.Setup(x => x.GetRebate("REB001")).Returns(rebate);
        _mockProductDataStore.Setup(x => x.GetProduct("PROD001")).Returns(product);
        _mockStrategyFactory.Setup(x => x.GetStrategy(IncentiveType.FixedCashAmount)).Returns(strategy);

        // Act
        var result = _rebateService.Calculate(request);

        // Assert
        Assert.False(result.Success);
        _mockRebateDataStore.Verify(x => x.StoreCalculationResult(It.IsAny<Rebate>(), It.IsAny<decimal>()), Times.Never);
    }

    [Fact]
    public void Calculate_WithInvalidAmount_ReturnsFailure()
    {
        // Arrange
        var request = new CalculateRebateRequest { RebateIdentifier = "REB001", ProductIdentifier = "PROD001", Volume = 1 };
        var rebate = new Rebate { Incentive = IncentiveType.FixedCashAmount, Amount = 0 }; // Invalid amount
        var product = new Product { SupportedIncentives = SupportedIncentiveType.FixedCashAmount };
        var strategy = new FixedCashAmountStrategy();

        _mockRebateDataStore.Setup(x => x.GetRebate("REB001")).Returns(rebate);
        _mockProductDataStore.Setup(x => x.GetProduct("PROD001")).Returns(product);
        _mockStrategyFactory.Setup(x => x.GetStrategy(IncentiveType.FixedCashAmount)).Returns(strategy);

        // Act
        var result = _rebateService.Calculate(request);

        // Assert
        Assert.False(result.Success);
        _mockRebateDataStore.Verify(x => x.StoreCalculationResult(It.IsAny<Rebate>(), It.IsAny<decimal>()), Times.Never);
    }

    [Fact]
    public void Calculate_WithNullRequest_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _rebateService.Calculate(null));
    }

    [Fact]
    public void Calculate_WithInvalidIncentiveType_ReturnsFailure()
    {
        // Arrange
        var request = new CalculateRebateRequest { RebateIdentifier = "REB001", ProductIdentifier = "PROD001", Volume = 1 };
        var rebate = new Rebate { Incentive = (IncentiveType)999 }; // Invalid incentive type
        var product = new Product { SupportedIncentives = SupportedIncentiveType.FixedCashAmount };

        _mockRebateDataStore.Setup(x => x.GetRebate("REB001")).Returns(rebate);
        _mockProductDataStore.Setup(x => x.GetProduct("PROD001")).Returns(product);
        _mockStrategyFactory.Setup(x => x.GetStrategy((IncentiveType)999)).Throws(new ArgumentException("Invalid incentive type"));

        // Act
        var result = _rebateService.Calculate(request);

        // Assert
        Assert.False(result.Success);
        _mockRebateDataStore.Verify(x => x.StoreCalculationResult(It.IsAny<Rebate>(), It.IsAny<decimal>()), Times.Never);
    }

    [Fact]
    public void Calculate_WithValidPercentageOff_ReturnsSuccess()
    {
        // Arrange
        var request = new CalculateRebateRequest { RebateIdentifier = "REB001", ProductIdentifier = "PROD001", Volume = 2 };
        var rebate = new Rebate { Incentive = IncentiveType.PercentageOff, Percentage = 0.15m };
        var product = new Product { SupportedIncentives = SupportedIncentiveType.PercentageOff, Price = 100 };
        var strategy = new PercentageOffStrategy();

        _mockRebateDataStore.Setup(x => x.GetRebate("REB001")).Returns(rebate);
        _mockProductDataStore.Setup(x => x.GetProduct("PROD001")).Returns(product);
        _mockStrategyFactory.Setup(x => x.GetStrategy(IncentiveType.PercentageOff)).Returns(strategy);

        // Act
        var result = _rebateService.Calculate(request);

        // Assert
        Assert.True(result.Success);
        _mockRebateDataStore.Verify(x => x.StoreCalculationResult(rebate, 30), Times.Once); // (100 * 2) * 0.15 = 30
    }
}

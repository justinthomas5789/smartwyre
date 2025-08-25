using System;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Runner;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Smartwyre Rebate Calculator");
        Console.WriteLine("============================");
        Console.WriteLine();

        // Initialize dependencies
        var rebateDataStore = new RebateDataStore();
        var productDataStore = new ProductDataStore();
        var strategyFactory = new IncentiveCalculationStrategyFactory();
        var rebateService = new RebateService(rebateDataStore, productDataStore, strategyFactory);

        try
        {
            // Get input from user
            var request = GetCalculateRebateRequest();

            // Calculate rebate
            var result = rebateService.Calculate(request);

            // Display result
            DisplayResult(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }

    static CalculateRebateRequest GetCalculateRebateRequest()
    {
        Console.WriteLine("Enter rebate calculation details:");
        Console.WriteLine();

        Console.Write("Rebate Identifier: ");
        var rebateIdentifier = Console.ReadLine() ?? "REB001";

        Console.Write("Product Identifier: ");
        var productIdentifier = Console.ReadLine() ?? "PROD001";

        Console.Write("Volume: ");
        var volumeInput = Console.ReadLine() ?? "1";
        if (!decimal.TryParse(volumeInput, out var volume))
        {
            volume = 1;
            Console.WriteLine("Invalid volume, using default value: 1");
        }

        Console.WriteLine();

        return new CalculateRebateRequest
        {
            RebateIdentifier = rebateIdentifier,
            ProductIdentifier = productIdentifier,
            Volume = volume
        };
    }

    static void DisplayResult(CalculateRebateResult result)
    {
        Console.WriteLine("Calculation Result:");
        Console.WriteLine("===================");
        
        if (result.Success)
        {
            Console.WriteLine("✅ SUCCESS: Rebate calculation completed successfully!");
            Console.WriteLine("The rebate calculation has been stored.");
        }
        else
        {
            Console.WriteLine("❌ FAILED: Rebate calculation could not be completed.");
            Console.WriteLine("Possible reasons:");
            Console.WriteLine("- Product does not support the incentive type");
            Console.WriteLine("- Invalid rebate parameters (amount, percentage, etc.)");
            Console.WriteLine("- Missing or invalid data");
        }
    }
}

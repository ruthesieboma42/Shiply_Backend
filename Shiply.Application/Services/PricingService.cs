using Shiply.Application.Interfaces;

public class PricingService : IPricingService
{
    private const decimal BaseFare = 1500.00m; 
    private const decimal RatePerKm = 200.00m; 

    public (decimal distance, decimal price) CalculateQuote(string pickup, string destination)
    {
        
        decimal simulatedDistance = new Random().Next(5, 50);

        decimal totalPrice = BaseFare + (simulatedDistance * RatePerKm);

        return (simulatedDistance, totalPrice);
    }
}
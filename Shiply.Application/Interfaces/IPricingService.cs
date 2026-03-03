

    namespace Shiply.Application.Interfaces
    {
        public interface IPricingService
        {
            (decimal distance, decimal price) CalculateQuote(string pickup, string destination);
        }
    }


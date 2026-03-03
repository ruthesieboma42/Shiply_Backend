namespace Shiply.Domain.Models
{
    public abstract class User
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; } = string.Empty;
    }

    public class Customer : User
    {
        public string Address { get; set; }
        public List<Shipment> Shipments { get; set; } = new();
    }

    public class Driver : User
    {
        public string LicenseNumber { get; set; }
        public string CurrentTruckId { get; set; }
        public bool IsAvailable { get; set; } = true;

        public double? CurrentLatitude { get; set; }
        public double? CurrentLongitude { get; set; }
        public DateTime? LastLocationUpdate { get; set; }

        public List<Shipment> Shipments { get; set; } = new();
    }
}
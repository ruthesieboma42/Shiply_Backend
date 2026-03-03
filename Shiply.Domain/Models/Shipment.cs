namespace Shiply.Domain.Models
{
    public class Shipment
    {
        public Guid Id { get; set; }
        public string TrackingNumber { get; set; }


        public DateTime CreatedAt { get; set; }

        // Foreign Key / Relationship to Customer
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }

        public Guid? DriverId { get; set; } // The assigned driver
        public Driver? Driver { get; set; }

        public decimal DistanceKm { get; set; }
        public decimal TotalPrice { get; set; }
        public bool IsPaid { get; set; }

        public ICollection<TrackingEvent> TrackingHistory { get; private set; } = new List<TrackingEvent>();
        public string PickupAddress { get; set; }
        public string ReceiverAddress { get; set; }
        public string Status { get; set; }

        public void AddUpdate(string status, string location)
        {
            var newEvent = new TrackingEvent
            {
                Id = Guid.NewGuid(),
                Timestamp = DateTime.UtcNow,
                Status = status,
                Location = location,
                ShipmentId = this.Id
            };
            TrackingHistory.Add(newEvent);
        }
    }

    
}

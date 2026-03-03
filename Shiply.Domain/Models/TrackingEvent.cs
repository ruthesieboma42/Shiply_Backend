

namespace Shiply.Domain.Models
{
    public class TrackingEvent
    {
        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Status { get; set; } 
        public string Location { get; set; }

        public Guid ShipmentId { get; set; }
    }
}

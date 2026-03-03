namespace Shiply.Application.DTOs
{
    public class ShipmentResponseDto
    {
        public string TrackingNumber { get; set; }
        public string SenderName { get; set; }
        public string CurrentStatus { get; set; }
        public string PickupAddress { get; set; }
        public string ReceiverAddress { get; set; }
        public decimal DistanceKm { get; set; }
        public decimal TotalPrice { get; set; }
        public List<TrackingEventDto> History { get; set; }
    }
}
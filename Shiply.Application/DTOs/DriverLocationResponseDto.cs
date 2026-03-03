namespace Shiply.Application.DTOs
{
    public class DriverLocationResponseDto
    {
        public string TrackingNumber { get; set; }
        public string DriverName { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public DateTime? LastUpdated { get; set; }
    }
}
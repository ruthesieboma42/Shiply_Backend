
namespace Shiply.Application.DTOs
{
    public class TrackingEventDto
    {
        public DateTime Timestamp { get; set; }
        public string Status { get; set; }
        public string Location { get; set; }
    }
}

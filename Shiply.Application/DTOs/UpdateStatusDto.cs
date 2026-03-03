
namespace Shiply.Application.DTOs
{
    public class UpdateStatusDto
    {
        public string TrackingNumber { get; set; }
        public string NewStatus { get; set; } 
        public string Location { get; set; }
    }
}

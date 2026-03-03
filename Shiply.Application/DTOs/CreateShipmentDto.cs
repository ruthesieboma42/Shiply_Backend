
namespace Shiply.Application.DTOs
{
    public class CreateShipmentDto
    {
        public Guid CustomerId { get; set; }

        public string PickupAddress { get; set; }
        public string ReceiverAddress { get; set; }
    }
}

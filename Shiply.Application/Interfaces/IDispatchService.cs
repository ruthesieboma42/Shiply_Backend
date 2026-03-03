using Shiply.Domain.Models;

namespace Shiply.Application.Interfaces
{
    public interface IDispatchService
    {
        Task<bool> AutoAssignShipmentAsync(Shipment shipment);
        Task<bool> ProcessShipmentPaymentAsync(string trackingNumber);
    }
}
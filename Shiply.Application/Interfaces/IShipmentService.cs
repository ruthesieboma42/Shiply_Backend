using Shiply.Application.DTOs;
using Shiply.Domain.Models;

namespace Shiply.Application.Interfaces
{
    public interface IShipmentService
    {
        Task<bool> ProcessShipmentPaymentAsync(string trackingNumber);
        Task<ShipmentResponseDto> GetShipmentStatusAsync(string trackingNumber);
        Task<Shipment> CreateShipmentAsync(CreateShipmentDto dto);
        Task<List<ShipmentResponseDto>> GetDriverShipmentsAsync(Guid driverId);
        Task<List<ShipmentResponseDto>> GetCustomerShipmentsAsync(Guid customerId);

        Task<bool> UpdateStatusAsync(Guid driverId, UpdateStatusDto dto);
        Task<bool> UpdateDriverLocationAsync(Guid driverId, UpdateDriverLocationDto dto);
        Task<DriverLocationResponseDto?> GetDriverLocationAsync(string trackingNumber);

        Task<List<ShipmentResponseDto>> GetAvailableShipmentsAsync();

        Task<bool> AcceptShipmentAsync(Guid driverId, string trackingNumber);
    }
}
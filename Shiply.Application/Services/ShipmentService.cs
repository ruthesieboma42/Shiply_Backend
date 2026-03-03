using Microsoft.EntityFrameworkCore;
using Shiply.Application.DTOs;
using Shiply.Application.Interfaces;
using Shiply.Domain.Models;
using Shiply.Infrastructure;

public class ShipmentService : IShipmentService
{
    private readonly AppDbContext _context;
    private readonly IPricingService _pricingService;
    private readonly IDispatchService _dispatchService;

    public ShipmentService(AppDbContext context, IPricingService pricingService, IDispatchService dispatchService)
    {
        _context = context;
        _pricingService = pricingService;
        _dispatchService = dispatchService;
    }

    public async Task<ShipmentResponseDto> GetShipmentStatusAsync(string trackingNumber)
    {
        var shipment = await _context.Shipments
            .AsNoTracking()
            .Include(s => s.Customer)
            .Include(s => s.TrackingHistory)
            .FirstOrDefaultAsync(s => s.TrackingNumber == trackingNumber);

        if (shipment == null) return null;

        return new ShipmentResponseDto
        {
            TrackingNumber = shipment.TrackingNumber,
            SenderName = $"{shipment.Customer.FirstName} {shipment.Customer.LastName}",
            PickupAddress = shipment.PickupAddress,
            ReceiverAddress = shipment.ReceiverAddress,
            DistanceKm = shipment.DistanceKm,
            TotalPrice = shipment.TotalPrice,
            CurrentStatus = shipment.TrackingHistory
                                .OrderByDescending(h => h.Timestamp)
                                .FirstOrDefault()?.Status ?? "Pending",
            History = shipment.TrackingHistory
                .Select(h => new TrackingEventDto
                {
                    Status = h.Status,
                    Timestamp = h.Timestamp,
                    Location = h.Location
                }).ToList()
        };
    }

    public async Task<Shipment> CreateShipmentAsync(CreateShipmentDto dto)
    {
        var (distance, price) = _pricingService.CalculateQuote(dto.PickupAddress, dto.ReceiverAddress);

        var shipment = new Shipment
        {
            Id = Guid.NewGuid(),
            CustomerId = dto.CustomerId,
            PickupAddress = dto.PickupAddress,
            ReceiverAddress = dto.ReceiverAddress,
            DistanceKm = distance,
            TotalPrice = price,
            Status = "Pending",
            CreatedAt = DateTime.UtcNow,
            TrackingNumber = "SHP-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper()
        };

        _context.Shipments.Add(shipment);
        await _context.SaveChangesAsync();
        
        return shipment;
    }

   
    public async Task<List<ShipmentResponseDto>> GetAvailableShipmentsAsync()
    {
        return await _context.Shipments
            .AsNoTracking()
            .Include(s => s.Customer)
            .Include(s => s.TrackingHistory)
            .Where(s => s.DriverId == null)
            .Select(s => new ShipmentResponseDto
            {
                TrackingNumber = s.TrackingNumber,
                SenderName = $"{s.Customer.FirstName} {s.Customer.LastName}",
                PickupAddress = s.PickupAddress,
                ReceiverAddress = s.ReceiverAddress,
                DistanceKm = s.DistanceKm,
                TotalPrice = s.TotalPrice,
                CurrentStatus = s.TrackingHistory
                    .OrderByDescending(h => h.Timestamp)
                    .Select(h => h.Status)
                    .FirstOrDefault() ?? "Pending",
                History = s.TrackingHistory.Select(h => new TrackingEventDto
                {
                    Status = h.Status,
                    Timestamp = h.Timestamp,
                    Location = h.Location
                }).ToList()
            }).ToListAsync();
    }


    public async Task<bool> AcceptShipmentAsync(Guid driverId, string trackingNumber)
    {
        var shipment = await _context.Shipments
            .FirstOrDefaultAsync(s => s.TrackingNumber == trackingNumber);

        if (shipment == null || shipment.DriverId != null) return false;

        shipment.DriverId = driverId;
        shipment.Status = "Assigned";

        var trackingEvent = new TrackingEvent
        {
            Id = Guid.NewGuid(),
            ShipmentId = shipment.Id,
            Status = "Driver Accepted",
            Location = "Driver Self-Assigned",
            Timestamp = DateTime.UtcNow
        };

        _context.Set<TrackingEvent>().Add(trackingEvent);

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<ShipmentResponseDto>> GetDriverShipmentsAsync(Guid driverId)
    {
        return await _context.Shipments
            .AsNoTracking()
            .Where(s => s.DriverId == driverId)
            .Include(s => s.TrackingHistory)
            .Select(s => new ShipmentResponseDto
            {
                TrackingNumber = s.TrackingNumber,
                PickupAddress = s.PickupAddress,
                ReceiverAddress = s.ReceiverAddress,
                DistanceKm = s.DistanceKm,
                TotalPrice = s.TotalPrice,
                CurrentStatus = s.TrackingHistory.OrderByDescending(h => h.Timestamp)
                    .Select(h => h.Status).FirstOrDefault() ?? "Pending",
                History = s.TrackingHistory.Select(h => new TrackingEventDto
                {
                    Status = h.Status,
                    Timestamp = h.Timestamp,
                    Location = h.Location
                }).ToList()
            }).ToListAsync();
    }

    public async Task<List<ShipmentResponseDto>> GetCustomerShipmentsAsync(Guid customerId)
    {
        return await _context.Shipments
            .AsNoTracking()
            .Where(s => s.CustomerId == customerId)
            .Include(s => s.TrackingHistory)
            .Select(s => new ShipmentResponseDto
            {
                TrackingNumber = s.TrackingNumber,
                PickupAddress = s.PickupAddress,
                ReceiverAddress = s.ReceiverAddress,
                DistanceKm = s.DistanceKm,
                TotalPrice = s.TotalPrice,
                CurrentStatus = s.TrackingHistory.OrderByDescending(h => h.Timestamp)
                    .Select(h => h.Status).FirstOrDefault() ?? "Pending",
                History = s.TrackingHistory.Select(h => new TrackingEventDto
                {
                    Status = h.Status,
                    Timestamp = h.Timestamp,
                    Location = h.Location
                }).ToList()
            }).ToListAsync();
    }

    public async Task<bool> UpdateStatusAsync(Guid driverId, UpdateStatusDto dto)
    {
        var shipment = await _context.Shipments
            .FirstOrDefaultAsync(s => s.TrackingNumber == dto.TrackingNumber);

        if (shipment == null || shipment.DriverId != driverId) return false;

        var trackingEvent = new TrackingEvent
        {
            Id = Guid.NewGuid(),
            ShipmentId = shipment.Id,
            Status = dto.NewStatus,
            Location = dto.Location,
            Timestamp = DateTime.UtcNow
        };

        shipment.Status = dto.NewStatus;
        _context.Set<TrackingEvent>().Add(trackingEvent);

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ProcessShipmentPaymentAsync(string trackingNumber)
    {
        var shipment = await _context.Shipments
            .FirstOrDefaultAsync(s => s.TrackingNumber == trackingNumber);

        if (shipment == null) return false;

        shipment.IsPaid = true;
        shipment.Status = "Paid";

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateDriverLocationAsync(Guid driverId, UpdateDriverLocationDto dto)
    {
        var driver = await _context.Drivers.FindAsync(driverId);
        if (driver == null) return false;

        driver.CurrentLatitude = dto.Latitude;
        driver.CurrentLongitude = dto.Longitude;
        driver.LastLocationUpdate = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<DriverLocationResponseDto?> GetDriverLocationAsync(string trackingNumber)
    {
        var shipment = await _context.Shipments
            .Include(s => s.Driver)
            .FirstOrDefaultAsync(s => s.TrackingNumber == trackingNumber);

        if (shipment == null || shipment.Driver == null) return null;

        return new DriverLocationResponseDto
        {
            TrackingNumber = trackingNumber,
            DriverName = $"{shipment.Driver.FirstName} {shipment.Driver.LastName}",
            Latitude = shipment.Driver.CurrentLatitude,
            Longitude = shipment.Driver.CurrentLongitude,
            LastUpdated = shipment.Driver.LastLocationUpdate
        };
    }
}
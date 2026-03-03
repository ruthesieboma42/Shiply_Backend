using Microsoft.EntityFrameworkCore;
using Shiply.Application.Interfaces;
using Shiply.Domain.Models;
using Shiply.Infrastructure;

public class DispatchService : IDispatchService
{
    private readonly AppDbContext _context;

    public DispatchService(AppDbContext context) => _context = context;

    public async Task<bool> AutoAssignShipmentAsync(Shipment shipment)
    {
        var availableDriver = await _context.Drivers
            .FirstOrDefaultAsync(d => d.IsAvailable);

        if (availableDriver == null) return false;

        shipment.DriverId = availableDriver.Id;
        shipment.Status = "Assigned";
        shipment.AddUpdate("Driver Assigned", "Dispatch System");

        availableDriver.IsAvailable = false;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> ProcessShipmentPaymentAsync(string trackingNumber)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var shipment = await _context.Shipments
                .FirstOrDefaultAsync(s => s.TrackingNumber == trackingNumber);

            if (shipment == null) return false;

            shipment.IsPaid = true;
            shipment.Status = "Paid";
            await _context.SaveChangesAsync();

            await AutoAssignShipmentAsync(shipment);

            await transaction.CommitAsync();
            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
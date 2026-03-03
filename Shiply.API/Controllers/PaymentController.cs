using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shiply.Infrastructure;
using Shiply.Application;
using Microsoft.AspNetCore.Authorization;
using Shiply.Application.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IShipmentService _shipmentService;

    
    public PaymentController(IShipmentService shipmentService)
    {
        _shipmentService = shipmentService;
    }

    [HttpPost("pay/{trackingNumber}")]
    public async Task<IActionResult> ProcessPayment(string trackingNumber)
    {
        var success = await _shipmentService.ProcessShipmentPaymentAsync(trackingNumber);

        if (!success) return NotFound();

        return Ok(new { Message = "Payment Successful" });
    }
}

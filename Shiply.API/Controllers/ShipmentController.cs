using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shiply.Application.DTOs;
using Shiply.Application.Interfaces;
using System.Security.Claims;

namespace Shiply.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShipmentsController : ControllerBase
    {
        private readonly IShipmentService _shipmentService;
        public ShipmentsController(IShipmentService shipmentService)
        {
            _shipmentService = shipmentService;
        }

        [HttpGet("Track_Package")]
        public async Task<IActionResult> GetStatus(string trackingNumber)
        {
            var result = await _shipmentService.GetShipmentStatusAsync(trackingNumber);
            return result != null ? Ok(result) : NotFound("Package not found");
        }

        [Authorize]
        [HttpPost("Create_Shipment")]
        public async Task<IActionResult> CreateShipment([FromBody] CreateShipmentDto dto)
        {
            var shipment = await _shipmentService.CreateShipmentAsync(dto);
            return Ok(new { TrackingNumber = shipment.TrackingNumber });
        }

        [Authorize(Roles = "Driver")]
        [HttpGet("Assigned_Shipments")]
        public async Task<IActionResult> GetAssigned()
        {
            var claimValue = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(claimValue)) return Unauthorized();

            var driverId = Guid.Parse(claimValue);
            var shipments = await _shipmentService.GetDriverShipmentsAsync(driverId);
            return Ok(shipments);
        }

        [Authorize(Roles = "Driver")]
        [HttpPost("Update_Shipment")]
        public async Task<IActionResult> ScanPackage([FromBody] UpdateStatusDto dto)
        {
            var claimValue = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(claimValue)) return Unauthorized();

            var driverId = Guid.Parse(claimValue);
            var success = await _shipmentService.UpdateStatusAsync(driverId, dto);

            return success ? Ok("Status Updated") : Forbid("You are not assigned to this package.");
        }

        [Authorize(Roles = "Driver")]
        [HttpPost("Update_Location")]
        public async Task<IActionResult> UpdateLocation([FromBody] UpdateDriverLocationDto dto)
        {
            var claimValue = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(claimValue)) return Unauthorized();

            var driverId = Guid.Parse(claimValue);
            var success = await _shipmentService.UpdateDriverLocationAsync(driverId, dto);

            return success ? Ok("Location updated") : NotFound("Driver not found");
        }

        [HttpGet("Driver_Location")]
        public async Task<IActionResult> GetDriverLocation(string trackingNumber)
        {
            var location = await _shipmentService.GetDriverLocationAsync(trackingNumber);
            return location != null ? Ok(location) : NotFound("Shipment or driver not found");
        }

        [Authorize(Roles = "Driver")]
        [HttpGet("Available_Shipments")]
        public async Task<IActionResult> GetAvailable()
        {
            var shipments = await _shipmentService.GetAvailableShipmentsAsync();
            return Ok(shipments);
        }

        [Authorize(Roles = "Driver")]
        [HttpPost("Accept_Shipment")]
        public async Task<IActionResult> AcceptShipment([FromBody] AcceptShipmentDto dto)
        {
            var claimValue = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(claimValue)) return Unauthorized();

            var driverId = Guid.Parse(claimValue);
            var success = await _shipmentService.AcceptShipmentAsync(driverId, dto.TrackingNumber);

            return success ? Ok("Shipment accepted") : BadRequest("Shipment is unavailable or already assigned.");
        }

        [Authorize(Roles = "Customer")]
        [HttpGet("My_Shipments")]
        public async Task<IActionResult> MyShipments()
        {
            var claimValue = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(claimValue)) return Unauthorized();

            var customerId = Guid.Parse(claimValue);
            var shipments = await _shipmentService.GetCustomerShipmentsAsync(customerId);
            return Ok(shipments);
        }
    }
}
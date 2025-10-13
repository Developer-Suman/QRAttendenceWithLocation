using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using QRWithSignalR.BackGroundServices.Interface;
using QRWithSignalR.Entity;
using QRWithSignalR.Interface;
using QRWithSignalR.ServiceDTOs;
using QRWithSignalR.Services;
using QRWithSignalR.SignalRHub;

namespace QRWithSignalR.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]

    public class PurchaseController : ControllerBase
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IPurchaseServices _purcahseServices;
        private readonly IUserActivityChannel _activityChannel;

        public PurchaseController(IHubContext<NotificationHub> hubContext, IPurchaseServices purchaseServices, IUserActivityChannel activityChannel)
        {
            _hubContext = hubContext;
            _purcahseServices = purchaseServices;
            _activityChannel = activityChannel;

        }

        [HttpPost("add")]
        public async Task<IActionResult> AddPurchase([FromBody] PurchaseDTOs purchase)
        {
            try
            {
                await _purcahseServices.AddPurchaseAsync(purchase);

                await _activityChannel.WriteAsync(new UserActivityLogs(
                    "User123",
                    "Created a new sales record",
                    DateTime.UtcNow
                ));

                return Ok("Sales created successfully");

                return Ok(new { success = true, message = "Purchase successful." });

            }
            catch (Exception )
            {
                throw;
            }
        }

        [HttpGet("test")]
        public async Task<IActionResult> AddData(string abc)
        {
            await _purcahseServices.CreateNewQR(abc);
            return Ok(new { success = true, message = "Purchase successful." });
        }


        [HttpPost("MarkAttendance")]
        public async Task<IActionResult> MarkAttendance([FromBody] AttandanceDTOs request)
        {
            var clientIp = HttpContext.Connection.RemoteIpAddress?.ToString();

            // Define allowed IP (office public IP)

            string realOfficeIp = "Public IP: 104.28.196.82";
            string officeIp = "104.28.208.19";  // replace with your office IP
            if (clientIp != request.wifiIpAddress)
            {
                return Unauthorized(new { message = "You must be inside the office to mark attendance." });
            }

            // Validate token (check expiry, signature, etc.)
            //bool isValid = await _attendanceService.ValidateTokenAsync(request.QrToken, request.EmployeeId);
            //if (!isValid)
            //{
            //    return BadRequest(new { message = "Invalid or expired QR token." });
            //}

            //// Mark attendance
            //await _attendanceService.MarkAsync(request.EmployeeId);

            return Ok(new { message = "Attendance marked successfully!" });
        }

    }
}

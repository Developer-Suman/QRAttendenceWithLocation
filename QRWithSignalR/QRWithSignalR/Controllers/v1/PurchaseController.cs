using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using QRWithSignalR.BackGroundServices.Interface;
using QRWithSignalR.BackGroundServices.Services;
using QRWithSignalR.Entity;
using QRWithSignalR.Interface;
using QRWithSignalR.ServiceDTOs;
using QRWithSignalR.Services;
using QRWithSignalR.SignalRHub;
using System.Diagnostics;

namespace QRWithSignalR.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]

    public class PurchaseController : ControllerBase
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IPurchaseServices _purcahseServices;
        private readonly AdminActivityChannel _adminActicity;
        private readonly UserActivityChannel _userActicity;

        public PurchaseController(IHubContext<NotificationHub> hubContext, IPurchaseServices purchaseServices, AdminActivityChannel adminActivityChannel, UserActivityChannel userActivityChannel)
        {
            _hubContext = hubContext;
            _purcahseServices = purchaseServices;
            _adminActicity = adminActivityChannel;
            _userActicity = userActivityChannel;


        }

        [HttpPost("add")]
        public async Task<IActionResult> AddPurchase([FromBody] PurchaseDTOs purchase)
        {
            try
            {
                await _purcahseServices.AddPurchaseAsync(purchase);

                var activity = new UserActivityLogs
                (
                    userId: "123",
                    action: $"Purchase added with role {purchase.Role}",
                    timeStamp: DateTime.UtcNow);
        

                if (purchase.Role == "Admin")
                    _adminActicity.AddActivity(activity);
                else if (purchase.Role == "User")
                    _userActicity.AddActivity(activity);
                else
                    return BadRequest("Unsupported role.");

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

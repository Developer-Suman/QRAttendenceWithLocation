using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace QRWithSignalR.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    public class QRController : ControllerBase
    {


        [HttpPost("ScanMe")]
        public async Task<ActionResult> ScanMe()
        {
            return Ok();
        }
    }
}

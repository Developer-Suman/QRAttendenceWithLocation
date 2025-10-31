using Asp.Versioning;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QRWithSignalR.BackGroundServices.Interface;
using QRWithSignalR.Services;
using System.Security.Claims;

namespace QRWithSignalR.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly TokenServices _tokenServices;
        private readonly IOAuthService _googleAuthService;

        public AuthController(TokenServices tokenServices, IOAuthService oAuthService)
        { 
            _googleAuthService = oAuthService;
            _tokenServices = tokenServices;
        }

        [HttpGet("login-google")]
        public IActionResult LoginWithGoogle()
        {
            var redirectUrl = Url.Action("GoogleCallback", "Auth");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("google-callback")]
        public async Task<IActionResult> GoogleCallback()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (!result.Succeeded || result.Principal == null)
                return BadRequest("Google authentication failed.");

            var email = result.Principal.FindFirst(ClaimTypes.Email)?.Value ?? "";
            var name = result.Principal.FindFirst(ClaimTypes.Name)?.Value ?? "";

            var jwt = _tokenServices.GenerateToken(email, name);

            // Optional: You can redirect to React app with token in query
            return Redirect($"http://localhost:3000/oauth-success?token={jwt}");
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok(new { message = "Logged out successfully." });
        }



        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequest request)
        {
            var email = await _googleAuthService.VerifyGoogleTokenAsync(request.Credential);

            if (email == null)
                return Unauthorized("Invalid Google token");

            // ✅ Here you can:
            // 1. Find or create user in your database
            // 2. Generate JWT token
            // 3. Return token to React

            return Ok(new
            {
                success = true,
                email,
                message = "Google login successful"
            });
        }


        [HttpPost("facebook-login")]
        public async Task<IActionResult> FacebookLogin([FromBody] FacebookLoginRequest request)
        {
            var email = await _googleAuthService.VerifyFacebookTokenAsync(request.AccessToken);
            if (email == null) return Unauthorized("Invalid Facebook token");

            // 1. Find or create user
            // 2. Generate JWT
            // 3. Return to React
            return Ok(new { success = true, email });
        }



    }

    public class GoogleLoginRequest
    {
        public string Credential { get; set; } = string.Empty;
    }

    public class FacebookLoginRequest
    {
        public string AccessToken { get; set; } = string.Empty;
    }

}

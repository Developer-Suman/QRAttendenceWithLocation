using Azure.Core;
using Google.Apis.Auth;
using QRWithSignalR.BackGroundServices.Interface;
using System.Net.Http;
using System.Text.Json;

namespace QRWithSignalR.BackGroundServices.Services
{
    public class OAuthFactory : IOAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public OAuthFactory(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public async Task<string> VerifyFacebookTokenAsync(string accessToken)
        {
            // 1. Get App Access Token (AppId|AppSecret)
            var appId = _configuration["Authentication:Facebook:AppId"];
            var appSecret = _configuration["Authentication:Facebook:AppSecret"];
            var appAccessToken = $"{appId}|{appSecret}";

            // 2. Validate user access token
            var validationUrl =
                $"https://graph.facebook.com/debug_token?input_token={accessToken}&access_token={appAccessToken}";

            var response = await _httpClient.GetAsync(validationUrl);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            using var jsonDoc = JsonDocument.Parse(content);
            var data = jsonDoc.RootElement.GetProperty("data");

            bool isValid = data.GetProperty("is_valid").GetBoolean();
            if (!isValid) return null;

            // 3. Get user info from Graph API
            var userInfoUrl = $"https://graph.facebook.com/me?fields=id,name,email&access_token={accessToken}";
            var userInfoResponse = await _httpClient.GetAsync(userInfoUrl);
            userInfoResponse.EnsureSuccessStatusCode();

            var userInfoContent = await userInfoResponse.Content.ReadAsStringAsync();
            using var userInfoJson = JsonDocument.Parse(userInfoContent);
            string email = userInfoJson.RootElement.TryGetProperty("email", out var emailProp)
                ? emailProp.GetString()!
                : null!;

            return email; // or return userId, name, etc. as needed
        }

        public async Task<string> VerifyGoogleTokenAsync(string token)
        {
            try
            {
                var clientId = _configuration["Authentication:Google:ClientId"];

                var payload = await GoogleJsonWebSignature.ValidateAsync(
                    token,
                    new GoogleJsonWebSignature.ValidationSettings
                    {
                        Audience = new[] { clientId }
                    });

                return payload.Email; // or payload.Subject (Google user ID)
            }
            catch (Exception ex)
            {
                // optional: log exception details
                return null!;
            }
        }
    }
}

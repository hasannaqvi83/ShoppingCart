using Microsoft.AspNetCore.Authentication;
using ShoppingCart.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShoppingCart.Services.Tokens
{
    public class TokenAcquireService : ITokenAcquireService
    {
        private readonly ITokenCache _cache;
        private readonly AzureAdConfig _config;
        private readonly IHttpClientFactory _httpClientFactory;

        public TokenAcquireService(ITokenCache cache, AzureAdConfig config, IHttpClientFactory httpClientFactory)
        {
            _cache = cache;
            _config = config;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<TokenInfo> GetAccessTokenInfoForCurrentUserAsync(string[] scopes)
        {
            TokenInfo tokenInfo = null;
            var refreshToken = await _cache.GetRefreshTokenForCurrentUserAsync(Provider.Microsoft);
            // we would not be disposing httpClient object as it is handled by httpClientFactory
            var httpClient = _httpClientFactory.CreateClient();
            var scope = string.Join(" ", scopes);
            var pairs = new Dictionary<string, string>(){
                            { "grant_type", "refresh_token" },
                            { "client_id", _config.ClientId },
                            { "client_secret", _config.ClientSecret },
                            { "refresh_token", refreshToken },
                            { "scope", scope  },
                            //{ "redirect_uri", Configuration["Authentication:CallbackUrl"]},
                        };

            var content = new FormUrlEncodedContent(pairs);
            var tokenResponse = await httpClient.PostAsync("https://login.microsoftonline.com/common/oauth2/v2.0/token", content);
            if (tokenResponse.IsSuccessStatusCode)
            {
                tokenInfo = await GetTokenInfo(tokenResponse);
                //var strResponse = await tokenResponse.Content.ReadAsStringAsync();
                //var tokenInfo = JsonConvert.DeserializeObject<TokenInfo>(strResponse);
                //retValue = tokenInfo.access_token;
            }
            return tokenInfo;
        }

        private static async Task<TokenInfo> GetTokenInfo(HttpResponseMessage response, string provider = null)
        {
            var tokenInfo = new TokenInfo();
            var strPayload = await response.Content.ReadAsStringAsync();
            var payload = JsonDocument.Parse(strPayload);

            var access_token = payload.RootElement.GetString("access_token");
            if (!string.IsNullOrEmpty(access_token))
            {
                tokenInfo.access_token = access_token;
            }

            var refresh_token = payload.RootElement.GetString("refresh_token");
            if (!string.IsNullOrEmpty(refresh_token))
            {
                tokenInfo.refresh_token = refresh_token;
            }

            if (payload.RootElement.TryGetProperty("expires_in", out var property) && property.TryGetInt32(out var seconds))
            {
                var expiresAt = DateTimeOffset.UtcNow + TimeSpan.FromSeconds(seconds);
                string strExpiresAt = expiresAt.ToString("o", CultureInfo.InvariantCulture);
                tokenInfo.expires_at = strExpiresAt;
            }

            //temp fix for sales force
            if (provider == "salesforce" && string.IsNullOrEmpty(tokenInfo.expires_at))
                tokenInfo.expires_at = DateTime.Now.AddMinutes(60).ToString("O", CultureInfo.InvariantCulture);

            return tokenInfo;
        }
    }
}

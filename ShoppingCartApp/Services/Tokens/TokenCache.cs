using Microsoft.AspNetCore.Identity;
using ShoppingCart.Identity.User;
using ShoppingCart.Models;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace ShoppingCart.Services.Tokens
{
    public class TokenCache : ITokenCache
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserIdentity _currentUser;
        private readonly string[] TOKENS_NAMES = new string[] { "access_token", "refresh_token", "expires_at" };

        public TokenCache(UserManager<AppUser> userManager, IUserIdentity user)
        {
            _userManager = userManager;
            _currentUser = user;
        }

        public async Task SaveTokenInfo(TokenInfo tokenInfo, Provider provider)
        {
            var strProvider = provider.ToString();

            string currentUserId = _currentUser.Id;// string.IsNullOrEmpty(userId) ? _currentUser.Id : userId;
            AppUser user = await _userManager.FindByIdAsync(currentUserId);

            if (user == null || tokenInfo == null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(tokenInfo.access_token))
                await _userManager.SetAuthenticationTokenAsync(user, strProvider, "access_token", tokenInfo.access_token);

            if (!string.IsNullOrEmpty(tokenInfo.refresh_token))
                await _userManager.SetAuthenticationTokenAsync(user, strProvider, "refresh_token", tokenInfo.refresh_token);

            if (!string.IsNullOrEmpty(tokenInfo.expires_at))
                await _userManager.SetAuthenticationTokenAsync(user, strProvider, "expires_at", tokenInfo.expires_at);
        }

        public async Task RemoveTokenInfo(Provider provider)
        {
            var strProvider = provider.ToString();

            string currentUserId = _currentUser.Id;// string.IsNullOrEmpty(userId) ? _currentUser.Id : userId;
            AppUser user = await _userManager.FindByIdAsync(currentUserId);

            foreach (var TOKEN_NAME in TOKENS_NAMES)
            {
                await _userManager.RemoveAuthenticationTokenAsync(user, strProvider, TOKEN_NAME);
            }
        }

        private async Task<string> GetToken(Provider provider, string tokenName)
        {
            string token = null;

            var user = await _userManager.FindByIdAsync(_currentUser.Id);
            if (user == null)
            {
                throw new System.Exception($"Unable to load user with ID '{_currentUser.Id}'.");
            }

            var strProvider = provider.ToString();

            if (tokenName == "access_token")
            {
                var expiresAt = await _userManager.GetAuthenticationTokenAsync(user, strProvider, "expires_at");

                if (!string.IsNullOrEmpty(expiresAt))
                {
                    var dtExpiryDate = DateTime.Parse(expiresAt, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
                    //if access token is not expired return the same access token
                    if (dtExpiryDate > DateTime.Now.AddMinutes(2))
                    {
                        var access_token = await _userManager.GetAuthenticationTokenAsync(user, strProvider, "access_token");
                        if (!string.IsNullOrEmpty(access_token))
                        {
                            token = access_token;
                        }
                    }
                }
            }
            else
            {
                token = await _userManager.GetAuthenticationTokenAsync(user, strProvider, tokenName);
            }

            return token;
        }

        public async Task<string> GetAccessTokenForCurrentUserAsync(Provider provider = Provider.Microsoft)
        {
            return await GetToken(provider, "access_token");
        }

        public async Task<string> GetRefreshTokenForCurrentUserAsync(Provider provider = Provider.Microsoft)
        {
            return await GetToken(provider, "refresh_token");
        }
    }
}

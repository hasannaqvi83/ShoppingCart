using System.Threading.Tasks;

namespace ShoppingCart.Services.Tokens
{
    public interface ITokenAcquireService
    {
        public Task<TokenInfo> GetAccessTokenInfoForCurrentUserAsync(string[] scopes);
    }
}
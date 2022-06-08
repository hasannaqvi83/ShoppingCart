using System.Threading.Tasks;

namespace ShoppingCart.Services.Tokens
{
    public interface ITokenCache
    {
        Task<string> GetAccessTokenForCurrentUserAsync(Provider provider = Provider.Microsoft);
        Task<string> GetRefreshTokenForCurrentUserAsync(Provider provider = Provider.Microsoft);
        Task RemoveTokenInfo(Provider provider);
        Task SaveTokenInfo(TokenInfo tokenInfo, Provider provider);
    }
}

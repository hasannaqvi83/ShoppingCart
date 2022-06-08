using System.Security.Claims;

namespace ShoppingCart.Identity.User
{
    public interface IUserIdentity
    {
        string Id { get; }
        string Email { get; }
        string Name { get; }
        ClaimsPrincipal CurrentUser { get; }

    }
}

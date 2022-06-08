using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace ShoppingCart.Identity.User
{
    public class UserIdentity : IUserIdentity
    {
        private readonly IHttpContextAccessor _ctx;

        public ClaimsPrincipal CurrentUser => _ctx.HttpContext.User;

        public IEnumerable<Claim> Claims => CurrentUser?.Claims;

        public UserIdentity(IHttpContextAccessor httpContextAccessor)
        {
            _ctx = httpContextAccessor;
        }

        public string Id => Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier || x.Type == "sub")?.Value;
        public string Name => Claims?.FirstOrDefault(x => x.Type == "name")?.Value;
        public string Email => Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Name || x.Type == "email")?.Value;

    }
}

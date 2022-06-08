using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShoppingCart.Data;
using ShoppingCart.Identity.User;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController<T> : ControllerBase
    {
        private readonly ILogger<T> _logger;
        protected readonly ShoppingContext _db;
        protected readonly IUserIdentity _user;
        protected string BuyerId => _user.Id;

        public BaseApiController(ShoppingContext db, IUserIdentity user, ILogger<T> logger)
        {
            _db = db;
            _user = user;
            _logger = logger;
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShoppingCart.Data;
using ShoppingCart.Data.UOW.Interfaces;
using ShoppingCart.Identity.User;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController<T> : ControllerBase
    {
        private readonly ILogger<T> _logger;
        protected readonly IUserIdentity _user;
        protected readonly IUnitOfWork _db;
        protected string BuyerId => _user.Id;

        public BaseApiController(IUnitOfWork db, IUserIdentity user, ILogger<T> logger)
        {
            _user = user;
            _logger = logger;
            _db = db;
        }
    }
}
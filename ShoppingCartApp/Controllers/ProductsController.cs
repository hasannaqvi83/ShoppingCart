using ShoppingCart.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using ShoppingCart.Models;
using ShoppingCart.Data;
using System.Linq;
using System.Collections.Generic;
using API.Controllers;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Identity.User;

namespace ShoppingCart.Controllers
{
    [Authorize]
    public class ProductsController : BaseApiController<ProductsController>
    {
        public ProductsController(ShoppingContext db, IUserIdentity user, ILogger<ProductsController> logger) : base(db, user, logger)
        {
        }

        [HttpGet()]
        public async Task<ActionResult<List<Product>>> Get()
        {
            List<Product> list;
            try
            {
                list = await _db.Products.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new MiddlewareException(ExceptionCode.ApplicationError, ex.Message, ex);

            }
            return list ?? new List<Product>();
        }
    }
}

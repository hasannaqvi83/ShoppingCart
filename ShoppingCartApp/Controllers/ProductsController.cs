using API.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShoppingCart.Data.Entities;
using ShoppingCart.Data.UOW.Interfaces;
using ShoppingCart.Exceptions;
using ShoppingCart.Identity.User;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingCart.Controllers
{
    [Authorize]
    public class ProductsController : BaseApiController<ProductsController>
    {
        public ProductsController(IUnitOfWork db, IUserIdentity user, ILogger<ProductsController> logger) : base(db, user, logger)
        {
        }

        [HttpGet()]
        public async Task<ActionResult<IList<Product>>> GetAllAsync()
        {
            List<Product> list;
            try
            {
                list = await _db.Products.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new MiddlewareException(ExceptionCode.ApplicationError, ex.Message, ex);

            }
            return list ?? new List<Product>();
        }
    }
}

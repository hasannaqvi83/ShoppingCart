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
    public class CountriesController : BaseApiController<CountriesController>
    {
        public CountriesController(IUnitOfWork db, IUserIdentity user, ILogger<CountriesController> logger) : base(db, user, logger)
        {
        }

        [HttpGet()]
        public async Task<ActionResult<List<Country>>> GetCountries()
        {
            List<Country> list;
            try
            {
                list = await _db.Countries.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new MiddlewareException(ExceptionCode.ApplicationError, ex.Message, ex);

            }
            return list ?? new List<Country>();
        }




    }
}
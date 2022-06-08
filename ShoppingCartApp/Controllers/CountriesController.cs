using System;
using System.Threading.Tasks;
using API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data;
using ShoppingCart.Identity.User;
using ShoppingCart.Models;
using ShoppingCart.Exceptions;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace ShoppingCart.Controllers
{
    [Authorize]
    public class CountriesController : BaseApiController<CountriesController>
    {
        public CountriesController(ShoppingContext db, IUserIdentity user, ILogger<CountriesController> logger) : base(db, user, logger)
        {
        }

        [HttpGet()]
        public async Task<ActionResult<List<Country>>> GetCountries()
        {
            List<Country> list;
            try
            {
                list = await _db.Country.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new MiddlewareException(ExceptionCode.ApplicationError, ex.Message, ex);

            }
            return list ?? new List<Country>();
        }

      


    }
}
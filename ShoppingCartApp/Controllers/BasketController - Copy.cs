using API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShoppingCart.Data;
using ShoppingCart.Data.Entities;
using ShoppingCart.Data.Extensions;
using ShoppingCart.DTOs;
using ShoppingCart.Exceptions;
using ShoppingCart.Identity.User;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.Controllers
{
    public class BasketController2 : BaseApiControllerOld<BasketController>
    {
        public BasketController2(ShoppingContext db, IUserIdentity user, ILogger<BasketController> logger) : base(db, user, logger)
        {
        }

        [HttpGet]
        public async Task<ActionResult<BasketDto>> GetBasket()
        {
            var basket = await RetrieveBasket();
            if (basket == null) return NotFound();
            return basket.MapBasketToDto();
        }

        [HttpGet("shippingCost")]
        public async Task<ActionResult<double>> GetShippingCost()
        {
            var basket = await _db.Baskets
            .RetrieveBasketWithItems(BuyerId)
            .FirstOrDefaultAsync();

            if (basket == null) return NotFound();

            var subtotal = basket.Items.Sum(item => item.Product.Price * item.Quantity);

            //The checkout page will call a backend to calculate the total shipping cost. $10 shipping cost for orders less of $50 dollars and less. $20 for orders more than $50.
            return subtotal.CalculateShippingCost();
        }

        [HttpPost]
        public async Task<ActionResult<BasketDto>> AddItemToBasket(int productId, int quantity)
        {
            ActionResult<BasketDto> retValue = null;
            try
            {
                //lets retrive users basket and if its not available lets set it up
                var basket = await RetrieveBasket();
                if (basket == null)
                {
                    basket = CreateBasket();
                }

                //now lets find the product that user has requested to add
                var product = await _db.Products.FindAsync(productId);
                if (product == null)
                {
                    return BadRequest(new ProblemDetails { Title = "Product not found" });
                }

                //now add product in the basket
               // basket.AddItem(product, quantity);

                //lets save our chagnes
                var result = await _db.SaveChangesAsync() > 0;

                if (result)
                {
                    var basketDto = basket.MapBasketToDto();
                    retValue = SendBasketInfo(basketDto);
                }
            }
            catch (MiddlewareException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                //log exception somewhere and send client a generic message
                //LogError(ex);
                throw new MiddlewareException(ExceptionCode.ApplicationError, "An error has occurred.");
            }
            return retValue;
        }

        [HttpPut]
        public async Task<ActionResult<BasketDto>> UpdateItemInBasket(int productId, int totalQuantity)
        {
            ActionResult<BasketDto> retValue = null;
            try
            {
                //lets retrive users basket and if its not available lets set it up
                var basket = await RetrieveBasket();
                if (basket == null)
                {
                    basket = CreateBasket();
                }

                //now lets find the product that user has requested to add
                var product = await _db.Products.FindAsync(productId);
                if (product == null)
                {
                    return BadRequest(new ProblemDetails { Title = "Product not found" });
                }

                //now add product in the basket
               // basket.UpdateItem(product, totalQuantity);

                //lets save our chagnes
                var result = await _db.SaveChangesAsync() > 0;

                if (result)
                {
                    var basketDto = basket.MapBasketToDto();
                    return SendBasketInfo(basketDto);
                }

            }
            catch (MiddlewareException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                //log exception somewhere and send client a generic message
                //LogError(ex);
                throw new MiddlewareException(ExceptionCode.ApplicationError, "An error has occurred.");
            }
            return retValue;
        }

        private ActionResult<BasketDto> SendBasketInfo(BasketDto basketDto)
        {
            //Created 201 with location header is not important here since we are developing both front-end and the backend
            //however we are returning the updated basket here
            return CreatedAtRoute(null, basketDto);
        }

        [HttpDelete]
        public async Task<ActionResult> RemoveBasketItem(int productId)
        {
            try
            {
                var basket = await RetrieveBasket();

                if (basket != null)
                {
                   // basket.RemoveItem(productId);

                    var result = await _db.SaveChangesAsync() > 0;

                    if (!result)
                        return BadRequest(new ProblemDetails { Title = "Problem removing item from the basket" });

                    return Ok(true);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                //log exception somewhere and send client a generic message
                //LogError(ex);
                throw new MiddlewareException(ExceptionCode.ApplicationError, "An error has occurred.");
            }
        }

        //[HttpGet("total")]
        //public async Task<ActionResult<int>> GetBasketTotal()
        //{
        //    return 10;
        //}

        private async Task<Basket> RetrieveBasket()
        {
            if (string.IsNullOrEmpty(BuyerId))
                return null;

            return await (from basket in _db.Baskets
                                        .Include(basket => basket.Items)
                                        .ThenInclude(basketItem => basketItem.Product)
                          where basket.BuyerId == BuyerId
                          select basket).FirstOrDefaultAsync();
        }

        private Basket CreateBasket()
        {
            var buyerId = _user.Id;
            if (string.IsNullOrEmpty(buyerId))
            {
                buyerId = Guid.NewGuid().ToString();
                var cookieOptions = new CookieOptions { IsEssential = true, Expires = DateTime.Now.AddDays(30) };
                Response.Cookies.Append("buyerId", buyerId, cookieOptions);
            }
            var basket = new Basket { BuyerId = buyerId };
            _db.Baskets.Add(basket);
            return basket;
        }


    }
}
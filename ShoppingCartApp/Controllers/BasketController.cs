using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShoppingCart.Data.Entities;
using ShoppingCart.Data.Extensions;
using ShoppingCart.Data.UOW.Interfaces;
using ShoppingCart.DTOs;
using ShoppingCart.Exceptions;
using ShoppingCart.Identity.User;
using System;
using System.Threading.Tasks;

namespace ShoppingCart.Controllers
{
    public class BasketController : BaseApiController<BasketController>
    {
        public BasketController(IUnitOfWork db, IUserIdentity user, ILogger<BasketController> logger) : base(db, user, logger)
        {
        }

        [HttpGet]
        public async Task<ActionResult<BasketDto>> GetBasket()
        {
            var basket = await _db.Baskets.GetBasketAsync(_user.Id);
            if (basket == null) return NotFound();
            return basket.MapBasketToDto();
        }

        [HttpGet("shippingCost")]
        public async Task<ActionResult<double>> GetShippingCost()
        {
            return await _db.Baskets.GetShippingCostAsync(_user.Id);
        }

        [HttpPost]
        public async Task<ActionResult<BasketDto>> AddItemToBasket(int productId, int quantity)
        {
            ActionResult<BasketDto> retValue = null;
            try
            {
                //lets retrive users basket and if its not available lets set it up
                var basket = await _db.Baskets.GetBasketAsync(_user.Id);
                if (basket == null)
                {
                    basket = new Basket { BuyerId = _user.Id };
                    await _db.Baskets.AddAsync(basket);
                }

                //now lets find the product that user has requested to add
                var product = await _db.Products.GetByIdAsync(productId);
                if (product == null)
                {
                    return BadRequest(new ProblemDetails { Title = "Product not found" });
                }

                //now add product in the basket
                await _db.BasketItems.AddBasketItemAsync(basket, productId, quantity);

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
                var basket = await _db.Baskets.GetBasketAsync(_user.Id);
                if (basket == null)
                {
                    basket = new Basket { BuyerId = _user.Id };
                    await _db.Baskets.AddAsync(basket);
                }

                //now lets find the product that user has requested to add
                var product = await _db.Products.GetByIdAsync(productId);
                if (product == null)
                {
                    return BadRequest(new ProblemDetails { Title = "Product not found" });
                }

                //now add product in the basket
                //basket.UpdateItem(product, totalQuantity);
                await _db.BasketItems.UpdateItemAsync(basket.Id, productId, totalQuantity);

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
                var basket = await _db.Baskets.GetBasketAsync(_user.Id);

                if (basket != null)
                {
                    await _db.BasketItems.RemoveItemAsync(basket.Id, productId);

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
    }
}
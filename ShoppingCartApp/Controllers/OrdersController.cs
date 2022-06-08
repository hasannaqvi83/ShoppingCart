using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShoppingCart.Data;
using ShoppingCart.DTOs;
using ShoppingCart.Extensions;
using ShoppingCart.Identity.User;
using ShoppingCart.Models;

namespace API.Controllers
{
    [Authorize]
    public class OrdersController : BaseApiController<OrdersController>
    {
        public OrdersController(ShoppingContext db, IUserIdentity user, ILogger<OrdersController> logger) : base(db, user, logger)
        {
        }

        [HttpGet("{id}", Name = "GetOrder")]
        public async Task<ActionResult<OrderDto>> GetOrder(int id)
        {
            return await _db.Orders
                .ProjectOrderToOrderDto()
                .Where(x => x.BuyerId == User.Identity.Name && x.Id == id)
                .FirstOrDefaultAsync();
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateOrder(CreateOrderDto orderDto)
        {
            var basket = await _db.Baskets
                .RetrieveBasketWithItems(BuyerId)
                .FirstOrDefaultAsync();

            if (basket == null) return BadRequest(new ProblemDetails { Title = "Could not locate basket" });

            var items = new List<OrderItem>();

            foreach (var item in basket.Items)
            {
                //var productItem = await _db.Products.FindAsync(item.ProductId);
                var orderItem = new OrderItem
                {
                    ProductId = item.ProductId,
                    Price = item.Product.Price,
                    Quantity = item.Quantity
                };
                items.Add(orderItem);
            }

            var subtotal = items.Sum(item => item.Price * item.Quantity);

            var order = new Order
            {
                OrderItems = items,
                BuyerId = User.Identity.Name,
                ShippingAddress = orderDto.ShippingAddress,
                Subtotal = subtotal,
                ShippingCost = subtotal.CalculateShippingCost()
            };

            _db.Orders.Add(order);
            _db.Baskets.Remove(basket);

            var result = await _db.SaveChangesAsync() > 0;

            if (result) return CreatedAtRoute("GetOrder", new { id = order.Id }, order.Id);

            return BadRequest(new ProblemDetails { Title = "Problem creating order" });
        }
    }
}
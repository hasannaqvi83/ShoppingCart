using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShoppingCart.Data;
using ShoppingCart.Data.Entities;
using ShoppingCart.Data.Extensions;
using ShoppingCart.Data.UOW.Interfaces;
using ShoppingCart.Data.UOW.Repository;
using ShoppingCart.Exceptions;
using System.Linq;
using System.Threading.Tasks;

namespace UOW.Infrastructure.Repository
{
    public class BasketRepository : GenericRepository<Basket>, IBasketRepository
    {
        public BasketRepository(ShoppingContext context, ILogger logger) : base(context, logger)
        {
        }

        public async Task<Basket> GetBasketAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return null;

            return await (from basket in _db.Baskets
                                        .Include(basket => basket.Items)
                                        .ThenInclude(basketItem => basketItem.Product)
                          where basket.BuyerId == userId
                          select basket).FirstOrDefaultAsync();
        }

        public async Task<double?> GetShippingCostAsync(string userId)
        {
            var basket = await _db.Baskets
            .RetrieveBasketWithItems(userId)
            .FirstOrDefaultAsync();

            if (basket == null) return null;

            var subtotal = basket.Items.Sum(item => item.Product.Price * item.Quantity);

            //The checkout page will call a backend to calculate the total shipping cost. $10 shipping cost for orders less of $50 dollars and less. $20 for orders more than $50.
            return subtotal.CalculateShippingCost();
        }
    }
}

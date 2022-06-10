using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShoppingCart.Data;
using ShoppingCart.Data.Entities;
using ShoppingCart.Data.UOW.Interfaces;
using ShoppingCart.Data.UOW.Repository;
using ShoppingCart.Exceptions;
using System.Linq;
using System.Threading.Tasks;
namespace UOW.Infrastructure.Repository
{
    public class BasketItemRepository : GenericRepository<BasketItem>, IBasketItemRepository
    {
        int MAX_PRODUCT_QUANTITY = 10;

        public BasketItemRepository(ShoppingContext db, ILogger logger) : base(db, logger)
        {

        }

        public async Task AddBasketItemAsync(Basket basket, int productId, int quantity)
        {
            //if no item matching product id is found
            if (basket.Items.All(item => item.ProductId != productId))
            {
                await AddAsync(new BasketItem { Basket = basket, ProductId = productId, Quantity = quantity });
            }
            else
            {
                var existingItem = basket.Items.FirstOrDefault(item => item.ProductId == productId);
                if (existingItem != null)
                {
                    var newQuantity = existingItem.Quantity + quantity;
                    EnsureProductQuantity(newQuantity);
                    existingItem.Quantity = newQuantity;
                }
            }
        }

        public async Task UpdateItemAsync(int basketId, int productId, int quantity)
        {
            var existingItem = await _db.BasketItems.FirstOrDefaultAsync(item => item.ProductId == productId && item.BasketId == basketId);
            if (existingItem != null)
            {
                EnsureProductQuantity(quantity);
                existingItem.Quantity = quantity;
            }
        }

        public async Task RemoveItemAsync(int basketId, int productId)
        {
            var existingItem = await _db.BasketItems.FirstOrDefaultAsync(item => item.ProductId == productId && item.BasketId == basketId);
            if (existingItem != null)
            {
                _db.BasketItems.Remove(existingItem);
            }
        }

        public void EnsureProductQuantity(int quantity)
        {
            if (quantity > MAX_PRODUCT_QUANTITY)
            {
                throw new MiddlewareException(ExceptionCode.ProductQuantityExceeded, $"You cannot buy more than {MAX_PRODUCT_QUANTITY} items of this product");
            }
        }
    }
}

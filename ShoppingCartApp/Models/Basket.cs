using ShoppingCart.Exceptions;
using System.Collections.Generic;
using System.Linq;
namespace ShoppingCart.Models
{
    public class Basket
    {
        public int Id { get; set; }
        public string BuyerId { get; set; }
        public List<BasketItem> Items { get; set; } = new();

        public void AddItem(Product product, int quantity)
        {
            //if no item matching product id is found
            if (Items.All(item => item.ProductId != product.Id))
            {
                Items.Add(new BasketItem { Product = product, Quantity = quantity });
            }
            else
            {
                var existingItem = Items.FirstOrDefault(item => item.ProductId == product.Id);
                if (existingItem != null)
                {
                    var newQuantity = existingItem.Quantity + quantity;
                    EnsureProductQuantity(newQuantity);
                    existingItem.Quantity = newQuantity;
                }
            }
        }

        public void UpdateItem(Product product, int quantity)
        {
            //if no item matching product id is found
            var existingItem = Items.FirstOrDefault(item => item.ProductId == product.Id);
            if (existingItem != null)
            {
                EnsureProductQuantity(quantity);
                existingItem.Quantity = quantity;
            }
        }

        public void RemoveItem(int productId)
        {
            var item = Items.FirstOrDefault(item => item.ProductId == productId);
            if (item == null)
            {
                return;
            }
            Items.Remove(item);
        }

        int MAX_PRODUCT_QUANTITY = 10;
        public void EnsureProductQuantity(int quantity)
        {
            if (quantity > MAX_PRODUCT_QUANTITY)
            {
                throw new MiddlewareException(ExceptionCode.ProductQuantityExceeded, $"You cannot buy more than {MAX_PRODUCT_QUANTITY} items of this product");
            }
        }
    }
}

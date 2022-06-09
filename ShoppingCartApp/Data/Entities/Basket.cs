using ShoppingCart.Exceptions;
using System.Collections.Generic;
using System.Linq;
namespace ShoppingCart.Data.Entities
{
    public class Basket
    {
        public int Id { get; set; }
        public string BuyerId { get; set; }
        public List<BasketItem> Items { get; set; } = new();
    }
}

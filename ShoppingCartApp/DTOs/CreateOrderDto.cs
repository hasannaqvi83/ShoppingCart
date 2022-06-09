using ShoppingCart.Data.Entities;

namespace ShoppingCart.DTOs
{
    public class CreateOrderDto
    {
        public ShippingAddress ShippingAddress { get; set; }
    }
}
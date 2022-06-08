using ShoppingCart.Models;

namespace ShoppingCart.DTOs
{
    public class CreateOrderDto
    {
         public ShippingAddress ShippingAddress { get; set; }
    }
}
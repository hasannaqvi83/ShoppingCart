using Microsoft.EntityFrameworkCore;

namespace ShoppingCart.Models
{
    [Owned]
    public class ShippingAddress
    {
        public string FullName { get; set; }
        public string Address { get; set; }
    }
}

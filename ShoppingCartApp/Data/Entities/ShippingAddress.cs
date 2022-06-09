using Microsoft.EntityFrameworkCore;

namespace ShoppingCart.Data.Entities
{
    [Owned]
    public class ShippingAddress
    {
        public string FullName { get; set; }
        public string Address { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace ShoppingCart.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string BuyerId { get; set; }
        public ShippingAddress ShippingAddress { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public List<OrderItem> OrderItems { get; set; }
        public double Subtotal { get; set; }
        public double ShippingCost { get; set; }
        
        public double Total
        {
            get
            {
                return Subtotal + ShippingCost;
            }
        }
    }
}
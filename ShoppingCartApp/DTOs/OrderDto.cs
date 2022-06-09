using ShoppingCart.Data.Entities;
using System;
using System.Collections.Generic;

namespace ShoppingCart.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string BuyerId { get; set; }
        public ShippingAddress ShippingAddress { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
        public double Subtotal { get; set; }
        public double ShippingCost { get; set; }
        public string OrderStatus { get; set; }
        public double Total { get; set; }
    }
}
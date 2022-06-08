using System.Linq;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.DTOs;
using ShoppingCart.Models;

namespace API.Extensions
{
    public static class OrderExtensions
    {
        public static IQueryable<OrderDto> ProjectOrderToOrderDto(this IQueryable<Order> query)
        {
            return query
                .Select(order => new OrderDto
                {
                    Id = order.Id,
                    BuyerId = order.BuyerId,
                    OrderDate = order.OrderDate,
                    ShippingAddress = order.ShippingAddress,
                    ShippingCost = order.ShippingCost,
                    Subtotal = order.Subtotal,
                    Total = order.Total,
                    OrderItems = order.OrderItems.Select(item => new OrderItemDto
                    {
                        ProductId = item.ProductId,
                        Price = item.Price,
                        Quantity = item.Quantity
                    }).ToList()
                }).AsNoTracking();
        }
    }
}
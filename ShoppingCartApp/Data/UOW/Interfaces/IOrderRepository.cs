using ShoppingCart.Data.Entities;
using ShoppingCart.DTOs;
using System.Threading.Tasks;

namespace ShoppingCart.Data.UOW.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<OrderDto> GetOrder(string userId, int id);
    }
}
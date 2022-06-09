using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShoppingCart.Data;
using ShoppingCart.Data.Entities;
using ShoppingCart.Data.Extensions;
using ShoppingCart.Data.UOW.Interfaces;
using ShoppingCart.Data.UOW.Repository;
using ShoppingCart.DTOs;
using ShoppingCart.Exceptions;
using System.Linq;
using System.Threading.Tasks;

namespace UOW.Infrastructure.Repository
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(ShoppingContext context, ILogger logger) : base(context, logger)
        {
        }

        public async Task<OrderDto> GetOrder(string userId, int id)
        {
            return await _db.Orders
                .ProjectOrderToOrderDto()
                .Where(x => x.BuyerId == userId && x.Id == id)
                .FirstOrDefaultAsync();
        }
    }
}

using ShoppingCart.Data.Entities;
using System.Threading.Tasks;

namespace ShoppingCart.Data.UOW.Interfaces
{
    public interface IBasketRepository : IGenericRepository<Basket>
    {
        public Task<Basket> GetBasketAsync(string userId);
        public Task<double?> GetShippingCostAsync(string userId);

    }
}
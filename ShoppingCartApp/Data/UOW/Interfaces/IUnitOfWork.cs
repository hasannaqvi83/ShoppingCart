using System;
using System.Threading.Tasks;

namespace ShoppingCart.Data.UOW.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IBasketRepository Baskets { get; }
        IProductRepository Products { get; }
        IBasketItemRepository BasketItems { get; }
        IOrderRepository Orders { get; }
        ICountryRepository Countries { get; }


        Task<int> SaveChangesAsync();
    }
}

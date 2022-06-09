using Microsoft.Extensions.Logging;
using ShoppingCart.Data;
using ShoppingCart.Data.UOW.Interfaces;
using ShoppingCart.Identity.User;
using System.Threading.Tasks;
using UOW.Infrastructure.Repository;

namespace UOW.Infrastructure.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IUserIdentity _user;
        private readonly ShoppingContext _db;
        private readonly ILogger _logger;
        public IBasketRepository Baskets { get; private set; }
        public IProductRepository Products { get; private set; }
        public IBasketItemRepository BasketItems { get; private set; }
        public IOrderRepository Orders { get; private set; }
        public ICountryRepository Countries { get; private set; }

        public UnitOfWork(IUserIdentity user, ShoppingContext context, ILoggerFactory logger)
        {
            _user = user;
            _db = context;
            _logger = logger.CreateLogger("logs");
            Baskets = new BasketRepository(_db, _logger);
            Products = new ProductRepository(_db, _logger);
            BasketItems = new BasketItemRepository(_db, _logger);
            Orders = new OrderRepository(_db, _logger);
            Countries = new CountryRepository(_db, _logger);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _db.SaveChangesAsync();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}

using Microsoft.Extensions.Logging;
using ShoppingCart.Data;
using ShoppingCart.Data.Entities;
using ShoppingCart.Data.UOW.Interfaces;
using ShoppingCart.Data.UOW.Repository;
namespace UOW.Infrastructure.Repository
{
    public class CountryRepository : GenericRepository<Country>, ICountryRepository
    {
        public CountryRepository(ShoppingContext db, ILogger logger) : base(db, logger)
        {
        }
    }
}

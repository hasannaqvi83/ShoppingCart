using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShoppingCart.Data;
using ShoppingCart.Data.Entities;
using ShoppingCart.Data.Extensions;
using ShoppingCart.Data.UOW.Interfaces;
using ShoppingCart.Data.UOW.Repository;
using System.Linq;
using System.Threading.Tasks;

namespace UOW.Infrastructure.Repository
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(ShoppingContext context, ILogger logger) : base(context, logger)
        {
        }



    }
}

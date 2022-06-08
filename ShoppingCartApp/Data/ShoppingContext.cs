using IdentityServer4.EntityFramework.Options;
using ShoppingCart.Models;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ShoppingCart.Data
{
    public class ShoppingContext : ApiAuthorizationDbContext<AppUser>
    {
        public ShoppingContext(DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {

        }
        
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Basket> Baskets { get; set; }
        public virtual DbSet<Country> Country { get; set; }
        public virtual DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }


    }
}
using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(ShoppingCart.Areas.Identity.IdentityHostingStartup))]
namespace ShoppingCart.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}
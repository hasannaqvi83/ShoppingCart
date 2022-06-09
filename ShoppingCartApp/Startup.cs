using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ShoppingCart.ActionFilters;
using ShoppingCart.Data;
using ShoppingCart.Data.UOW.Interfaces;
using ShoppingCart.Identity.User;
using ShoppingCart.Models;
using System.Security.Cryptography.X509Certificates;
using UOW.Infrastructure.UOW;
using UOW.Service.Service;

namespace ShoppingCart
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            HostingEnvironment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment HostingEnvironment { get; }

        private X509Certificate2 LoadCertificate()
        {
            X509Certificate2 cert = null;
            using (X509Store certStore = new X509Store(StoreName.My, StoreLocation.CurrentUser))
            {
                certStore.Open(OpenFlags.ReadOnly);
                if (HostingEnvironment.IsStaging())
                {
                }
                else if (HostingEnvironment.IsProduction())
                {
                }
            }
            return cert;
        }



        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var cs = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ShoppingContext>(options =>
                options.UseSqlServer(cs));

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ShoppingContext>();


            var idsrv = services.AddIdentityServer(options =>
            {
                //options.Authentication.CookieLifetime = TimeSpan.FromHours(1);
                //options.Authentication.CookieSlidingExpiration = false;
            });

            if (!HostingEnvironment.IsDevelopment())
                idsrv = idsrv.AddSigningCredential(LoadCertificate());

            idsrv.AddApiAuthorization<AppUser, ShoppingContext>();

            services.AddAuthentication()
                  .AddIdentityServerJwt();


            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new HttpResponseExceptionFilter());
            });
            services.AddRazorPages();

            //https://docs.microsoft.com/en-us/aspnet/core/fundamentals/http-context?view=aspnetcore-3.1
            services.AddHttpContextAccessor();

            //https://www.talkingdotnet.com/3-ways-to-use-httpclientfactory-in-asp-net-core-2-1/
            services.AddHttpClient();

            services.AddScoped<IUserIdentity, UserIdentity>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient(typeof(BasketService), typeof(BasketService));

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });
            //services.AddApplicationInsightsTelemetry(Configuration["APPINSIGHTS_CONNECTIONSTRING"]);


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    //if you want to run the react dev server separately then uncomment the following line
                    // spa.UseProxyToSpaDevelopmentServer("http://localhost:3000");

                    //if you want to run the react dev server while you debug the aspnet core app then uncomment the following line
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}

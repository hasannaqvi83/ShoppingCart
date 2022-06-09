using Microsoft.AspNetCore.Identity;
using ShoppingCart.Data.Entities;
using ShoppingCart.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(ShoppingContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Setup roles
            var adminRole = await roleManager.FindByNameAsync("Admin");
            if (adminRole == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            if (await roleManager.FindByNameAsync("Member") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Member"));
            }

            if (!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    UserName = "hasantest",
                    Email = "hasan@test.com"
                };

                await userManager.CreateAsync(user, "P@ssw0rd");
                await userManager.AddToRoleAsync(user, "Member");

                var admin = new AppUser
                {
                    UserName = "admin",
                    Email = "admin@test.com"
                };

                await userManager.CreateAsync(admin, "P@ssw0rd");
                await userManager.AddToRolesAsync(admin, new[] { "Member", "Admin" });
            }

            if (!context.Products.Any())
            {
                var products = new List<Product>
                {
                    new Product
                    {
                        Name = "Product 1",
                        ProductCode = "P2022-0301",
                        Description = "This is product 1 description This is product 1 description This is product 1 description.",
                        Price = 25,
                        PictureUrl = "https://res.cloudinary.com/du2bfoiba/image/upload/v1654263395/cld-sample-5.jpg",
                    },
                    new Product
                    {
                        Name = "Product 2",
                        ProductCode = "P2023-0221",
                        Description = "This is product 2 description This is product 2 description This is product 2 description.",
                        Price = 20,
                        PictureUrl = "https://res.cloudinary.com/du2bfoiba/image/upload/v1654263393/cld-sample-2.jpg",
                    },
                    new Product
                    {
                        Name = "Product 3",
                        ProductCode = "P2025-02421",
                        Description = "This is product 3 description This is product 3 description This is product 3 description.",
                        Price = 67,
                        PictureUrl = "https://res.cloudinary.com/du2bfoiba/image/upload/v1654263367/sample.jpg",
                    },
                    new Product
                    {
                        Name = "Product 4",
                        ProductCode = "P2025-02421",
                        Description = "This is product 2 description This is product 2 description This is product 2 description.",
                        Price = 34,
                        PictureUrl = "https://res.cloudinary.com/du2bfoiba/image/upload/v1654263367/sample.jpg",
                    },
                    new Product
                    {
                        Name = "Product 5",
                        ProductCode = "P2023-0534324",
                        Description =
                        "This is product 2 description This is product 2 description This is product 2 description.",
                        Price = 10,
                        PictureUrl = "https://res.cloudinary.com/du2bfoiba/image/upload/v1654263367/sample.jpg",
                    },
                    new Product
                    {
                        Name = "Product 6",
                        ProductCode = "P2026-03434324",
                        Description = "This is product 2 description This is product 2 description This is product 2 description.",
                        Price = 36,
                        PictureUrl = "https://res.cloudinary.com/du2bfoiba/image/upload/v1654263367/sample.jpg",
                    },
                    new Product
                    {
                        Name = "Product 7",
                        ProductCode = "P2026-03434324",
                        Description =
                        "This is product 2 description This is product 2 description This is product 2 description.",
                        Price = 65,
                        PictureUrl = "https://res.cloudinary.com/du2bfoiba/image/upload/v1654263367/sample.jpg",
                    },
                    new Product
                    {
                        Name = "Product 8",
                        ProductCode = "P2026-03434324",
                        Description =
                        "This is product 2 description This is product 2 description This is product 2 description.",
                        Price = 23,
                        PictureUrl = "https://res.cloudinary.com/du2bfoiba/image/upload/v1654263367/sample.jpg",
                    }
                };

                foreach (var product in products)
                {
                    context.Products.Add(product);
                }
            }

            if (!context.Country.Any())
            {
                var countries = new List<Country>
                {
                    new Country
                    {
                        Name = "Australia",
                        CurrencyRate = 1,
                        CurrencySymbol = "$",
                        CurrencyCode = "AUD"
                    },
                     new Country
                    {
                        Name = "USA",
                        CurrencyRate = 0.72255064,
                        CurrencySymbol = "$",
                        CurrencyCode = "USD"
                    },
                      new Country
                    {
                        Name = "UK",
                        CurrencyRate = 0.57569287,
                        CurrencySymbol = "£",
                        CurrencyCode = "USD"
                    },
                     new Country
                    {
                        Name = "Japan",
                        CurrencyRate = 94.459706,
                        CurrencySymbol = "¥",
                        CurrencyCode = "JPY"
                    }
                };

                foreach (var country in countries)
                {
                    context.Country.Add(country);
                }
            }

            context.SaveChanges();
        }
    }
}
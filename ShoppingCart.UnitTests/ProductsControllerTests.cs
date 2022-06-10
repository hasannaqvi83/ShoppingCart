using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ShoppingCart.Controllers;
using ShoppingCart.Data;
using ShoppingCart.Data.Entities;
using ShoppingCart.Data.UOW.Interfaces;
using ShoppingCart.Identity.User;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ShoppingCart.UnitTests
{
    public class ProductsControllerTests
    {
        [Fact]
        public async void GetAllAsync_ShouldReturnListOfProducts()
        {

            // Arrange
            var productsRepo = new Mock<IProductRepository>();
            productsRepo.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(DbInitializer.GetProducts());

            var mockRepo = new Mock<IUnitOfWork>();
            mockRepo.Setup(uof => uof.Products)
                .Returns(productsRepo.Object);

            var user = new Mock<IUserIdentity>();

            var logger = new Mock<ILogger<ProductsController>>();

            var controller = new ProductsController(mockRepo.Object, user.Object, logger.Object);

            // Act
            var result = await controller.GetAllAsync();

            // Assert
            //var viewResult = Assert.IsType<ActionResult<IEnumerable<Product>>>(result);

            //var model = Assert.IsAssignableFrom<IEnumerable<Product>>(viewResult.ViewData.Model);
            //Assert.Equal(2, model.Count());
            Assert.NotNull(result.Value);
            Assert.Equal(8, result.Value.Count);
            mockRepo.Verify();
        }
    }
}

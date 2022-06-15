using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Xunit;
using Catalog.API.Repositories;
using Catalog.API.Entities;
using Microsoft.Extensions.Logging;
using Catalog.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Catalog.API.Dtos;
using FluentAssertions;

namespace Catalog.UnitTests
{
    public class ProductControllerTests
    {
        //convension: public void UnitOfWork_StateUnderTest_ExpectedBehavior()
        // stub: face instance

        private readonly Mock<IProductRepository> repositoryStub = new();
        //private readonly Mock<ILogger<ProductController>> loggerStub = new();
        private readonly Random rand = new();

        [Fact]
        public async Task GetProductAsync_WithUnexistingItem_ReturnsNotFound()
        {
            // Arrange 
            //var repositoryStub = new Mock<IProductRepository>();
            repositoryStub.Setup(repo => repo.GetProductAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Product)null);

            //var loggerStub = new Mock<ILogger<ProductController>>();

            var controller = new ProductController(repositoryStub.Object);

            // Act 
            var result = await controller.GetProductAsync(Guid.NewGuid());

            // Assert
            //Assert.IsType<NotFoundResult>(result.Result);
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetProductAsync_WithExistingProduct_ReturnsExpectedProduct()
        {
            // Arrange 
            //var repositoryStub = new Mock<IProductRepository>();
            var expectedProduct = CreateRandomProduct();

            repositoryStub.Setup(repo => repo.GetProductAsync(It.IsAny<Guid>()))
                .ReturnsAsync(expectedProduct);


            var controller = new ProductController(repositoryStub.Object);

            // Act 
            var result = await controller.GetProductAsync(Guid.NewGuid());

            // Assert
            /*
            Assert.IsType<ProductDto>(result.Value);
            var dto = (result as ActionResult<ProductDto>).Value;
            Assert.Equal(expectedProduct.Id, dto.Id);
            Assert.Equal(expectedProduct.Name, dto.Name);
            Assert.Equal(expectedProduct.Price, dto.Price);
            Assert.Equal(expectedProduct.CreatedDate, dto.CreatedDate);
            */
            //result.Value.Should().BeEquivalentTo(expectedProduct);
            result.Value.Should().BeEquivalentTo(
                expectedProduct,
                options => options.ComparingByMembers<Product>()
            );
        }

        [Fact]
        public async Task GetProductAsync_WithExistingProducts_ReturnsAllProducts()
        {
            // Arrange 
            var expectedProducts = new[]{
                CreateRandomProduct(),
                CreateRandomProduct(),
                CreateRandomProduct(),
            };

            repositoryStub.Setup(repo => repo.GetProductsAsync())
                .ReturnsAsync(expectedProducts);

            var controller = new ProductController(repositoryStub.Object);

            // Act 
            var actualProducts = await controller.GetProductsAsync();

            // Assert
            actualProducts.Should().BeEquivalentTo(
                expectedProducts,
                options => options.ComparingByMembers<Product>());

        }

        [Fact]
        public async Task CreateProductAsync_WithProductToCrete_ReturnsCreatedProduct()
        {
            // Arrange
            var productToCreate = new CreateProductDto()
            {
                Name = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                Price = rand.Next(1000)
            };

            var controller = new ProductController(repositoryStub.Object);

            // Act 
            var result = await controller.CreateProductAsync(productToCreate);

            // Assert
            var createdProduct = (result.Result as CreatedAtActionResult).Value as ProductDto;

            productToCreate.Should().BeEquivalentTo(
                createdProduct,
                options => options.ComparingByMembers<ProductDto>().ExcludingMissingMembers()
            );
            createdProduct.Id.Should().NotBeEmpty();
            createdProduct.CreatedDate.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
        }


        [Fact]
        public async Task UpdateProductAsync_WithExistingProduct_ReturnsNoContent()
        {
            // Arrange
            var existingProduct = CreateRandomProduct();

            repositoryStub.Setup(repo => repo.GetProductAsync(It.IsAny<Guid>()))
                .ReturnsAsync(existingProduct);

            var productId = existingProduct.Id;
            var productToUpdate = new UpdateProductDto()
            {
                Name = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                Price = existingProduct.Price + 3
            };

            var controller = new ProductController(repositoryStub.Object);

            // Act
            var result = await controller.UpdateProductAsync(productId, productToUpdate);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteProductAsync_WithExistingProduct_ReturnsNoContent()
        {
            // Arrange
            var existingProduct = CreateRandomProduct();

            repositoryStub.Setup(repo => repo.GetProductAsync(It.IsAny<Guid>()))
                .ReturnsAsync(existingProduct);

            var controller = new ProductController(repositoryStub.Object);

            // Act
            var result = await controller.DeleteProductAsync(existingProduct.Id);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task GetProductsAsync_WithMachingProducts_ReturnsMachingProducts()
        {
            // Arrange 
            var allProducts = new[]
            {
                new Product(){ Name = "Potion" },
                new Product(){ Name = "Antdote" },
                new Product(){ Name = "Hi-Potion" },
            };

            var nameToMatch = "Potion";

            repositoryStub.Setup(repo => repo.GetProductsAsync())
                .ReturnsAsync(allProducts);

            var controller = new ProductController(repositoryStub.Object);

            // Act
            IEnumerable<ProductDto> foundItems = await controller.GetProductsAsync(nameToMatch);

            // Assert
            foundItems.Should().OnlyContain(
                product => product.Name == allProducts[0].Name || product.Name == allProducts[2].Name
            );
        }

        private Product CreateRandomProduct()
        {
            return new()
            {
                Id = Guid.NewGuid(),
                Name = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                Price = rand.Next(1000),
                CreatedDate = DateTimeOffset.UtcNow
            };
        }
    }
}
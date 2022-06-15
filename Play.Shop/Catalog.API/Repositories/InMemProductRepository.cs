using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.API.Entities;

namespace Catalog.API.Repositories
{

    public class InMemProductRepository : IProductRepository
    {
        private readonly List<Product> products = new()
        {
            new Product { Id = Guid.NewGuid(), Name = "Potion", Price= 9, CreatedDate = DateTimeOffset.UtcNow },
            new Product { Id = Guid.NewGuid(), Name = "Iron Sword", Price= 20, CreatedDate = DateTimeOffset.UtcNow },
            new Product { Id = Guid.NewGuid(), Name = "Bronze Shield ", Price= 18, CreatedDate = DateTimeOffset.UtcNow },
        };

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await Task.FromResult(products);
        }

        public async Task<Product> GetProductAsync(Guid id)
        {
            Console.WriteLine(id);
            var product = products.Where(p => p.Id == id).SingleOrDefault();
            return await Task.FromResult(product);
        }

        public async Task CreateProductAsync(Product product)
        {
            products.Add(product);
            await Task.CompletedTask;
        }

        public async Task UpdateProductAsync(Product product)
        {
            var index = products.FindIndex(p => p.Id == product.Id);

            products[index] = product;
            await Task.CompletedTask;

        }

        public async Task DeleteProductAsync(Guid id)
        {
            var index = products.FindIndex(p => p.Id == id);

            products.RemoveAt(index);
            await Task.CompletedTask;
        }
    }
}
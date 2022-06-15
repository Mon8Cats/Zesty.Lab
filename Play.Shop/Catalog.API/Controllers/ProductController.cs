using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.API.Dtos;
using Catalog.API.Entities;
using Catalog.API.Helpers;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _repository;

        public ProductController(IProductRepository repository)
        {
            //repository = new InMemProductRepository();
            _repository = repository;
        }

        //[HttpGet("{name?}")]
        [HttpGet]
        public async Task<IEnumerable<ProductDto>> GetProductsAsync(string name = null)
        {
            var products = (await _repository.GetProductsAsync())
                            .Select(p => p.AsDto());

            if (string.IsNullOrWhiteSpace(name) || name.ToLower() == "empty")
            {
                //products = products.Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
                return products;
            }

             products = products.Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase));

            return products;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProductAsync(Guid id)
        {
            var product = await _repository.GetProductAsync(id);

            if (product == null)
            {
                return NotFound();
            }
            //return Ok(product.AsDto());
            return product.AsDto();
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateProductAsync(CreateProductDto productDto)
        {
            Product product = new()
            {
                Id = Guid.NewGuid(),
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };

            await _repository.CreateProductAsync(product);

            return CreatedAtAction(nameof(GetProductAsync), new { id = product.Id }, product.AsDto());
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProductAsync(Guid id, UpdateProductDto productDto)
        {
            var existingProduct = await _repository.GetProductAsync(id);

            if (existingProduct is null)
            {
                return NotFound();
            }

            /*
            Product updatedProduct = existingProduct with
            {
                Name = productDto.Name,
                Price = productDto.Price
            };
            */
            existingProduct.Name = productDto.Name;
            existingProduct.Description = productDto.Description;
            existingProduct.Price = productDto.Price;

            /*
             Product updatedProduct = new Product
            {
                Id = existingProduct.Id,
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                CreatedDate = existingProduct.CreatedDate
            };
            */

            await _repository.UpdateProductAsync(existingProduct);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProductAsync(Guid id)
        {
            var existingProduct = await _repository.GetProductAsync(id);

            if (existingProduct is null)
            {
                return NotFound();
            }

            await _repository.DeleteProductAsync(id);

            return NoContent();
        }

       
    }
}
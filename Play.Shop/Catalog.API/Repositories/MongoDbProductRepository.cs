using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Repositories
{
    public class MongoDbProductRepository : IProductRepository
    {
        private const string databaseName = "catalog";
        private const string collectionName = "products";
        private readonly IMongoCollection<Product> _productCollection;
        private readonly FilterDefinitionBuilder<Product> _filterBuilder = Builders<Product>.Filter;
        public MongoDbProductRepository(IMongoClient mongoClient)
        {
            IMongoDatabase database = mongoClient.GetDatabase(databaseName);
            _productCollection = database.GetCollection<Product>(collectionName);
        }

        public async Task CreateProductAsync(Product product)
        {
            await _productCollection.InsertOneAsync(product);
        }

        public async Task DeleteProductAsync(Guid id)
        {
            var filter = _filterBuilder.Eq(p => p.Id, id);
            await _productCollection.DeleteOneAsync(filter);
        }

        public async Task<Product> GetProductAsync(Guid id)
        {
            var filter = _filterBuilder.Eq(p => p.Id, id);
            return await _productCollection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _productCollection.Find(_filterBuilder.Empty).ToListAsync();
        }

        public async Task UpdateProductAsync(Product product)
        {
            var filter = _filterBuilder.Eq(p => p.Id, product.Id);
            await _productCollection.ReplaceOneAsync(filter, product);
        }
    }
}
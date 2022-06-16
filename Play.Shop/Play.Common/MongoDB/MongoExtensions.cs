using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Play.Common.Contracts;
using Play.Common.Settings;

namespace Play.Common.MongoDB
{
    public static class MongoExtensions
    {
        public static IServiceCollection AddMongo(this IServiceCollection services)
        {

            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String)); 
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String)); 

            services.AddSingleton<IMongoDatabase>(sp => {

                var configuration = sp.GetRequiredService<IConfiguration>();
                var serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();

                var mongoDbSettings = configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
                var mongoClient = new MongoClient(mongoDbSettings.ConnectionString);
                return mongoClient.GetDatabase(serviceSettings.ServiceName);
            });

            return services;
        }

        public static IServiceCollection AddMongoRepository<T>(this IServiceCollection services, string colleciontName) 
            where T : IEntity
        {

            services.AddSingleton<IRepository<T>>(sp => 
            {
                var database = sp.GetRequiredService<IMongoDatabase>();
                return  new MongoRepository<T>(database, colleciontName);
            });

            return services;
        }
    }
}
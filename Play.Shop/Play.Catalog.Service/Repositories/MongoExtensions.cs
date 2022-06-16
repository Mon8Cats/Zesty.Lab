using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Play.Catalog.Service.Entities;
using Play.Catalog.Service.Settings;

namespace Play.Catalog.Service.Repositories
{
    public static class MongoExtensions
    {
        public static IServiceCollection AddMongoX(this IServiceCollection services)
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

        public static IServiceCollection AddMongoRepositoryX<T>(this IServiceCollection services, string colleciontName) 
            where T : IEntityX
        {

            services.AddSingleton<IRepositoryX<T>>(sp => 
            {
                var database = sp.GetRequiredService<IMongoDatabase>();
                return  new MongoRepositoryX<T>(database, colleciontName);
            });

            return services;
        }
    }
}
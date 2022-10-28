using CatalogService.Settings;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Driver;
using CatalogService.Entities;
using CatalogService.Interfaces;
using CatalogService.Repositories;

namespace CatalogService.Extensions.Settings
{
    public static class SettingExtension
    {
        public static IServiceCollection AddMongo(this IServiceCollection services)
        {
            //Serializer information in a better way
            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

            services.AddScoped(serviceProvider =>
            {
                var configuration = serviceProvider.GetService<IConfiguration>();
                var serviceSettings = configuration.GetSection(nameof(ServicesSettings)).Get<ServicesSettings>();
                var mongoDbSettings = configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
                var mongoClient = new MongoClient(mongoDbSettings.ConnectionString);
                return mongoClient.GetDatabase(serviceSettings.ServiceName);
            });

            return services;
        }

        public static IServiceCollection AddRepositories<T>(this IServiceCollection services, string collectionName) where T : IEntity
        {
            services.AddTransient<IRepository<T>>(serviceProvider => {
                var database = serviceProvider.GetService<IMongoDatabase>();
                return new MongoRepository<T>(database, collectionName);
            });
            return services;
        }

        //public static WebApplicationBuilder AddMongo(this WebApplicationBuilder builder) {
        //    ServicesSettings servicesSettings = builder.Configuration.GetSection(nameof(ServicesSettings)).Get<ServicesSettings>();

        //    //Serializer information in a better way
        //    BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
        //    BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

        //    builder.Services.AddScoped(serviceProvider =>
        //    {
        //        var mongoDbSettings = builder.Configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
        //        var mongoClient = new MongoClient(mongoDbSettings.ConnectionString);
        //        return mongoClient.GetDatabase(servicesSettings.ServiceName);
        //    });

        //    return builder;
        //}

        //public static WebApplicationBuilder AddRepositories<T>(this WebApplicationBuilder builder,string collectionName) where T : IEntity {
        //    builder.Services.AddTransient<IRepository<T>>(serviceProvider => {
        //        var database = serviceProvider.GetService<IMongoDatabase>();
        //        return new MongoRepository<T>(database, collectionName);
        //    });
        //    return builder;
        //}
    }
}

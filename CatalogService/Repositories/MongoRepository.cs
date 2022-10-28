using CatalogService.Entities;
using CatalogService.Interfaces;
using MongoDB.Driver;

namespace CatalogService.Repositories
{
    public class MongoRepository<T> : IRepository<T> where T : IEntity
    {
        private readonly IMongoCollection<T> dbCollection;
        private readonly FilterDefinitionBuilder<T> filterBuiler = Builders<T>.Filter;

        public MongoRepository(IMongoDatabase mongoDatabase, string collectionName) {
            //var mongoClient = new MongoClient("mongodb://localhost:27017");
            //var database = mongoClient.GetDatabase("Catalog");
            //dbCollection = database.GetCollection<Item>(collectionName);
            dbCollection = mongoDatabase.GetCollection<T>(collectionName);
        }

        public async Task<IReadOnlyCollection<T>> GetAllAsync() {
            return await dbCollection.Find(filterBuiler.Empty).ToListAsync();
        }

        public async Task<T> GetAsync(Guid id)
        {
            FilterDefinition<T> filter = filterBuiler.Eq(x => x.Id,id);
            return await dbCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(T item) {
            if (item == null) {
                throw new ArgumentException(nameof(item));
            }

            await dbCollection.InsertOneAsync(item);
        }

        public async Task UpdateAsync(T item)
        {
            if (item == null)
            {
                throw new ArgumentException(nameof(item));
            }

            FilterDefinition<T> filter = filterBuiler.Eq(x => x.Id, item.Id);

            await dbCollection.ReplaceOneAsync(filter,item);
        }

        public async Task DeleteAsync(Guid id) {
            FilterDefinition<T> filter = filterBuiler.Eq(x => x.Id, id);
            await dbCollection.DeleteOneAsync(filter);
        }
    }
}

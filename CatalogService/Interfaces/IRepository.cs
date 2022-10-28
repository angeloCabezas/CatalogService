using CatalogService.Entities;

namespace CatalogService.Interfaces
{
    public interface IRepository<T> where T : IEntity
    {
        public Task<IReadOnlyCollection<T>> GetAllAsync();
        public Task<T> GetAsync(Guid id);
        public Task CreateAsync(T item);
        public Task UpdateAsync(T item);
        public Task DeleteAsync(Guid id);
    }
}

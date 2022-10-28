using PlayCommon;
using System.Linq.Expressions;

namespace CatalogService.Interfaces
{
    public interface IRepository<T> where T : IEntity
    {
        public Task<IReadOnlyCollection<T>> GetAllAsync();
        public Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T,bool>> filter);
        public Task<T> GetAsync(Guid id);
        public Task<T> GetAsync(Expression<Func<T, bool>> filter);
        public Task CreateAsync(T item);
        public Task UpdateAsync(T item);
        public Task DeleteAsync(Guid id);
    }
}

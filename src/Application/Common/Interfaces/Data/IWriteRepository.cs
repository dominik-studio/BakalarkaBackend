using PromobayBackend.Domain.Common;

namespace PromobayBackend.Application.Common.Interfaces.Data
{
    public interface IWriteRepository<T> where T : class, IAggregateRoot
    {
        Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken);
        IQueryable<T> GetQueryable();
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task SaveAsync(CancellationToken cancellationToken);
    }
} 

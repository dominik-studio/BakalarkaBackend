using CRMBackend.Domain.Common;

namespace CRMBackend.Application.Common.Interfaces.Repositories
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

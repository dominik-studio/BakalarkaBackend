using Microsoft.EntityFrameworkCore;
using PromobayBackend.Application.Common.Interfaces.Data;
using PromobayBackend.Domain.Common;

namespace PromobayBackend.Infrastructure.Data.Repositories
{
    public class WriteRepository<T> : IWriteRepository<T> where T : class, IAggregateRoot
    {
        private readonly ApplicationDbContext _context;
        public WriteRepository(ApplicationDbContext context) => _context = context;

        public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Set<T>().FindAsync(new object[] { id }, cancellationToken);
        }

        public IQueryable<T> GetQueryable()
        {
            return _context.Set<T>();
        }

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }
        
        public async Task SaveAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
} 

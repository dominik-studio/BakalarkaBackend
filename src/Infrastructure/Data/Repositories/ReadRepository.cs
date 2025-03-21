using Microsoft.EntityFrameworkCore;
using PromobayBackend.Application.Common.Interfaces.Data;

namespace PromobayBackend.Infrastructure.Data.Repositories
{
    public class ReadRepository<T> : IReadRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;

        public ReadRepository(ApplicationDbContext context) => _context = context;

        public IQueryable<T> GetQueryableNoTracking()
        {
            return _context.Set<T>().AsNoTracking();
        }
    }
} 

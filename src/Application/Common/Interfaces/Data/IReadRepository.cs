
namespace PromobayBackend.Application.Common.Interfaces.Data
{
    public interface IReadRepository<T>
    {
        IQueryable<T> GetQueryableNoTracking();
    }
}

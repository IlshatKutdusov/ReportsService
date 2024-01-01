using Domain.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IDatabaseService
    {
        IQueryable<T> Get<T>() where T : class, IBaseEntity;
        IQueryable<T> GetAll<T>() where T : class, IBaseEntity;

        Task<int> Add<T>(T newEntity) where T : class, IBaseEntity;
        Task AddRange<T>(IEnumerable<T> newEntities) where T : class, IBaseEntity;

        Task Remove<T>(T entity) where T : class, IBaseEntity;
        Task RemoveRange<T>(IEnumerable<T> entities) where T : class, IBaseEntity;

        Task Update<T>(T entity) where T : class, IBaseEntity;

        Task SaveChangesAsync();
    }
}

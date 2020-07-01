using Chris.Personnel.Management.Common;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Chris.Personnel.Management.Repository
{
    public interface IBaseRepository<TEntity> where TEntity : EntityBase
    {
        void Add(TEntity entity);
        void Edit(TEntity entity);
        void Delete(TEntity entity);
        IQueryable<TEntity> GetAll();
        ValueTask<TEntity> Get(Guid id);
    }
}
using System;
using System.Linq;
using System.Threading.Tasks;
using Chris.Personnel.Management.Common.EntityModel;

namespace Chris.Personnel.Management.Common
{
    public interface IBaseRepository<TEntity> where TEntity : RootEntity
    {
        void Add(TEntity entity);
        void Edit(TEntity entity);
        void Delete(TEntity entity);
        IQueryable<TEntity> GetAll();
        ValueTask<TEntity> Get(Guid id);
    }
}
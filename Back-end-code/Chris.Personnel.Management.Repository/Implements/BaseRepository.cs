using System;
using System.Linq;
using System.Threading.Tasks;
using Chris.Personnel.Management.Common;
using Microsoft.EntityFrameworkCore;

namespace Chris.Personnel.Management.Repository.Implements
{
    public abstract class BaseRepository<TEntity> where TEntity : RootEntity
    {
        private readonly IDbContextProvider _dbContextProvider;
        protected DbContext DbContext => _dbContextProvider.GetDbContext();

        protected BaseRepository(IDbContextProvider dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
        }

        public void Add(TEntity entity)
        {
            DbContext.Set<TEntity>().Add(entity);
        }

        public void Edit(TEntity entity)
        {
            var entry = DbContext.Entry(entity);
            DbContext.Set<TEntity>().Attach(entity);
            entry.State = EntityState.Modified;
        }

        public void Delete(TEntity entity)
        {
            DbContext.Set<TEntity>().Remove(entity);
        }

        public IQueryable<TEntity> GetAll()
        {
            return DbContext.Set<TEntity>().AsQueryable();
        }

        public ValueTask<TEntity> Get(Guid id)
        {
            return DbContext.Set<TEntity>().FindAsync(id);
        }
    }
}
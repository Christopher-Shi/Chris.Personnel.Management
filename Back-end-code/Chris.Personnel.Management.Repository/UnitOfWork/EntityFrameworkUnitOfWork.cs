using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Chris.Personnel.Management.Repository.UnitOfWork
{
    public class EntityFrameworkUnitOfWork : IUnitOfWork
    {
        private DbContext _dbContext;

        public EntityFrameworkUnitOfWork(IDbContextProvider dbProvider)
        {
            _dbContext = dbProvider.GetDbContext();
        }

        public async Task Commit()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            if (_dbContext != null)
            {
                _dbContext.Dispose();
                _dbContext = null;
            }

            GC.SuppressFinalize(this);
        }
    }
}

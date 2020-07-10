using Autofac.Util;
using Chris.Personnel.Management.EF.Storage;
using Microsoft.EntityFrameworkCore;

namespace Chris.Personnel.Management.Repository.Implements
{
    public class SqlServerDbContextProvider : Disposable, IDbContextProvider
    {
        private readonly SqlServerContext _context;

        public SqlServerDbContextProvider(SqlServerContext context)
        {
            _context = context;
        }

        public DbContext GetDbContext()
        {
            return _context;
        }
    }
}
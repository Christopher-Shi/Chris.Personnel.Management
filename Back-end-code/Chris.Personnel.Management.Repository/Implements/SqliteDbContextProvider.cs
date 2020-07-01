using Autofac.Util;
using Chris.Personnel.Management.EF.Storage;
using Microsoft.EntityFrameworkCore;

namespace Chris.Personnel.Management.Repository.Implements
{
    public class SqliteDbContextProvider : Disposable, IDbContextProvider
    {
        private readonly SqliteContext _context;

        public SqliteDbContextProvider(SqliteContext context)
        {
            _context = context;
        }

        public DbContext GetDbContext()
        {
            return _context;
        }
    }
}
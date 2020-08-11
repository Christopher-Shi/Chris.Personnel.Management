using Microsoft.EntityFrameworkCore;

namespace Chris.Personnel.Management.EF.Storage
{
    public class SqliteContext : BaseDbContext
    {
        private readonly IConnectionStringManager _connectionStringManager;

        public SqliteContext(IConnectionStringManager connectionStringManager)
        {
            _connectionStringManager = connectionStringManager;
        }

        protected override void BuildDbContextOptionsBuilder(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite(_connectionStringManager.ConnectionString);
            }
        }
    }
}
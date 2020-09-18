using Microsoft.EntityFrameworkCore;

namespace Chris.Personnel.Management.EF.Storage
{
    public class MySqlContext : BaseDbContext
    {
        private readonly IConnectionStringManager _connectionStringManager;

        public MySqlContext(IConnectionStringManager connectionStringManager)
        {
            _connectionStringManager = connectionStringManager;
        }

        protected override void BuildDbContextOptionsBuilder(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseLoggerFactory(MyLoggerFactory).UseMySql(_connectionStringManager.ConnectionString);
            }
        }
    }
}

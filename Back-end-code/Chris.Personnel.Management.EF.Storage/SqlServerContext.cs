using Microsoft.EntityFrameworkCore;

namespace Chris.Personnel.Management.EF.Storage
{
    public class SqlServerContext : BaseDbContext
    {
        private readonly IConnectionStringManager _connectionStringManager;

        public SqlServerContext(IConnectionStringManager connectionStringManager)
        {
            _connectionStringManager = connectionStringManager;
        }

        protected override void BuildDbContextOptionsBuilder(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseLoggerFactory(MyLoggerFactory)
                    .UseLazyLoadingProxies()
                    .UseSqlServer(
                        _connectionStringManager.ConnectionString,
                        opt => opt.UseRowNumberForPaging());
            }
        }
    }
}

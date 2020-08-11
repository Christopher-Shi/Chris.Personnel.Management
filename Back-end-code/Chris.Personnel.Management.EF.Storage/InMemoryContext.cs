using Microsoft.EntityFrameworkCore;

namespace Chris.Personnel.Management.EF.Storage
{
    public class InMemoryContext : BaseDbContext
    {
        private readonly IConnectionStringManager _connectionStringManager;

        public InMemoryContext(IConnectionStringManager connectionStringManager)
        {
            _connectionStringManager = connectionStringManager;
        }

        protected override void BuildDbContextOptionsBuilder(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(_connectionStringManager.ConnectionString);
        }
    }
}
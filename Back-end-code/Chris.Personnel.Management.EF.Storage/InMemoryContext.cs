using Microsoft.EntityFrameworkCore;

namespace Chris.Personnel.Management.EF.Storage
{
    public class InMemoryContext : BaseDbContext
    {
        protected override void BuildDbContextOptionsBuilder(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("test_database");
        }
    }
}
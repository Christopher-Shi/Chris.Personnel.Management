using Microsoft.EntityFrameworkCore;

namespace Chris.Personnel.Management.EF.Storage
{
    public class SqliteContext : BaseDbContext
    {
        protected override void BuildDbContextOptionsBuilder(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=chrisPersonnelManagement.db");
        }
    }
}
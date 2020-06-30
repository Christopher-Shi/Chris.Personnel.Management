using System.Linq;
using Chris.Personnel.Management.EF.Storage.BasicData;
using Chris.Personnel.Management.EF.Storage.Mappings;
using Chris.Personnel.Management.Entity;
using Microsoft.EntityFrameworkCore;

namespace Chris.Personnel.Management.EF.Storage
{
    public class ChrisPersonnelManagementDbContext : DbContext
    {
        public ChrisPersonnelManagementDbContext(DbContextOptions<ChrisPersonnelManagementDbContext> options)
            : base(options)
        {

        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //应用map配置
            modelBuilder.ApplyConfiguration(new UserMap());

            //seed data
            modelBuilder.Entity<User>().HasData(UserCreator.Create());

            //foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            //{
            //    relationship.DeleteBehavior = DeleteBehavior.Restrict;
            //}

            base.OnModelCreating(modelBuilder);
        }
    }
}

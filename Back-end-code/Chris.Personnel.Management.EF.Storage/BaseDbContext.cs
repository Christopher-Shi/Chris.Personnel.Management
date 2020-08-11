using Chris.Personnel.Management.EF.Storage.BasicData;
using Chris.Personnel.Management.EF.Storage.Mappings;
using Chris.Personnel.Management.Entity;
using Microsoft.EntityFrameworkCore;

namespace Chris.Personnel.Management.EF.Storage
{
    public abstract class BaseDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            BuildDbContextOptionsBuilder(optionsBuilder);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //应用map配置
            modelBuilder.ApplyConfiguration(new UserMap());
            modelBuilder.ApplyConfiguration(new RoleMap());

            //seed data
            modelBuilder.Entity<User>().HasData(UserCreator.Create());
            modelBuilder.Entity<Role>().HasData(RoleCreator.Create());

            //foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            //{
            //    relationship.DeleteBehavior = DeleteBehavior.Restrict;
            //}

            base.OnModelCreating(modelBuilder);
        }

        protected abstract void BuildDbContextOptionsBuilder(DbContextOptionsBuilder optionsBuilder);
    }
}

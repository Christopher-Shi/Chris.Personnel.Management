using Chris.Personnel.Management.EF.Storage.BasicData;
using Chris.Personnel.Management.EF.Storage.Mappings;
using Chris.Personnel.Management.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Logging.Debug;

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

        public static readonly ILoggerFactory MyLoggerFactory
            = LoggerFactory.Create(builder =>
            {
                builder.AddConsole(options =>
                {
                });
            });

        protected abstract void BuildDbContextOptionsBuilder(DbContextOptionsBuilder optionsBuilder);
    }
}

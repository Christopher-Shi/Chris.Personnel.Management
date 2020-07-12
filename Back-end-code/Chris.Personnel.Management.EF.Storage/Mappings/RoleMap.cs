using Chris.Personnel.Management.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chris.Personnel.Management.EF.Storage.Mappings
{
    public class RoleMap : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            // Primary Key
            builder.HasKey(t => t.Id);

            // Properties
            builder.Property(t => t.Name).IsRequired().HasMaxLength(50);
            builder.Property(t => t.Memo).HasMaxLength(200);

            // Table & Column Mappings
            builder.ToTable("Role");
            builder.Property(t => t.Id).HasColumnName("Id");
            builder.Property(t => t.Name).HasColumnName("Name");
            builder.Property(t => t.Memo).HasColumnName("Memo");
            builder.Property(t => t.IsDeleted).HasColumnName("IsDeleted");
            builder.Property(t => t.CreatedUserId).HasColumnName("CreatedUserId");
            builder.Property(t => t.CreatedTime).HasColumnName("CreatedTime");
            builder.Property(t => t.LastModifiedUserId).HasColumnName("LastModifiedUserId");
            builder.Property(t => t.LastModifiedTime).HasColumnName("LastModifiedTime");

            // Relationships
        }
    }
}
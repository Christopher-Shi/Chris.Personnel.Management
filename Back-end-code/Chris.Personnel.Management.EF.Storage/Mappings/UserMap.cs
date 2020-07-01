using Chris.Personnel.Management.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chris.Personnel.Management.EF.Storage.Mappings
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Primary Key
            builder.HasKey(t => t.Id);

            //Index
            builder.HasIndex(t => t.Name).IsUnique();

            // Properties
            builder.Property(t => t.Name).IsRequired().HasMaxLength(100);
            builder.Property(t => t.Password).IsRequired().HasMaxLength(100);
            builder.Property(t => t.TrueName).IsRequired().HasMaxLength(100);
            builder.Property(t => t.Gender).IsRequired();
            builder.Property(t => t.CardId).IsRequired().HasMaxLength(100);
            builder.Property(t => t.Phone).IsRequired().HasMaxLength(100);
            builder.Property(t => t.IsEnabled).IsRequired();

            // Table & Column Mappings
            builder.ToTable("User");
            builder.Property(t => t.Id).HasColumnName("Id");
            builder.Property(t => t.Name).HasColumnName("Name");
            builder.Property(t => t.Password).HasColumnName("Password");
            builder.Property(t => t.TrueName).HasColumnName("TrueName");
            builder.Property(t => t.Gender).HasColumnName("Gender");
            builder.Property(t => t.CardId).HasColumnName("CardId");
            builder.Property(t => t.Phone).HasColumnName("Phone");
            builder.Property(t => t.IsEnabled).HasColumnName("IsEnabled");

            // Relationships
        }
    }
}

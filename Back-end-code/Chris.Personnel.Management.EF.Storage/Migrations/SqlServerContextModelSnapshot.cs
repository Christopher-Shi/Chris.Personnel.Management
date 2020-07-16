﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Chris.Personnel.Management.EF.Storage.Migrations
{
    [DbContext(typeof(SqlServerContext))]
    partial class SqlServerContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Chris.Personnel.Management.Entity.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnName("CreatedTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CreatedUserId")
                        .HasColumnName("CreatedUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnName("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModifiedTime")
                        .HasColumnName("LastModifiedTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("LastModifiedUserId")
                        .HasColumnName("LastModifiedUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Memo")
                        .HasColumnName("Memo")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("Name")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("Role");

                    b.HasData(
                        new
                        {
                            Id = new Guid("32ec1e12-fe6d-4606-902e-6705beb0afc1"),
                            CreatedTime = new DateTime(2020, 7, 9, 23, 15, 14, 0, DateTimeKind.Unspecified),
                            CreatedUserId = new Guid("32ec1e37-fe6d-4606-902e-6705beb0afc0"),
                            IsDeleted = false,
                            Name = "管理员"
                        });
                });

            modelBuilder.Entity("Chris.Personnel.Management.Entity.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CardId")
                        .IsRequired()
                        .HasColumnName("CardId")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnName("CreatedTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("CreatedUserId")
                        .HasColumnName("CreatedUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Gender")
                        .HasColumnName("Gender")
                        .HasColumnType("int");

                    b.Property<int>("IsEnabled")
                        .HasColumnName("IsEnabled")
                        .HasColumnType("int");

                    b.Property<DateTime?>("LastModifiedTime")
                        .HasColumnName("LastModifiedTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("LastModifiedUserId")
                        .HasColumnName("LastModifiedUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("Name")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnName("Password")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnName("Phone")
                        .HasColumnType("nvarchar(11)")
                        .HasMaxLength(11);

                    b.Property<Guid?>("RoleId")
                        .HasColumnName("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasColumnType("nvarchar(36)")
                        .HasMaxLength(36);

                    b.Property<string>("TrueName")
                        .IsRequired()
                        .HasColumnName("TrueName")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("RoleId");

                    b.ToTable("User");

                    b.HasData(
                        new
                        {
                            Id = new Guid("32ec1e37-fe6d-4606-902e-6705beb0afc0"),
                            CardId = "140226199401294051",
                            CreatedTime = new DateTime(2020, 7, 9, 23, 15, 14, 0, DateTimeKind.Unspecified),
                            Gender = 1,
                            IsEnabled = 1,
                            Name = "Admin",
                            Password = "C62A9E820F5AB0ADA440C395F5B3D707945EA47C200FC1A5DDA6B1F542647FB4",
                            Phone = "13259769759",
                            RoleId = new Guid("32ec1e12-fe6d-4606-902e-6705beb0afc1"),
                            Salt = "a7386c09-b3e5-4767-840e-0316ab580a92",
                            TrueName = "Admin"
                        });
                });

            modelBuilder.Entity("Chris.Personnel.Management.Entity.User", b =>
                {
                    b.HasOne("Chris.Personnel.Management.Entity.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId");
                });
#pragma warning restore 612, 618
        }
    }
}

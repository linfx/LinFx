﻿// <auto-generated />
using LinFx.Extensions.EntityFrameworkCore;
using LinFx.Extensions.PermissionManagement.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace TenantManagementService.Migrations.PermissionManagementDb
{
    [DbContext(typeof(PermissionManagementDbContext))]
    partial class PermissionManagementDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("_DatabaseProvider", EfDatabaseProvider.Sqlite)
                .HasAnnotation("ProductVersion", "5.0.10");

            modelBuilder.Entity("LinFx.Extensions.PermissionManagement.PermissionGrant", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(64)
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderName")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("TEXT");

                    b.Property<string>("TenantId")
                        .HasMaxLength(36)
                        .HasColumnType("TEXT")
                        .HasColumnName("TenantId");

                    b.HasKey("Id");

                    b.ToTable("Core_PermissionGrant");
                });
#pragma warning restore 612, 618
        }
    }
}

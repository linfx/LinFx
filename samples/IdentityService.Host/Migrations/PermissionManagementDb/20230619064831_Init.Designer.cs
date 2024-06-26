﻿// <auto-generated />
using LinFx.Extensions.PermissionManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace IdentityService.Migrations.PermissionManagementDb
{
    [DbContext(typeof(PermissionManagementDbContext))]
    [Migration("20230619064831_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.5");

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

                    b.HasIndex("TenantId", "Name", "ProviderName", "ProviderKey")
                        .IsUnique();

                    b.ToTable("PermissionGrants", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}

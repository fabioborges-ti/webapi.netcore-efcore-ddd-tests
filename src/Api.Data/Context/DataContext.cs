using System;
using System.Diagnostics.CodeAnalysis;
using Api.Data.Mappings;
using Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Data.Context
{
    [ExcludeFromCodeCoverage]
    public class DataContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<AddressEntity> Addresses { get; set; }
        public DbSet<PhoneEntity> Phones { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserEntity>(new UserMap().Configure);
            modelBuilder.Entity<AddressEntity>(new AddressMap().Configure);
            modelBuilder.Entity<PhoneEntity>(new PhoneMap().Configure);

            var user = new UserEntity("admin", "admin@apinetcore.com", true)
            {
                Document = "12345678901",
                UpdatedAt = DateTime.UtcNow
            };

            modelBuilder
                .Entity<UserEntity>()
                .HasData(user);
        }
    }
}

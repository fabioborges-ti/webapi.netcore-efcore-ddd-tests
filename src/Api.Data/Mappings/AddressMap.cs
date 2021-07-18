using Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Api.Data.Mappings
{
    [ExcludeFromCodeCoverage]
    public class AddressMap : IEntityTypeConfiguration<AddressEntity>
    {
        public void Configure(EntityTypeBuilder<AddressEntity> builder)
        {
            builder
                .ToTable("address");

            builder
                .HasKey(a => a.Id);

            builder
                .HasOne(u => u.User)
                .WithMany(a => a.Addresses)
                .HasForeignKey(p => p.UserId);

            builder
                .Property(p => p.Address)
                .HasColumnType("varchar(100)")
                .IsRequired();

            builder
                .Property(p => p.City)
                .HasColumnType("varchar(50)")
                .IsRequired();

            builder
                .Property(p => p.State)
                .HasColumnType("varchar(2)")
                .IsRequired();

            builder
                .Property(p => p.Zip)
                .HasColumnType("varchar(10)");
        }
    }
}

using Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Api.Data.Mappings
{
    [ExcludeFromCodeCoverage]
    public class PhoneMap : IEntityTypeConfiguration<PhoneEntity>
    {
        public void Configure(EntityTypeBuilder<PhoneEntity> builder)
        {
            builder
                .ToTable("phone");

            builder
                .HasKey(p => p.Id);

            builder
                .HasOne(u => u.User)
                .WithMany(p => p.Phones)
                .HasForeignKey(p => p.UserId);

            builder
                .Property(p => p.Number)
                .HasColumnType("varchar(20)")
                .IsRequired();
        }
    }
}

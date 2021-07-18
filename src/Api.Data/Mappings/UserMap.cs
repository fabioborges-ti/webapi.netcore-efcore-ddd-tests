using Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Api.Data.Mappings
{
    [ExcludeFromCodeCoverage]
    public class UserMap : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder
                .ToTable("user");

            builder
                .HasKey(u => u.Id);

            builder
                .HasIndex(u => u.Email)
                .IsUnique();

            builder
                .Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder
                .Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder
                .Property(u => u.Document)
                .IsRequired()
                .HasMaxLength(11);
        }
    }
}

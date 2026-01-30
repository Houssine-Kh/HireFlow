using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HireFlow.Domain.Users.Entities;
using HireFlow.Domain.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HireFlow.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            // Primary Key
            builder.HasKey(u => u.Id);

            // Value Object: PersonName
            builder.OwnsOne(u => u.Name, name =>
            {
                name.Property(n => n.FirstName)
                    .HasColumnName("FirstName")
                    .HasMaxLength(100)
                    .IsRequired();

                name.Property(n => n.LastName)
                    .HasColumnName("LastName")
                    .HasMaxLength(100)
                    .IsRequired();
            });

            // Value Object : Email
            builder.Property(x => x.Email)
                   .HasConversion(
                    email => email.Value,
                    value => new Email(value)
                   )
                   .HasMaxLength(100)
                   .IsRequired();

            // Enum: UserRole
            builder.Property(u => u.Role)
                   .HasConversion<string>()
                   .HasMaxLength(50)
                   .IsRequired();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HireFlow.Domain.Candidates.Entities;
using HireFlow.Domain.Candidates.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HireFlow.Infrastructure.Persistence.Configurations
{
    public class CandidateConfiguration : IEntityTypeConfiguration<Candidate>
    {
        public void Configure(EntityTypeBuilder<Candidate> builder)
        {
            builder.ToTable("Candidates");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.UserId)
                .IsRequired();

            builder.HasIndex(c => c.UserId)
                .IsUnique();
            
            builder.Property(c => c.Resume)
                .HasConversion(
                    resume => resume.Url,
                    value => new Resume(value)
                )
                .HasColumnName("ResumeUrl")
                .IsRequired();
            
            builder.Property(c => c.Phone)
                .HasConversion(
                    phone => phone.Value,
                    value => new PhoneNumber(value)
                )
                .HasColumnName("PhoneNumber")
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(c => c.LinkedIn)
                .HasConversion(
                    linkedIn => linkedIn!.Value,
                    value => new LinkedInUrl(value)
                )
                .HasColumnName("LinkedInUrl")
                .IsRequired(false);
        }
    }
}
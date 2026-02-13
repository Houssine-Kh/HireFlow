using HireFlow.Domain.Jobs.Entities;
using HireFlow.Domain.Jobs.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HireFlow.Infrastructure.Persistence.Configurations
{
    public class JobConfiguration : IEntityTypeConfiguration<Job>
    {
        public void Configure(EntityTypeBuilder<Job> builder)
        {
            builder.HasKey(j => j.Id);

            builder.Property(j => j.Title)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(j => j.Description)
                .HasMaxLength(4000) 
                .IsRequired(false);

            builder.Property(j => j.RecruiterId)
                .IsRequired();

            // Index RecruiterId for fast dashboard queries
            builder.HasIndex(j => j.RecruiterId);

            //  Save Enums as Strings 
            builder.Property(j => j.Status)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(j => j.WorkMode)
                .HasConversion<string>()
                .IsRequired(false);
        }
    }
}
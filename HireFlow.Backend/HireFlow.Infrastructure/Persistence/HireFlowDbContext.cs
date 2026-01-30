using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HireFlow.Domain.Candidates.Entities;
using HireFlow.Domain.Users.Entities;
using HireFlow.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HireFlow.Infrastructure.Persistence
{
    public class HireFlowDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
    {
        public HireFlowDbContext(DbContextOptions<HireFlowDbContext> options) : base(options){}
        public DbSet<User> DomainUsers{get; set;}
        public DbSet<Candidate> Candidates{get; set;}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Automatically apply all IEntityTypeConfiguration<T>
            builder.ApplyConfigurationsFromAssembly(typeof(HireFlowDbContext).Assembly);
        }
    }
}
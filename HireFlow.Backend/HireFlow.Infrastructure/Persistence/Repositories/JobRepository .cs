using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HireFlow.Domain.Jobs.Entities;
using HireFlow.Domain.Jobs.Repositories;
using HireFlow.Domain.Users.Entities;
using Microsoft.EntityFrameworkCore;

namespace HireFlow.Infrastructure.Persistence.Repositories
{
    public class JobRepository : IJobRepository
    {
         private readonly HireFlowDbContext _context;

        public JobRepository( HireFlowDbContext context )
        {
            _context = context;
        }

        public async Task<Job?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _context.Jobs.FirstOrDefaultAsync(j => j.Id == id);
        }

        public async Task<List<Job>> GetAllAsync(CancellationToken ct = default)
        {
            return await _context.Jobs.ToListAsync();
        }

        public async Task<List<Job>> GetByRecruiterIdAsync(Guid recruiterId, CancellationToken ct = default)
        {
            return await _context.Jobs
                .Where(j => j.RecruiterId == recruiterId)
                .ToListAsync(ct);
        }

        public async Task AddAsync(Job job, CancellationToken ct = default)
        {
            await _context.Jobs.AddAsync(job);
        }
    }
}
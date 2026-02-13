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

        public async Task<Job?> GetByIdAsync(Guid id)
        {
            return await _context.Jobs.FirstOrDefaultAsync(j => j.Id == id);
        }

        public async Task<List<Job>> GetAllAsync()
        {
            return await _context.Jobs.ToListAsync();
        }

        public async Task AddAsync(Job job)
        {
            await _context.Jobs.AddAsync(job);
        }
    }
}
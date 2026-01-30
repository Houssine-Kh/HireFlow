using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HireFlow.Domain.Candidates.Entities;
using HireFlow.Domain.Candidates.Repositories;

namespace HireFlow.Infrastructure.Persistence.Repositories
{
    public class CandidateRepository : ICandidateRepository
    {
        private readonly HireFlowDbContext _context;

        public CandidateRepository(HireFlowDbContext context)
        {
            _context = context;
        }

        public async Task<Candidate?> GetByUserIdAsync(Guid userId)
        {
            return await _context.Candidates.FindAsync(userId);
        }

        public async Task AddAsync(Candidate candidate)
        {
            await _context.Candidates.AddAsync(candidate);
        } 
    }
}
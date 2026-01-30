using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HireFlow.Domain.Candidates.Entities;

namespace HireFlow.Domain.Candidates.Repositories
{
    public interface ICandidateRepository
    {
        Task<Candidate?> GetByUserIdAsync(Guid userId);

        Task AddAsync(Candidate candidate);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HireFlow.Domain.Jobs.Entities;

namespace HireFlow.Domain.Jobs.Repositories
{
    public interface IJobRepository
    {
        Task<Job?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<List<Job>> GetAllAsync(CancellationToken ct = default);
        Task<List<Job>> GetByRecruiterIdAsync(Guid recruiterId, CancellationToken ct = default);
        Task AddAsync(Job job, CancellationToken ct = default);
    }
}
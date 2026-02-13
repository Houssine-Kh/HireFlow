using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HireFlow.Domain.Jobs.Entities;

namespace HireFlow.Domain.Jobs.Repositories
{
    public interface IJobRepository
    {
        Task<Job?> GetByIdAsync(Guid id);
        Task<List<Job>> GetAllAsync();
        Task AddAsync(Job job);
    }
}
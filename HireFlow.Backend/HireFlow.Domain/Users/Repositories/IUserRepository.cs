using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HireFlow.Domain.Users.Entities;

namespace HireFlow.Application.Common.Interfaces.Persistence
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task<User?> GetByIdAsync(Guid id);
        Task<List<User>> GetAllAsync();
        void Delete(User user);
    }
}
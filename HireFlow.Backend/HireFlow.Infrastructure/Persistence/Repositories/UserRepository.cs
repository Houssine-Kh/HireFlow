using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HireFlow.Application.Common.Interfaces.Persistence;
using HireFlow.Domain.Users.Entities;
using Microsoft.EntityFrameworkCore;

namespace HireFlow.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly HireFlowDbContext _context;
        public UserRepository(HireFlowDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(User user)
        {
            await _context.DomainUsers.AddAsync(user);
        }
        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _context.DomainUsers.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.DomainUsers.ToListAsync();
        }
        public void Delete(User user)
        {
            _context.Remove(user);
        }
    }
}
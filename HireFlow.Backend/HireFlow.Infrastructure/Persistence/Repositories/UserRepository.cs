using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HireFlow.Application.Common.Interfaces.Persistence;
using HireFlow.Domain.Users.Entities;
using HireFlow.Domain.Users.ValueObjects;
using HireFlow.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
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
            await _context.SaveChangesAsync();
        }
        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _context.DomainUsers.FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}
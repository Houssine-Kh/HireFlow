using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HireFlow.Application.Common.Interfaces.Persistence;

namespace HireFlow.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HireFlowDbContext _context;

        public UnitOfWork(HireFlowDbContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // You can add logic here before saving (e.g., auditing, domain events)
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
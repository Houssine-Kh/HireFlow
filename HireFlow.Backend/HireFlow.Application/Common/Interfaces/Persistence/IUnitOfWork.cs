using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HireFlow.Application.Common.Interfaces.Persistence
{
    public interface IUnitOfWork
    {
        // Commits all changes to the database.
        // Returns the number of rows affected.
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);        
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HireFlow.Domain.Jobs.Enums;

namespace HireFlow.Api.Contracts.Jobs
{
    public record CreateJobRequest(
        string Title,
        string ? Description,
        WorkMode? WorkMode
    );
    
}
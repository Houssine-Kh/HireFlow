using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HireFlow.Application.Common.Models;
using HireFlow.Application.Jobs.Common;
using MediatR;

namespace HireFlow.Application.Jobs.Queries
{
    public record GetAllJobsQuery() : IRequest<Result<List<JobDto>>>;

}
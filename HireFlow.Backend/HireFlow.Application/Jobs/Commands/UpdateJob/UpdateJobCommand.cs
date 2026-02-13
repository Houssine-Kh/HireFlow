using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HireFlow.Application.Common.Models;
using HireFlow.Application.Jobs.Common;
using HireFlow.Domain.Jobs.Entities;
using HireFlow.Domain.Jobs.Enums;
using MediatR;

namespace HireFlow.Application.Jobs.Commands.UpdateJob
{
    public record UpdateJobCommand( Guid Id, string Title, string? Description, WorkMode? WorkMode) : IRequest<Result<JobDto>>;

}
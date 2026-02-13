using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HireFlow.Application.Common.Models;
using HireFlow.Domain.Jobs.Enums;
using MediatR;

namespace HireFlow.Application.Jobs.Commands.CreateJob
{
    public record CreateJobCommand(string Title, string? Description, WorkMode? WorkMode) : IRequest<Result<Guid>>;
}
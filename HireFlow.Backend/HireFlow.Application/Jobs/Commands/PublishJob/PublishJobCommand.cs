using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HireFlow.Application.Common.Models;
using MediatR;

namespace HireFlow.Application.Jobs.Commands.PublishJob
{
    public record PublishJobCommand(Guid Id) : IRequest<Result>;

}
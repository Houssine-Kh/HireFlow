using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HireFlow.Application.Common.Models;
using MediatR;

namespace HireFlow.Application.Jobs.Commands.CloseJob
{
    public record CloseJobCommand(Guid Id) : IRequest<Result>;
 
}
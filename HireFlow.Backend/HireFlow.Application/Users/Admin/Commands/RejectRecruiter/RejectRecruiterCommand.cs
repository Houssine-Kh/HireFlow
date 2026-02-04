using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HireFlow.Application.Common.Models;
using MediatR;

namespace HireFlow.Application.Users.Admin.Commands.RejectRecruiter
{
    public record RejectRecruiterCommand(Guid UserId) : IRequest<Result>;
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HireFlow.Application.Common.Models;
using MediatR;

namespace HireFlow.Application.Users.Admin.Commands.UnlockUser
{
    public record UnlockUserCommand(Guid UserId) : IRequest<Result>;

 
}
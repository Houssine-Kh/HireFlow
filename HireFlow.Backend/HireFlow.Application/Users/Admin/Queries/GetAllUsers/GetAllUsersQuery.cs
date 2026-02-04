using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HireFlow.Application.Common.Models;
using HireFlow.Application.Users.Common;
using MediatR;

namespace HireFlow.Application.Users.Admin.Queries.GetAllUsers
{
    public record GetAllUsersQuery() : IRequest<Result<List<UserDto>>>;

}
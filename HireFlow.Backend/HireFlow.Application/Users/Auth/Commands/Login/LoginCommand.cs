using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HireFlow.Application.Users.Auth.Dtos;
using HireFlow.Application.Common.Models;
using MediatR;

namespace HireFlow.Application.Users.Auth.Commands.Login
{
    public record LoginCommand(string Email, string Password) : IRequest<Result<AuthResponseDto>>;
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HireFlow.Application.Users.Auth.Dtos;
using HireFlow.Application.Common.Models;
using HireFlow.Domain.Users.Enums;
using MediatR;

namespace HireFlow.Application.Users.Auth.Commands.Register
{
    public record RegisterCommand(string FirstName, string LastName, string Email, string Password, UserRole Role) : IRequest<Result<AuthResponseDto>>;
 
}
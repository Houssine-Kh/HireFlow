using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HireFlow.Domain.Users.Enums;

namespace HireFlow.Application.Users.Common
{
    public record UserDto(
        Guid Id,
        string FirstName,
        string LastName,
        string Email,
        string Role,
        UserStatus Status
    );

}
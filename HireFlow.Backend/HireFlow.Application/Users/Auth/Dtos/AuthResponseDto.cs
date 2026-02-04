using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HireFlow.Application.Users.Auth.Dtos
{
    public record AuthResponseDto(
        Guid Id,
        string FirstName,
        string LastName,
        string Email,
        string? Token,
        string Role,
        string ? Message = null,
        bool ProfileIsComplete = false
    );
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HireFlow.Application.Jobs.Common
{
    public record JobDto(
        Guid Id,
        Guid RecruiterId,
        string Title,
        string? Description,
        string? WorkMode,
        string Status
    );

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HireFlow.Application.Candidates.Commands.Common
{
    public record CandidateDto
    (
        Guid Id,
        Guid UserId,
        string ResumeUrl,
        string PhoneNumber,
        string? LinkedInUrl,
        string? EducationLevel
    );
}
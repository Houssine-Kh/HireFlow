using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HireFlow.Domain.Candidates.Enums;

namespace HireFlow.Api.Contracts.Candidates
{
    public record CreateProfileRequest(
        string ResumeUrl,
        string PhoneNumber,
        string? LinkedInUrl,
        EducationLevel? EducationLevel
    );
}
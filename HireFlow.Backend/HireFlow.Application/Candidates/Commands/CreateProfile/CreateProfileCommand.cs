using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HireFlow.Application.Candidates.Commands.Common;
using HireFlow.Application.Common.Models;
using HireFlow.Domain.Candidates.Enums;
using HireFlow.Domain.Candidates.ValueObjects;
using MediatR;

namespace HireFlow.Application.Candidates.Commands.CreateProfile
{
    public record CreateProfileCommand(Guid UserId, string ResumeUrl, string PhoneNumber, string? LinkedInUrl, EducationLevel? EducationLevel) : IRequest<Result<Guid>>;
   
}
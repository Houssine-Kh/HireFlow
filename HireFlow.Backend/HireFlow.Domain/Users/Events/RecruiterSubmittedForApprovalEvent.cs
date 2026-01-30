using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HireFlow.Domain.Common.Interfaces;

namespace HireFlow.Domain.Users.Events
{
    public record RecruiterSubmittedForApprovalEvent(
        Guid RecruiterId,
        string Email,
        string FirstName,
        string LastName,
        DateTime SubmittedAt
    ) : IDomainEvent;
  
}
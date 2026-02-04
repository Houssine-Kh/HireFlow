using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HireFlow.Domain.Users.ValueObjects;
using HireFlow.Domain.Users.Enums;
using HireFlow.Domain.Common;
using HireFlow.Domain.Exceptions;
using System.Data;
using HireFlow.Domain.Users.Events;


namespace HireFlow.Domain.Users.Entities
{
    public class User : AggregateRoot
    {
        public PersonName Name { get; private set; } = default!;
        public Email Email { get; private set; } = default!;
        public UserRole Role { get; private set; }

        public UserStatus Status { get; private set; }

        private User() { } // EF

        private User(Guid id, PersonName name, Email email, UserRole role)
        {
            Id = id;
            Name = name;
            Email = email;
            Role = role;
            Status = (role == UserRole.Recruiter) ? UserStatus.Pending : UserStatus.Active;    // Recruiters start as Pending, Candidates start as Active
        }

        public static User Create(Guid id, PersonName name, Email email, UserRole role)
        {
            var user = new User(id, name, email, role);
            if (role == UserRole.Recruiter)
            {
                user.AddDomainEvent(new RecruiterSubmittedForApprovalEvent(
                    user.Id,
                    user.Email.Value,
                    user.Name.FirstName,
                    user.Name.LastName,
                    DateTime.UtcNow
                ));
            }
            return user;
        }


        // Action for the Admin to call later
        public void ApproveRecruiter()
        {
            if (Role != UserRole.Recruiter)
                throw new DomainException("Only recruiters need approval.");
        
            if (Status == UserStatus.Active)
                // Idempotency check: If already approved, do nothing or warn
                return;
            
            Status = UserStatus.Active;
        }

        public void Ban()
        {
            if (Status == UserStatus.Banned)
                return;
            
            Status = UserStatus.Banned;
        }

        public void unlock()
        {
            if (Status != UserStatus.Banned)
                throw new DomainException("Only banned users can be unlocked.");

            Status = UserStatus.Active;
        }
    }
}
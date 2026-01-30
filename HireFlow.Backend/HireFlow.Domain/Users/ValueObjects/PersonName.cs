using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HireFlow.Domain.Exceptions;

namespace HireFlow.Domain.Users.ValueObjects
{
    public record PersonName
    {
        public string FirstName { get; }
        public string LastName { get; }
        public string FullName => $"{FirstName} {LastName}";

        public PersonName(string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new DomainException("First name cannot be empty.");

            if (string.IsNullOrWhiteSpace(lastName))
                throw new DomainException("Last name cannot be empty.");

            FirstName = firstName.Trim();
            LastName = lastName.Trim();
        }
    }
}
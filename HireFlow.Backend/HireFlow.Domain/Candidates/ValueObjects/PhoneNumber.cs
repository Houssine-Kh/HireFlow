using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HireFlow.Domain.Exceptions;

namespace HireFlow.Domain.Candidates.ValueObjects
{
    public record PhoneNumber
    {
        public string Value {get;}

        public PhoneNumber(string value)
        {
            Value = value;
        } 

        public static PhoneNumber Create(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new DomainException("Phone number cannot be empty.");
    
            if(value.Length <8)
                throw new DomainException("Phone number is too short.");

            return new PhoneNumber(value);
        }

    }
}
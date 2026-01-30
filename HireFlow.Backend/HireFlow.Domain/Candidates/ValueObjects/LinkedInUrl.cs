using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using HireFlow.Domain.Exceptions;

namespace HireFlow.Domain.Candidates.ValueObjects
{
    public record LinkedInUrl
    {
        public string Value{get;}

        public LinkedInUrl(string value)
        {
            Value = value;
        }

        public static LinkedInUrl Create(string value)
        {
            if(string.IsNullOrEmpty(value))
                throw new DomainException("LinkedIn URL cannot be empty.");

            var normalized = value.Trim();
            
            if (!normalized.ToLowerInvariant().Contains("linkedin.com/in/"))
            {
                throw new DomainException("Invalid LinkedIn profile URL. It must contain 'linkedin.com/in/'.");
            }

            return new LinkedInUrl(normalized);
        }
    }
}
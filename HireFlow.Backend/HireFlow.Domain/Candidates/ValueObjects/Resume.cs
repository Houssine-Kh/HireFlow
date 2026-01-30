using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HireFlow.Domain.Exceptions;

namespace HireFlow.Domain.Candidates.ValueObjects
{
    public record Resume
    {
        public string Url{get;}

        public Resume(string url)
        {
            Url = url;
        }

        public static Resume Create(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new DomainException("Resume URL is missing.");
            
            if (!url.EndsWith(".pdf") && !url.EndsWith(".docx"))
                throw new DomainException("Only PDF and DOCX formats are supported.");
                
            return new Resume(url);
        }
    }
}
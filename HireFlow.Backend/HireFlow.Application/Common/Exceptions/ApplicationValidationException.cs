using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HireFlow.Application.Common.Exceptions
{
    public class ApplicationValidationException : Exception
    {
        public IDictionary<string, string[]> Errors { get; }

        public ApplicationValidationException(IDictionary<string, string[]> errors) : base("Validation failed.")
        {
            Errors = errors;
        }
    }
}
using System;
using System.Text.RegularExpressions;

namespace HireFlow.Domain.Users.ValueObjects
{
    public record Email
    {
        public string Value { get; }

        private const string Pattern = @"^(.+)@(.+)$";

        // EF Core needs a parameterless constructor if not using a Converter
        // But since we use a Converter, we strictly enforce the public constructor below.
        
        public Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Email cannot be empty.", nameof(value));

            var normalized = value.Trim().ToLowerInvariant();

            if (!Regex.IsMatch(normalized, Pattern))
                throw new ArgumentException("Email is invalid.", nameof(value));

            Value = normalized;
        }
    }
}
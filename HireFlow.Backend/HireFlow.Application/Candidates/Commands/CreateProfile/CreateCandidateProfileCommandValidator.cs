using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace HireFlow.Application.Candidates.Commands.CreateProfile
{
    public class CreateCandidateProfileCommandValidator : AbstractValidator<CreateProfileCommand>
    {
       public CreateCandidateProfileCommandValidator()
        {
            // 1. Resume: Must be a valid URL
            RuleFor(x => x.ResumeUrl)
                .NotEmpty().WithMessage("Resume URL is required.")
                .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
                .WithMessage("Resume URL must be a valid link (e.g., https://...).");

            // 2. Phone: Basic format checks
            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .MinimumLength(8).WithMessage("Phone number is too short.")
                .MaximumLength(20).WithMessage("Phone number is too long.")
                .Matches(@"^\+?[0-9\s]*$").WithMessage("Phone number can only contain digits and spaces.");

            // 3. LinkedIn: strict check ONLY IF provided
            RuleFor(x => x.LinkedInUrl)
                .Must(url => url!.Contains("linkedin.com/in/"))
                .WithMessage("LinkedIn URL must contain 'linkedin.com/in/'.")
                .When(x => !string.IsNullOrEmpty(x.LinkedInUrl)); // 👈 Skip if empty

            // 4. Enum Check
            RuleFor(x => x.EducationLevel)
                .IsInEnum().WithMessage("Invalid education level selected.")
                .When(x => x.EducationLevel.HasValue);
        }       
    }
}
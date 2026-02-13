using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace HireFlow.Application.Jobs.Commands.CreateJob
{
    public class CreateJobCommandValidator : AbstractValidator<CreateJobCommand>
    {
        public CreateJobCommandValidator()
        {
            RuleFor(v => v.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

            RuleFor(v => v.Description)
                .MaximumLength(4000).WithMessage("Description must not exceed 4000 characters.");

            // We don't need to validate WorkMode because ASP.NET handles Enum parsing,
            // and it is allowed to be null for Drafts.
        }
    }
}
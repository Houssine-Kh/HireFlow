using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace HireFlow.Application.Jobs.Commands.UpdateJob
{
    public class UpdateJobCommandValidator : AbstractValidator<UpdateJobCommand>
    {
        public UpdateJobCommandValidator()
        {
            RuleFor(v => v.Id)
                .NotEmpty().WithMessage("Job ID is required.");

            RuleFor(v => v.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

            RuleFor(v => v.Description)
                .MaximumLength(4000).WithMessage("Description must not exceed 4000 characters.");
        }
    }
}
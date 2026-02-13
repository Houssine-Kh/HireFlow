using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace HireFlow.Application.Jobs.Commands.CloseJob
{
    public class CloseJobCommandValidator : AbstractValidator<CloseJobCommand>
    {
        public CloseJobCommandValidator()
        {
            RuleFor(v => v.Id)
                .NotEmpty().WithMessage("Job ID is required to publish.");
        }
    }
}
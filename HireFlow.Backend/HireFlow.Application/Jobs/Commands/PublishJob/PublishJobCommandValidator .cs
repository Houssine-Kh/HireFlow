using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace HireFlow.Application.Jobs.Commands.PublishJob
{
    public class PublishJobCommandValidator : AbstractValidator<PublishJobCommand>
    {
        public PublishJobCommandValidator()
        {
            RuleFor(v => v.Id)
                .NotEmpty().WithMessage("Job ID is required to publish.");
        }
    }
}
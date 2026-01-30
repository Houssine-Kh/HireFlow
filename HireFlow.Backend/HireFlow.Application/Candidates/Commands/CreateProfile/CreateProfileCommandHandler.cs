using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using HireFlow.Application.Candidates.Commands.Common;
using HireFlow.Application.Common.Interfaces.Persistence;
using HireFlow.Application.Common.Models;
using HireFlow.Domain.Candidates.Entities;
using HireFlow.Domain.Candidates.Repositories;
using HireFlow.Domain.Candidates.ValueObjects;
using HireFlow.Domain.Users.Entities;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace HireFlow.Application.Candidates.Commands.CreateProfile
{

    public class CreateProfileCommandHandler : IRequestHandler<CreateProfileCommand, Result<Guid>>
    {
        private readonly ICandidateRepository _candidateRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateProfileCommandHandler> _logger;

        public CreateProfileCommandHandler(ICandidateRepository candidateRepository, IUnitOfWork unitOfWork, ILogger<CreateProfileCommandHandler> logger)
        {
            _candidateRepository = candidateRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<Guid>> Handle(CreateProfileCommand request, CancellationToken cancellationToken)
        {

            if (await _candidateRepository.GetByUserIdAsync(request.UserId) != null) // candidates table not null means the user has completed the wizard 
            {
                return Result<Guid>.Fail("Profile already exists.");
            }

            var resume = Resume.Create(request.ResumeUrl);
            var phone = PhoneNumber.Create(request.PhoneNumber);
            LinkedInUrl? linkedIn = null;

            if (!string.IsNullOrWhiteSpace(request.LinkedInUrl))
            {
                linkedIn = LinkedInUrl.Create(request.LinkedInUrl);
            }

            var candidate = Candidate.Create(
                    Guid.NewGuid(),
                    request.UserId,
                    resume,
                    phone,
                    linkedIn,
                    request.EducationLevel
                );
            await _candidateRepository.AddAsync(candidate);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Ok(candidate.Id);
        }
    }
}
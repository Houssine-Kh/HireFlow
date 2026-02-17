using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HireFlow.Application.Common.Interfaces.Persistence;
using HireFlow.Application.Common.Interfaces.Services;
using HireFlow.Application.Common.Models;
using HireFlow.Application.Jobs.Common;
using HireFlow.Domain.Jobs.Entities;
using HireFlow.Domain.Jobs.Repositories;
using MediatR;

namespace HireFlow.Application.Jobs.Commands.CreateJob
{
    public class CreateJobCommandHandler : IRequestHandler<CreateJobCommand, Result<JobDto>>
    {
        private readonly IJobRepository _jobRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public CreateJobCommandHandler(IJobRepository jobRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _jobRepository = jobRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }
        public async Task<Result<JobDto>> Handle(CreateJobCommand request, CancellationToken ct)
        {
            var currentUserId = _currentUserService.UserId;

            if(currentUserId == null)
                return Result<JobDto>.Fail("Unauthorized: User is not logged in.");

            var job  = Job.Create(
                Guid.NewGuid(),
                currentUserId.Value,
                request.Title,
                request.Description,
                request.WorkMode
            );

            await _jobRepository.AddAsync(job);
            await _unitOfWork.SaveChangesAsync(ct);

            return Result<JobDto>.Ok(new JobDto(
                job.Id,
                job.RecruiterId,
                job.Title,
                job.Description,
                job.WorkMode?.ToString(),
                job.Status.ToString()
            ));
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HireFlow.Application.Common.Interfaces.Persistence;
using HireFlow.Application.Common.Interfaces.Services;
using HireFlow.Application.Common.Models;
using HireFlow.Application.Jobs.Common;
using HireFlow.Application.Users.Common;
using HireFlow.Domain.Jobs.Repositories;
using HireFlow.Domain.Users.Entities;
using MediatR;

namespace HireFlow.Application.Jobs.Commands.UpdateJob
{
    public class UpdateJobCommandHandler : IRequestHandler<UpdateJobCommand, Result<JobDto>>
    {
        private readonly IJobRepository _jobRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public UpdateJobCommandHandler(IJobRepository jobRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _jobRepository = jobRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Result<JobDto>> Handle(UpdateJobCommand request, CancellationToken ct)
        {
            var currentUserId = _currentUserService.UserId;

            if (currentUserId == null)
                return Result<JobDto>.Fail("Unauthorized: User is not logged in.");

            var job = await _jobRepository.GetByIdAsync(request.Id);

            if (job == null)
                return Result<JobDto>.Fail($"Job with ID {request.Id} not found.");

            if (job.RecruiterId != currentUserId)
                return Result<JobDto>.Fail("Forbidden: You do not own this job.");

            job.Update(request.Title, request.Description, request.WorkMode);

            await _unitOfWork.SaveChangesAsync(ct);

            return Result<JobDto>.Ok(new JobDto(
                job.Id,
                currentUserId.Value,
                job.Title,
                job.Description,
                job.WorkMode.ToString(),
                job.Status.ToString()
            ));
        }
    }
}
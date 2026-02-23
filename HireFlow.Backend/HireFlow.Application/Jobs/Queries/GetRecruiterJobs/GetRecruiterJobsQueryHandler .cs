using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using HireFlow.Application.Common.Interfaces.Services;
using HireFlow.Application.Common.Models;
using HireFlow.Application.Jobs.Common;
using HireFlow.Domain.Jobs.Repositories;
using MediatR;

namespace HireFlow.Application.Jobs.Queries.GetRecruiterJobs
{
    public class GetRecruiterJobsQueryHandler : IRequestHandler<GetRecruiterJobsQuery, Result<List<JobDto>>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IJobRepository _jobRepository;
        public GetRecruiterJobsQueryHandler(ICurrentUserService currentUserService, IJobRepository jobRepository)
        {
            _currentUserService = currentUserService;
            _jobRepository = jobRepository;
        }
        public async Task<Result<List<JobDto>>> Handle(GetRecruiterJobsQuery request, CancellationToken ct)
        {
            var currentUserId = _currentUserService.UserId;

            if(currentUserId == null)
                return Result<List<JobDto>>.Fail("Unauthorized: User is not logged in.");

            var jobs = await _jobRepository.GetByRecruiterIdAsync(currentUserId.Value, ct);

            var dtos = jobs.Select(j => new JobDto(
                j.Id,
                j.RecruiterId,
                j.Title,
                j.Description,
                j.WorkMode?.ToString(),
                j.Status.ToString()
            )).ToList();

            return Result<List<JobDto>>.Ok(dtos);
        }
    }
}
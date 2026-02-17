using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using HireFlow.Application.Common.Models;
using HireFlow.Application.Jobs.Common;
using HireFlow.Application.Users.Common;
using HireFlow.Domain.Jobs.Entities;
using HireFlow.Domain.Jobs.Repositories;
using MediatR;

namespace HireFlow.Application.Jobs.Queries
{
    public class GetAllJobsQueryHandler : IRequestHandler<GetAllJobsQuery, Result<List<JobDto>>>
    {
        private readonly IJobRepository _jobRepository;

        public GetAllJobsQueryHandler(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }
        public async Task<Result<List<JobDto>>> Handle(GetAllJobsQuery request, CancellationToken ct)
        {
            var jobs = await _jobRepository.GetAllAsync() ?? new List<Job>();

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
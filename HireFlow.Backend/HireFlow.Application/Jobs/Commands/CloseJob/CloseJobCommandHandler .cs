using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HireFlow.Application.Common.Interfaces.Persistence;
using HireFlow.Application.Common.Interfaces.Services;
using HireFlow.Application.Common.Models;
using HireFlow.Domain.Jobs.Repositories;
using MediatR;

namespace HireFlow.Application.Jobs.Commands.CloseJob
{
    public class CloseJobCommandHandler : IRequestHandler<CloseJobCommand,Result>
    {
        private readonly IJobRepository _jobRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public CloseJobCommandHandler(IJobRepository jobRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _jobRepository = jobRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Result> Handle(CloseJobCommand request, CancellationToken ct)
        {
            var currentUserId = _currentUserService.UserId;

            if (currentUserId == null)
                return Result.Fail("Unauthorized: User is not logged in.");    
                        
            var job = await _jobRepository.GetByIdAsync(request.Id);

            if(job == null)
                return Result.Fail($"Cannot close job: Job with Id '{request.Id}' does not exist.");

            // The Anti-Spoofing Check
            if (job.RecruiterId != currentUserId)
                return Result.Fail("Forbidden: You do not own this job.");

            job.Close();
            await _unitOfWork.SaveChangesAsync(ct);
            
            return Result.Ok();
        }
    }
}
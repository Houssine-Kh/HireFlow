using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HireFlow.Application.Common.Interfaces.Auth;
using HireFlow.Application.Common.Interfaces.Persistence;
using HireFlow.Application.Common.Models;
using HireFlow.Domain.Users.Enums;
using HireFlow.Domain.Users.Repositories;
using MediatR;

namespace HireFlow.Application.Users.Admin.Commands.RejectRecruiter
{

    public class RejectRecruiterCommandHandler : IRequestHandler<RejectRecruiterCommand,Result> 
    {
        private readonly IUserRepository _userRepository;
        private readonly IIdentityService _identityservice;
        private readonly IUnitOfWork _unitOfWork;
    

        public RejectRecruiterCommandHandler(IUserRepository userRepository, IIdentityService identityService, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _identityservice = identityService;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(RejectRecruiterCommand request, CancellationToken ct)
        {
            var domainUser = await _userRepository.GetByIdAsync(request.UserId);
            if(domainUser == null)
                return Result.Fail($"User with ID {request.UserId} not found.");
            
            // Only allow pending recruiters
            if(domainUser.Role != UserRole.Recruiter || domainUser.Status != UserStatus.Pending)
                return Result.Fail("Only pending recruiters can be rejected.");

            domainUser.Ban();

            await _unitOfWork.SaveChangesAsync(ct);

            return Result.Ok();

        }
    }
}
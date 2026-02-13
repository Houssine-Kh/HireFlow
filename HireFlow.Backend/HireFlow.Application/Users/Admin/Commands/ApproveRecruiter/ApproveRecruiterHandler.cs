using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HireFlow.Application.Common.Interfaces.Persistence;
using HireFlow.Application.Common.Models;
using HireFlow.Domain.Users.Repositories;
using MediatR;
using MediatR.Pipeline;

namespace HireFlow.Application.Users.Admin.Commands.ApproveRecruiter
{
    public class ApproveRecruiterHandler : IRequestHandler<ApproveRecruiterCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ApproveRecruiterHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(ApproveRecruiterCommand request, CancellationToken ct)
        {
            var domainUser = await _userRepository.GetByIdAsync(request.UserId);

            if(domainUser == null)
            {
                return Result<bool>.Fail("User do not exists");
            }
            domainUser.ApproveRecruiter();

            await _unitOfWork.SaveChangesAsync(ct);
            return Result.Ok();

        
        }
    }
}
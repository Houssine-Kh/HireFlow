using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using HireFlow.Application.Common.Interfaces.Persistence;
using HireFlow.Application.Common.Models;
using MediatR;
using MediatR.Pipeline;

namespace HireFlow.Application.Users.Admin.Commands.UnlockUser
{
    public class UnlockUserCommandHandler : IRequestHandler<UnlockUserCommand,Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        public UnlockUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(UnlockUserCommand request, CancellationToken ct)
        {
            var domainUser = await _userRepository.GetByIdAsync(request.UserId);

            if(domainUser == null)
                return Result.Fail($"User with ID {request.UserId} not found.");

            domainUser.unlock();

            await _unitOfWork.SaveChangesAsync();

            return Result.Ok();
        }
    }
}
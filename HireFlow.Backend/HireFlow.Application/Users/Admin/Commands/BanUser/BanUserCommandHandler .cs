using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HireFlow.Application.Common.Interfaces.Persistence;
using HireFlow.Application.Common.Models;
using HireFlow.Domain.Users.Repositories;
using MediatR;

namespace HireFlow.Application.Users.Admin.Commands.BanUser
{
    public class BanUserCommandHandler : IRequestHandler<BanUserCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public BanUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(BanUserCommand request, CancellationToken ct)
        {
            var domainUser = await _userRepository.GetByIdAsync(request.UserId);

            if(domainUser == null)
                return Result.Fail($"User with ID {request.UserId} not found.");

            domainUser.Ban();

            await _unitOfWork.SaveChangesAsync();

            return Result.Ok();
        }
        
    }
}
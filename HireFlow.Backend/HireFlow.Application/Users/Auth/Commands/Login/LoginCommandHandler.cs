using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using HireFlow.Application.Users.Auth.Dtos;
using HireFlow.Application.Common.Interfaces.Auth;
using HireFlow.Application.Common.Interfaces.Persistence;
using HireFlow.Application.Common.Models;
using HireFlow.Domain.Users.Entities;
using MediatR;
using HireFlow.Domain.Users.Enums;
using System.Runtime.CompilerServices;
using HireFlow.Domain.Candidates.Repositories;

namespace HireFlow.Application.Users.Auth.Commands.Login
{

    public class LoginCommandHandler : IRequestHandler<LoginCommand,Result<AuthResponseDto>>
    {
        private readonly IIdentityService _identityService;
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepository;

        private readonly ICandidateRepository _candidateRepository;

        public LoginCommandHandler(IIdentityService identityService, ITokenService tokenService, IUserRepository userRepository, ICandidateRepository candidateRepository)
        {
            _identityService = identityService;
            _tokenService = tokenService;
            _userRepository = userRepository;
            _candidateRepository = candidateRepository;
        }
        public async Task<Result<AuthResponseDto>> Handle(LoginCommand request, CancellationToken ct)
        {
            var userId = await _identityService.GetIdentityUserIdByEmail(request.Email);

            if(userId == Guid.Empty)
                return Result<AuthResponseDto>.Fail("Invalid Email or Password.");

            var IsValid = await  _identityService.CheckPassword(userId,request.Password);
            if (!IsValid)
                return Result<AuthResponseDto>.Fail("Invalid Email Or Password.");

            var domainUser = await _userRepository.GetByIdAsync(userId);

            if(domainUser == null)
                return Result<AuthResponseDto>.Fail("User not found in domain.");

            if(domainUser.Status == UserStatus.Pending)
                return Result<AuthResponseDto>.Fail("Your account is pending approval by an Administrator.");
            

            if(domainUser.Status == UserStatus.Banned)
                return Result<AuthResponseDto>.Fail("Your account has been suspended. Contact support.");


            var token = _tokenService.CreateToken(userId, request.Email, domainUser.Role.ToString());

            bool isProfileComplete = true;

            if(domainUser.Role == UserRole.Candidate)
            {
                var candidateProfile = await _candidateRepository.GetByUserIdAsync(userId);
                isProfileComplete = candidateProfile != null;
            }

            return Result<AuthResponseDto>.Ok(new AuthResponseDto(userId, domainUser.Name.FirstName, domainUser.Name.LastName,request.Email,token,domainUser.Role.ToString() ,ProfileIsComplete : isProfileComplete));
        }
    }
}
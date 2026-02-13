using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using HireFlow.Application.Users.Auth.Dtos;
using HireFlow.Application.Common.Interfaces.Auth;
using HireFlow.Application.Common.Models;
using HireFlow.Domain.Users.Entities;
using HireFlow.Domain.Users.ValueObjects;
using HireFlow.Domain.Users.Enums;

using MediatR;
using HireFlow.Application.Common.Interfaces.Persistence;
using HireFlow.Domain.Users.Repositories;

namespace HireFlow.Application.Users.Auth.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<AuthResponseDto>>
    {
        private readonly IIdentityService _identityService;
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterCommandHandler(IIdentityService identityService, ITokenService tokenService, IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _identityService = identityService;
            _tokenService = tokenService;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<AuthResponseDto>> Handle(RegisterCommand request, CancellationToken ct)
        {
                if (await _identityService.EmailExists(request.Email))
                {
                    return Result<AuthResponseDto>.Fail("user with this email already exists.");
                }
                var identityResult = await _identityService.CreateIdentityUser(request.FirstName,
                    request.LastName,
                    request.Email,
                    request.Password,
                    request.Role.ToString()
                );

                if(!identityResult.IsSuccess){
                    return Result<AuthResponseDto>.Fail("Failded to create identity user."+identityResult.Error);
                }
                
                var userId = identityResult.Value;

                var domainUser = User.Create(
                    userId,
                    new PersonName(request.FirstName, request.LastName),
                    new Email(request.Email),
                    request.Role
                );

                await _userRepository.AddAsync(domainUser);

                await _unitOfWork.SaveChangesAsync(ct);


                if(domainUser.Status == UserStatus.Pending)
                {
                    return Result<AuthResponseDto>.Ok(new AuthResponseDto(
                        userId,
                        domainUser.Name.FirstName,
                        domainUser.Name.LastName,
                        domainUser.Email.Value,
                        Token : null,
                        domainUser.Role.ToString(),
                        Message : "Account created successfully. Please wait for Admin approval.",
                        ProfileIsComplete : true
                    ));
                }

                var token = _tokenService.CreateToken(userId, request.Email, domainUser.Role.ToString());
                return Result<AuthResponseDto>.Ok(new AuthResponseDto(
                    userId,
                    domainUser.Name.FirstName,
                    domainUser.Name.LastName,
                    request.Email,
                    token,
                    domainUser.Role.ToString())
                    );
       
        }

    }
}
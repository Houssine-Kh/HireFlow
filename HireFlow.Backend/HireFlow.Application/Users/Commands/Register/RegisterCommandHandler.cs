using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using HireFlow.Application.Users.Dtos;
using HireFlow.Application.Common.Interfaces.Auth;
using HireFlow.Application.Common.Models;
using HireFlow.Domain.Users.Entities;
using HireFlow.Domain.Users.ValueObjects;
using HireFlow.Domain.Users.Enums;

using MediatR;
using System.Data;
using HireFlow.Application.Common.Interfaces.Persistence;
using System.Transactions;

namespace HireFlow.Application.Users.Commands.Register
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
            // Transaction Scope ensures both Identity and Domain user are created, or NEITHER is.
            using  var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                if (await _identityService.EmailExists(request.Email))
                {
                    return Result<AuthResponseDto>.Fail("user with this email already exists.");
                }
                var userId = await _identityService.CreateIdentityUser(request.FirstName,
                    request.LastName,
                    request.Email,
                    request.Password,
                    request.Role.ToString()
                );

                bool isActive = request.Role != UserRole.Recruiter;

                var domainUser = User.Create(
                    userId,
                    new PersonName(request.FirstName, request.LastName),
                    new Email(request.Email),
                    request.Role
                );

                await _userRepository.AddAsync(domainUser);

                await _unitOfWork.SaveChangesAsync(ct);

                transaction.Complete();

                if(request.Role == UserRole.Recruiter)
                {
                    return Result<AuthResponseDto>.Ok(new AuthResponseDto(
                        userId,
                        domainUser.Name.FirstName,
                        domainUser.Name.LastName,
                        domainUser.Email.Value,
                        Token : null,
                        domainUser.Role.ToString(),
                        Message : "Account created successfully. Please wait for Admin approval."
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
            catch (Exception)
            {
                return Result<AuthResponseDto>.Fail("Registration failed due to an internal error.");
            }
        }

    }
}
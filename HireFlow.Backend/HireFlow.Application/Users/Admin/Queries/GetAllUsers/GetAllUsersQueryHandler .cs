using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using HireFlow.Application.Common.Interfaces.Persistence;
using HireFlow.Application.Common.Models;
using HireFlow.Application.Users.Common;
using HireFlow.Domain.Users.Entities;
using HireFlow.Domain.Users.Repositories;
using MediatR;

namespace HireFlow.Application.Users.Admin.Queries.GetAllUsers
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, Result<List<UserDto>>>
    {
        private readonly IUserRepository _userRepository;
        public GetAllUsersQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<Result<List<UserDto>>> Handle(GetAllUsersQuery request, CancellationToken ct)
        {
            var domainUsers = await _userRepository.GetAllAsync();

            var userDtos = domainUsers.OrderBy(u => u.Status)
                         .Select(user => new UserDto(
                            user.Id,
                            user.Name.FirstName,
                            user.Name.LastName,
                            user.Email.Value,
                            user.Role.ToString(),
                            user.Status
                            ))
                            .ToList();

            return Result<List<UserDto>>.Ok(userDtos);
        }
    }
}
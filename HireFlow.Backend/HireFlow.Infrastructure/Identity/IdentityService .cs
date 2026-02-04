
using HireFlow.Application.Common.Interfaces.Auth;
using HireFlow.Application.Common.Models;
using Microsoft.AspNetCore.Identity;

namespace HireFlow.Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public IdentityService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<bool> EmailExists(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if(user != null)
            {
                return true;
            }
            return false;
        }
        public async Task<Guid> GetIdentityUserIdByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            return user?.Id ?? Guid.Empty ;
    
        }
        public async Task<Result<Guid>> CreateIdentityUser(string firstName, string lastName, string email, string Password, string role)
        {
            var appUser = new AppUser
            {
               FirstName = firstName,
               LastName = lastName,
               Email = email,
               UserName = email,
            };
            var result = await _userManager.CreateAsync(appUser,Password);
            if(!result.Succeeded)
            {
                return Result<Guid>.Fail(string.Join(", ",result.Errors.Select(e => e.Description)));
            }
            await _userManager.AddToRoleAsync(appUser, role); 
            
            return Result<Guid>.Ok(appUser.Id);
        }
        public async Task<bool> CheckPassword( Guid UserId,string Password)
        {
            var appUser = await _userManager.FindByIdAsync(UserId.ToString());
            if(appUser == null) return false;
            return await _userManager.CheckPasswordAsync(appUser,Password);
        }
        public async Task<string> GetUserRole(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if(user == null)
                throw new Exception("User not found");

            var roles = await _userManager.GetRolesAsync(user);
            return roles.FirstOrDefault() ?? "Candidate";
        }

        public async Task<Result> DeleteIdentityUser(Guid userId)
        {
            var IdentityUser = await _userManager.FindByIdAsync(userId.ToString());

            if(IdentityUser == null)
            return Result.Ok();

            var result = await _userManager.DeleteAsync(IdentityUser);

            if (!result.Succeeded)
            {
                var errorDescripton = string.Join(",",result.Errors.Select(e => e.Description ));
                return Result.Fail(errorDescripton);
            }
            return Result.Ok();

        }
    }
}
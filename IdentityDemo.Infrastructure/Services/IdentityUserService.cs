using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using IdentityDemo.Application.Dtos;
using IdentityDemo.Application.Users;
using IdentityDemo.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;

namespace IdentityDemo.Infrastructure.Services
{
    public class IdentityUserService // Infrastructure-servicen
    (
    UserManager<ApplicationUser> userManager, // Hanterar användare
    SignInManager<ApplicationUser> signInManager, // Hanterar inlogging
    RoleManager<IdentityRole> roleManager // Hanterar roller
    ) : IIdentityUserService
    {
        public async Task<UserResultDto> CreateUserAsync(UserProfileDto user, string password)
        {
            ApplicationUser userxxx = new ApplicationUser
            {
                UserName = user.Email,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
            var result = await userManager.CreateAsync(userxxx, password);
            
            if (result.Succeeded)
            {
                result = await userManager.AddClaimsAsync(userxxx, [
                new Claim("Department", "IT"),
                new Claim("ShoeSize", "42")
                ]);
            }
            //var result = await userManager.CreateAsync(new ApplicationUser
            //{
            //    UserName = user.Email,
            //    Email = user.Email,
            //    FirstName = user.FirstName,
            //    LastName = user.LastName
            //}, password);






            return new UserResultDto(result.Errors.FirstOrDefault()?.Description);
        }

        public async Task<UserResultDto> SignInAsync(string email, string password)
        {
            // Logga in
            SignInResult result = await signInManager.PasswordSignInAsync(
            email,
            password,
            isPersistent: false,
            lockoutOnFailure: false);
            bool wasUserSignedIn = result.Succeeded;
            // Logga ut
            //await signInManager.SignOutAsync();

            return new UserResultDto(result.Succeeded ? null : "Invalid user credentials");
        }
    }
}

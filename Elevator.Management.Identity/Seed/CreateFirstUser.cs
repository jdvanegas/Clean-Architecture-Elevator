using Elevator.Management.Identity.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Elevator.Management.Identity.Seed
{
    public static class UserCreator
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
        {
            var applicationUser = new ApplicationUser
            {
                FirstName = "John",
                LastName = "Due",
                UserName = "johndue",
                Email = "john@test.com",
                EmailConfirmed = true
            };

            var user = await userManager.FindByEmailAsync(applicationUser.Email);
            if (user == null)
                await userManager.CreateAsync(applicationUser, "Elevator#01");
            
        }
    }
}
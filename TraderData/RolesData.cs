using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using TraderData.Models;

namespace TraderData
{
    public static class RolesData
    {
        private static readonly string[] Roles = new string[] { "Admin", "Premium", "Super Admin" };

        public static async Task SeedRoles(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {

                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                foreach (var role in Roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

				// find the user with the admin email 
				var _user = await UserManager.FindByEmailAsync("kieran@admin.com");

				// check if the user exists
				if (_user == null)
				{
					//Here you could create the super admin who will maintain the web app
					var poweruser = new ApplicationUser
					{
						UserName = "kieran@admin.com",
						Email = "kieran@admin.com"
					};
					string adminPassword = "Crypto123?";

					var createPowerUser = await UserManager.CreateAsync(poweruser, adminPassword);
					if (createPowerUser.Succeeded)
					{
						//here we tie the new user to the role
						await UserManager.AddToRoleAsync(poweruser, "Admin");

					}
				}

            }
        }
    }
}

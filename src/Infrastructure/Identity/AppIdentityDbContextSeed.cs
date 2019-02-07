using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.QueueManager.Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager)
        {
            var defaultUser = new ApplicationUser {
                Name = "Demo User",
                UserName = "demouser",
                PhoneNumber = "55555555",
                Email = "demouser@queue.com"
            };

            var defaultUser2 = new ApplicationUser
            {
                Name = "Alex Geovany Soler",
                UserName = "alexsoler",
                PhoneNumber = "99900000",
                Email = "alex@soler.com"
            };

            await userManager.CreateAsync(defaultUser, "Pass@word1");
            await userManager.CreateAsync(defaultUser2, "Pass@word2");

            await roleManager.CreateAsync(new IdentityRole("Administrador"));
            await roleManager.CreateAsync(new IdentityRole("Agente de Atención"));

            await userManager.AddToRolesAsync(defaultUser, new string[] { "Administrador", "Agente de Atención" });
            await userManager.AddToRolesAsync(defaultUser2, new string[] { "Administrador" });
        }
    }
}

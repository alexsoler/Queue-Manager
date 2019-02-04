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
            var defaultUser = new ApplicationUser { UserName = "demouser", Email = "demouser@queue.com" };
            await userManager.CreateAsync(defaultUser, "Pass@word1");

            await roleManager.CreateAsync(new IdentityRole("Administrador"));
            await roleManager.CreateAsync(new IdentityRole("Agente de Atención"));

            var usuario = await userManager.FindByNameAsync("demouser");

            await userManager.AddToRoleAsync(usuario, "Administrador");
        }
    }
}

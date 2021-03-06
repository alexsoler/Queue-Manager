﻿using ApplicationCore;
using ApplicationCore.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.QueueManager.Infrastructure.Data
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager)
        {
            var defaultUser = new ApplicationUser {
                Name = "Demo User",
                UserName = "demouser",
                PhoneNumber = "55555555",
                Email = "demouser@queue.com",
                Activo = true,
                CreationDate = DateTime.Now
            };

            await userManager.CreateAsync(defaultUser, "Pass@word1");

            await roleManager.CreateAsync(new IdentityRole(RolesStatic.Admin));
            await roleManager.CreateAsync(new IdentityRole(RolesStatic.Operator));
            await roleManager.CreateAsync(new IdentityRole(RolesStatic.TouchScreen));
            await roleManager.CreateAsync(new IdentityRole(RolesStatic.DisplayScreen));

            await userManager.AddToRolesAsync(defaultUser, new string[] {
                RolesStatic.Admin, RolesStatic.Operator,
                RolesStatic.TouchScreen, RolesStatic.DisplayScreen
            });
        }
    }
}

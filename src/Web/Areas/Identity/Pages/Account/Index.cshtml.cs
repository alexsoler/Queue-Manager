using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Web.Areas.Identity.ViewModels;

namespace Web.Areas.Identity.Pages.Account
{
    [Authorize(Roles = "Administrador")]
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public IndexModel(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public UsuariosViewModel UsuariosRoles { get; private set; } = new UsuariosViewModel();


        public async Task OnGetAsync()
        {
            await LoadUsuarios();
        }

        public async Task<PartialViewResult> OnGetViewPartialTable()
        {
            await LoadUsuarios();

            return PartialView("Account/_TablaUsuarios", UsuariosRoles);
        }
 

        [NonAction]
        private PartialViewResult PartialView(string nombre, object model)
        {
            return new PartialViewResult
            {
                ViewName = nombre,
                ViewData = new ViewDataDictionary<object>(ViewData, model),
                TempData = this.TempData
            };
        }

        [NonAction]
        private async Task LoadUsuarios()
        {
            UsuariosRoles.Usuarios = await _userManager.Users.ToListAsync();

            foreach (var user in _userManager.Users)
            {
                string userRoles = string.Empty;

                foreach (var rol in await _userManager.GetRolesAsync(user))
                {
                    userRoles = (string.IsNullOrEmpty(userRoles)) ?
                        rol : userRoles + " - " + rol;
                }

                UsuariosRoles.Roles.Add(userRoles);
            }
        }
    }
}
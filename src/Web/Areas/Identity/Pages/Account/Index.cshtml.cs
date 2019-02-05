using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.QueueManager.Infrastructure.Identity;
using Web.Areas.Identity.ViewModels;

namespace Web.Areas.Identity.Pages.Account
{
    [Authorize(Roles = "Administrador")]
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public UsuarioViewModel UsuariosRoles { get; private set; } = new UsuarioViewModel();

        public async Task OnGetAsync()
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

        public PartialViewResult OnGetViewPartialTable()
        {
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
    }
}
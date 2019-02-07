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
        private readonly RoleManager<IdentityRole> _roleManager;

        public IndexModel(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public UsuariosViewModel UsuariosRoles { get; private set; } = new UsuariosViewModel();

        public EditarUsuarioViewModel EditUser { get; set; }

        public async Task OnGetAsync()
        {
            await LoadUsuarios();
        }

        public async Task<PartialViewResult> OnGetViewPartialTable()
        {
            await LoadUsuarios();

            return PartialView("Account/_TablaUsuarios", UsuariosRoles);
        }

        public async Task<IActionResult> OnGetViewPartialDetailsAsync(string username)
        {
            if (string.IsNullOrEmpty(username))
                return NotFound();

            var userDetails = await _userManager.FindByNameAsync(username);
            var roles = await _userManager.GetRolesAsync(userDetails);

            DetallesUsuarioViewModel detalles = new DetallesUsuarioViewModel
            {
                Nombre = userDetails.Name,
                Usuario = userDetails.UserName,
                Email = userDetails.Email,
                Telefono = userDetails.PhoneNumber,
                Roles = roles
            };

            return PartialView("Account/_DetallesUsuarios", detalles);
        }

        public async Task<IActionResult> OnGetViewPartialEditAsync(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                return NotFound();

           // ModelState.Clear();

            var user = await _userManager.FindByNameAsync(userName);

            EditUser = new EditarUsuarioViewModel
            {
                Name = user.Name,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Roles = new List<RolesAsignadosData>()
            };

            foreach (var rol in _roleManager.Roles)
            {
                if (await _userManager.IsInRoleAsync(user, rol.Name))
                    EditUser.Roles.Add(new RolesAsignadosData { Asignado = true, Rol = rol.Name });
                else
                    EditUser.Roles.Add(new RolesAsignadosData { Asignado = false, Rol = rol.Name });
            }

            return PartialView("Account/_EditarUsuario", EditUser);
        }

        public async Task<IActionResult> OnPostViewPartialEditAsync([FromBody]EditarUsuarioViewModel EditUser)
        {
            var user = await _userManager.FindByNameAsync(EditUser.UserName);

            if(user == null)
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

            if (user.Name != EditUser.Name)
            {
                user.Name = EditUser.Name;
                await _userManager.UpdateAsync(user);
            }

            if (user.Email != EditUser.Email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, EditUser.Email);
                if (!setEmailResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting email for user with ID '{userId}'.");
                }
            }

            if (user.PhoneNumber != EditUser.PhoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, EditUser.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting phone number for user with ID '{userId}'.");
                }
            }

            if (!string.IsNullOrWhiteSpace(EditUser.NewPassword))
            {
                var removePasswordResult = await _userManager.RemovePasswordAsync(user);
                if(!removePasswordResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting password for user with ID '{userId}'.");
                }

                var addPasswordResult = await _userManager.AddPasswordAsync(user, EditUser.NewPassword);
                if (!addPasswordResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting new password for user with ID '{userId}'.");
                }
            }

            foreach (var item in EditUser.Roles)
            {
                if(await _userManager.IsInRoleAsync(user, item.Rol))
                {
                    if (!item.Asignado)
                        await _userManager.RemoveFromRoleAsync(user, item.Rol);
                }
                else
                {
                    if (item.Asignado)
                        await _userManager.AddToRoleAsync(user, item.Rol);
                }
            }

            return new JsonResult("Usuario editado con exito");

        }

        public IActionResult OnGetViewPartialDelete(string username)
        {
            return PartialView("_EliminarUsuario", username);
        }

        public async Task<IActionResult> OnPostViewPartialDeleteAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

            var deleteUserResult = await _userManager.DeleteAsync(user);
            if (!deleteUserResult.Succeeded)
                throw new InvalidOperationException($"Unexpected error occurred deleting user with UserName '{username}'.");

            return new JsonResult("Usuario eliminado con exito");

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
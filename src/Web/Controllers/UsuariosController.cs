using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore;
using ApplicationCore.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Areas.Identity.ViewModels;

namespace Web.Controllers
{
    [Authorize(Roles = RolesStatic.Admin)]
    [Route("Usuarios")]
    public class UsuariosController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsuariosController(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }


        [HttpGet]
        [Route("Details")]
        public async Task<IActionResult> ViewPartialDetailsAsync(string username)
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
            
            return PartialView("_DetallesUsuarios", detalles);
        }

        [HttpGet]
        [Route("Edit")]
        public async Task<IActionResult> ViewPartialEditAsync(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                return NotFound();

            // ModelState.Clear();

            var user = await _userManager.FindByNameAsync(userName);

            EditarUsuarioViewModel EditUser = new EditarUsuarioViewModel
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

            return PartialView("_EditarUsuario", EditUser);
        }

        [HttpPost]
        [Route("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ViewPartialEditAsync([FromBody]EditarUsuarioViewModel EditUser)
        {
            var user = await _userManager.FindByNameAsync(EditUser.UserName);

            if (user == null)
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
                if (!removePasswordResult.Succeeded)
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
                if (await _userManager.IsInRoleAsync(user, item.Rol))
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

        [HttpGet]
        [Route("Delete")]
        public IActionResult ViewPartialDelete(string username)
        {
            return PartialView("_EliminarUsuario", username);
        }

        [HttpPost]
        [Route("Delete")]
        [ValidateAntiForgeryToken]
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

        [HttpGet]
        [Route("Search")]
        public async Task<IActionResult> SearchAsync(string currentSearch, string typeResult)
        {
            var users = from u in _userManager.Users
                        select u;

            if (!string.IsNullOrWhiteSpace(currentSearch))
                users = users.Where(x => x.Name.Contains(currentSearch, StringComparison.CurrentCultureIgnoreCase) ||
                                         x.UserName.Contains(currentSearch, StringComparison.CurrentCultureIgnoreCase) ||
                                         x.Email.Contains(currentSearch, StringComparison.OrdinalIgnoreCase));

            UsuariosViewModel UsersWithRoles = new UsuariosViewModel()
            {
                Usuarios = await users.ToListAsync()
            };

            foreach (var user in users)
            {
                string userRoles = string.Empty;

                foreach (var rol in await _userManager.GetRolesAsync(user))
                {
                    userRoles = (string.IsNullOrEmpty(userRoles)) ?
                        rol : userRoles + " - " + rol;
                }

                UsersWithRoles.Roles.Add(userRoles);
            }

            ViewData["typeResult"] = typeResult;

            return PartialView("_ResultadoSearch", UsersWithRoles);
        }
    }
}
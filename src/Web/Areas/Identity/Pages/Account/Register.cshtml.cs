using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.QueueManager.Infrastructure.Identity;
using Web.Areas.Identity.ViewModels;

namespace Web.Areas.Identity.Pages.Account
{
    [Authorize(Roles = "Administrador")]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public List<SelectListItem> Roles { get; private set; } = new List<SelectListItem>();

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "El campo {0} es requerido.")]
            [StringLength(100, ErrorMessage = "El campo {0} debe tener una longitud de {1} a {2} caracteres", MinimumLength = 4)]
            [Display(Name = "Nombre")]
            public string Name { get; set; }

            [Required(ErrorMessage = "El campo {0} es requerido.")]
            [StringLength(100, ErrorMessage = "El {0} debe tener longitud de {1} a {2} caracteres.", MinimumLength = 4)]
            [Display(Name = "Usuario")]
            public string UserName { get; set; }

            [Required(ErrorMessage = "El campo {0} es requerido.")]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Phone(ErrorMessage = "El formato del numero telefonico no es el correcto")]
            [Display(Name = "Telefono")]
            public string PhoneNumber { get; set; }

            [Required(ErrorMessage = "El campo {0} es requerido.")]
            [StringLength(100, ErrorMessage = "El {0} debe tener longitud de {1} a {2} caracteres", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Contraseña")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirmar")]
            [Compare("Password", ErrorMessage = "La contraseña y confirmar contraseña no coinciden.")]
            public string ConfirmPassword { get; set; }

            [Required(ErrorMessage = "El campo {0} es requerido.")]
            [Display(Name = "Rol")]
            public List<RolesAsignadosData> Roles { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            Input = new InputModel() {
                Roles = new List<RolesAsignadosData>()
            };
            foreach(var rol in _roleManager.Roles)
            {
                Input.Roles.Add(new RolesAsignadosData { Rol = rol.Name, Asignado = false });
            }
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = Input.UserName, Email = Input.Email, PhoneNumber = Input.PhoneNumber, Name = Input.Name };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId = user.Id, code = code },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    foreach (var rol in Input.Roles)
                    {
                        if(rol.Asignado)
                            await _userManager.AddToRoleAsync(user, rol.Rol);
                    }

                    return LocalRedirect(Url.Page("index"));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}

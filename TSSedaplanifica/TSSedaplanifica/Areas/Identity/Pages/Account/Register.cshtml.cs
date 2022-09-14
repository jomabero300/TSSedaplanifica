// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using TSSedaplanifica.Common;
using TSSedaplanifica.Data.Entities;
using TSSedaplanifica.Helpers.Gene;

namespace TSSedaplanifica.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IMailHelper _mailHelper;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IMailHelper mailHelper)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _mailHelper = mailHelper;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            [Display(Name = "Correo electrónico")]
            [Required(ErrorMessage = "El campo {0} es obligatorio.")]
            [EmailAddress(ErrorMessage = "El campo de correo electrónico no es una dirección válida")]
            public string Email { get; set; }

            [Display(Name = "Documento")]
            [MaxLength(20, ErrorMessage = "El campo {0} debe tener un máximo de {1} caractéres.")]
            [Required(ErrorMessage = "El campo {0} es obligatorio.")]
            public string Document { get; set; }

            [Display(Name = "Nombres")]
            [MaxLength(50, ErrorMessage = "El campo {0} debe tener un máximo de {1} caractéres.")]
            [Required(ErrorMessage = "El campo {0} es obligatorio.")]
            public string FirstName { get; set; }

            [Display(Name = "Apellidos")]
            [MaxLength(50, ErrorMessage = "El {0} debe tener un máximo de {1} caractéres.")]
            [Required(ErrorMessage = "El campo {0} es obligatorio.")]
            public string LastName { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "El {0} debe tener al menos {2} y un máximo de {1} caractéres.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Contraseña")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirmar contraseña")]
            [Compare("Password", ErrorMessage = "La contraseña y la contraseña de confirmación no coinciden.")]
            public string ConfirmPassword { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = CreateUser();

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

                user.Document = Input.Document;

                user.FirstName = lLowr(Input.FirstName);

                user.LastName = lLowr(Input.LastName);

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    Response response = _mailHelper.SendMail(
                                        user.Email,
                                        "Sedaplanifica - Confirmación de cuenta",
                                        $"<h1>Sedaplanifica - Confirmación de cuenta</h1>" +
                                        $"Para habilitar el usuario, " +
                                        $"por favor hacer clic en el siguiente enlace: <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>Confirmar Email</a>.");

                    
                    return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });

                }

                foreach (var error in result.Errors)
                {
                    string lMensaje = string.Empty;

                    if (error.Description.Contains("alphanumeric character"))
                    {
                        lMensaje = "Las contraseñas deben contener al menos un caracter numérico.";
                    }
                    else if (error.Description.Contains("lowercase ('a'-'z')"))
                    {
                        lMensaje = "Las contraseñas deben contener al menos una letra minúscula ('a'-'z').";
                    }
                    else if (error.Description.Contains("uppercase ('A'-'Z')"))
                    {
                        lMensaje = "Las contraseñas deben contener al menos una letra mayúsculas ('A'-'Z').";
                    }
                    else
                    {
                        lMensaje = "revise la conformación de las contraseñas";
                    }

                    ModelState.AddModelError(string.Empty, lMensaje);
                }

            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }

        private string lLowr(string cadena)
        {
            if (cadena.Length > 0)
            {
                var letra = cadena.Substring(0, 1).ToUpper();

                cadena = letra + cadena.Substring(1, cadena.Length - 1);
            }

            return cadena;
        }

    }
}

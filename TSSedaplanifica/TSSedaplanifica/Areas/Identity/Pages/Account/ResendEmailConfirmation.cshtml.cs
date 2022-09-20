// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authorization;
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
    [AllowAnonymous]
    public class ResendEmailConfirmationModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMailHelper _mailHelper;

        public ResendEmailConfirmationModel(UserManager<ApplicationUser> userManager, IMailHelper mailHelper)
        {
            _userManager = userManager;
            _mailHelper = mailHelper;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>

            [Display(Name = "Correo electrónico")]
            [Required(ErrorMessage = "El campo {0} es obligatorio.")]
            [EmailAddress(ErrorMessage = "El campo de correo electrónico no es una dirección válida")]

            public string Email { get; set; }
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Verifique el correo electronico a enviar. Por favor revise su correo electrónico.");
                return Page();
            }

            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId = userId, code = code },
                protocol: Request.Scheme);
            Response response= _mailHelper.SendMail(
                    Input.Email,
                    "Sedaplanifica - confirme su email",
                    $"<h1>Sedaplanifica - Confirmación de cuenta</h1>" +
                    $"Para habilitar el usuario, " +
                    $"Por favor confirme su cuenta por <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>haciendo clic aquí</a>.");

            ModelState.AddModelError(string.Empty, "El mensaje de verificación se envió. Por favor revise su correo electrónico. También la carpeta de Spam o Correo no deseado (Haga clic en este sitio Parece seguro)");
            return Page();
        }
    }
}

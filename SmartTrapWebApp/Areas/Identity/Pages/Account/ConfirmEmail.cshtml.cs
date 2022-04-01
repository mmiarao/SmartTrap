using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using SmartTrapWebApp.Models;

namespace SmartTrapWebApp.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ConfirmEmailModel(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"ユーザーが見つかりません({userId})");
            }
            //code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            StatusMessage = result.Succeeded ? "承認が正常に完了しました" : "承認エラー";
            if (user.Email?.Contains(@"@metamedia.co.jp") == true)
            {
                var role = new IdentityRole(Helpers.Constants.Role.SystemAdmin);
                var claim = new Claim(ClaimTypes.Role, role.Name);
#if false
                try
                {
                    await _roleManager.RemoveClaimAsync(role, claim);
                }
                catch { }
                try
                {
                    await _roleManager.DeleteAsync(role);
                }
                catch { }
                try
                {
                    await _roleManager.CreateAsync(role);
                }
                catch { }
                try
                {
                    await _roleManager.AddClaimAsync(role, claim);
                }
                catch { }

#endif
                if (!await _roleManager.RoleExistsAsync(Helpers.Constants.Role.SystemAdmin))
                {
                    await _roleManager.CreateAsync(role);
                    await _roleManager.AddClaimAsync(role, claim);
                }

                await _userManager.AddToRoleAsync(user, Helpers.Constants.Role.SystemAdmin);
            }
            var userRole = new IdentityRole(Helpers.Constants.Role.User);
            var userRoleClaim = new Claim(ClaimTypes.Role, userRole.Name);
            if (!await _roleManager.RoleExistsAsync(Helpers.Constants.Role.User))
            {
                await _roleManager.CreateAsync(userRole);
                await _roleManager.AddClaimAsync(userRole, userRoleClaim);
            }
            await _userManager.AddToRoleAsync(user, Helpers.Constants.Role.User);
            return Page();
        }
    }
}

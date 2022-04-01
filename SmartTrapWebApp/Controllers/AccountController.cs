using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartTrapWebApp.Models;

namespace SmartTrapWebApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Base.Controller
    {
        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<AccountController> logger
            )
        {
            UserManager = userManager;
            SignInManager = signInManager;
            Logger = logger;
        }

        UserManager<ApplicationUser> UserManager { get; }
        SignInManager<ApplicationUser> SignInManager { get; }

        internal override ILogger Logger { set; get; }

        [HttpPost]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePasswordAsync(ChangePasswordInput input)
        {
            InfoLog("パスワード変更要求");
            if (!ModelState.IsValid)
            {
                WarnLog($"パラメタ異常：{ModelState.ToString()}");
                return BadRequest();
            }
            
            //var user = await _userManager.GetUserAsync(User);
            foreach(var c in User.Claims)
            {
                DebugLog(c.Type);
            }
            var user = await UserManager.FindByIdAsync(User.Claims?.FirstOrDefault(x => x.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            if (user == null)
            {
                WarnLog($"ユーザー情報の取得に失敗しました(ID:{UserManager.GetUserId(User)})");
                return NotFound($"ユーザー情報の取得に失敗しました(ID:{UserManager.GetUserId(User)})");
            }

            var changePasswordResult = await UserManager.ChangePasswordAsync(user, input.OldPassword, input.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                WarnLog($"パスワード変更処理異常:{ModelState.ToString()}");
                return Problem(ModelState.ToString());
            }

            await SignInManager.RefreshSignInAsync(user);
            InfoLog("パスワード変更完了");
            return Ok();

        }
        [HttpPost]
        [Route("DeleteUser")]
        public async Task<IActionResult> DeleteUserAsync()
        {
            InfoLog("ユーザー削除要求");
            if (!ModelState.IsValid)
            {
                WarnLog($"パラメタ異常：{ModelState.ToString()}");
                return BadRequest();
            }

            var user = await UserManager.FindByIdAsync(User.Claims?.FirstOrDefault(x => x.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            if (user == null)
            {
                WarnLog($"ユーザー情報の取得に失敗しました(ID:{UserManager.GetUserId(User)})");
                return NotFound($"ユーザー情報の取得に失敗しました(ID:{UserManager.GetUserId(User)})");
            }


            var r = await UserManager.DeleteAsync(user);
            if (r.Succeeded)
            {
                await SignInManager.RefreshSignInAsync(user);
                InfoLog("ユーザー削除完了");
                return Redirect("/");
            }
            return Problem("ユーザー削除失敗");
        }

        public class ChangePasswordInput
        {
            public string OldPassword { set; get; }
            public string NewPassword { set; get; }
        }
    }
}
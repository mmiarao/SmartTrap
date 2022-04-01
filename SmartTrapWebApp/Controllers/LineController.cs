using LineWebApiClient.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.CodeAnalysis.Options;
using SmartTrapWebApp.Models.Configurations;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace SmartTrapWebApp.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class LineController : Base.Controller
    {
        public LineController(
            IOptions<ApiKeys> apiKeys, 
            IOptions<Constants> constants, 
            IEmailSender emailSender,
            ILogger<LineController> logger
            )
        {
            ApiKeys = apiKeys.Value;
            Constants = constants.Value;
            EmailSender = emailSender;
            Logger = logger;
        }
        ApiKeys ApiKeys { get; }
        Constants Constants { get; }

        IEmailSender EmailSender { get; }
        internal override ILogger Logger { set; get; }

        [HttpGet]
        public ActionResult Get()
        {
            DebugLog("LineController GET呼び出し");
            return Ok(new
            {
                Result = "OK(Test)"
            });
        }

        [HttpGet]
        [Route("Request/Register")]
        public ActionResult RedirectToLineCenter([FromQuery]LineRequest req)
        {
            var secret = Helper.Secret(req.Id);
            if(req?.Token?.Equals(secret) != true)
            {
                return new ViewResult()
                {
                    ViewName = "_UnAuthorized"
                };
                //return Unauthorized(new ViewResult()
                //{
                //    ViewName = "_UnAuthorized"
                //});
            }
            var url = string.Format("https://access.line.me/oauth2/v2.1/authorize?response_type=code&client_id={0}&redirect_uri=https%3A%2F%2Fsmarttrapwebapi.azurewebsites.net%2Fapi%2FAuth?id={1}&state={2}&scope=openid", ApiKeys.LineLogin.Id, req.Id, req.Token);
            return Redirect(url);
        }


        [HttpPost]
        public ActionResult Post(WebhookEventNotification notification)
        {
            try
            {
                InfoLog("LINE Webhookイベント受信");
                notification.Events.ForEach(x =>
                {
                    DebugLog(string.Format("User ID = {0}", x.Source.Id));
                    
                });
                return Ok();
            }
            catch (Exception ex)
            {
                ErrorLog(ex.ToString());
                return Problem(ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("Request")]
        public async Task<ActionResult> RequestAuthEmail([FromBody]LineRequest req)
        {
#if DEBUG
            DebugLog("Email送信先({0})をデバッグ用に書き換えます", req.Email);
            req.Email = "arao@metamedia.co.jp";
#endif
            var secret = Helper.Secret(req.Id);
            var url = string.Format("https://access.line.me/oauth2/v2.1/authorize?response_type=code&client_id={0}&redirect_uri=https%3A%2F%2Fsmarttrapwebapi.azurewebsites.net%2Fapi%2FAuth?id={1}&state={2}&scope=openid", ApiKeys.LineLogin.Id, req.Id, secret);
            url = string.Format("{0}://{1}{2}/Register?id={3}&token={4}", Request.Scheme, Request.Host, Request.Path, req.Id, secret);
            string title = string.Format("{0} LINE通知登録", Constants.SystemNameJp);
            var body =
            $@"このメールは{Constants.SystemNameJp} からのLINEアカウント連携リクエストメールです

状態変動時の通知をLINEで受信する場合は以下のURLをクリックしてください
{url}

LINEによるメッセージ受信には以下の条件が必要です。

1.LINEを現在利用している(使用を開始する)
2.{Constants.SystemNameJp}公式アカウントをお友達に追加している

LINEは以下のURLから無料でインストール可能です

Google Play(Android)
https://play.google.com/store/apps/details?id=jp.naver.line.android

App Store(iPhone/ iPad)
https://apps.apple.com/jp/app/line/id443904275


本メールに心当たりのない方はお手数ですが以下のアドレスまでご連絡願います。
mailto: admin @metamedia.co.jp

------------------------------------------------

{Constants.SystemNameJp}事務局

------------------------------------------------
";
            await EmailSender.SendEmailAsync(req.Email, title, body);
            return Ok();
        }

        public class LineRequest
        {
            [JsonProperty("id")]
            public string Id { set; get; }
            [JsonProperty("email")]
            public string Email { set; get; }
            [JsonProperty("token")]
            public string Token { set; get; }

        }
    }
}

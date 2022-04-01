using LineWebApiClient.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using SmartTrapWebApp.Models.Configurations;
using Microsoft.Extensions.Options;
using SmartTrapWebApp.Services.Tables;

namespace SmartTrapWebApp.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Base.Controller
    {
        public AuthController(
            IOptions<ApiKeys> apiKeys,
            MemberTableClient memberTable,
            ILogger<AuthController> logger
            )
        {
            ApiKeys = apiKeys.Value;
            Logger = logger;
            _memberTable = memberTable;
        }

        internal override ILogger Logger { set; get; }

        private MemberTableClient _memberTable;

        ApiKeys ApiKeys { get; }

        string ChannelId
        {
            get
            {
                return ApiKeys.LineLogin.Id;
            }
        }
        string ChannelSecret
        {
            get
            {
                return ApiKeys.LineLogin.Key;
            }
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery]string id, [FromQuery]string code, [FromQuery]string state)
        {
            var redirectUrl = string.Format("{0}://{1}{2}?id={3}", Request.Scheme, Request.Host, Request.Path, id);
            HttpClient client = new HttpClient();
            var r = await client.PostAsync("https://api.line.me/oauth2/v2.1/token", new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("code", code),
                new KeyValuePair<string, string>("client_id", ChannelId),
                new KeyValuePair<string, string>("client_secret", ChannelSecret),
                new KeyValuePair<string, string>("redirect_uri", redirectUrl),
            }));
            try
            {
                r.EnsureSuccessStatusCode();
                var res = await r.Content.ReadAsStringAsync();
                var token = JsonConvert.DeserializeObject<LoginAccessToken>(res);
                //DebugLog(token.AccessToken);
                //DebugLog(token.IdToken);
                //DebugLog(token.UserInfo(ChannelSecret).UserId);
                var lineId = token.UserInfo(ChannelSecret).UserId;
                InfoLog($"{id} = {lineId}");

                var member = await _memberTable.GetMember(id);
                if(member == null)
                {
                    return NotFound();
                }
                member.LineId = lineId;
                member.UseLine = true;

                await _memberTable.UpdateMember(member);

                return new ViewResult()
                {
                    ViewName = "_LineRegistered"
                };
            }
            catch(HttpRequestException ex)
            {
                WarnLog($"LINE ユーザーID取得エラー:{ex.Message}");
            }
            catch(Exception ex)
            {
                ErrorLog(ex.ToString());
            }
            return BadRequest();

        }

    }
}

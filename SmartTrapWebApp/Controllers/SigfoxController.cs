using MessageSendLib;
using SmartTrapWebApp.Models.Sigfox;
using SoracomWebApiClient.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.Options;
using SmartTrapWebApp.Models.Configurations;
using Microsoft.AspNetCore.Identity.UI.Services;
using SmartTrapWebApp.Services.Tables;
using SmartTrapWebApp.Models.Member;

namespace SmartTrapWebApp.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class SigfoxController : Base.Controller
    {
        const string SIGNATURE_HEADER = "x-soracom-signature";
        const string TIMESTAMP_HEADER = "x-soracom-timestamp";
        const string SIGFOX_DEVICE_ID_HEADER = "x-soracom-sigfox-device-id";
        const string LINE_TEST_ID = "Ufcc239675ad6dae3c9cfe954a2341dde";

        public SigfoxController(
            IOptions<ApiKeys> apiKeys, 
            IOptions<Secrets> secrets,
            MemberTableClient memberTable,
            SensorTableClient sensorTable,
            SensorMemberTableClient sensorMemberTable,
            IEmailSender emailSender,
            ILogger<SigfoxController> logger)
        {
            ApiKeys = apiKeys.Value;
            Secrets = secrets.Value;
            Logger = logger;
            _emailSender = emailSender;
            _memberTable = memberTable;
            _sensorTable = sensorTable;
            _sensorMemberTable = sensorMemberTable;
        }
        ApiKeys ApiKeys { get; }
        Secrets Secrets { get; }
        internal override ILogger Logger { set; get; }

        private IEmailSender _emailSender;
        private MemberTableClient _memberTable;
        private SensorTableClient _sensorTable;
        private SensorMemberTableClient _sensorMemberTable;

        LineWebApiClient.Api lineApi;
        LineWebApiClient.Apis.MessagingApi LineApi
        {
            get
            {
                if(lineApi == null)
                {
                    lineApi = new LineWebApiClient.Api(
                        ApiKeys.LineMessaging.Id,
                        ApiKeys.LineMessaging.Key
                        );
                }
                return lineApi.Messaging;
            }
        }
        string SoracomBeamSecret
        {
            get
            {
                return Secrets.SoracomBeam;
            }
        }

        [HttpGet]
        // GET api/values
        public ActionResult Get()
        {

            DebugLog("Sigfox Get");
            return Ok();
        }

        [HttpPost]
        public ActionResult Post(SmartTrapCommand data)
        {
            try
            {
                CheckSignature();
            }
            catch
            {
                return Unauthorized();
            }
            try
            {
                InfoLog($"Sigfoxデータ受信 ID = {data.DeviceId}, CommandID = {data.Command.CommandId}");
                DebugLog($"Sigfoxデータ詳細 : {data.RawData}({data.SeqNumber})");
                bool notify = true;
                bool trapped;
                string title, message;
                switch (data.Command.CommandId)
                {
                    case SigfoxCommand.CommandType.Init:
                        title = string.Format("SmartTrap端末{0}起動", data.DeviceId);
                        message = string.Format("スマートトラップ端末{0}が起動しました({1})", data.DeviceId, data.Timestamp);
                        Task.Run(async () =>
                        {
                            InfoLog(title);
                            var sensor = await _sensorTable.GetSensor(data.DeviceId);
                            if (sensor != null)
                            {
                                sensor.Status = "起動";
                                sensor.Started = DateTimeOffset.Now;
                                sensor.Updated = DateTimeOffset.Now;
                                await _sensorTable.UpdateSensor(sensor);
                            }

                        });
                        break;

                    case SigfoxCommand.CommandType.WatchDog:
                        notify = false;
                        trapped = data.Command.Data.First() == 1;
                        title = string.Format("SmartTrap端末{0}生存通知", data.DeviceId);
                        message = string.Format("スマートトラップ端末{0}生存通知({1})", data.DeviceId, data.Timestamp);
                        Task.Run(async () =>
                        {
                            InfoLog(title);
                            var sensor = await _sensorTable.GetSensor(data.DeviceId);
                            if (sensor != null)
                            {
                                sensor.Status = "正常";
                                sensor.Trapped = trapped;
                                sensor.Updated = DateTimeOffset.Now;
                                await _sensorTable.UpdateSensor(sensor);
                            }

                        });
                        break;

                    case SigfoxCommand.CommandType.NotifyTrap:
                        trapped = data.Command.Data.First() == 1;
                        if(trapped)
                        {
                            title = string.Format("SmartTrap端末{0}動作通知", data.DeviceId);
                            message = string.Format("スマートトラップ端末{0}が動作しました({1})", data.DeviceId, data.Timestamp);
                        }
                        else
                        {
                            title = string.Format("SmartTrap端末{0}復旧通知", data.DeviceId);
                            message = string.Format("スマートトラップ端末{0}が復旧しました({1})", data.DeviceId, data.Timestamp);
                        }
                        Task.Run(async () =>
                        {
                            InfoLog(title);
                            var sensor = await _sensorTable.GetSensor(data.DeviceId);
                            if (sensor != null)
                            {
                                sensor.Trapped = trapped;
                                sensor.Updated = DateTimeOffset.Now;
                                await _sensorTable.UpdateSensor(sensor);
                            }
                        });
                        break;

                    default:
                        title = string.Format("SmartTrap端末{0}不明な通信", data.DeviceId);
                        message = string.Format("スマートトラップ端末{0}の不明な通信({1})", data.DeviceId, data.Timestamp);
                        break;
                }
                if (notify)
                {
                    Task.Run(async () =>
                    {
                        InfoLog("Sigfoxコマンドを通知します");
                        try
                        {
                            List<string> memberIds = new List<string>();
                            var tableResult =  await _sensorMemberTable.GetMembers(data.DeviceId, 999);
                            memberIds.AddRange(tableResult.Results);
                            while(tableResult.ContinuationToken != null)
                            {
                                tableResult = await _sensorMemberTable.GetMembers(data.DeviceId, 999, tableResult.ContinuationToken);
                                memberIds.AddRange(tableResult.Results);
                            }
                            List<MemberEntity> members = new List<MemberEntity>();
                            memberIds.ForEach(async x =>
                            {
                                var member = await _memberTable.GetMember(x);
                                if (member?.UseEmail == true)
                                {
                                    try
                                    {
                                        InfoLog("Eメール通知開始");
                                        await _emailSender.SendEmailAsync(member.Email, title, message);
                                    }
                                    catch (Exception ex)
                                    {
                                        ErrorLog($"Eメール送信失敗：{ex.Message}");
                                        DebugLog($"詳細：{ex.ToString()}");
                                    }
                                }
                                if (member?.UseLine == true)
                                {
                                    try
                                    {
                                        InfoLog($"LINE通知開始({member.LineId})");
                                        await LineApi.SendMessage(new List<string>
                                        {
                                            member.LineId
                                        }, message);
                                    }
                                    catch (Exception ex)
                                    {
                                        ErrorLog($"LINE送信失敗：{ex.Message}");
                                        DebugLog($"詳細：{ex.ToString()}");
                                    }
                                }
                            });
                            //await _emailSender.SendEmailAsync("arao@metamedia.co.jp", title, message);
                            //await LineApi.SendMessage(new List<string>
                            //{
                            //    LINE_TEST_ID
                            //}, message);
                        }
                        catch (Exception ex)
                        {
                            ErrorLog($"Sigfoxコマンド通知失敗：{ex.Message}");
                            DebugLog($"詳細：{ex.ToString()}");
                        }
                    });
                }

                return Ok();
            }
            catch(Exception ex)
            {
                ErrorLog(ex.ToString());
                return Problem(ex.Message);
            }
        }

        void CheckSignature()
        {
            StringBuilder sb = new StringBuilder();
            DebugLog("Request Headers");
            foreach (var h in Request.Headers)
            {
                string comma = "";
                sb.Clear();
                sb.Append(h.Key);
                sb.Append(" : ");
                foreach (var v in h.Value)
                {
                    sb.Append(comma);
                    sb.Append(v);
                    comma = ",";
                }
                DebugLog(sb.ToString());
            }
            var toSign = string.Format("{0}x-soracom-sigfox-device-id={1}x-soracom-timestamp={2}", SoracomBeamSecret, SigfoxDeviceId, Timestamp);
            DebugLog(string.Format("To Signe : {0}", toSign));
            var sign = CalcSHA256(toSign);
            DebugLog(string.Format("Signature : {0}{1}Calc : {2}", Signature, Environment.NewLine, sign));
            if (sign?.Equals(Signature) != true)
            {
                WarnLog($"シグネチャ不一致({sign} : {Signature}");
                throw new Exception("Signature Auth Error");
            }
        }

        string CalcSHA256(string src)
        {
            byte[] input = Encoding.UTF8.GetBytes(src);
            SHA256 sha = new SHA256CryptoServiceProvider();
            byte[] hash_sha256 = sha.ComputeHash(input);

            string result = "";

            for (int i = 0; i < hash_sha256.Length; i++)
            {
                result = result + string.Format("{0:x2}", hash_sha256[i]);
            }
            return result;
        }

        string SigfoxDeviceId
        {
            get
            {
                StringValues values;
                if(Request.Headers.TryGetValue(SIGFOX_DEVICE_ID_HEADER, out values))
                {
                    return values.FirstOrDefault();
                }
                return null;
            }
        }
        string Timestamp
        {
            get
            {
                StringValues values;
                if(Request.Headers.TryGetValue(TIMESTAMP_HEADER, out values))
                {
                    return values.FirstOrDefault();
                }
                return null;
            }
        }
        string Signature
        {
            get
            {
                StringValues values;
                if(Request.Headers.TryGetValue(SIGNATURE_HEADER, out values))
                {
                    return values.FirstOrDefault();
                }
                return null;
            }
        }

    }
}

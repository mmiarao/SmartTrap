using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage.Table;
using SmartTrapWebApp.Services.Tables;
using SmartTrapWebApp.Models;
using SmartTrapWebApp.Models.Configurations;
using SmartTrapWebApp.Models.Member;

namespace SmartTrapWebApp.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class SensorMemberController : Base.Controller
    {
        public SensorMemberController(
            IOptions<Constants> constants,
            IOptions<Secrets> secrets,
            ILogger<MemberController> logger
            )
        {
            Logger = logger;
            Constants = constants.Value;
            Secrets = secrets.Value;
        }

        internal override ILogger Logger { set; get; }
        Constants Constants { get; }
        Secrets Secrets { get; }

        SensorMemberTableClient client;

        SensorMemberTableClient Client
        {
            get
            {
                if(client == null)
                {
                    client = new SensorMemberTableClient(Constants, Secrets);
                }
                return client;
            }
        }


        // GET: api/SensorMember/5
        [HttpGet("{sensorId}", Name = "Get")]
        public async Task<IActionResult> GetMembers(string sensorId, [FromQuery]int? count)
        {
            var result = new List<string>();
            var r = await Client.GetMembers(sensorId, count ?? 20, null);
            result.AddRange(r.Results);
            var token = r.ContinuationToken;
            while (token != null)
            {
                r = await Client.GetMembers(sensorId, count ?? 20, token);
                result.AddRange(r.Results);
                token = r.ContinuationToken;

            }
            return Ok(result);
        }

        // POST: api/SensorMember
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SensorMember sensorMember)
        {
            if (!ModelState.IsValid)
            {
                WarnLog($"パラメタ異常：{ModelState.ToString()}");
                return BadRequest();
            }
            try
            {
                await Client.AddSensorMember(sensorMember);
                return Ok();
            }
            catch(Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("Sensor/{sensorId}/Member/{memberId}")]
        public async Task<IActionResult> Delete(string sensorId, string memberId)
        {
            try
            {
                await Client.RemoveSensorMember(sensorId, memberId);
                return Ok();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}

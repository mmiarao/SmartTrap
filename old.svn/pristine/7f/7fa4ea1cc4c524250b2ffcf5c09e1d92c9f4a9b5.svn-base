using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : Base.Controller 
    {
        public MemberController(
            IOptions<Constants> constants,
            IOptions<Secrets> secrets,
            MemberTableClient memberTable,
            SensorMemberTableClient sensorMemberTable,
            ILogger<MemberController> logger
            )
        {
            Logger = logger;
            Constants = constants.Value;
            Secrets = secrets.Value;
            _memberTable = memberTable;
            _sensorMemberTable = sensorMemberTable;
        }

        internal override ILogger Logger { set; get; }
        Constants Constants { get; }
        Secrets Secrets { get; }

        private MemberTableClient _memberTable;
        private SensorMemberTableClient _sensorMemberTable;

        // GET: api/Member
        [HttpGet]
        public async Task<MembersResponse> Get([FromQuery]int? count = 20, [FromQuery]string rowKey = null)
        {
            var iCount = count ?? 20;
            var r = await _memberTable.GetMembers(iCount, new Microsoft.WindowsAzure.Storage.Table.TableContinuationToken()
            {
                TargetLocation = Microsoft.WindowsAzure.Storage.StorageLocation.Primary,
                NextTableName = _memberTable.Table.Name,
                NextPartitionKey = Constants.SystemNameEn,
                NextRowKey = rowKey
            });
            List<MemberModel> members = new List<MemberModel>();
            r.Results.ForEach(x =>
            {
                members.Add(MemberModel.Create(x.RowKey, x));
            });
            return new MembersResponse()
            {
                Members = members,
                NextKey = r?.ContinuationToken?.NextRowKey,
            };
        }

        // GET: api/Member/5
        [HttpGet("{id}")]
        public async Task<MemberEntity> Get(string id)
        {
            return await _memberTable.GetMember(id);
        }

        // POST: api/Member
        [HttpPost]
        public async Task<ActionResult<MemberModel>> Post([FromBody] MemberInput value)
        {
            if (!ModelState.IsValid)
            {
                WarnLog($"パラメタ異常：{ModelState.ToString()}");
                return BadRequest();
            }
            string id = Guid.NewGuid().ToString();
            while(await _memberTable.Any(id))
            {
                id = Guid.NewGuid().ToString();
            }
            var entity = MemberEntity.Create(Constants.SystemNameEn, id, value);
            var r = await _memberTable.AddMember(entity);
            if(r == null)
            {
                return null;
            }
            return MemberModel.Create(id, value);
        }

        [Authorize(Roles = "SystemAdministrator")]
        // PUT: api/Member/5
        [HttpPut("{id}")]
        public async Task<ActionResult<MemberModel>> Put(string id, [FromBody] MemberInput value)
        {
            if (!ModelState.IsValid)
            {
                WarnLog($"パラメタ異常：{ModelState.ToString()}");
                return BadRequest();
            }
            var entity = MemberEntity.Create(Constants.SystemNameEn, id, value);
            var r = await _memberTable.UpdateMember(entity);
            if (r == null)
            {
                return null;
            }
            return MemberModel.Create(id, value);

        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            await _memberTable.DeleteEntity(Constants.SystemNameEn, id);
            await _sensorMemberTable.DeleteAllMember(id);
        }


    }
}

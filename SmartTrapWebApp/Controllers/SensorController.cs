using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SmartTrapWebApp.Services.Tables;
using SmartTrapWebApp.Models.Configurations;
using SmartTrapWebApp.Models.Sensor;
using SoracomWebApiClient.Api;
using SmartTrapWebApp.Models;

namespace SmartTrapWebApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SensorController : Base.Controller
    {
        public SensorController(
            IOptions<Constants> constants,
            IOptions<Secrets> secrets,
            IOptions<ApiKeys> apiKeys,
            SensorTableClient sensorTable,
            SensorMemberTableClient sensorMemberTable,
            SoracomWebApiClient.Api.AuthApi soracomAuthApi,
            SoracomWebApiClient.Api.SigfoxDeviceApi sigfoxApi,
            ILogger<MemberController> logger
            )
        {
            Logger = logger;
            Constants = constants.Value;
            Secrets = secrets.Value;
            _apiKeys = apiKeys.Value;
            _sensorTable = sensorTable;
            _sensorMemberTable = sensorMemberTable;
            _soracomAuthApi = soracomAuthApi;
            _sigfoxApi = sigfoxApi;
        }

        internal override ILogger Logger { set; get; }
        Constants Constants { get; }
        Secrets Secrets { get; }

        private ApiKeys _apiKeys;
        private SensorTableClient _sensorTable;
        private SensorMemberTableClient _sensorMemberTable;
        private AuthApi _soracomAuthApi;
        private SigfoxDeviceApi _sigfoxApi;

        async Task CheckSoracomAuth()
        {
            if (!_soracomAuthApi.Authed)
            {
                await _soracomAuthApi.AuthByKey(_apiKeys.SoracomSam.Id, _apiKeys.SoracomSam.Key);
            }
        }

        // GET: api/Member
        [HttpGet]
        public async Task<SensorsResponse> Get([FromQuery]int? count = 20, [FromQuery]string rowKey = null)
        {
            var iCount = count ?? 20;
            var r = await _sensorTable.GetSensors(iCount, new Microsoft.WindowsAzure.Storage.Table.TableContinuationToken()
            {
                TargetLocation = Microsoft.WindowsAzure.Storage.StorageLocation.Primary,
                NextTableName = _sensorTable.Table.Name,
                NextPartitionKey = Constants.SystemNameEn,
                NextRowKey = rowKey
            });
            List<SensorModel> results = new List<SensorModel>();
            r.Results.ForEach(x =>
            {
                results.Add(SensorModel.Create(x));
            });
            return new SensorsResponse()
            {
                Sensors = results,
                NextKey = r?.ContinuationToken?.NextRowKey,
            };
        }

        // GET: api/Member/5
        [HttpGet("{id}")]
        public async Task<SensorEntity> Get(string id)
        {
            return await _sensorTable.GetSensor(id);
        }

        // POST: api/Member
        [HttpPost]
        public async Task<ActionResult<SensorModel>> Post([FromBody] SensorModel value)
        {
            if (!ModelState.IsValid)
            {
                WarnLog($"パラメタ異常：{ModelState.ToString()}");
                return BadRequest();
            }
            string secret = Guid.NewGuid().ToString();
            var entity = SensorEntity.Create(Constants.SystemNameEn, secret, value);
            var r = await _sensorTable.InsertSensor(entity);
            if (r == null)
            {
                return null;
            }
#if SORACOM_ENABLE
            var needRoleBack = true;
            try
            {
                await CheckSoracomAuth();
                var deviceId = new SoracomWebApiClient.Models.SigfoxDeviceId()
                {
                    DeviceIdStr = value.Id,
                };
                var device = await _sigfoxApi.RegisterSigfoxDevice(deviceId, secret, null);
                await _sigfoxApi.SetSigfoxDeviceGroup(deviceId, Constants.SigfoxGroupId);
                device = await _sigfoxApi.GetSigfoxDevice(deviceId);
                if(device.GroupId == Constants.SigfoxGroupId)
                {
                    needRoleBack = false;
                }
            }
            catch(Exception ex)
            {
                return Problem(ex.Message);
            }
            finally
            {
                if (needRoleBack)
                {
                    await _sensorTable.DeleteEntity(entity.PartitionKey, entity.RowKey);
                }
            }

#endif
            return SensorModel.Create(value);
        }

        // PUT: api/Member/5
        [HttpPut("{id}")]
        public async Task<ActionResult<SensorModel>> Put(string id, [FromBody] SensorModel value)
        {
            if (!ModelState.IsValid)
            {
                WarnLog($"パラメタ異常：{ModelState.ToString()}");
                return BadRequest();
            }
            var entity = await _sensorTable.GetSensor(id);
            entity.Update(value);
            
            var r = await _sensorTable.UpdateSensor(entity);
            if (r == null)
            {
                return null;
            }
            return SensorModel.Create(value);

        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            await _sensorTable.DeleteEntity(Constants.SystemNameEn, id);
            await _sensorMemberTable.DeleteAllSensor(id);
        }
    }
}

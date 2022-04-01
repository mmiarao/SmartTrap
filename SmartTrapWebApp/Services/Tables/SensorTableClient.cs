using Microsoft.WindowsAzure.Storage.Table;
using SmartTrapWebApp.Models.Configurations;
using SmartTrapWebApp.Models.Sensor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartTrapWebApp.Services.Tables
{
    public class SensorTableClient:Base.TableClientBase
    {
        public SensorTableClient(Constants constants, Secrets secrets) : base("Sensor", constants, secrets)
        {
        }
        public async Task<TableQuerySegment<SensorEntity>> GetSensors(int count, TableContinuationToken token = null)
        {
            return await this.GetEntities<SensorEntity>(Constants.SystemNameEn, count, token);
        }

        public async Task<SensorEntity> GetSensor(string sensorId)
        {
            return await this.GetEntity<SensorEntity>(Constants.SystemNameEn, sensorId);
        }

        public async Task<bool> Any(string sensorId)
        {
            return await GetSensor(sensorId) != null ? true : false;
        }

        public async Task<SensorEntity> InsertSensor(SensorEntity sensor)
        {
            var r = await this.InsertEntity(sensor);
            return r.Result as SensorEntity;
        }

        public async Task<SensorEntity> UpdateSensor(SensorEntity sensor)
        {
            var r = await this.InsertOrReplaceEntity(sensor);
            return r.Result as SensorEntity;
        }
    }
}

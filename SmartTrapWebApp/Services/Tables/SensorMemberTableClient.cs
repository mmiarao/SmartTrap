using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using SmartTrapWebApp.Models;
using SmartTrapWebApp.Models.Configurations;
using SmartTrapWebApp.Models.Member;

namespace SmartTrapWebApp.Services.Tables
{
    public class SensorMemberTableClient : Base.TableClientBase
    {
        public SensorMemberTableClient(Constants constants, Secrets secrets) : base("SensorMember", constants, secrets)
        {
        }
        public async Task<TableQuerySegment<string>> GetMembers(string sensorId, int count, TableContinuationToken token = null)
        {
            var partitionKey = SensorMemberEntity.CreatePartitionKey(this.Constants.SystemNameEn, sensorId);
            TableQuery q = new TableQuery().Where(
                TableQuery.GenerateFilterCondition("PartitionKey", AzureStorageLibPortal.Models.QueryComparisons.Equal, partitionKey)
            ).Take(count).Select(new List<string> { "RowKey" });
            EntityResolver<string> resolver = (pk, rk, ts, props, etag) => rk;
            var r = await Table.ExecuteQuerySegmentedAsync<string>(q, resolver, token);
            return r;
        }
        public async Task<TableResult> AddSensorMember(SensorMember sensorMember)
        {
            return await this.InsertOrReplaceEntity(SensorMemberEntity.Create(this.Constants.SystemNameEn, sensorMember.SensorId, sensorMember.MemberId));
        }

        public async Task<TableResult> RemoveSensorMember(string sensorId, string memberId)
        {
            var entity = SensorMemberEntity.Create(this.Constants.SystemNameEn, sensorId, memberId);
            return await this.DeleteEntity(entity.PartitionKey, entity.RowKey);
        }

        public async Task<IList<TableResult>> DeleteAllSensor(string sensorId)
        {
            return await this.DeletePartition(SensorMemberEntity.CreatePartitionKey(this.Constants.SystemNameEn, sensorId));
        }

        public async Task<IList<TableResult>> DeleteAllMember(string memberId)
        {
            List<ITableEntity> result = new List<ITableEntity>();
            TableQuery q = new TableQuery().Where(
                TableQuery.GenerateFilterCondition("RowKey", AzureStorageLibPortal.Models.QueryComparisons.Equal, memberId)
            );
            EntityResolver<ITableEntity> resolver = (pk, rk, ts, props, etag) =>
            {
                var resolvedEntity = new SensorMemberEntity();
                resolvedEntity.PartitionKey = pk;
                resolvedEntity.RowKey = rk;
                resolvedEntity.Timestamp = ts;
                resolvedEntity.ETag = etag;
                resolvedEntity.ReadEntity(props, null);

                return resolvedEntity;
            };
            var r = await Table.ExecuteQuerySegmentedAsync<ITableEntity>(q, resolver, null);
            result.AddRange(r.Results);
            var token = r.ContinuationToken;
            while(token != null)
            {
                r = await Table.ExecuteQuerySegmentedAsync<ITableEntity>(q, resolver, null);
                result.AddRange(r.Results);
                token = r.ContinuationToken;
            }
            return await DeleteEntities(result);
        }
    }
}

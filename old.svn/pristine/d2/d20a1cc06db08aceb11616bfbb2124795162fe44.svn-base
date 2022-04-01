using Microsoft.WindowsAzure.Storage.Table;
using SmartTrapWebApp.Models.Member;
using SmartTrapWebApp.Models.Sensor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartTrapWebApp.Models
{
    public interface ISensorMember
    {
        string SensorId { set; get; }
        string MemberId { set; get; }
    }

    public class SensorMemberEntity : TableEntity
    {
    
        const string PARTITION_KEY_SPLITTER = ".";

        [IgnoreProperty]
        public string SensorId
        {
            get
            {
                var strs = this.PartitionKey.Split(PARTITION_KEY_SPLITTER);
                return strs.ElementAtOrDefault(strs.Length - 1);
            }
        }
        [IgnoreProperty]
        public string MemberId
        {
            get
            {
                return RowKey;
            }
        }

        public static string CreatePartitionKey(string partitionKey, string sensorId)
        {
            return $"{partitionKey}{PARTITION_KEY_SPLITTER}{sensorId}";
        }

        public static SensorMemberEntity Create(string partitionKey, string sensorId, string memberId)
        {
            return new SensorMemberEntity()
            {
                PartitionKey = CreatePartitionKey(partitionKey, sensorId),
                RowKey = memberId,
            };
        }

        public SensorMember ToModel()
        {
            return new SensorMember()
            {
                SensorId = SensorId,
                MemberId = MemberId
            };
        }
    }
    public class SensorMember:ISensorMember
    {

        public string SensorId { set; get; }
        public string MemberId { set; get; }
    }
}

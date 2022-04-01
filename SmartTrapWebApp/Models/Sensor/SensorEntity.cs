using Microsoft.WindowsAzure.Storage.Table;
using SmartTrapWebApp.Models.Member;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartTrapWebApp.Models.Sensor
{
    public class SensorEntity : TableEntity, ISensor
    {
        [IgnoreProperty]
        public string Id 
        {
            set
            {
                this.RowKey = value;
            }
            get
            {
                return this.RowKey;
            }
        }

        public string Pac { set; get; }

        public string Name { set; get; }

        public bool Registered { set; get; }
        public bool Trapped { set; get; }

        public string Status { set; get; }

        public DateTimeOffset Started { set; get; }

        public DateTimeOffset Updated { set; get; }

        public string Secrets { set; get; }

        internal static SensorEntity Create(string partitionKey, string secrets, ISensor sensor)
        {
            return new SensorEntity()
            {
                PartitionKey = partitionKey,
                RowKey = sensor.Id,
                Name = sensor.Name,
                Pac = sensor.Pac,
                Registered = sensor.Registered,
                Trapped = sensor.Trapped,
                Status = sensor.Status,
                Secrets = secrets,
                Started = new DateTimeOffset(1970, 1, 1, 0, 0, 0, 0, TimeSpan.Zero),
                Updated = new DateTimeOffset(1970, 1, 1, 0, 0, 0, 0, TimeSpan.Zero),
            };
        }

        internal void Update(ISensor sensor)
        {
            this.Id = sensor.Id;
            this.Name = sensor.Name;
            this.Pac = sensor.Pac;
            this.Registered = sensor.Registered;
            this.Started = sensor.Started;
            this.Status = sensor.Status;
            this.Trapped = sensor.Trapped;
            this.Updated = sensor.Updated;
        }
    }
}

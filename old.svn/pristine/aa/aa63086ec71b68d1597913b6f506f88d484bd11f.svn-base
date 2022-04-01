using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartTrapWebApp.Models.Abstracts;
using SmartTrapWebApp.Models.Member;

namespace SmartTrapWebApp.Models.Sensor
{
    public class SensorsResponse : TableListResponseBase
    {
        public List<SensorModel> Sensors { set; get; }
    }
    public class SensorModel : ISensor
    {
        public string Id { set; get; }

        public string Pac { set; get; }

        public string Name { set; get; }

        public bool Registered { set; get; }
        public bool Trapped { set; get; }

        public string Status { set; get; }

        public DateTimeOffset Started { set; get; }

        public DateTimeOffset Updated { set; get; }


        internal static SensorModel Create(ISensor sensor)
        {
            return new SensorModel()
            {
                Id = sensor.Id,
                Pac = sensor.Pac,
                Name = sensor.Name,
                Registered = sensor.Registered,
                Status = sensor.Status,
                Trapped = sensor.Trapped,
                Started = sensor.Started,
                Updated = sensor.Updated,
            };
        }
    }
}

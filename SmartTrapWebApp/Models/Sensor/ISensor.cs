using SmartTrapWebApp.Models.Member;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartTrapWebApp.Models.Sensor
{
    public interface ISensor
    {
        string Id { set; get; }
        string Pac { set; get; }
        string Name { set; get; }
        bool Registered { set; get; }
        bool Trapped { set; get; }
        string Status { set; get; }
        DateTimeOffset Started { set; get; }
        DateTimeOffset Updated { set; get; }
    }
}

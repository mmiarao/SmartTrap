using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SmartTrapClientSimulator.Models
{
    public class SoracomBeamData
    {
        [JsonProperty("device")]
        public string SigfoxId { set; get; }


        [JsonProperty("time")]
        public DateTimeOffset Timestamp 
        {
            get
            {
                return DateTimeOffset.Now;
            }
        }

        [JsonIgnore]
        public List<byte> DataBytes { set; get; }
        [JsonProperty("data")]
        public string DataStr
        {
            get
            {
                return BitConverter.ToString(DataBytes.ToArray()).Replace("-", "");
            }
        }

        public int SeqNumber { set; get; }

        [JsonProperty("lqi")]
        public string Lpi
        {
            get
            {
                return "Good";
            }
        }

        [JsonProperty("operatorName")]
        public string OperatorName
        {
            get
            {
                return "SIGFOX_Japan_KCCS";
            }
        }

        [JsonProperty("countryCode")]
        public string CountryCode
        {
            get
            {
                return "392";
            }
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

    }
}

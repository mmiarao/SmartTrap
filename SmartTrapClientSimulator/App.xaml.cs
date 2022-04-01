using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using SoracomWebApiClient.Converters;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SmartTrapClientSimulator
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            JsonConvert.DefaultSettings = () =>
            {
                var setting = new JsonSerializerSettings
                {
                    Formatting = Formatting.None,
                    NullValueHandling = NullValueHandling.Ignore,
                    ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new CamelCaseNamingStrategy()
                    },


                };
                setting.Converters.Add(new UnixMsDateTimeConverter());
                setting.Converters.Add(new StringEnumConverter { NamingStrategy = new CamelCaseNamingStrategy() });
                return setting;
            };
        }

    }
}

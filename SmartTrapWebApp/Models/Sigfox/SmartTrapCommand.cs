using SoracomWebApiClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartTrapWebApp.Models.Sigfox
{
    public class SmartTrapCommand:SigfoxUplinkData
    {
        public SigfoxCommand Command
        {
            get
            {
                return SigfoxCommand.Parse(RawDataBytes);
            }
        }
    }
}
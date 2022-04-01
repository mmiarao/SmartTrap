using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartTrapWebApp.Controllers.Base
{
    public abstract class Controller:ControllerBase
    {
        internal abstract ILogger Logger { get; set; }

        internal void DebugLog(string message, params object[] args)
        {
            Logger.LogDebug(message, args);
        }
        internal void DebugLog(int eventId, string message, params object[] args)
        {
            Logger.LogDebug(new EventId(eventId), message, args);
        }
        internal void InfoLog(string message, params object[] args)
        {
            Logger.LogInformation(message, args);
        }
        internal void InfoLog(int eventId, string message, params object[] args)
        {
            Logger.LogInformation(new EventId(eventId), message, args);
        }
        internal void WarnLog(string message, params object[] args)
        {
            Logger.LogWarning(message, args);
        }
        internal void WarnLog(int eventId, string message, params object[] args)
        {
            Logger.LogWarning(new EventId(eventId), message, args);
        }
        internal void ErrorLog(string message, params object[] args)
        {
            Logger.LogError(message, args);
        }
        internal void ErrorLog(int eventId, string message, params object[] args)
        {
            Logger.LogError(new EventId(eventId), message, args);
        }

    }

}

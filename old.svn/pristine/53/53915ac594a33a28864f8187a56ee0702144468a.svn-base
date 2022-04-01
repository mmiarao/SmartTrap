using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartTrapWebApp.Models.Sigfox
{
    public class SigfoxCommand
    {
        /// <summary>
        /// Sigfoxコマンド種別
        /// </summary>
        public enum CommandType:byte
        {
            Unknown = 0x00,
            Init = 0xFF,
            WatchDog = 0x01,
            NotifyTrap = 0x02,
        }
        public CommandType CommandId
        {

            get
            {
                if(RawData?.FirstOrDefault() is byte b)
                {
                    if(Enum.IsDefined(typeof(CommandType), b))
                    {
                        return (CommandType)b;
                    }
                }
                return CommandType.Unknown;
            }
        }
        public List<byte> RawData { set; private get; }

        /// <summary>
        /// データ
        /// </summary>
        public List<byte> Data
        {
            get
            {
                if(RawData?.Any() == true && RawData?.Count > 1)
                {
                    return RawData.Skip(1).ToList();
                }
                return new List<byte>();
            }
        }

        public static SigfoxCommand Parse(IEnumerable<byte> buf)
        {
            return new SigfoxCommand()
            {
                RawData = buf?.ToList()
            };
        }

    }
}
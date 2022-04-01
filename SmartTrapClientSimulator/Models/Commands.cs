using SmartTrapClientSimulator.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTrapClientSimulator.Models.Commands
{
    interface ISmartTrapCommand
    {
        List<byte> Serialize();
    }
    internal class StartUpCommand : ISmartTrapCommand
    {
        public List<byte> Serialize()
        {
            return new List<byte>
            {
                0xFF,
            };
        }
    }

    internal class WatchDogCommand : ISmartTrapCommand
    {
        public bool Trapped { set; get; }
        public List<byte> Serialize()
        {
            return new List<byte>
            {
                0x01,
                Trapped ? (byte)0x01 : (byte)0x00,
            };
        }
    }

    internal class TrappedCommand : ISmartTrapCommand
    {
        public bool Trapped { set; get; }
        public List<byte> Serialize()
        {
            return new List<byte>
            {
                0x02,
                Trapped ? (byte)0x01 : (byte)0x00,
            };
        }
    }
}

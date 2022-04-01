using MessageSendLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartTrapWebApp
{
    public class Helper
    {
        const string KEY2 = "C3780F96-5E74";
        const string KEY1 = "-456C-A5C2-";
        const string KEY3 = "7DB5991D1490";
        internal static string Secret(string buf)
        {
            string r;
            Encryptor.Main.Encrypt(buf, out r, KEY2 + KEY1 + KEY3);
            return r;
        }

    }
}
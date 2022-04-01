using SmartTrapClientSimulator.Models;
using SmartTrapClientSimulator.Models.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SmartTrapClientSimulator
{
    public class SigfoxWebApiClient
    {
        const string OPERATOR_HEADER = "x-soracom-operator-id";
        const string SIGNATURE_VERSION_HEADER = "x-soracom-signature-version";
        const string SIGNATURE_HEADER = "x-soracom-signature";
        const string TIMESTAMP_HEADER = "x-soracom-timestamp";
        const string SIGFOX_DEVICE_ID_HEADER = "x-soracom-sigfox-device-id";

        HttpClient client;
        HttpClient Client
        {
            get
            {
                if(client == null)
                {
                    var baseUrl = new Uri(Properties.Settings.Default.BaseAddress);
#if DEBUG_LOCAL
                    baseUrl = new Uri("https://localhost:44335/");
#endif
                    client = new HttpClient()
                    {
                        BaseAddress = baseUrl
                    };
                    client.DefaultRequestHeaders.Add(OPERATOR_HEADER, Properties.Settings.Default.OperatorId);
                    client.DefaultRequestHeaders.Add(SIGNATURE_VERSION_HEADER, "20151001");

                }
                return client;
            }
        }

        int SequenceNumber { set; get; }
        internal async Task SendCommand(string sigfoxId, ISmartTrapCommand command)
        {
            SequenceNumber++;
            var data = new SoracomBeamData()
            {
                DataBytes = command.Serialize(),
                SigfoxId = sigfoxId,
                SeqNumber = SequenceNumber,
            };
            var timestamp = GetTimestamp(DateTime.Now);
            var toSign = string.Format("{0}x-soracom-sigfox-device-id={1}x-soracom-timestamp={2}", SoracomBeamSecret, sigfoxId, timestamp);
            var sign = CalcSHA256(toSign);
            var req = new HttpRequestMessage(HttpMethod.Post, "api/Sigfox");
            req.Headers.Add("ContentType", "application/json");
            req.Headers.Add(TIMESTAMP_HEADER, timestamp);
            req.Headers.Add(SIGFOX_DEVICE_ID_HEADER, sigfoxId);
            req.Headers.Add(SIGNATURE_HEADER, sign);
            req.Content = new StringContent(data.ToJson(), Encoding.UTF8, "application/json");
            var r = await Client.SendAsync(req);
            r.EnsureSuccessStatusCode();
        }
        string SoracomBeamSecret
        {
            get
            {
                return Properties.Settings.Default.SoracomBeamSecret;
            }
        }

        string GetTimestamp(DateTimeOffset dt)
        {
            var unixTime  = (DateTimeOffset.Now - dt).Ticks / 10000000;
            return unixTime.ToString();
        }

        string CalcSHA256(string src)
        {
            byte[] input = Encoding.UTF8.GetBytes(src);
            SHA256 sha = new SHA256CryptoServiceProvider();
            byte[] hash_sha256 = sha.ComputeHash(input);

            string result = "";

            for (int i = 0; i < hash_sha256.Length; i++)
            {
                result = result + string.Format("{0:x2}", hash_sha256[i]);
            }
            return result;
        }
    }
}

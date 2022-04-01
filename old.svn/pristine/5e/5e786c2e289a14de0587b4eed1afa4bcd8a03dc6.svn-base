using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using SmartTrapWebApp.Models.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartTrapWebApp.Models.Member
{
    public class MembersResponse: TableListResponseBase
    {
        public List<MemberModel> Members { set; get; }
    }
    public class MemberInput : IMemberInput
    {
        [JsonProperty("name")]
        public string Name { set; get; }

        [JsonProperty("email")]
        public string Email { set; get; }

        [JsonProperty("phone")]
        public string Phone { set; get; }

        [JsonProperty("useEmail")]
        public bool UseEmail { set; get; }

        [JsonProperty("line")]
        public string LineId { set; get; }

        [JsonProperty("useLine")]
        public bool UseLine { set; get; }

        internal static MemberInput Create(IMemberInput member)
        {
            return new MemberInput()
            {
                Name = member.Name,
                Email = member.Email,
                Phone = member.Phone,
                UseEmail = member.UseEmail,
                LineId = member.LineId,
                UseLine = member.UseLine,
            };
        }
    }

    public class MemberModel : MemberInput, IMember
    {
        public string Id { set; get; }

        internal static MemberModel Create(string id, IMemberInput member)
        {
            return new MemberModel()
            {
                Id = id,
                Email = member.Email,
                LineId = member.LineId,
                Name = member.Name,
                UseEmail = member.UseEmail,
                UseLine = member.UseLine,
            };
        }



    }
}

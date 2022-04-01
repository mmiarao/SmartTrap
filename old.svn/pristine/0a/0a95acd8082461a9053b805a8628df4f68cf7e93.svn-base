using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartTrapWebApp.Models.Member
{
    public class MemberEntity: TableEntity,IMemberInput
    {
        public MemberEntity()
        {
        }
        public string Name { set; get; }

        public string Email { set; get; }

        public string Phone { set; get; }

        public bool UseEmail { set; get; }

        public string LineId { set; get; }

        public bool UseLine { set; get; }

        internal static MemberEntity CreateNew(string partitionKey)
        {
            return new MemberEntity()
            {
                PartitionKey = partitionKey,
                RowKey = Guid.NewGuid().ToString()
            };
        }

        internal static MemberEntity Create(string partitionKey, string rowKey, IMemberInput member)
        {
            return new MemberEntity()
            {
                PartitionKey = partitionKey,
                RowKey = rowKey,
                Name = member.Name,
                Email = member.Email,
                Phone = member.Phone,
                LineId = member.LineId,
                UseEmail = member.UseEmail,
                UseLine = member.UseLine,
            };
        }
    }
}

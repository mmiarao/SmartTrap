using AzureStorageLibPortal;
using Microsoft.WindowsAzure.Storage.Table;
using SmartTrapWebApp.Models;
using SmartTrapWebApp.Models.Configurations;
using SmartTrapWebApp.Models.Member;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartTrapWebApp.Services.Tables
{
    public class MemberTableClient : Base.TableClientBase
    {
        public MemberTableClient(Constants constants, Secrets secrets) : base("Member", constants, secrets)
        {
        }
        public async Task<TableQuerySegment<MemberEntity>> GetMembers(int count, TableContinuationToken token = null)
        {
            return await this.GetEntities<MemberEntity>(Constants.SystemNameEn, count, token);
        }

        public async Task<MemberEntity> GetMember(string memberId)
        {
            return await this.GetEntity<MemberEntity>(Constants.SystemNameEn, memberId);
        }

        public async Task<bool> Any(string memberId)
        {
            return await GetMember(memberId) != null ? true : false;
        }

        public async Task<MemberEntity> AddMember(MemberEntity member)
        {
            var r = await this.InsertEntity(member);
            return r.Result as MemberEntity;
        }

        public async Task<MemberEntity> UpdateMember(MemberEntity member)
        {
            var r = await this.InsertOrReplaceEntity(member);
            return r.Result as MemberEntity;
        }
    }
}

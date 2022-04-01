using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartTrapWebApp.Models.Member
{
    public interface IMemberInput
    {
        string Name { set; get; }

        string Email { set; get; }

        string Phone { set; get; }

        bool UseEmail { set; get; }

        string LineId { set; get; }

        bool UseLine { set; get; }
    }

    public interface IMember : IMemberInput
    {
        string Id { set; get; }
    }

}

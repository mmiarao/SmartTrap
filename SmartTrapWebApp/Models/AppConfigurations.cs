using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartTrapWebApp.Models.Configurations
{
    public class ApiKeys
    {
        public IdKey LineLogin { get; set; }
        public IdKey LineMessaging { get; set; }
        public IdKey SoracomSam { set; get; }
    }

    public class IdKey
    {
        public string Id { get; set; }
        public string Key { get; set; }
    }

    public class Linelogin
    {
        public string Id { get; set; }
        public string Key { get; set; }
    }

    public class Linemessaging
    {
        public string Id { get; set; }
        public string Key { get; set; }
    }

    public class Soracomsam
    {
        public string Id { set; get; }
        public string Key { set; get; }
    }

    public class Secrets
    {
        public string SoracomBeam { get; set; }
        public string AzureStorage { set; get; }
    }

    public class Constants
    {
        public string SystemNameJp { set; get; }

        public string SystemNameEn { set; get; }

        public string SystemAuthor { set; get; }

        public string AzureStorageName { set; get; }

        public string SigfoxGroupId { set; get; }

        public string SoracomOperatorId { set; get; }
    }


}

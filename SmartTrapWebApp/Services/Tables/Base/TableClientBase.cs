﻿using AzureStorageLibPortal;
using AzureStorageLibPortal.Services;
using SmartTrapWebApp.Models.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartTrapWebApp.Services.Tables.Base
{
    public abstract class TableClientBase : TableControllerBase
    {
        public TableClientBase(string tableName, Constants constants, Secrets secrets) : base(ConnStringService.GenerateStorageAccessStr(constants.AzureStorageName, secrets.AzureStorage), tableName)
        {
            Constants = constants;
            Secrets = secrets;
        }

        internal Constants Constants { get; }
        internal Secrets Secrets { get; }
    }
}

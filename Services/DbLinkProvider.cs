﻿using Halcyon.HAL.Attributes;
using KeePassWeb.Controllers.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Threax.AspNetCore.Halcyon.Ext;

namespace KeePassWeb.Services
{
    public class DbLinkProvider
    {
        public static IEnumerable<HalLinkAttribute> CreateHalLinks(bool dbClosed)
        {
            if (dbClosed)
            {
                yield return new HalActionLinkAttribute(typeof(KeepassDatabaseController), nameof(KeepassDatabaseController.Open));
            }
            else
            {
                yield return new HalActionLinkAttribute(typeof(KeepassDatabaseController), nameof(KeepassDatabaseController.Close));
            }
        }
    }
}

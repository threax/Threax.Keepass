﻿using Halcyon.HAL.Attributes;
using Threax.Keepass.Controllers.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Threax.AspNetCore.Halcyon.Ext;
using Threax.AspNetCore.Models;

namespace Threax.Keepass.ViewModels
{
    [HalModel]
    [HalSelfActionLink(typeof(EntriesController), nameof(EntriesController.GetPassword))]
    [HalActionLink(typeof(ItemsController), nameof(ItemsController.Get), "GetItem")]
    [HalActionLink(typeof(EntriesController), nameof(EntriesController.Get), "GetEntry")]
    public class PasswordInfo
    {
        public Guid ItemId { get; set; }

        [PasswordUiType]
        public String Password { get; set; }
    }
}

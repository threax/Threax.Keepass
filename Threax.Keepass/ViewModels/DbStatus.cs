﻿using Halcyon.HAL.Attributes;
using Threax.Keepass.Controllers.Api;
using Threax.Keepass.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Threax.AspNetCore.Halcyon.Ext;
using Threax.AspNetCore.UserBuilder.Entities.Mvc;
using Threax.AspNetCore.UserLookup.Mvc.Controllers;

namespace Threax.Keepass.ViewModels
{
    [HalModel]
    [HalEntryPoint]
    [CacheEndpointDoc]
    [HalSelfActionLink(typeof(EntryPointController), nameof(EntryPointController.Get))]
    //This first set of links is for role editing, you can erase them if you don't have users or roles.
    [HalActionLink(RolesControllerRels.GetUser, typeof(RolesController))]
    [HalActionLink(RolesControllerRels.ListUsers, typeof(RolesController))]
    [HalActionLink(RolesControllerRels.SetUser, typeof(RolesController))]
    //User Search Actions
    [HalActionLink(typeof(UserSearchController), nameof(UserSearchController.List), "ListAppUsers")]
    [DeclareHalLink(typeof(KeepassDatabaseController), nameof(KeepassDatabaseController.Open))]
    [DeclareHalLink(typeof(KeepassDatabaseController), nameof(KeepassDatabaseController.Close))]
    //The additional entry point links are in the other entry point partial classes, expand this node to see them
    public partial class DbStatus : IHalLinkProvider
    {
        public bool DbClosed { get; set; }

        public IEnumerable<HalLinkAttribute> CreateHalLinks(ILinkProviderContext context)
        {
            return DbLinkProvider.CreateHalLinks(DbClosed);
        }
    }
}

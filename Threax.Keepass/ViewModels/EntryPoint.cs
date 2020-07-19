using Halcyon.HAL.Attributes;
using Threax.Keepass.Controllers.Api;
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
    //The additional entry point links are in the other entry point partial classes, expand this node to see them
    [HalActionLink(typeof(KeepassDatabaseController), nameof(KeepassDatabaseController.Status), "GetDbStatus")]
    [HalActionLink(typeof(KeepassDatabaseController), nameof(KeepassDatabaseController.Open), "OpenDb")]
    [HalActionLink(typeof(KeepassDatabaseController), nameof(KeepassDatabaseController.Close), "CloseDb")]
    public partial class EntryPoint
    {
    }
}

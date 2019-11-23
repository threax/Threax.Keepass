using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Threax.AspNetCore.Halcyon.Ext;
using Threax.AspNetCore.UserBuilder.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Threax.AspNetCore.UserBuilder.Entities.Mvc;
using Halcyon.HAL.Attributes;

namespace KeePassWeb.Controllers.Api
{
    [Route("api/[controller]")]
    [ResponseCache(NoStore = true)]
    public class RolesController : RolesControllerBase<RoleAssignments, UserCollection>
    {
        public RolesController(IRoleManager roleManager, IHttpContextAccessor contextAccessor)
            : base(roleManager, contextAccessor)
        {

        }

        protected override UserCollection GetUserCollection(RolesQuery query, int total, IEnumerable<RoleAssignments> users)
        {
            return new UserCollection(query, total, users);
        }
    }

    [HalModel]
    [HalSelfActionLink(RolesControllerRels.ListUsers, typeof(RolesController))]
    [HalActionLink(typeof(RolesController), nameof(RolesController.GetUser), CrudRels.Get, DocsOnly = true)] //This provides access to docs for showing items
    [HalActionLink(typeof(RolesController), nameof(RolesController.ListUsers), CrudRels.List, DocsOnly = true)] //Provides docs for search
    [HalActionLink(typeof(RolesController), nameof(RolesController.SetUser), CrudRels.Update, DocsOnly = true)] //This provides access to docs for updating items if the ui has different modes
    [HalActionLink(typeof(RolesController), nameof(RolesController.SetUser), CrudRels.Add)]
    [DeclareHalLink(PagedCollectionView<Object>.Rels.Next, RolesControllerRels.ListUsers, typeof(RolesController), ResponseOnly = true)]
    [DeclareHalLink(PagedCollectionView<Object>.Rels.Previous, RolesControllerRels.ListUsers, typeof(RolesController), ResponseOnly = true)]
    [DeclareHalLink(PagedCollectionView<Object>.Rels.First, RolesControllerRels.ListUsers, typeof(RolesController), ResponseOnly = true)]
    [DeclareHalLink(PagedCollectionView<Object>.Rels.Last, RolesControllerRels.ListUsers, typeof(RolesController), ResponseOnly = true)]
    public class UserCollection : UserCollectionBase<RoleAssignments>
    {
        public UserCollection(RolesQuery query, int total, IEnumerable<RoleAssignments> items) : base(query, total, items)
        {
        }
    }
}

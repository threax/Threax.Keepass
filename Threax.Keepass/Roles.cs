using Halcyon.HAL.Attributes;
using Threax.Keepass.Controllers.Api;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Threax.AspNetCore.Halcyon.Ext;
using Threax.AspNetCore.UserBuilder.Entities.Mvc;

namespace Threax.Keepass
{
    /// <summary>
    /// This class makes it easy to keep track of role constants throught the system.
    /// </summary>
    public static class Roles
    {
        /// <summary>
        /// All roles, any roles added above that you want to add to the database should be defined here.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<String> DatabaseRoles()
        {
            yield break;
        }
    }

    [HalModel]
    [HalSelfActionLink(RolesControllerRels.GetUser, typeof(RolesController))]
    [HalActionLink(RolesControllerRels.SetUser, typeof(RolesController))]
    [HalActionLink(CrudRels.Update, RolesControllerRels.SetUser, typeof(RolesController))]
    [HalActionLink(CrudRels.Delete, RolesControllerRels.DeleteUser, typeof(RolesController))]
    public class RoleAssignments : ReflectedRoleAssignments
    {
        
    }
}

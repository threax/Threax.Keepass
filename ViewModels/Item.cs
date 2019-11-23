using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Halcyon.HAL.Attributes;
using Threax.AspNetCore.Halcyon.Ext;
using Threax.AspNetCore.Models;
using Threax.AspNetCore.Tracking;
using KeePassWeb.Controllers.Api;
using Threax.AspNetCore.Halcyon.Ext.ValueProviders;

namespace KeePassWeb.ViewModels 
{
    [HalModel]
    [HalSelfActionLink(typeof(ItemsController), nameof(ItemsController.Get))]
    [HalActionLink(typeof(ItemsController), nameof(ItemsController.Update))]
    [HalActionLink(typeof(ItemsController), nameof(ItemsController.Delete))]
    public partial class Item : ICreatedModified
    {
        public String ItemId { get; set; }

        public bool IsGroup { get; set; }

        public String Name { get; set; }

        [UiOrder(0, 2147483646)]
        public DateTime Created { get; set; }

        [UiOrder(0, 2147483647)]
        public DateTime Modified { get; set; }

    }
}

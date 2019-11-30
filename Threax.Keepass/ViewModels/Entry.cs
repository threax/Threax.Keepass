using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Halcyon.HAL.Attributes;
using Threax.AspNetCore.Halcyon.Ext;
using Threax.AspNetCore.Models;
using Threax.AspNetCore.Tracking;
using Threax.Keepass.Controllers.Api;
using Threax.AspNetCore.Halcyon.Ext.ValueProviders;

namespace Threax.Keepass.ViewModels 
{
    [HalModel]
    [HalSelfActionLink(typeof(ItemsController), nameof(ItemsController.GetEntry))]
    [HalActionLink(typeof(ItemsController), nameof(ItemsController.Update))]
    [HalActionLink(typeof(ItemsController), nameof(ItemsController.Delete))]
    [DeclareHalLink(typeof(ItemsController), nameof(ItemsController.GetPassword))]
    public partial class Entry : ICreatedModified
    {
        public Guid ItemId { get; set; }

        public String Name { get; set; }

        public String UserName { get; set; }

        public String Url { get; set; }

        [TextAreaUiType()]
        public String Notes { get; set; }

        [UiOrder(0, 2147483646)]
        public DateTime Created { get; set; }

        [UiOrder(0, 2147483647)]
        public DateTime Modified { get; set; }

    }
}

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
    [CacheEndpointDoc]
    [HalSelfActionLink(typeof(ItemsController), nameof(ItemsController.Get))]
    [DeclareHalLink(typeof(ItemsController), nameof(ItemsController.GetEntry))]
    [HalActionLink(typeof(ItemsController), nameof(ItemsController.Update))]
    [HalActionLink(typeof(ItemsController), nameof(ItemsController.Delete))]
    [DeclareHalLink(typeof(ItemsController), nameof(ItemsController.GetPassword))]
    public partial class Item : ICreatedModified, IHalLinkProvider
    {
        public Guid ItemId { get; set; }

        public String Name { get; set; }

        public bool IsGroup { get; set; }

        [UiOrder(0, 2147483646)]
        public DateTime Created { get; set; }

        [UiOrder(0, 2147483647)]
        public DateTime Modified { get; set; }

        public IEnumerable<HalLinkAttribute> CreateHalLinks(ILinkProviderContext context)
        {
            if (!IsGroup)
            {
                yield return new HalActionLinkAttribute(typeof(ItemsController), nameof(ItemsController.GetEntry));
                yield return new HalActionLinkAttribute(typeof(ItemsController), nameof(ItemsController.GetPassword));
            }
        }
    }
}

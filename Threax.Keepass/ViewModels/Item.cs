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
    [HalSelfActionLink(typeof(ItemsController), nameof(ItemsController.Get))]
    [DeclareHalLink(typeof(EntriesController), nameof(EntriesController.Get), "GetEntry")]
    [DeclareHalLink(typeof(EntriesController), nameof(EntriesController.Update))]
    [DeclareHalLink(typeof(EntriesController), nameof(EntriesController.Delete))]
    [DeclareHalLink(typeof(EntriesController), nameof(EntriesController.GetPassword))]
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
                yield return new HalActionLinkAttribute(typeof(EntriesController), nameof(EntriesController.Get), "GetEntry");
                yield return new HalActionLinkAttribute(typeof(EntriesController), nameof(EntriesController.Update));
                yield return new HalActionLinkAttribute(typeof(EntriesController), nameof(EntriesController.Delete));
                yield return new HalActionLinkAttribute(typeof(EntriesController), nameof(EntriesController.GetPassword));
            }
        }
    }
}

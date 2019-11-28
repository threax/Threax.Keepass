using Halcyon.HAL.Attributes;
using Threax.Keepass.Controllers.Api;
using Threax.Keepass.InputModels;
using Threax.Keepass.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Threax.AspNetCore.Halcyon.Ext;

namespace Threax.Keepass.ViewModels
{
    [HalModel]
    [HalSelfActionLink(typeof(ItemsController), nameof(ItemsController.List))]
    [HalActionLink(typeof(ItemsController), nameof(ItemsController.Get), DocsOnly = true)] //This provides access to docs for showing items
    [HalActionLink(typeof(ItemsController), nameof(ItemsController.List), DocsOnly = true)] //This provides docs for searching the list
    [HalActionLink(typeof(ItemsController), nameof(ItemsController.Update), DocsOnly = true)] //This provides access to docs for updating items if the ui has different modes
    [HalActionLink(typeof(ItemsController), nameof(ItemsController.Add))]
    [DeclareHalLink(typeof(ItemsController), nameof(ItemsController.List), PagedCollectionView<Object>.Rels.Next, ResponseOnly = true)]
    [DeclareHalLink(typeof(ItemsController), nameof(ItemsController.List), PagedCollectionView<Object>.Rels.Previous, ResponseOnly = true)]
    [DeclareHalLink(typeof(ItemsController), nameof(ItemsController.List), PagedCollectionView<Object>.Rels.First, ResponseOnly = true)]
    [DeclareHalLink(typeof(ItemsController), nameof(ItemsController.List), PagedCollectionView<Object>.Rels.Last, ResponseOnly = true)]
    [DeclareHalLink(typeof(KeepassDatabaseController), nameof(KeepassDatabaseController.Open))]
    [DeclareHalLink(typeof(KeepassDatabaseController), nameof(KeepassDatabaseController.Close))]
    public partial class ItemCollection : PagedCollectionViewWithQuery<Item, ItemQuery>
    {
        public bool DbClosed { get; set; }

        public ItemCollection(ItemQuery query, int total, IEnumerable<Item> items) : base(query, total, items)
        {
            
        }

        public override IEnumerable<HalLinkAttribute> CreateHalLinks(ILinkProviderContext context)
        {
            return base.CreateHalLinks(context).Concat(DbLinkProvider.CreateHalLinks(DbClosed));
        }
    }
}
using Halcyon.HAL.Attributes;
using KeePassWeb.Controllers.Api;
using KeePassWeb.InputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Threax.AspNetCore.Halcyon.Ext;

namespace KeePassWeb.ViewModels
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
    public partial class ItemCollection : PagedCollectionViewWithQuery<Item, ItemQuery>
    {
        public ItemCollection(ItemQuery query, int total, IEnumerable<Item> items) : base(query, total, items)
        {
            
        }
    }
}
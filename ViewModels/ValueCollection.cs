using Halcyon.HAL.Attributes;
using KeePassWeb.Controllers.Api;
using KeePassWeb.Models;
using KeePassWeb.InputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Threax.AspNetCore.Halcyon.Ext;

namespace KeePassWeb.ViewModels
{
    [HalModel]
    [HalSelfActionLink(typeof(ValuesController), nameof(ValuesController.List))]
    [HalActionLink(typeof(ValuesController), nameof(ValuesController.Get), DocsOnly = true)] //This provides access to docs for showing items
    [HalActionLink(typeof(ValuesController), nameof(ValuesController.List), DocsOnly = true)] //This provides docs for searching the list
    [HalActionLink(typeof(ValuesController), nameof(ValuesController.Update), DocsOnly = true)] //This provides access to docs for updating items if the ui has different modes
    [HalActionLink(typeof(ValuesController), nameof(ValuesController.Add))]
    [DeclareHalLink(typeof(ValuesController), nameof(ValuesController.List), PagedCollectionView<Object>.Rels.Next, ResponseOnly = true)]
    [DeclareHalLink(typeof(ValuesController), nameof(ValuesController.List), PagedCollectionView<Object>.Rels.Previous, ResponseOnly = true)]
    [DeclareHalLink(typeof(ValuesController), nameof(ValuesController.List), PagedCollectionView<Object>.Rels.First, ResponseOnly = true)]
    [DeclareHalLink(typeof(ValuesController), nameof(ValuesController.List), PagedCollectionView<Object>.Rels.Last, ResponseOnly = true)]
    public partial class ValueCollection : PagedCollectionViewWithQuery<Value, ValueQuery>
    {
        public ValueCollection(ValueQuery query, int total, IEnumerable<Value> items) : base(query, total, items)
        {
            
        }
    }
}
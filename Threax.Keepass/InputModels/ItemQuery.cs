using Halcyon.HAL.Attributes;
using Threax.Keepass.Controllers;
using Threax.Keepass.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Threax.AspNetCore.Halcyon.Ext;
using Threax.AspNetCore.Halcyon.Ext.ValueProviders;
using Threax.AspNetCore.Models;
using System.ComponentModel.DataAnnotations;

namespace Threax.Keepass.InputModels
{
    [HalModel]
    [CacheEndpointDoc]
    public partial class ItemQuery : PagedCollectionQuery
    {
        /// <summary>
        /// Lookup a item by id.
        /// </summary>
        public Guid? ItemId { get; set; }

        /// <summary>
        /// Lookup the items contained in a given parent id.
        /// </summary>
        public Guid? ParentItemId { get; set; }

        [UiSearch]
        public String Search { get; set; }

        /// <summary>
        /// Populate an IQueryable. Does not apply the skip or limit.
        /// </summary>
        /// <param name="query">The query to populate.</param>
        /// <returns>The query passed in populated with additional conditions.</returns>
        public Task<IQueryable<ItemEntity>> Create(IQueryable<ItemEntity> query)
        {
            if (ItemId != null)
            {
                query = query.Where(i => i.ItemId == ItemId);
            }
            else
            {
                //Customize query further
            }

            return Task.FromResult(query);
        }
    }
}
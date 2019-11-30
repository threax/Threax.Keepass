using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Threax.Keepass.InputModels;
using Threax.Keepass.ViewModels;
using Threax.AspNetCore.Halcyon.Ext;

namespace Threax.Keepass.Repository
{
    public partial interface IItemRepository
    {
        Task<Item> Get(Guid itemId);
        Task<ItemCollection> List(ItemQuery query);
    }
}
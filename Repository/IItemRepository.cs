using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KeePassWeb.InputModels;
using KeePassWeb.ViewModels;
using Threax.AspNetCore.Halcyon.Ext;

namespace KeePassWeb.Repository
{
    public partial interface IItemRepository
    {
        Task<Item> Add(ItemInput value);
        Task AddRange(IEnumerable<ItemInput> values);
        Task Delete(String id);
        Task<Item> Get(String itemId);
        Task<bool> HasItems();
        Task<ItemCollection> List(ItemQuery query);
        Task<Item> Update(String itemId, ItemInput value);
    }
}
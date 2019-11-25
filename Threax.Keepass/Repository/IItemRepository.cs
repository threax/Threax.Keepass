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
        Task Delete(Guid id);
        Task<Item> Get(Guid itemId);
        Task<PasswordInfo> GetPassword(Guid itemId);
        Task<bool> HasItems();
        Task<ItemCollection> List(ItemQuery query);
        Task<Item> Update(Guid itemId, ItemInput value);
    }
}
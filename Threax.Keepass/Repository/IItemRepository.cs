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
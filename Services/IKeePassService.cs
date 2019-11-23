using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KeePassWeb.Database;
using KeePassWeb.InputModels;
using KeePassWeb.ViewModels;

namespace KeePassWeb.Services
{
    public interface IKeePassService : IDisposable
    {
        Task<Item> Add(ItemInput item);
        Task Delete(string id);
        Task<ItemEntity> Get(string itemId);
        Task<IEnumerable<ItemEntity>> List(ItemQuery query);
        Task<Item> Update(string itemId, ItemInput item);
    }
}
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
        Task Open();
        Task Close();
        Task<bool> IsOpen();
        Task<Item> Add(ItemInput item);
        Task Delete(Guid id);
        Task<ItemEntity> Get(Guid itemId);
        Task<IEnumerable<ItemEntity>> List(ItemQuery query);
        Task<Item> Update(Guid itemId, ItemInput item);
    }
}
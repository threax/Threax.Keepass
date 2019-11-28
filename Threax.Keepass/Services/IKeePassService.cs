using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Threax.Keepass.Database;
using Threax.Keepass.InputModels;
using Threax.Keepass.ViewModels;

namespace Threax.Keepass.Services
{
    public interface IKeePassService : IDisposable
    {
        Task Open(String password);
        Task Close();
        Task<bool> IsOpen();
        Task<Item> Add(ItemInput item);
        Task Delete(Guid id);
        Task<ItemEntity> Get(Guid itemId);
        Task<String> GetPassword(Guid itemId);
        Task<IEnumerable<ItemEntity>> List(ItemQuery query);
        Task<Item> Update(Guid itemId, ItemInput item);
    }
}
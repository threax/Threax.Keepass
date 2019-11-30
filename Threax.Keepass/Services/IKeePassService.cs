using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Threax.Keepass.Database;
using Threax.Keepass.InputModels;
using Threax.Keepass.ViewModels;

namespace Threax.Keepass.Services
{
    public interface IKeePassService
    {
        Task<ItemEntity> Add(Guid? parent, ItemInput item);
        Task Close();
        Task Delete(Guid id);
        void Dispose();
        Task<ItemEntity> Get(Guid itemId);
        Task<Entry> GetEntry(Guid itemId);
        Task<string> GetPassword(Guid itemId);
        Task<bool> IsOpen();
        Task<IEnumerable<ItemEntity>> List(ItemQuery query);
        Task Open(string password);
        Task<ItemEntity> Update(Guid itemId, ItemInput item);
    }
}
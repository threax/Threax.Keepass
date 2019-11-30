using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Threax.Keepass.Database;
using Threax.Keepass.InputModels;

namespace Threax.Keepass.Services
{
    public interface IKeePassService
    {
        Task<EntryEntity> Add(Guid? parent, EntryInput item);
        Task Close();
        Task Delete(Guid id);
        void Dispose();
        Task<ItemEntity> Get(Guid itemId);
        Task<EntryEntity> GetEntry(Guid itemId);
        Task<string> GetPassword(Guid itemId);
        Task<bool> IsOpen();
        Task<IEnumerable<ItemEntity>> List(ItemQuery query);
        Task Open(string password);
        Task<EntryEntity> Update(Guid itemId, EntryInput item);
    }
}
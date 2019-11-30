using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Threax.Keepass.InputModels;
using Threax.Keepass.ViewModels;
using Threax.AspNetCore.Halcyon.Ext;

namespace Threax.Keepass.Repository
{
    public partial interface IEntryRepository
    {
        Task<Entry> Add(Guid? parent, EntryInput value);
        Task Delete(Guid id);
        Task<Entry> Get(Guid itemId);
        Task<EntryCollection> List(EntryQuery query);
        Task<Entry> Update(Guid itemId, EntryInput value);
        Task<PasswordInfo> GetPassword(Guid itemId);
    }
}
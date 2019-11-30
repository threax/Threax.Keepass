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
        Task<Entry> Add(EntryInput value);
        Task AddRange(IEnumerable<EntryInput> values);
        Task Delete(Guid id);
        Task<Entry> Get(Guid itemId);
        Task<bool> HasEntries();
        Task<EntryCollection> List(EntryQuery query);
        Task<Entry> Update(Guid itemId, EntryInput value);
    }
}
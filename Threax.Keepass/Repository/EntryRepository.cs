using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Threax.Keepass.Database;
using Threax.Keepass.InputModels;
using Threax.Keepass.ViewModels;
using Threax.Keepass.Mappers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Threax.AspNetCore.Halcyon.Ext;
using Threax.Keepass.Services;

namespace Threax.Keepass.Repository
{
    public partial class EntryRepository : IEntryRepository
    {
        private IKeePassService keepass;
        private AppMapper mapper;

        public EntryRepository(IKeePassService keepass, AppMapper mapper)
        {
            this.keepass = keepass;
            this.mapper = mapper;
        }

        public async Task<EntryCollection> List(EntryQuery query)
        {
            throw new NotImplementedException();
        }

        public async Task<Entry> Get(Guid itemId)
        {
            var entity = await keepass.GetEntry(itemId);
            return mapper.MapEntry(entity, new Entry());
        }

        public async Task<Entry> Add(EntryInput entry)
        {
            var entity = await keepass.Add(entry);
            return mapper.MapEntry(entity, new Entry());
        }

        public async Task<Entry> Update(Guid itemId, EntryInput entry)
        {
            var entity = await keepass.Update(itemId, entry);
            return mapper.MapEntry(entity, new Entry());
        }

        public async Task Delete(Guid id)
        {
            await keepass.Delete(id);
        }

        public async Task<PasswordInfo> GetPassword(Guid itemId)
        {
            return new PasswordInfo()
            {
                Password = await keepass.GetPassword(itemId),
                ItemId = itemId
            };
        }
    }
}
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

namespace Threax.Keepass.Repository
{
    public partial class EntryRepository : IEntryRepository
    {
        private AppDbContext dbContext;
        private AppMapper mapper;

        public EntryRepository(AppDbContext dbContext, AppMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<EntryCollection> List(EntryQuery query)
        {
            var dbQuery = await query.Create(this.Entities);

            var total = await dbQuery.CountAsync();
            dbQuery = dbQuery.Skip(query.SkipTo(total)).Take(query.Limit);
            var results = await mapper.ProjectEntry(dbQuery).ToListAsync();

            return new EntryCollection(query, total, results);
        }

        public async Task<Entry> Get(Guid itemId)
        {
            var entity = await this.Entity(itemId);
            return mapper.MapEntry(entity, new Entry());
        }

        public async Task<Entry> Add(EntryInput entry)
        {
            var entity = mapper.MapEntry(entry, new EntryEntity());
            this.dbContext.Add(entity);
            await SaveChanges();
            return mapper.MapEntry(entity, new Entry());
        }

        public async Task<Entry> Update(Guid itemId, EntryInput entry)
        {
            var entity = await this.Entity(itemId);
            if (entity != null)
            {
                mapper.MapEntry(entry, entity);
                await SaveChanges();
                return mapper.MapEntry(entity, new Entry());
            }
            throw new KeyNotFoundException($"Cannot find entry {itemId.ToString()}");
        }

        public async Task Delete(Guid id)
        {
            var entity = await this.Entity(id);
            if (entity != null)
            {
                Entities.Remove(entity);
                await SaveChanges();
            }
        }

        public virtual async Task<bool> HasEntries()
        {
            return await Entities.CountAsync() > 0;
        }

        public virtual async Task AddRange(IEnumerable<EntryInput> entries)
        {
            var entities = entries.Select(i => mapper.MapEntry(i, new EntryEntity()));
            this.dbContext.Entries.AddRange(entities);
            await SaveChanges();
        }

        protected virtual async Task SaveChanges()
        {
            await this.dbContext.SaveChangesAsync();
        }

        private DbSet<EntryEntity> Entities
        {
            get
            {
                return dbContext.Entries;
            }
        }

        private Task<EntryEntity> Entity(Guid itemId)
        {
            return Entities.Where(i => i.ItemId == itemId).FirstOrDefaultAsync();
        }
    }
}
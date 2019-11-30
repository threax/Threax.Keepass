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
    public partial class ItemRepository : IItemRepository
    {
        private IKeePassService keepass;
        private AppMapper mapper;

        public ItemRepository(IKeePassService dbContext, AppMapper mapper)
        {
            this.keepass = dbContext;
            this.mapper = mapper;
        }

        public async Task<ItemCollection> List(ItemQuery query)
        {
            if (!await this.keepass.IsOpen())
            {
                return new ItemCollection(query, 0, Enumerable.Empty<Item>())
                {
                    DbClosed = true
                };
            }

            var dbQuery = await keepass.List(query);

            var total = dbQuery.Count();
            dbQuery = dbQuery.Skip(query.SkipTo(total)).Take(query.Limit);
            var results = mapper.ProjectItem(dbQuery).ToList();

            return new ItemCollection(query, total, results);
        }

        public async Task<Item> Get(Guid itemId)
        {
            var entity = await keepass.Get(itemId);
            return mapper.MapItem(entity, new Item());
        }

        public Task<Entry> GetEntry(Guid itemId)
        {
            return keepass.GetEntry(itemId);
        }
        
        public async Task<PasswordInfo> GetPassword(Guid itemId)
        {
            return new PasswordInfo()
            {
                Password = await keepass.GetPassword(itemId),
                ItemId = itemId
            };
        }

        public async Task<Item> Add(ItemInput entry)
        {
            var entity = await keepass.Add(null, entry);
            return mapper.MapItem(entity, new Item());
        }

        public async Task<Item> Add(Guid? parent, ItemInput entry)
        {
            var entity = await keepass.Add(parent, entry);
            return mapper.MapItem(entity, new Item());
        }

        public async Task<Item> Update(Guid itemId, ItemInput entry)
        {
            var entity = await keepass.Update(itemId, entry);
            return mapper.MapItem(entity, new Item());
        }

        public async Task Delete(Guid id)
        {
            await keepass.Delete(id);
        }
    }
}
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using KeePassWeb.Database;
using KeePassWeb.InputModels;
using KeePassWeb.ViewModels;
using KeePassWeb.Mappers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Threax.AspNetCore.Halcyon.Ext;
using KeePassWeb.Services;

namespace KeePassWeb.Repository
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
            if(! await this.keepass.IsOpen())
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

        public async Task<Item> Add(ItemInput item)
        {
            throw new NotImplementedException();
            //var entity = mapper.MapItem(item, new ItemEntity());
            //this.dbContext.Add(entity);
            //await SaveChanges();
            //return mapper.MapItem(entity, new Item());
        }

        public async Task<Item> Update(Guid itemId, ItemInput item)
        {
            throw new NotImplementedException();
            //var entity = await this.Entity(itemId);
            //if (entity != null)
            //{
            //    mapper.MapItem(item, entity);
            //    await SaveChanges();
            //    return mapper.MapItem(entity, new Item());
            //}
            //throw new KeyNotFoundException($"Cannot find item {itemId.ToString()}");
        }

        public async Task Delete(Guid id)
        {
            throw new NotImplementedException();
            //var entity = await this.Entity(id);
            //if (entity != null)
            //{
            //    Entities.Remove(entity);
            //    await SaveChanges();
            //}
        }

        public virtual async Task<bool> HasItems()
        {
            return true;
            //return await Entities.CountAsync() > 0;
        }

        public virtual async Task AddRange(IEnumerable<ItemInput> items)
        {
            throw new NotImplementedException();
            //var entities = items.Select(i => mapper.MapItem(i, new ItemEntity()));
            //this.dbContext.Items.AddRange(entities);
            //await SaveChanges();
        }
    }
}
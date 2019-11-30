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
    }
}
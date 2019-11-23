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

namespace KeePassWeb.Repository
{
    public partial class ItemRepository : IItemRepository
    {
        private AppMapper mapper;

        public ItemRepository(AppMapper mapper)
        {
            this.mapper = mapper;
        }

        public async Task<ItemCollection> List(ItemQuery query)
        {
            throw new NotImplementedException();
        }

        public async Task<Item> Get(String itemId)
        {
            throw new NotImplementedException();
        }

        public async Task<Item> Add(ItemInput item)
        {
            throw new NotImplementedException();
        }

        public async Task<Item> Update(String itemId, ItemInput item)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(String id)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<bool> HasItems()
        {
            throw new NotImplementedException();
        }

        public virtual async Task AddRange(IEnumerable<ItemInput> items)
        {
            throw new NotImplementedException();
        }
    }
}
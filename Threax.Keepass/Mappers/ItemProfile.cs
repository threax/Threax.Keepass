using System;
using System.Collections.Generic;
using System.Linq;
using Threax.Keepass.Database;
using Threax.Keepass.InputModels;
using Threax.Keepass.ViewModels;

namespace Threax.Keepass.Mappers
{
    public partial class AppMapper
    {
        public ItemEntity MapItem(ItemInput src, ItemEntity dest)
        {
            //dest.ItemId ignored
            dest.Name = src.Name;
            dest.IsGroup = src.IsGroup;
            dest.Created = GetCreated(dest.Created);
            dest.Modified = DateTime.UtcNow;

            return dest;
        }

        public Item MapItem(ItemEntity src, Item dest)
        {
            dest.ItemId = src.ItemId;
            dest.Name = src.Name;
            dest.IsGroup = src.IsGroup;
            dest.Created = src.Created;
            dest.Modified = src.Modified;

            return dest;
        }

        public IEnumerable<Item> ProjectItem(IEnumerable<ItemEntity> query)
        {
            return query.Select(i => new Item()
            {
                ItemId = i.ItemId,
                Name = i.Name,
                IsGroup = i.IsGroup,
                Created = i.Created,
                Modified = i.Modified,
            });
        }
    }
}
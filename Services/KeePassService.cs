using KeePassLib;
using KeePassLib.Interfaces;
using KeePassLib.Keys;
using KeePassWeb.Database;
using KeePassWeb.InputModels;
using KeePassWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KeePassWeb.Services
{
    public class KeePassService : IKeePassService
    {
        PwDatabase db;

        public KeePassService(KeePassConfig config, IStatusLogger statusLogger)
        {
            db = new PwDatabase();
            var keys = new CompositeKey();
            keys.AddUserKey(new KcpPassword(config.Password));

            db.Open(new KeePassLib.Serialization.IOConnectionInfo()
            {
                Path = config.DbFile,
                Password = config.Password
            }, keys, statusLogger);
        }
        public void Dispose()
        {
            db.Close();
        }

        private ItemEntity GroupToItemEntity(PwGroup i)
        {
            return new ItemEntity()
            {
                Created = i.CreationTime,
                Modified = i.LastAccessTime,
                IsGroup = true,
                ItemId = new Guid(i.Uuid.UuidBytes),
                Name = i.Name
            };
        }

        private ItemEntity EntryToItemEntity(PwEntry i)
        {
            return new ItemEntity()
            {
                Created = i.CreationTime,
                Modified = i.LastAccessTime,
                IsGroup = false,
                ItemId = new Guid(i.Uuid.UuidBytes),
                Name = i.Strings.Get("Title").ReadString()
            };
        }

        private IEnumerable<ItemEntity> GetItems(PwGroup group)
        {
            return group.Groups.Select(i => GroupToItemEntity(i))
                .Concat(group.Entries.Select(i => EntryToItemEntity(i)));
        }

        public Task<IEnumerable<ItemEntity>> List(ItemQuery query)
        {
            var group = db.RootGroup;
            if (query.ItemId != null)
            {
                var bytes = query.ItemId.Value.ToByteArray();
                var id = new PwUuid(bytes);
                group = group.FindGroup(id, true);
            }

            return Task.FromResult(GetItems(group));
        }

        public Task<ItemEntity> Get(Guid itemId)
        {
            if (itemId != null)
            {
                var bytes = itemId.ToByteArray();
                var id = new PwUuid(bytes);
                var group = db.RootGroup.FindGroup(id, true);
                if (group != null)
                {
                    return Task.FromResult(GroupToItemEntity(group));
                }

                var entry = db.RootGroup.FindEntry(id, true);
                if (entry != null)
                {
                    return Task.FromResult(EntryToItemEntity(entry));
                }
            }
            return Task.FromResult(default(ItemEntity));
        }

        public async Task<Item> Add(ItemInput item)
        {
            throw new NotImplementedException();
        }

        public async Task<Item> Update(Guid itemId, ItemInput item)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}

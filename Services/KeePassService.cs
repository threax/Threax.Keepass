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

            var first = db.RootGroup.Entries.First();

            var prot = first.Strings.Where(i => i.Key == config.Password).First();
        }
        public void Dispose()
        {
            db.Close();
        }

        private ItemEntity GroupToItemEntity(PwGroup group)
        {
            return new ItemEntity()
            {
                Created = group.CreationTime,
                Modified = group.LastAccessTime,
                IsGroup = true,
                ItemId = Convert.ToBase64String(group.Uuid.UuidBytes),
                Name = group.Name
            };
        }

        private IEnumerable<ItemEntity> GetItems(PwGroup group)
        {
            return group.Groups.Select(i => GroupToItemEntity(i));
        }

        public Task<IEnumerable<ItemEntity>> List(ItemQuery query)
        {
            var group = db.RootGroup;
            if (query.ItemId != null)
            {
                var bytes = Convert.FromBase64String(query.ItemId);
                var id = new PwUuid(bytes);
                group = group.FindGroup(id, true);
            }

            return Task.FromResult(GetItems(group));
        }

        public Task<ItemEntity> Get(String itemId)
        {
            var group = db.RootGroup;
            if (itemId != null)
            {
                var bytes = Convert.FromBase64String(itemId);
                var id = new PwUuid(bytes);
                group = group.FindGroup(id, true);
                if (group != null)
                {
                    return Task.FromResult(GroupToItemEntity(group));
                }
            }
            return Task.FromResult(default(ItemEntity));
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
    }
}

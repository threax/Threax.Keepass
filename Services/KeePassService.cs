using KeePassLib;
using KeePassLib.Collections;
using KeePassLib.Interfaces;
using KeePassLib.Keys;
using KeePassWeb.Database;
using KeePassWeb.InputModels;
using KeePassWeb.ViewModels;
using Nito.AsyncEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KeePassWeb.Services
{
    public class KeePassService : IKeePassService
    {
        private PwDatabase db;
        private KeePassConfig config;
        private IStatusLogger statusLogger;
        private readonly AsyncLock mutex = new AsyncLock();

        public KeePassService(KeePassConfig config, IStatusLogger statusLogger)
        {
            this.db = new PwDatabase();
            this.config = config;
            this.statusLogger = statusLogger;
        }

        public void Dispose()
        {
            if (db.IsOpen)
            {
                db.Close();
            }
        }

        public async Task Open(String password)
        {
            using (await mutex.LockAsync())
            {
                if (db.IsOpen)
                {
                    return;
                }

                var keys = new CompositeKey();
                keys.AddUserKey(new KcpPassword(password));

                db.Open(new KeePassLib.Serialization.IOConnectionInfo()
                {
                    Path = config.DbFile
                }, keys, statusLogger);
            }
        }

        public async Task Close()
        {
            using (await mutex.LockAsync())
            {
                if (db.IsOpen)
                {
                    db.Close();
                }
            }
        }

        public async Task<bool> IsOpen()
        {
            using (await mutex.LockAsync())
            {
                return db.IsOpen;
            }
        }

        public async Task<IEnumerable<ItemEntity>> List(ItemQuery query)
        {
            using (await mutex.LockAsync())
            {
                CheckDb();
                if (query.ItemId != null)
                {
                    var item = await Get(query.ItemId.Value);
                    return new ItemEntity[] { item };
                }

                if (query.Search != null)
                {
                    var resultList = new PwObjectList<PwEntry>();
                    db.RootGroup.SearchEntries(new SearchParameters()
                    {
                        SearchInTitles = true,
                        SearchString = query.Search
                    }, resultList);

                    return resultList.Select(i => EntryToItemEntity(i));
                }

                var group = db.RootGroup;
                if (query.ParentItemId != null)
                {
                    var bytes = query.ItemId.Value.ToByteArray();
                    var id = new PwUuid(bytes);
                    group = db.RootGroup.FindGroup(id, true);
                }

                return GetItems(group);
            }
        }

        public async Task<ItemEntity> Get(Guid itemId)
        {
            using (await mutex.LockAsync())
            {
                CheckDb();
                if (itemId != null)
                {
                    var bytes = itemId.ToByteArray();
                    var id = new PwUuid(bytes);
                    var group = db.RootGroup.FindGroup(id, true);
                    if (group != null)
                    {
                        return GroupToItemEntity(group);
                    }

                    var entry = db.RootGroup.FindEntry(id, true);
                    if (entry != null)
                    {
                        return EntryToItemEntity(entry);
                    }
                }
                return default(ItemEntity);
            }
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

        private static ItemEntity GroupToItemEntity(PwGroup i)
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

        private static ItemEntity EntryToItemEntity(PwEntry i)
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

        private static IEnumerable<ItemEntity> GetItems(PwGroup group)
        {
            return group.Groups.Select(i => GroupToItemEntity(i))
                .Concat(group.Entries.Select(i => EntryToItemEntity(i)));
        }

        private void CheckDb()
        {
            if (!db.IsOpen)
            {
                throw new KeePassDbClosedException("Keepass Database is closed.");
            }
        }
    }
}

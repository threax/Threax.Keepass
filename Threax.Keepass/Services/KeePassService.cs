using KeePassLib;
using KeePassLib.Collections;
using KeePassLib.Interfaces;
using KeePassLib.Keys;
using Threax.Keepass.Database;
using Threax.Keepass.InputModels;
using Threax.Keepass.ViewModels;
using Nito.AsyncEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KeePassLib.Security;

namespace Threax.Keepass.Services
{
    public class KeePassService : IKeePassService
    {
        private PwDatabase db;
        private KeePassConfig config;
        private IStatusLogger statusLogger;
        private readonly AsyncLock mutex = new AsyncLock();
        private Timer timer;
        private int timeoutMs = 10000;

        public KeePassService(KeePassConfig config, IStatusLogger statusLogger)
        {
            this.db = new PwDatabase();
            this.config = config;
            this.statusLogger = statusLogger;
            this.timeoutMs = config.TimeoutMs;
            this.timer = new Timer(s =>
            {
                using (mutex.Lock())
                {
                    if (db.IsOpen)
                    {
                        db.Close();
                    }
                }
            }, null, Timeout.Infinite, Timeout.Infinite);
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
            ResetTimer();

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
            ResetTimer();
            using (await mutex.LockAsync())
            {
                return db.IsOpen;
            }
        }

        public async Task<IEnumerable<ItemEntity>> List(ItemQuery query)
        {
            ResetTimer();
            using (await mutex.LockAsync())
            {
                CheckDb();

                if (query.ItemId != null)
                {
                    var item = DoGet(query.ItemId.Value);
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
                    var bytes = query.ParentItemId.Value.ToByteArray();
                    var id = new PwUuid(bytes);
                    group = db.RootGroup.FindGroup(id, true);
                }

                return GetItems(group);
            }
        }

        public async Task<ItemEntity> Get(Guid itemId)
        {
            ResetTimer();
            using (await mutex.LockAsync())
            {
                CheckDb();
                return DoGet(itemId);
            }
        }

        private ItemEntity DoGet(Guid itemId)
        {
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

        public async Task<EntryEntity> GetEntry(Guid itemId)
        {
            ResetTimer();
            using (await mutex.LockAsync())
            {
                CheckDb();
                return DoGetEntry(itemId);
            }
        }

        private EntryEntity DoGetEntry(Guid itemId)
        {
            if (itemId != null)
            {
                var bytes = itemId.ToByteArray();
                var id = new PwUuid(bytes);

                var entry = db.RootGroup.FindEntry(id, true);
                if (entry != null)
                {
                    return EntryToEntity(entry);
                }
            }
            return default(EntryEntity);
        }

        public async Task<String> GetPassword(Guid itemId)
        {
            ResetTimer();
            using (await mutex.LockAsync())
            {
                CheckDb();
                if (itemId != null)
                {
                    var bytes = itemId.ToByteArray();
                    var id = new PwUuid(bytes);

                    var entry = db.RootGroup.FindEntry(id, true);
                    if (entry != null)
                    {
                        return entry.Strings.Get("Password").ReadString();
                    }
                }
                return null;
            }
        }

        public async Task<EntryEntity> Add(EntryInput item)
        {
            ResetTimer();
            using (await mutex.LockAsync())
            {
                var entry = new PwEntry(true, true);
                entry = EntryInputToEntry(item, entry);
                db.RootGroup.AddEntry(entry, true);
                db.Save(statusLogger);
                return EntryToEntity(entry);
            }
        }

        public async Task<EntryEntity> Update(Guid itemId, EntryInput item)
        {
            ResetTimer();
            using (await mutex.LockAsync())
            {
                var bytes = itemId.ToByteArray();
                var id = new PwUuid(bytes);
                var entry = db.RootGroup.FindEntry(id, true);
                if (entry == null)
                {
                    throw new KeyNotFoundException($"Cannto find entry {itemId.ToString()}");
                }
                entry = EntryInputToEntry(item, entry);
                db.Save(statusLogger);
                return EntryToEntity(entry);
            }
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

        private static EntryEntity EntryToEntity(PwEntry i)
        {
            return new EntryEntity()
            {
                Created = i.CreationTime,
                Modified = i.LastAccessTime,
                ItemId = new Guid(i.Uuid.UuidBytes),
                Name = i.Strings.Get("Title").ReadString(),
                Notes = i.Strings.Get("Notes").ReadString(),
                Url = i.Strings.Get("URL").ReadString(),
                UserName = i.Strings.Get("UserName").ReadString(),
            };
        }

        private static PwEntry EntryInputToEntry(EntryInput i, PwEntry entry)
        {
            entry.LastModificationTime = DateTime.Now;
            entry.Strings.Set("Title", new ProtectedString(false, i.Name));
            entry.Strings.Set("Notes", new ProtectedString(false, i.Notes));
            entry.Strings.Set("URL", new ProtectedString(false, i.Url));
            entry.Strings.Set("UserName", new ProtectedString(false, i.UserName));

            return entry;
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

        private void ResetTimer()
        {
            timer.Change(timeoutMs, Timeout.Infinite);
        }
    }
}

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

        public async Task<Entry> GetEntry(Guid itemId)
        {
            ResetTimer();
            using (await mutex.LockAsync())
            {
                CheckDb();
                return DoGetEntry(itemId);
            }
        }

        private Entry DoGetEntry(Guid itemId)
        {
            if (itemId != null)
            {
                var bytes = itemId.ToByteArray();
                var id = new PwUuid(bytes);

                var entry = db.RootGroup.FindEntry(id, true);
                if (entry != null)
                {
                    return EntryToView(entry);
                }
            }
            return default(Entry);
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
                        return entry.Strings.Get("Password")?.ReadString();
                    }
                }
                return null;
            }
        }

        public async Task<ItemEntity> Add(Guid? parent, ItemInput item)
        {
            ResetTimer();
            using (await mutex.LockAsync())
            {
                var entry = new PwEntry(true, true);
                entry = ItemInputToEntry(item, entry);
                var group = db.RootGroup;
                if(parent != null)
                {
                    var bytes = parent.Value.ToByteArray();
                    var id = new PwUuid(bytes);
                    group = db.RootGroup.FindGroup(id, true);
                    if(group == null)
                    {
                        throw new InvalidOperationException($"Cannot find group {parent}.");
                    }
                }
                group.AddEntry(entry, true);
                db.Save(statusLogger);
                return EntryToItemEntity(entry);
            }
        }

        public async Task<ItemEntity> Update(Guid itemId, ItemInput item)
        {
            ResetTimer();
            using (await mutex.LockAsync())
            {
                PwEntry entry = GetEntryFromGuid(itemId);
                entry = ItemInputToEntry(item, entry);
                db.Save(statusLogger);
                return EntryToItemEntity(entry);
            }
        }

        public async Task Delete(Guid id)
        {
            ResetTimer();
            using (await mutex.LockAsync())
            {
                PwEntry entry = GetEntryFromGuid(id);
                var group = entry.ParentGroup.Entries.Remove(entry);
                db.Save(statusLogger);
            }
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
                Name = i.Strings.Get("Title")?.ReadString()
            };
        }

        private static Entry EntryToView(PwEntry i)
        {
            return new Entry()
            {
                Created = i.CreationTime,
                Modified = i.LastAccessTime,
                ItemId = new Guid(i.Uuid.UuidBytes),
                Name = i.Strings.Get("Title")?.ReadString(),
                Notes = i.Strings.Get("Notes")?.ReadString(),
                Url = i.Strings.Get("URL")?.ReadString(),
                UserName = i.Strings.Get("UserName")?.ReadString(),
            };
        }

        private static PwEntry ItemInputToEntry(ItemInput i, PwEntry entry)
        {
            entry.LastModificationTime = DateTime.UtcNow;
            UpdateString(entry, "Title", false, i.Name);
            UpdateString(entry, "Notes", false, i.Notes);
            UpdateString(entry, "URL", false, i.Url);
            UpdateString(entry, "UserName", false, i.UserName);
            return entry;
        }

        private static void UpdateString(PwEntry entry, String name, bool protect, String value)
        {
            if (value != null)
            {
                entry.Strings.Set(name, new ProtectedString(protect, value));
            }
            else
            {
                entry.Strings.Remove(name);
            }
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

        private PwEntry GetEntryFromGuid(Guid itemId)
        {
            var bytes = itemId.ToByteArray();
            var id = new PwUuid(bytes);
            var entry = db.RootGroup.FindEntry(id, true);
            if (entry == null)
            {
                throw new KeyNotFoundException($"Cannto find entry {itemId.ToString()}");
            }

            return entry;
        }
    }
}

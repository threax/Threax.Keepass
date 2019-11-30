using Microsoft.EntityFrameworkCore;

namespace Threax.Keepass.Database
{
    public partial class AppDbContext
    {
        public DbSet<EntryEntity> Entries { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;

namespace KeePassWeb.Database
{
    public partial class AppDbContext
    {
        public DbSet<ItemEntity> Items { get; set; }
    }
}

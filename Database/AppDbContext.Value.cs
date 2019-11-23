using Microsoft.EntityFrameworkCore;

namespace KeePassWeb.Database
{
    public partial class AppDbContext
    {
        public DbSet<ValueEntity> Values { get; set; }
    }
}

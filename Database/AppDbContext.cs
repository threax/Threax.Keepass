using Microsoft.EntityFrameworkCore;
using Threax.AspNetCore.UserBuilder.Entities;

namespace KeePassWeb.Database
{
    /// <summary>
    /// By default the app db context extends the UsersDbContext from Authorization. 
    /// This gives it the Users, Roles and UsersToRoles tables.
    /// </summary>
    public partial class AppDbContext : UsersDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        //The dbset declarations are in the other parial classes. Expand the AppDbContext.cs class node to see them.
    }
}

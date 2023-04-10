using Microsoft.EntityFrameworkCore;

namespace UsersAPI
{
    public class UserDb : DbContext
    {
        public UserDb(DbContextOptions<UserDb> options) : base(options) { }

        public DbSet<Model.User> Users => Set<Model.User>();
    }
}

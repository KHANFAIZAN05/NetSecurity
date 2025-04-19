using Authentication.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}
        public DbSet<User> Users => Set<User>();
    }
}

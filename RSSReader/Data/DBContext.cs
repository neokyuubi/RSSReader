using Microsoft.EntityFrameworkCore;
using RSSReader.Models;

namespace RSSReader.Data
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Feed> Feeds { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;

namespace GreenFarm.Models
{
    public class Database : DbContext
    {
        public Database()
        {
            Database.EnsureCreated();
        }

        public Database(DbContextOptions<Database> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql("Server=157.230.97.233;Port=6606;Database=GF;Uid=pro;Pwd=rsE>9S^2Fu:kNVc:");
            }
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Item> Items { get; set; }



    }
}

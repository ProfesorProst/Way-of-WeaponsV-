using System.Data.Entity;
using wayofweapon.Entities;

namespace wayofweapon.Data
{
    class Context : DbContext
    {
        public Context()
            : base("DbConnection")
        {
            
        }

        public DbSet<Guild> guilds { get; set; }
        public DbSet<Item> items { get; set; }
        public DbSet<Inventory> inventories { get; set; }
        public DbSet<Person> people { get; set; }
        public DbSet<User> users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}

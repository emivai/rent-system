using Microsoft.EntityFrameworkCore;
using RentSystem.Core.Entities;

namespace RentSystem.Repositories
{
    public class RentDBContext : DbContext
    {
        public RentDBContext()
        {
        }

        public RentDBContext(DbContextOptions<RentDBContext> options) : base(options)
        {
        }

        public virtual DbSet<Advert> Adverts { get; set; }
        public virtual DbSet<Item> Items { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Advert>().HasMany(a => a.Items).WithOne(i => i.Advert).OnDelete(DeleteBehavior.Cascade);
        }
    }
}

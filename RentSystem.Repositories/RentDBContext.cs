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
        public virtual DbSet<Reservation> Reservations { get; set; }
        public virtual DbSet<Contract> Contracts { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Request> Requests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Advert>().HasMany(a => a.Items).WithOne(i => i.Advert).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Advert>().HasOne(a => a.User).WithMany(u => u.Adverts);

            modelBuilder.Entity<Item>().HasOne(i => i.User).WithMany(u => u.Items);

            modelBuilder.Entity<Reservation>().HasOne(r => r.Item).WithOne(i => i.Reservation).HasForeignKey<Reservation>(r => r.ItemId);
            modelBuilder.Entity<Reservation>().HasOne(r => r.User).WithMany(u => u.Reservations);

            modelBuilder.Entity<Contract>().HasOne(c => c.Item).WithOne(i => i.Contract).HasForeignKey<Contract>(c => c.ItemId); ;
            modelBuilder.Entity<Contract>().HasOne(c => c.Owner).WithMany(o => o.OwnerContracts);
            modelBuilder.Entity<Contract>().HasOne(c => c.Renter).WithMany(r => r.RenterContracts);

            modelBuilder.Entity<Request>().HasOne(r => r.User).WithMany(u => u.Requests);
        }
    }
}

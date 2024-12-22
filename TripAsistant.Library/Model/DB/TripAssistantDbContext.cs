

using Microsoft.EntityFrameworkCore;

namespace TripAssistant.Library.Model.DB
{
    public class TripAssistantDbContext : DbContext
    {

        public TripAssistantDbContext(DbContextOptions<TripAssistantDbContext> options) : base(options)
        {

        }
        public virtual DbSet<Trip> Trips { get; set; }
        public virtual DbSet<TripMember> TripMembers { get; set; }
        public virtual DbSet<TripUser> Users { get; set; }
        public virtual DbSet<TripTransaction> Transactions { get; set; }
        public virtual DbSet<TripDebtor> TripDebtors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TripDebtor>(p => p.HasKey(k => new { k.IdTripTransaction, k.IdDebtor }));
            modelBuilder.Entity<TripUser>(p => p.HasKey(k => new { k.IdTrip, k.IdUser }));

        }
    }
}

using HRApplicationAPI.Models.DbModels;
using Microsoft.EntityFrameworkCore;

namespace HRApplicationAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<UserEvent> UserEvents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasKey(u => new { u.UserId });

            modelBuilder.Entity<Event>()
                .HasKey(e => new { e.Id });

            modelBuilder.Entity<UserEvent>()
                .HasKey(ue => new { ue.UserId, ue.EventId });

            modelBuilder.Entity<UserEvent>()
                .HasOne(ue => ue.User)
                .WithMany(u => u.UserEvents)
                .HasForeignKey(u => u.UserId);

            modelBuilder.Entity<UserEvent>()
                .HasOne(uc => uc.Event)
                .WithMany(e => e.UserEvents)
                .HasForeignKey(e => e.EventId);
        }
    }
}

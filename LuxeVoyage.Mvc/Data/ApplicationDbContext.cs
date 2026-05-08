using LuxeVoyage.Mvc.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LuxeVoyage.Mvc.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Destination> Destinations => Set<Destination>();
    public DbSet<Experience> Experiences => Set<Experience>();
    public DbSet<Tour> Tours => Set<Tour>();
    public DbSet<Stay> Stays => Set<Stay>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<Favorite> Favorites => Set<Favorite>();
    public DbSet<Attraction> Attractions => Set<Attraction>();
    public DbSet<CartItem> CartItems => Set<CartItem>();
    public DbSet<Payment> Payments => Set<Payment>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Destination>()
            .HasIndex(d => d.Slug)
            .IsUnique();

        builder.Entity<Experience>()
            .HasIndex(e => e.Slug)
            .IsUnique();

        builder.Entity<Tour>()
            .HasIndex(t => t.Slug)
            .IsUnique();

        builder.Entity<Stay>()
            .HasIndex(s => s.Slug)
            .IsUnique();

        builder.Entity<Favorite>()
            .HasIndex(f => new { f.UserId, f.TargetKind, f.TargetId })
            .IsUnique();

        builder.Entity<Booking>()
            .HasOne(b => b.User)
            .WithMany()
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Booking>()
            .HasOne(b => b.Tour)
            .WithMany()
            .HasForeignKey(b => b.TourId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Entity<Booking>()
            .HasOne(b => b.Stay)
            .WithMany()
            .HasForeignKey(b => b.StayId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Entity<Booking>()
            .HasOne(b => b.Experience)
            .WithMany()
            .HasForeignKey(b => b.ExperienceId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Entity<Booking>()
            .HasOne(b => b.Destination)
            .WithMany()
            .HasForeignKey(b => b.DestinationId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Entity<Notification>()
            .HasOne(n => n.User)
            .WithMany()
            .HasForeignKey(n => n.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Notification>()
            .HasOne(n => n.Reservation)
            .WithMany()
            .HasForeignKey(n => n.ReservationId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Entity<CartItem>()
            .HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<CartItem>()
            .HasIndex(c => new { c.UserId, c.ItemType, c.ItemId });

        builder.Entity<Payment>()
            .HasOne(p => p.Booking)
            .WithMany(b => b.Payments)
            .HasForeignKey(p => p.BookingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Payment>()
            .HasOne(p => p.User)
            .WithMany()
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Payment>()
            .HasIndex(p => p.BookingId);

        builder.Entity<Payment>()
            .HasIndex(p => new { p.BookingId, p.Status });
    }
}

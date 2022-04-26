using Microsoft.EntityFrameworkCore;
using ReservationSystem_Server.Data.Visual;
using ReservationSystem_Server.Utility.DataSeed;

namespace ReservationSystem_Server.Data;

public class ApplicationDbContext : VisualDbContext
{
    public DbSet<Person> People => Set<Person>();
    public DbSet<Customer> Customers => Set<Customer>();

    public DbSet<Reservation> Reservations => Set<Reservation>();
    public DbSet<ReservationOrigin> ReservationOrigins => Set<ReservationOrigin>();
    public DbSet<ReservationStatus> ReservationStatuses => Set<ReservationStatus>();

    public DbSet<Restaurant> Restaurants => Set<Restaurant>();
    public DbSet<RestaurantArea> RestaurantAreas => Set<RestaurantArea>();

    public DbSet<Sitting> Sittings => Set<Sitting>();
    public DbSet<SittingType> SittingTypes => Set<SittingType>();
    public DbSet<Table> Tables => Set<Table>();

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Reservation>().HasOne(r => r.Sitting)
            .WithMany(s => s.Reservations).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Reservation>().HasOne(r => r.Customer)
            .WithMany(c => c.Reservations).OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Reservation>().HasOne(r => r.ReservationOrigin)
            .WithMany().OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Reservation>().HasOne(r => r.ReservationStatus)
            .WithMany().OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Customer>().HasOne(c => c.User)
            .WithOne().OnDelete(DeleteBehavior.SetNull);
        
        builder.Entity<RestaurantArea>().HasOne(r => r.Restaurant)
            .WithMany(r => r.Areas).OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Sitting>().HasOne(s => s.SittingType)
            .WithMany().OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Sitting>().HasOne(s => s.Restaurant)
            .WithMany(r => r.Sittings).OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Table>().HasOne(t => t.Area)
            .WithMany(r => r.Tables).OnDelete(DeleteBehavior.Restrict);


        DataSeeder.Seed(builder);
    }
}
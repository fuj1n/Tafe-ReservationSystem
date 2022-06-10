using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReservationSystem_Server.Data.Visual.Layout;

namespace ReservationSystem_Server.Data.Visual;

public abstract class VisualDbContext : IdentityDbContext
{
    public DbSet<ReservationStatusVisual> ReservationStatusVisuals => Set<ReservationStatusVisual>();
    public DbSet<RestaurantCarouselItemVisual> RestaurantCarouselItemVisuals => Set<RestaurantCarouselItemVisual>();
    
    // Layout
    public DbSet<RectangleVisual> RectangleVisuals => Set<RectangleVisual>();
    public DbSet<RestaurantAreaVisual> RestaurantAreaVisuals => Set<RestaurantAreaVisual>();

    protected VisualDbContext(DbContextOptions options)
        : base(options)
    {
    }
}
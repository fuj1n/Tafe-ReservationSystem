﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ReservationSystem_Server.Data.Visual;

public abstract class VisualDbContext : IdentityDbContext
{
    public DbSet<ReservationStatusVisual> ReservationStatusVisuals => Set<ReservationStatusVisual>();
    
    public VisualDbContext(DbContextOptions options)
        : base(options)
    {
    }
}
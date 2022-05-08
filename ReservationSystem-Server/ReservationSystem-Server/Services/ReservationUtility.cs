using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReservationSystem_Server.Data;
using ReservationSystem_Server.Data.Visual;

namespace ReservationSystem_Server.Services;

[PublicAPI]
public class ReservationUtility
{
    private readonly ApplicationDbContext _context;

    public ReservationUtility(ApplicationDbContext context)
    {
        _context = context;
    }

    /**
     * Gets all time slots between <paramref name="startTime"/> and <paramref name="endTime"/>
     * in <paramref name="slotLength"/> increments
     */
    public List<DateTime> GetTimeSlots(DateTime startTime, DateTime endTime, TimeSpan slotLength)
    {
        List<DateTime> timeSlots = new();

        TimeSpan sittingDuration = endTime - startTime;

        for (TimeSpan time = new(0); time < sittingDuration; time += slotLength)
        {
            timeSlots.Add(startTime + time);
        }

        return timeSlots;
    }

    public async Task<Dictionary<int, ReservationStatusVisual>> GetReservationStatusVisualsAsync()
    {
        return await _context.ReservationStatusVisuals
            .ToDictionaryAsync(v => v.Id, v => v);
    }

    /// <summary>
    /// Gets all reservation origins from database
    /// </summary>
    /// <returns>Array of reservation origins</returns>
    public async Task<ReservationOrigin[]> GetOriginsAsync()
    {
        return await _context.ReservationOrigins.ToArrayAsync();
    }

    /// <summary>
    /// Gets a reservation origin from database by id
    /// </summary>
    /// <param name="id">The id of the origin to get</param>
    /// <returns>A reservation origin mapped to that id, or null</returns>
    public async Task<ReservationOrigin?> GetOriginAsync(int id)
    {
        return await _context.ReservationOrigins.FindAsync(id);
    }
    
    /// <summary>
    /// Get all reservation origins from database and use them to populate a select list
    /// </summary>
    /// <returns>A select list populated with all reservation origins</returns>
    public async Task<SelectList> GetOriginsAsSelectListAsync()
    {
        return new SelectList(await GetOriginsAsync(), 
            nameof(ReservationOrigin.Id), 
            nameof(ReservationOrigin.Description));
    }
}
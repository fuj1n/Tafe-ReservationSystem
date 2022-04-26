using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using ReservationSystem_Server.Data;
using ReservationSystem_Server.Data.Visual;
using ReservationSystem_Server.Models;

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

        //TODO: configurable time slot length
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
}
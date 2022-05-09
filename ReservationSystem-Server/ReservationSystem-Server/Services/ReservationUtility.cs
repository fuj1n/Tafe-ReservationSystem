using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReservationSystem_Server.Data;
using ReservationSystem_Server.Data.Visual;

namespace ReservationSystem_Server.Services;

[PublicAPI]
public class ReservationUtility
{
    private readonly ApplicationDbContext _context;
    private readonly SittingUtility _sittingUtility;

    public ReservationUtility(ApplicationDbContext context, SittingUtility sittingUtility)
    {
        _context = context;
        _sittingUtility = sittingUtility;
    }

    /// <summary>
    /// Gets all reservations for a given sitting
    /// </summary>
    /// <param name="sittingId">The ID of the sitting to retrieve reservations for</param>
    /// <returns>All reservations for given sitting</returns>
    public async Task<Reservation[]> GetReservationsForSittingAsync(int sittingId)
    {
        return await _context.Reservations.Where(r => r.SittingId == sittingId).ToArrayAsync();
    }

    /// <summary>
    /// Gets all reservations created by a given user
    /// </summary>
    /// <param name="userId">The user the sittings were created by</param>
    /// <returns>All reservations by given user</returns>
    public async Task<Reservation[]> GetReservationsForUserAsync(string userId)
    {
        return await _context.Reservations.Include(r => r.Customer)
                .Where(r => r.Customer.UserId == userId).ToArrayAsync();
    }

    /// <summary>
    /// Gets all reservations created between <paramref name="start"/> and <paramref name="end"/>
    /// </summary>
    /// <remarks>
    /// Compares both the start and end times of the reservation to the given times on both ends.
    /// </remarks>
    /// <param name="start">The left boundary</param>
    /// <param name="end">The right boundary</param>
    /// <returns>The reservations for a given time range</returns>
    public async Task<Reservation[]> GetReservationsForTimeRangeAsync(DateTime start, DateTime end)
    {
        return await _context.Reservations.Where(r => r.StartTime >= start && r.StartTime <= end
                                                                           && r.EndTime >= start && r.EndTime <= end).ToArrayAsync();
    }

    /// <summary>
    /// Gets a reservation by the given <paramref name="id"/>
    /// </summary>
    /// <param name="id">The id of the reservation to retrieve</param>
    /// <returns>The requested reservation or null if it does not exist</returns>
    public async Task<Reservation?> GetReservationAsync(int id)
    {
        return await _context.Reservations.FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task ValidateReservationAsync(Reservation reservation, ModelStateDictionary modelState, bool requireAvailable)
    {
        if (reservation.CustomerId == 0)
            modelState.AddModelError(nameof(Reservation.CustomerId), "Customer ID must be set");

        Sitting? sitting = await _sittingUtility.GetSittingAsync(reservation.SittingId);

        if (sitting == null)
        {
            modelState.AddModelError(nameof(Reservation.SittingId), $"The sitting with ID {reservation.SittingId} does not exist");
            return; // All below validations require the sitting to be found
        }

        if(requireAvailable && _sittingUtility.EvaluateAvailability(sitting))
            modelState.AddModelError(nameof(Reservation.SittingId), "The given sitting is not available");
    }
    
    /// <summary>
    /// Adds the given <paramref name="reservation"/> to the database
    /// </summary>
    /// <remarks>
    /// This method does not validate the reservation, so ensure it is done first so as not to cause database errors
    /// </remarks>
    /// <param name="reservation">The reservation to add</param>
    public async Task CreateReservationAsync(Reservation reservation)
    {
        _context.Reservations.Add(reservation);
        await _context.SaveChangesAsync();
    }
    
    /// <summary>
    /// Updates the given <paramref name="reservation"/> in the database
    /// </summary>
    /// <remarks>
    /// This method does not validate the sitting, so ensure it is done first so as not to cause database errors
    /// </remarks>
    /// <param name="reservation">The reservation to update</param>
    /// <exception cref="ArgumentException">If trying to move the reservation to another sitting or reservation is not found</exception>
    public async Task EditReservationAsync(Reservation reservation)
    {
        Reservation? original = await GetReservationAsync(reservation.Id);
        
        if(original == null)
            throw new ArgumentException("The original reservation does not exist");
        
        if (original.SittingId != reservation.SittingId)
            throw new ArgumentException("Cannot move reservation between different sittings");
        
        _context.Reservations.Update(reservation);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Gets all time slots between <paramref name="startTime"/> and <paramref name="endTime"/>
    /// in <paramref name="slotLength"/> increments
    /// </summary>
    /// <param name="startTime">The left bound of the calculation</param>
    /// <param name="endTime">The right bound of the calculation</param>
    /// <param name="slotLength">The step distance for the calculation</param>
    /// <returns>A list of the calculated time slots</returns>
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

    /// <summary>
    /// Fetches the reservation status visual information from the database
    /// </summary>
    /// <returns>A dictionary mapping ReservationStatusId to a given reservation status visual entity</returns>
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
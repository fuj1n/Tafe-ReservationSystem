using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReservationSystem_Server.Data;
using ReservationSystem_Server.Data.Visual;
using ReservationSystem_Server.Helper;

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
    /// <param name="filter">The query to inject before the where clause (usually used for includes)</param>
    /// <returns>All reservations for given sitting</returns>
    public async Task<Reservation[]> GetReservationsForSittingAsync(int sittingId,
        Func<IQueryable<Reservation>, IQueryable<Reservation>>? filter = null)
    {
        return await _context.Reservations.ApplyFilter(filter).Where(r => r.SittingId == sittingId).ToArrayAsync();
    }

    /// <summary>
    /// Gets all reservations created by a given user
    /// </summary>
    /// <param name="userId">The user the sittings were created by</param>
    /// <param name="filter">The query to inject before the where clause (usually used for includes)</param>
    /// <returns>All reservations by given user</returns>
    public async Task<Reservation[]> GetReservationsForUserAsync(string userId,
        Func<IQueryable<Reservation>, IQueryable<Reservation>>? filter = null)
    {
        return await _context.Reservations.ApplyFilter(filter).Include(r => r.Customer)
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
    /// <param name="filter">The query to inject before the where clause (usually used for includes)</param>
    /// <returns>The reservations for a given time range</returns>
    public async Task<Reservation[]> GetReservationsForTimeRangeAsync(DateTime start, DateTime end,
        Func<IQueryable<Reservation>, IQueryable<Reservation>>? filter = null)
    {
        return await _context.Reservations.ApplyFilter(filter)
            .Where(r => r.StartTime >= start && r.StartTime <= end
                                             && r.EndTime >= start && r.EndTime <= end).ToArrayAsync();
    }

    /// <summary>
    /// Gets a reservation by the given <paramref name="id"/>
    /// </summary>
    /// <param name="id">The id of the reservation to retrieve</param>
    /// <param name="filter">The query to inject before the where clause (usually used for includes)</param>
    /// <returns>The requested reservation or null if it does not exist</returns>
    public async Task<Reservation?> GetReservationAsync(int id,
        Func<IQueryable<Reservation>, IQueryable<Reservation>>? filter = null)
    {
        return await _context.Reservations.ApplyFilter(filter).FirstOrDefaultAsync(r => r.Id == id);
    }

    /// <summary>
    /// Validates the reservation against business rules
    /// </summary>
    /// <param name="reservation">The reservation to validate</param>
    /// <param name="modelState">The model state to store result in</param>
    /// <param name="requireAvailable">Whether the availability validation runs (shouldn't for edit operation)</param>
    /// <param name="allowDurationAlter">Whether to allow the duration to be altered from the default value</param>
    public async Task ValidateReservationAsync(Reservation reservation, ModelStateDictionary modelState,
        bool requireAvailable, bool allowDurationAlter = false)
    {
        Sitting? sitting = await _sittingUtility.GetSittingAsync(reservation.SittingId);

        if (await GetOriginAsync(reservation.ReservationOriginId) == null)
            modelState.AddModelError(nameof(Reservation.ReservationOriginId),
                $"The origin with ID {reservation.ReservationOriginId} does not exist");

        if (await GetStatusAsync(reservation.ReservationStatusId) == null)
            modelState.AddModelError(nameof(reservation.ReservationStatusId),
                $"The status with ID {reservation.ReservationStatusId} does not exist");

        if (sitting == null)
        {
            modelState.AddModelError(nameof(Reservation.SittingId),
                $"The sitting with ID {reservation.SittingId} does not exist");
            return; // All below validations require the sitting to be found
        }

        if (requireAvailable && _sittingUtility.EvaluateAvailability(sitting))
            modelState.AddModelError(nameof(Reservation.SittingId), "The given sitting is not available");
        
        if (!GetTimeSlots(sitting.StartTime, sitting.EndTime, sitting.DefaultDuration).Contains(reservation.StartTime))
            modelState.AddModelError(nameof(Reservation.StartTime),
                $"The start time must be between {sitting.StartTime} and {sitting.EndTime} in {sitting.DefaultDuration} intervals");
        
        if(!allowDurationAlter && reservation.Duration != sitting.DefaultDuration)
            modelState.AddModelError(nameof(Reservation.Duration),
                $"The duration must be {sitting.DefaultDuration}");
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

        if (original == null)
            throw new ArgumentException("The original reservation does not exist");

        if (original.SittingId != reservation.SittingId)
            throw new ArgumentException("Cannot move reservation between different sittings");

        original.StartTime = reservation.StartTime;
        original.Duration = reservation.Duration;
        original.Notes = reservation.Notes;
        original.NumberOfPeople = reservation.NumberOfPeople;
        original.CustomerId = reservation.CustomerId;
        original.ReservationStatusId = reservation.ReservationStatusId;
        original.ReservationOriginId = reservation.ReservationOriginId;
        original.Tables = reservation.Tables;
        original.SecurityStamp = reservation.SecurityStamp;
        
        _context.Reservations.Update(original);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Gets all time slots between <paramref name="startTime"/> and <paramref name="endTime"/>
    /// in <paramref name="slotLength"/> increments
    /// </summary>
    /// <remarks>
    /// Doesn't check that <paramref name="endTime"/> is after <paramref name="startTime"/>,
    /// so in those situations, an empty list will be returned
    /// </remarks>
    /// <param name="startTime">The left bound of the calculation</param>
    /// <param name="endTime">The right bound of the calculation</param>
    /// <param name="slotLength">The step distance for the calculation</param>
    /// <returns>A list of the calculated time slots</returns>
    public List<DateTime> GetTimeSlots(DateTime startTime, DateTime endTime, TimeSpan slotLength)
    {
        List<DateTime> timeSlots = new();

        TimeSpan sittingDuration = endTime - startTime;
        TimeSpan lastTime = new(-1);
        
        for (TimeSpan time = new(0); time < sittingDuration; time += slotLength)
        {
            if (lastTime == time)
                throw new ArgumentException("Time not advancing, slot length probably too small");
            lastTime = time;
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

    /// <summary>
    /// Gets all reservation statuses from database
    /// </summary>
    /// <returns>Array of all reservation statuses</returns>
    public async Task<ReservationStatus[]> GetStatusesAsync()
    {
        return await _context.ReservationStatuses.ToArrayAsync();
    }

    /// <summary>
    /// Gets a reservation status from database by id
    /// </summary>
    /// <param name="id">The id of the status to get</param>
    /// <returns>A reservation status mapped to that id, or null</returns>
    public async Task<ReservationStatus?> GetStatusAsync(int id)
    {
        return await _context.ReservationStatuses.FindAsync(id);
    }
}
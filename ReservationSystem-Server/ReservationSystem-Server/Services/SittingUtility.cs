using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using ReservationSystem_Server.Data;
using ReservationSystem_Server.Helper;

namespace ReservationSystem_Server.Services;

[PublicAPI]
public class SittingUtility
{
    private readonly ApplicationDbContext _context;

    public SittingUtility(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Gets all sittings based on the provided criteria
    /// </summary>
    /// <param name="pastSittings">Whether past sittings are included</param>
    /// <param name="closedSittings">Whether closed sittings are included</param>
    /// <param name="filter">The query to inject before the where clause (usually used for includes)</param>
    /// <returns>An array of sittings that meets the provided criteria</returns>
    public async Task<Sitting[]> GetSittingsAsync(bool pastSittings = false, bool closedSittings = false,
        Func<IQueryable<Sitting>, IQueryable<Sitting>>? filter = null)
    {
        return await _context.Sittings.ApplyFilter(filter)
            .OrderBy(s => s.StartTime)
            .Where(s => (pastSittings || s.EndTime >= DateTime.Now) && (closedSittings || !s.IsClosed)) // Inline as can't call method from query
            .ToArrayAsync();
    }

    /// <summary>
    /// Gets the sitting with the given id.
    /// </summary>
    /// <param name="id">The ID of the sitting</param>
    /// <param name="filter">The query to inject before the where clause (usually used for includes)</param>
    /// <returns></returns>
    public async Task<Sitting?> GetSittingAsync(int id, Func<IQueryable<Sitting>, IQueryable<Sitting>>? filter = null)
    {
        return await _context.Sittings.ApplyFilter(filter).Where(s => s.Id == id).FirstOrDefaultAsync();
    }

    /// <summary>
    /// Validates that the sitting is valid and places all the errors into the given <paramref name="modelState"/>
    /// </summary>
    /// <param name="sitting">The sitting to validate</param>
    /// <param name="modelState">The model state to place validation errors into</param>
    /// <param name="requireFuture">Whether to require that the sitting is in the future,
    /// generally should be <see langword="true"/> for create, but <see langword="false"/> for update</param>
    public async Task ValidateSittingAsync(Sitting sitting, ModelStateDictionary modelState, bool requireFuture = true)
    {
        if (requireFuture && sitting.StartTime < DateTime.Now)
            modelState.AddModelError(nameof(Sitting.StartTime), "The sitting must start in the future");
        if (sitting.StartTime > sitting.EndTime)
            modelState.AddModelError("", "Start time must be before end time");
        if (await GetSittingTypeAsync(sitting.SittingTypeId) == null)
            modelState.AddModelError(nameof(Sitting.SittingTypeId), "The sitting type specified does not exist");
        if (sitting.Capacity < 0)
            modelState.AddModelError(nameof(Sitting.Capacity), "The sitting must have a positive capacity");
    }

    /// <summary>
    /// Adds a given sitting to the database
    /// </summary>
    /// <remarks>
    /// This method does not validate the sitting, so ensure it is done first so as not to cause database errors
    /// </remarks>
    /// <param name="sitting">The sitting to be added</param>
    public async Task CreateSittingAsync(Sitting sitting)
    {
        _context.Sittings.Add(sitting);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Updates a given sitting in the database
    /// </summary>
    /// <remarks>
    /// This method does not validate the sitting, so ensure it is done first so as not to cause database errors
    /// </remarks>
    /// <param name="sitting">The sitting to be updated</param>
    /// <throws cref="ArgumentNullException">If the sitting does not exist</throws>
    public async Task EditSittingAsync(Sitting sitting)
    {
        Sitting? original = await GetSittingAsync(sitting.Id);
        
        if(original == null)
            throw new ArgumentException("The sitting does not exist");

        original.StartTime = sitting.StartTime;
        original.EndTime = sitting.EndTime;
        original.IsClosed = sitting.IsClosed;
        original.Capacity = sitting.Capacity;
        original.SittingTypeId = sitting.SittingTypeId;
        original.RestaurantId = sitting.RestaurantId;
        
        _context.Sittings.Update(original);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Returns true if the sitting matches the given criteria.
    /// </summary>
    /// <param name="sitting">The sitting to evaluate</param>
    /// <param name="allowPast">Whether sittings in the past are allowed</param>
    /// <param name="allowClosed">Whether closed sittings are allowed</param>
    /// <returns></returns>
    public bool EvaluateAvailability(Sitting sitting, bool allowPast = false, bool allowClosed = false)
    {
        return (allowPast || sitting.EndTime >= DateTime.Now) && (allowClosed || !sitting.IsClosed);
    }

    /// <summary>
    /// Gets all existing sitting types
    /// </summary>
    /// <returns>The sitting types</returns>
    public async Task<SittingType[]> GetSittingTypesAsync()
    {
        return await _context.SittingTypes.ToArrayAsync();
    }

    /// <summary>
    /// Gets a sitting type by its id
    /// </summary>
    /// <param name="id">The id of the sitting type to get</param>
    /// <returns>The sitting type or null if not found</returns>
    public async Task<SittingType?> GetSittingTypeAsync(int id)
    {
        return await _context.SittingTypes.FirstOrDefaultAsync(s => s.Id == id);
    }
}
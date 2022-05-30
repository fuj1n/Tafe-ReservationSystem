using ReservationSystem_Server.Data;

namespace ReservationSystem_Server.Services;

public class RestaurantProvider
{
    private const int RestaurantId = 1; // Multi-tenant ready
    private ApplicationDbContext _context;

    public RestaurantProvider(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Gets the restaurant information
    /// </summary>
    /// <remarks>
    /// Does not implicitly include any extra information
    /// </remarks>
    /// <param name="id">The ID of the restaurant to get</param>
    /// <returns>The restaurant information</returns>
    public async Task<Restaurant?> GetRestaurantDataAsync(int id = RestaurantId)
    {
        return await _context.Restaurants.FindAsync(id);
    }
}
using Microsoft.EntityFrameworkCore;
using ReservationSystem_Server.Data;

namespace ReservationSystem_Server.Services;

public class SittingUtility
{
    private readonly ApplicationDbContext _context;

    public SittingUtility(ApplicationDbContext context)
    {
        _context = context;
    }

    public IQueryable<Sitting> GetSittings(bool pastSittings = false, bool closedSittings = false)
    {
        return _context.Sittings.Where(s => (pastSittings || s.EndTime >= DateTime.Now) && (closedSittings || !s.IsClosed));
    }
    
    public async Task<Sitting?> GetSittingAsync(int id, Func<IQueryable<Sitting>, IQueryable<Sitting>>? filter = null)
    {
        IQueryable<Sitting> query = _context.Sittings;
        if (filter != null)
        {
            query = filter(_context.Sittings);
        }

        return await query.Where(s => s.Id == id).FirstOrDefaultAsync();
    }
}
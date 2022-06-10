using Microsoft.EntityFrameworkCore;
using ReservationSystem_Server.Areas.Admin.Models;
using ReservationSystem_Server.Data;

namespace ReservationSystem_Server.Services;

public class LayoutUtility
{
    private readonly ApplicationDbContext _context;

    public LayoutUtility(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<LayoutModel> BuildLayoutModel()
    {
        Table[] tables = await _context.Tables.ToArrayAsync();
        LayoutModel model = new()
        {
            Areas = await _context.RestaurantAreas
                .Select(a =>
                    new LayoutModel.Area
                    {
                        Id = a.Id,
                        Name = a.Name,
                        Rect = LayoutModel.Rect.FromRectangleVisual(
                            _context.RestaurantAreaVisuals
                                .Select(v => new {v.AreaId, v.Rect})
                                .FirstOrDefault(v => v.AreaId == a.Id)!.Rect)
                    })
                .ToArrayAsync(),
            Tables = tables.Aggregate(new Dictionary<int, List<LayoutModel.Table>>(),
                (acc, t) =>
                {
                    if (!acc.ContainsKey(t.AreaId))
                        acc.Add(t.AreaId, new List<LayoutModel.Table>());
                    acc[t.AreaId].Add(new LayoutModel.Table
                    {
                        Id = t.Id,
                        Name = t.Name,
                        AreaId = t.AreaId
                    });

                    return acc;
                })
        };

        return model;
    }
}
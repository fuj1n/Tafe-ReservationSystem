using System.Numerics;
using Microsoft.EntityFrameworkCore;
using ReservationSystem_Server.Areas.Admin.Models;
using ReservationSystem_Server.Data;
using ReservationSystem_Server.Data.Visual.Layout;

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
        TableVisual[] tables = await _context.TableVisuals.Include(v => v.Table).ToArrayAsync();
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
                (acc, v) =>
                {
                    if (!acc.ContainsKey(v.Table.AreaId))
                        acc.Add(v.Table.AreaId, new List<LayoutModel.Table>());
                    acc[v.Table.AreaId].Add(new LayoutModel.Table
                    {
                        Id = v.Table.Id,
                        Name = v.Table.Name,
                        AreaId = v.Table.AreaId,
                        Position = new Vector2(v.X, v.Y),
                        Rotation = v.Rotation,
                        TableTypeId = v.TableTypeId
                    });

                    return acc;
                }),
            TableTypes = await _context.TableTypeVisuals.Include(v => v.Rects).ToDictionaryAsync(v => v.Id, v => new LayoutModel.TableType
            {
                Id = v.Id,
                Name = v.Name,
                CanvasSize = new Vector2(v.Width, v.Height),
                Rectangles = v.Rects.Select(LayoutModel.Rect.FromRectangleVisual).ToArray()
            })
        };

        return model;
    }
}
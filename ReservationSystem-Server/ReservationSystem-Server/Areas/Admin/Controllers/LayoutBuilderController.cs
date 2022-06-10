using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservationSystem_Server.Areas.Admin.Models;
using ReservationSystem_Server.Areas.Admin.Models.LayoutBuilder;
using ReservationSystem_Server.Data;
using ReservationSystem_Server.Data.Visual.Layout;

namespace ReservationSystem_Server.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Manager")]
[ApiController]
[Route("[area]/[controller]")]
#if DEBUG
[AllowAnonymous]
#endif
public class LayoutBuilderController : Controller
{
    private readonly ApplicationDbContext _context;

    public LayoutBuilderController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("{*path}", Order = 999)] // Catch all route, so that the underlying react app can use navigation links
    public IActionResult Index(string? path)
    {
        return View();
    }

    [HttpGet("AreaLayout")]
    public async Task<IActionResult> GetAreaLayout()
    {
        return Ok(
            await _context.RestaurantAreas
                .GroupJoin(_context.RestaurantAreaVisuals.DefaultIfEmpty(),
                    a => a.Id,
                    v => v.AreaId,
                    (a, v) => new { a, v })
                .SelectMany(val => val.v.DefaultIfEmpty(), (val, v) => new AreaLayoutModel
                {
                    Id = val.a.Id,
                    Name = val.a.Name,
                    Rect = LayoutModel.Rect.FromRectangleVisual(v != null ? v.Rect : null)
                }).ToArrayAsync());
    }

    [HttpPut("AreaLayout")]
    public async Task<IActionResult> PutAreaLayout(AreaLayoutModel[] areas)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        foreach (AreaLayoutModel model in areas)
        {
            RestaurantArea? area = await _context.RestaurantAreas.FirstOrDefaultAsync(a => a.Id == model.Id);

            if (area == null)
            {
                // New area
                area = new RestaurantArea
                {
                    Name = model.Name,
                    RestaurantId = 1
                };

                _context.RestaurantAreas.Add(area);
                await _context.SaveChangesAsync();

                model.Id = area.Id;
            }
            else
            {
                // Update area
                area.Name = model.Name;
            }

            RestaurantAreaVisual? visual =
                await _context.RestaurantAreaVisuals.Include(v => v.Rect)
                    .FirstOrDefaultAsync(v => v.AreaId == area.Id);
            if (visual == null)
            {
                visual = new RestaurantAreaVisual
                {
                    AreaId = area.Id,
                    Rect = LayoutModel.Rect.ToRectangleVisual(model.Rect),
                };

                _context.RestaurantAreaVisuals.Add(visual);
            }
            else
            {
                RectangleVisual changedRect = LayoutModel.Rect.ToRectangleVisual(model.Rect);

                visual.Rect.X = changedRect.X;
                visual.Rect.Y = changedRect.Y;
                visual.Rect.Width = changedRect.Width;
                visual.Rect.Height = changedRect.Height;
                visual.Rect.R = changedRect.R;
                visual.Rect.G = changedRect.G;
                visual.Rect.B = changedRect.B;
                visual.Rect.A = changedRect.A;
            }
        }

        // ToArray as EF couldn't translate areas.All({lambda})
        foreach (RestaurantArea area in (await _context.RestaurantAreas.Include(a => a.Tables).ToArrayAsync()).Where(a =>
                     areas.All(m => a.Id != m.Id)))
        {
            RestaurantAreaVisual? visual =
                await _context.RestaurantAreaVisuals
                    .Include(v => v.Rect)
                    .FirstOrDefaultAsync(v => v.AreaId == area.Id);

            if (visual != null)
            {
                _context.RectangleVisuals.Remove(visual.Rect);
                _context.RestaurantAreaVisuals.Remove(visual);
            }

            _context.RestaurantAreaVisuals.RemoveRange(
                await _context.RestaurantAreaVisuals.Where(v => v.AreaId == area.Id).ToArrayAsync());
            
            _context.Tables.RemoveRange(area.Tables);
            _context.RestaurantAreas.Remove(area);
        }

        await _context.SaveChangesAsync();
        return NoContent();
    }
}
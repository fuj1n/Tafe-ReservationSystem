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
                .SelectMany(val => val.v.DefaultIfEmpty(), (val, v) => new AreasLayoutModel
                {
                    Id = val.a.Id,
                    Name = val.a.Name,
                    Rect = LayoutModel.Rect.FromRectangleVisual(v != null ? v.Rect : null)
                }).ToArrayAsync());
    }

    [HttpPut("AreaLayout")]
    public async Task<IActionResult> PutAreaLayout(AreasLayoutModel[] areas)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        foreach (AreasLayoutModel model in areas)
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

        // ToArray as EF couldn't translate areas.All(...)
        foreach (RestaurantArea area in (await _context.RestaurantAreas.Include(a => a.Tables).ToArrayAsync()).Where(
                     a =>
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

    [HttpGet("TableTypes")]
    public async Task<IActionResult> GetTableTypes()
    {
        return Ok(await _context.TableTypeVisuals
            .Include(v => v.Rects)
            .Select(v => new TableTypeModel
            {
                Id = v.Id,
                Name = v.Name,
                Seats = v.Seats,
                Width = v.Width,
                Height = v.Height,
                Rects = v.Rects.Select(r => LayoutModel.Rect.FromRectangleVisual(r)).ToArray()
            })
            .ToArrayAsync());
    }

    [HttpPut("TableTypes")]
    public async Task<IActionResult> PutTableTypes(TableTypeModel[] tables)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        foreach (TableTypeModel model in tables)
        {
            TableTypeVisual? existing = await _context.TableTypeVisuals.Include(v => v.Rects)
                .FirstOrDefaultAsync(v => v.Id == model.Id);
            if (existing == null)
            {
                model.Id = 0;
                _context.TableTypeVisuals.Add(new TableTypeVisual
                {
                    Name = model.Name,
                    Seats = model.Seats,
                    Width = model.Width,
                    Height = model.Height,
                    Rects = model.Rects.Select(LayoutModel.Rect.ToRectangleVisual).ToList()
                });
            }
            else
            {
                existing.Name = model.Name;
                existing.Seats = model.Seats;
                existing.Width = model.Width;
                existing.Height = model.Height;

                _context.RectangleVisuals.RemoveRange(existing.Rects);
                existing.Rects.Clear();
                foreach (LayoutModel.Rect rect in model.Rects)
                {
                    existing.Rects.Add(new RectangleVisual
                    {
                        X = rect.X,
                        Y = rect.Y,
                        Width = rect.Width,
                        Height = rect.Height,
                        R = rect.Color.R,
                        G = rect.Color.G,
                        B = rect.Color.B,
                        A = rect.Color.A
                    });
                }
            }
        }

        // ToArray as EF couldn't translate tables.All(...)
        foreach (TableTypeVisual tableType in (await _context.TableTypeVisuals.Include(t => t.Rects).ToArrayAsync())
                 .Where(t => tables.All(tt => tt.Id != t.Id)))
        {
            _context.RectangleVisuals.RemoveRange(tableType.Rects);
            _context.TableTypeVisuals.Remove(tableType);
        }

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("Areas")]
    public async Task<IActionResult> GetAreas()
    {
        return Ok(await _context.RestaurantAreas.ToArrayAsync());
    }

    [HttpGet("Area/{id:int}")]
    public async Task<IActionResult> GetArea(int id)
    {
        RestaurantArea? area =
            await _context.RestaurantAreas.FirstOrDefaultAsync(a => a.Id == id);
        if (area == null)
        {
            return NotFound();
        }

        return Ok(new AreaLayoutModel
        {
            Id = area.Id,
            Name = area.Name,
            AreaRect = LayoutModel.Rect.FromRectangleVisual(
                (await _context.RestaurantAreaVisuals.Include(a => a.Rect).FirstOrDefaultAsync(v => v.AreaId == id))?.Rect),
            Tables = await _context.Tables.Where(t => t.AreaId == id).GroupJoin(
                _context.TableVisuals.DefaultIfEmpty(),
                t => t.Id,
                t => t.TableId,
                (t, v) => new { t, v }
            ).SelectMany(val => val.v.DefaultIfEmpty(), (val, v) => new AreaLayoutModel.TableModel
            {
                Id = val.t.Id,
                Name = val.t.Name,
                X = v != null ? v.X : 50,
                Y = v != null ? v.Y : 50,
                Rotation = v != null ? v.Rotation : 0,
                TableTypeId = v != null ? v.TableTypeId : 0
            }).ToArrayAsync()
        });
    }

    [HttpPut("Area")]
    public async Task<IActionResult> PutArea(AreaLayoutModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        RestaurantArea? area = await _context.RestaurantAreas.Include(a => a.Tables)
            .FirstOrDefaultAsync(a => a.Id == model.Id);
        if (area == null)
        {
            return NotFound();
        }

        foreach (AreaLayoutModel.TableModel table in model.Tables)
        {
            Table? existing = area.Tables.FirstOrDefault(t => t.Id == table.Id);
            TableTypeVisual? tableType = await _context.TableTypeVisuals.FirstOrDefaultAsync(t => t.Id == table.TableTypeId);

            if (tableType == null)
            {
                ModelState.AddModelError("TableTypeId", "Table type not found");
                return BadRequest(ModelState);
            }
            
            if (existing == null)
            {
                _context.Tables.Add(new Table
                {
                    AreaId = area.Id,
                    Name = table.Name,
                    Seats = tableType.Seats
                });
                await _context.SaveChangesAsync(); // To get the new table id

                _context.TableVisuals.Add(new TableVisual
                {
                    TableId = area.Tables.Last().Id,
                    X = table.X,
                    Y = table.Y,
                    Rotation = table.Rotation,
                    TableTypeId = table.TableTypeId
                });
            }
            else
            {
                existing.Name = table.Name;
                existing.Seats = tableType.Seats;
                
                TableVisual? existingVisual = await _context.TableVisuals.FirstOrDefaultAsync(v => v.TableId == existing.Id);
                if (existingVisual == null)
                {
                    _context.TableVisuals.Add(new TableVisual
                    {
                        TableId = existing.Id,
                        X = table.X,
                        Y = table.Y,
                        Rotation = table.Rotation,
                        TableTypeId = table.TableTypeId
                    });
                }
                else
                {
                    existingVisual.X = table.X;
                    existingVisual.Y = table.Y;
                    existingVisual.Rotation = table.Rotation;
                    existingVisual.TableTypeId = table.TableTypeId;
                }
            }
        }
        
        // ToArray as EF couldn't translate Tables.All(...)
        foreach (Table table in
                 (await _context.Tables.Where(t => t.AreaId == area.Id).ToArrayAsync())
                 .Where(t => model.Tables.All(mt => mt.Id != t.Id)))
        {
            _context.TableVisuals.RemoveRange(await _context.TableVisuals.Where(v => v.TableId == table.Id).ToArrayAsync());
            _context.Tables.Remove(table);
        }
        
        await _context.SaveChangesAsync();
        return NoContent();
    }

}
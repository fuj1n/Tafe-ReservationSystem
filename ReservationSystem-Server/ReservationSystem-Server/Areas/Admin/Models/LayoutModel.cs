using System.Drawing;
using System.Numerics;
using JetBrains.Annotations;

namespace ReservationSystem_Server.Areas.Admin.Models;

[PublicAPI]
public class LayoutModel
{
    public Area[] Areas { get; set; } = Array.Empty<Area>();
    
    //TODO: editable table types
    public Dictionary<int, TableType> TableTypes { get; set; } = new()
    {
        {1, new TableType
        {
            Id = 1,
            Name = "Square",
            Rectangles = new []
            {
                new Rect(0, 0, 5, 5, Color.Chocolate),
                new Rect(-5, 4, 1, 4, Color.Maroon),
                new Rect(50, 4, 1, 4, Color.Maroon),
            }
        }}
    };
    public Dictionary<int, List<Table>> Tables { get; set; } = new();

    [PublicAPI]
    public struct Rect
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }

        public Color Color { get; set; }
        
        public Rect(float x, float y, float width, float height) : this(x, y, width, height, Color.Black) { }
        public Rect(float x, float y, float width, float height, Color color)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Color = color;
        }
    }
    
    [PublicAPI]
    public class Area
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public Rect Rect { get; set; } = new(50F, 50F, 25F, 25F, Color.FromArgb(unchecked((int)0xFFe37474)));
    }

    [PublicAPI]
    public class TableType
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        
        public Vector2 CanvasSize { get; set; } = new(10F, 10F);
        public Rect[] Rectangles { get; set; } = Array.Empty<Rect>();
    }
    
    [PublicAPI]
    public class Table
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        
        public int AreaId { get; set; }
        public int TableTypeId { get; set; } = 1;
        
        public Vector2 Position { get; set; } = Vector2.One * 50;
    }
}
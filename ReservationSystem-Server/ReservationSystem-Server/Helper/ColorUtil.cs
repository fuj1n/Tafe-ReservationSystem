using System.Drawing;

namespace ReservationSystem_Server.Helper;

public static class ColorUtil
{
    public static Color Darken(this Color color, float by)
    {
        return Color.FromArgb(color.A, (int)(color.R / by), (int)(color.G / by), (int)(color.B / by));
    }
}
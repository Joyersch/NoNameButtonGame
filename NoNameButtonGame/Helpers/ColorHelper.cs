using Microsoft.Xna.Framework;

namespace NoNameButtonGame.Helpers;

public static class ColorHelper
{
    public static Color DarkenColor(Color color, float percent)
    {
        int r = (int)(color.R * percent);
        int g = (int)(color.G * percent);
        int b = (int)(color.B * percent);
        return new Color(r, g, b);
    }
}
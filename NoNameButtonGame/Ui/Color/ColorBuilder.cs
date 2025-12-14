using System;
using System.Collections.Generic;

namespace NoNameButtonGame.Ui.Color;

public sealed class ColorBuilder
{
    private List<Microsoft.Xna.Framework.Color> _colors = new();

    public void AddColor(Microsoft.Xna.Framework.Color color, int length)
    {
        for (int i = 0; i < length; i++)
            _colors.Add(color);
    }

    public Microsoft.Xna.Framework.Color[] GetColor()
        => _colors.ToArray();

    public void AddColor(Microsoft.Xna.Framework.Color[] color)
        => _colors.AddRange(color);

    public void AddColor(string color, int length)
        => AddColor(FromString(color), length);

    public static Microsoft.Xna.Framework.Color FromString(string color)
    {
        if (color.StartsWith('#'))
            color = color[1..];

        int r = Convert.ToInt32(color.Substring(0, 2), 16);
        int g = Convert.ToInt32(color.Substring(2, 2), 16);
        int b = Convert.ToInt32(color.Substring(4, 2), 16);
        return new Microsoft.Xna.Framework.Color(r, g, b);
    }
}
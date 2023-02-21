using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace NoNameButtonGame.Colors;

public class PulsatingPurple : AnimatedColor
{
    public PulsatingPurple()
    {
        var color = new List<Color>();
        int r = 128, g = 10, b = 255;
        for (; r < 160;r++,b--)
            color.Add(new Color(r,g,b));
        for (; r > 128;r--, b++)
            color.Add(new Color(r,g,b));
        Color = color.ToArray();
    }
}
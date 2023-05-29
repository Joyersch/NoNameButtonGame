using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoUtils.Ui.Color;

namespace NoNameButtonGame.Colors;

public class PulsatingRed : AnimatedColor
{
    public PulsatingRed()
    {
        var color = new List<Color>();

        int r = 128, g = 0, b = 10;
        for (; r < 200; r++, g += r % 5 == 0 ? 1 : 0, b -= r % 5 == 0 ? 1 : 0)
            color.Add(new Color(r, g, b));
        for (; r > 120; r--, g -= r % 5 == 0 ? 1 : 0, b += r % 5 == 0 ? 1 : 0)
            color.Add(new Color(r, g, b));
        Color = color.ToArray();
    }
}
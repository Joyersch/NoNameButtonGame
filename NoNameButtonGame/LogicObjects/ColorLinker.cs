using System.Collections.Generic;
using Microsoft.Xna.Framework;
using NoNameButtonGame.Colors;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.Interfaces;
using NoNameButtonGame.Text;

namespace NoNameButtonGame.LogicObjects;

public class ColorLinker
{
    private List<(AnimatedColor color, IColorable colorable)> mappings;

    public ColorLinker()
    {
        mappings = new List<(AnimatedColor color, IColorable colorable)>();
    }

    public void Update(GameTime gameTime)
        => mappings.ForEach(m => m.colorable.ChangeColor(m.color.GetColor(m.colorable.ColorLength())));

    public void Add(AnimatedColor color, IColorable text)
        => mappings.Add((color, text));
}
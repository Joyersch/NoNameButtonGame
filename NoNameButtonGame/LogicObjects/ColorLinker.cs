using System.Collections.Generic;
using Microsoft.Xna.Framework;
using NoNameButtonGame.Colors;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.Text;

namespace NoNameButtonGame.LogicObjects;

public class ColorLinker
{
    private List<(AnimatedColor color, TextBuilder text)> mappings;

    public ColorLinker()
    {
        mappings = new List<(AnimatedColor color, TextBuilder text)>();
    }

    public void Update(GameTime gameTime)
        => mappings.ForEach(m => m.text.ChangeColor(m.color.GetColor(m.text.Length)));

    public void Add(AnimatedColor color, TextBuilder text)
        => mappings.Add((color, text));
}
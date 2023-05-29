using System;
using Microsoft.Xna.Framework;
using MonoUtils.Ui.Color;

namespace NoNameButtonGame.Colors;

public class SingleRandomColor : AnimatedColor
{
    private readonly Random _random;

    public SingleRandomColor(Random random)
    {
        _random = random;
    }

    public override void Update(GameTime gameTime)
    {
        Color = new[] {new Color(_random.Next(0, 255), _random.Next(0, 255), _random.Next(0, 255))};
    }
}
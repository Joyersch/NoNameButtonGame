using System;
using Microsoft.Xna.Framework;
using MonoUtils.Ui.Color;

namespace NoNameButtonGame.Colors;

public sealed class SingleRandomColor : AnimatedColor
{
    private readonly Random _random;

    public SingleRandomColor(Random random)
    {
        _random = random;
        Color = [new Color(_random.Next(0, 255), _random.Next(0, 255), _random.Next(0, 255))];
    }

    public override void Update(GameTime gameTime)
    {
        Color = [new Color(_random.Next(0, 255), _random.Next(0, 255), _random.Next(0, 255))];
    }
}
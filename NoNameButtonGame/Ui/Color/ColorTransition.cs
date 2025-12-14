using System;
using Microsoft.Xna.Framework;
using IUpdateable = Joyersch.Monogame.IUpdateable;

namespace NoNameButtonGame.Ui.Color;

public class ColorTransition : IUpdateable, IColorable
{
    private readonly Microsoft.Xna.Framework.Color _start;
    private readonly Microsoft.Xna.Framework.Color _end;
    private readonly float _passTime;
    private float _processedTime;

    public event Action FinishedTransition;

    private Microsoft.Xna.Framework.Color _color;

    public ColorTransition(Microsoft.Xna.Framework.Color start, Microsoft.Xna.Framework.Color end, float passTime)
    {
        _start = start;
        _end = end;
        _passTime = passTime;
    }

    public void Update(GameTime gameTime)
    {
        float passed = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

        _processedTime += passed;

        if (_processedTime >= _passTime)
        {
            FinishedTransition?.Invoke();
            _color = _end;
            return;
        }


        float lerpFactor = _processedTime / _passTime;

        byte r = (byte)MathHelper.Lerp(_start.R, _end.R, lerpFactor);
        byte g = (byte)MathHelper.Lerp(_start.G, _end.G, lerpFactor);
        byte b = (byte)MathHelper.Lerp(_start.B, _end.B, lerpFactor);
        byte a = (byte)MathHelper.Lerp(_start.A, _end.A, lerpFactor);

        _color = new Microsoft.Xna.Framework.Color(r, g, b, a);
    }

    public void ChangeColor(Microsoft.Xna.Framework.Color[] input)
    {
        // ignored
    }

    public int ColorLength()
        => 1;

    public Microsoft.Xna.Framework.Color[] GetColor()
        => [_color];
}
using System;
using Microsoft.Xna.Framework;

namespace NoNameButtonGame.Colors;

public class AnimatedColor
{
    protected Color[] Color;
    private int _index;
    private float _storedGameTime;
    public int Increment = 1;
    public float GameTimeStepInterval = 25;
    public int Offset;

    protected AnimatedColor()
        => Color = Array.Empty<Color>();

    public virtual void Update(GameTime gameTime)
    {
        if (Color.Length == 0 || GameTimeStepInterval <= 0)
            return;

        _storedGameTime += (float) gameTime.ElapsedGameTime.TotalMilliseconds;
        while (_storedGameTime > GameTimeStepInterval)
        {
            _storedGameTime -= GameTimeStepInterval;
            _index += Increment;
            _index %= Color.Length;
        }
    }

    public Color[] GetColor(int length)
    {
        var getColor = new Color[length];
        for (int i = 0; i < length; i++)
        {
            getColor[i] = Color[(i * Increment + _index + Offset) % Color.Length];
        }

        return getColor;
    }
}
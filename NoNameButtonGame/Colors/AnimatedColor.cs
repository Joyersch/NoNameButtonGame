using System;
using Microsoft.Xna.Framework;

namespace NoNameButtonGame.Colors;

public class AnimatedColor
{
    protected Color[] Color;
    private int index;
    private float storedGameTime;
    public int Increment = 1;
    public float GameTimeStepInterval = 25;
    public int Offset;

    protected AnimatedColor()
        => Init();

    protected virtual void Init()
    {
        Color = Array.Empty<Color>();
    }

    public virtual void Update(GameTime gameTime)
    {
        if (Color.Length == 0 || GameTimeStepInterval <= 0)
            return;

        storedGameTime += (float) gameTime.ElapsedGameTime.TotalMilliseconds;
        while (storedGameTime > GameTimeStepInterval)
        {
            storedGameTime -= GameTimeStepInterval;
            index += Increment;
            index %= Color.Length;
        }
    }

    public Color[] GetColor(int length)
    {
        var getColor = new Color[length];
        for (int i = 0; i < length; i++)
        {
            getColor[i] = Color[(i * Increment + index + Offset) % Color.Length];
        }

        return getColor;
    }
}
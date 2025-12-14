using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NoNameButtonGame.Ui.Color;

public class AnimatedColor : IManageable
{
    protected Microsoft.Xna.Framework.Color[] Color;
    private int _index;
    private float _storedGameTime;
    public int Increment = 1;
    public bool NoGradient;
    public float GameTimeStepInterval = 25;
    public int Offset;

    protected AnimatedColor()
        => Color = Array.Empty<Microsoft.Xna.Framework.Color>();

    public Rectangle Rectangle { get; } = Rectangle.Empty;

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

    public void Draw(SpriteBatch spriteBatch)
    {
    }

    public Microsoft.Xna.Framework.Color[] GetColor(int length)
    {
        var getColor = new Microsoft.Xna.Framework.Color[length];

        for (int i = 0, a = 0; i < length; i++)
        {
            if (!NoGradient)
                a = i;
            getColor[i] = Color[(a * Increment + _index + Offset) % Color.Length];
        }

        return getColor;
    }
}
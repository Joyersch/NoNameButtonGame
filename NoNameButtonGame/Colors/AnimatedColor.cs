using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using NoNameButtonGame.GameObjects;

namespace NoNameButtonGame.Colors;

public class AnimatedColor
{
    public Color[] Color;
    private int Index;
    private float storedGameTime;
    public int Increment = 1;
    public float GameTimeStepInterval = 25;
    public int Offset;

    public AnimatedColor()
    {
        Init();
    }

    public virtual void Init()
    {
        Color = new Color[0];
    }

    public virtual void Update(GameTime gameTime)
    {
        if (Color.Length == 0 || GameTimeStepInterval <= 0)
            return;

        storedGameTime += (float) gameTime.ElapsedGameTime.TotalMilliseconds;
        while (storedGameTime > GameTimeStepInterval)
        {
            storedGameTime -= GameTimeStepInterval;
            Index += Increment;
            Index %= Color.Length;
        }
    }

    public Color[] GetColor(int Length)
    {
        Color[] rc = new Color[Length];
        for (int i = 0; i < Length; i++)
        {
            rc[i] = Color[(i * Increment + Index + Offset) % Color.Length];
        }

        return rc;
    }
}
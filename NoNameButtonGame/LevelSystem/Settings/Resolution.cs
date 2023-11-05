using Microsoft.Xna.Framework;

namespace NoNameButtonGame.LevelSystem.Settings;

public class Resolution
{
    public int Width { get; private set; }
    public int Height { get; private set; }

    public Vector2 Size => new Vector2(Width, Height);

    public Resolution(int width, int height)
    {
        Width = width;
        Height = height;
    }

    public override string ToString()
    {
        return Width + "x" + Height;
    }
}
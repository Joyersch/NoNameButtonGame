

namespace Joyersch.Monogame.Console;

public sealed class BacklogColorSet
{
    public Microsoft.Xna.Framework.Color[] Color { get; private set; }

    public BacklogColorSet(Microsoft.Xna.Framework.Color[] color)
    {
        Color = color;
    }

    public BacklogColorSet(int length) : this(Microsoft.Xna.Framework.Color.White, length)
    {
    }

    public BacklogColorSet(Microsoft.Xna.Framework.Color color, int length)
    {
        Color = new Microsoft.Xna.Framework.Color[length];
        for (int i = 0; i < length; i++)
            Color[i] = color;
    }
}
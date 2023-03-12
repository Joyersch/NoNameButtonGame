using Microsoft.Xna.Framework;

namespace NoNameButtonGame.LogicObjects;

public static class ScreenMatrix
{
    public static Rectangle Screen(int x, int y, float d)
        => new(new Point((int) (Display.Display.Width * x / d), (int) (Display.Display.Height * y / d)),
            (Display.Display.Size / d).ToPoint());
}
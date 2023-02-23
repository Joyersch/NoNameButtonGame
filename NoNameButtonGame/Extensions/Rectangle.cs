using Framework = Microsoft.Xna.Framework;

namespace NoNameButtonGame.Extensions;

public static class Rectangle
{
    public static Framework.Rectangle ExtendFromCenter(this Framework.Rectangle sender, float scale)
        => new((int)(sender.X - sender.Width * scale / 2F), (int)(sender.Y - sender.Height * scale / 2F),
            (int)(sender.Width * scale), (int)(sender.Height * scale));
}
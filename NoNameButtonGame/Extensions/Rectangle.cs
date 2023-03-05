using Framework = Microsoft.Xna.Framework;

namespace NoNameButtonGame.Extensions;

public static class Rectangle
{
    public static Framework.Rectangle ExtendFromCenter(this Framework.Rectangle sender, float scale)
        => new((int) (sender.Center.X - sender.Width * scale / 2F), (int) (sender.Center.Y - sender.Height * scale / 2F),
            (int) (sender.Width * scale), (int) (sender.Height * scale));

    public static Framework.Vector2 BottomRightCornor(this Framework.Rectangle sender)
        => sender.Location.ToVector2() + sender.Size.ToVector2();

    public static Framework.Vector2 BottomLeftCornor(this Framework.Rectangle sender)
        => sender.Location.ToVector2() + new Framework.Vector2(0, sender.Height);

    public static Framework.Vector2 TopRightCornor(this Framework.Rectangle sender)
        => sender.Location.ToVector2() + new Framework.Vector2(sender.Width, 0);

    public static Framework.Vector2 TopLeftCornor(this Framework.Rectangle sender)
        => sender.Location.ToVector2();
}
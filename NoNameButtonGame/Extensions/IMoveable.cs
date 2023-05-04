using NoNameButtonGame.Interfaces;
using NoNameButtonGame.LogicObjects;

namespace NoNameButtonGame.Extensions;

public static class IMoveableExtension
{
    public static PositionCalculator GetCalculator(this IMoveable sender, Microsoft.Xna.Framework.Rectangle rectangle)
        => new (rectangle, sender);
    
    public static PositionCalculator GetCalculator(this IMoveable sender, Microsoft.Xna.Framework.Vector2 position, Microsoft.Xna.Framework.Vector2 size)
        => new (new Microsoft.Xna.Framework.Rectangle(position.ToPoint(), size.ToPoint()), sender);
}
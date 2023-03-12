using NoNameButtonGame.Interfaces;
using NoNameButtonGame.LogicObjects;
using GameObject = NoNameButtonGame.GameObjects.GameObject;

namespace NoNameButtonGame.Extensions;

public static class IMoveableExtension
{
    public static PositionCalculator GetCalculator(this IMoveable sender, Microsoft.Xna.Framework.Rectangle rectangle)
        => new (rectangle, sender);
}
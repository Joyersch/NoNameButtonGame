using Joyersch.Monogame;

namespace NoNameButtonGame;

public static class MoveableExtension
{
    public static PositionCalculator InRectangle(this IMoveable sender, IRectangle rectangle)
        => new (rectangle, sender);


    public static AnchorCalculator GetAnchor(this IMoveable sender, IMoveable receiver)
        => new(sender, receiver);

}
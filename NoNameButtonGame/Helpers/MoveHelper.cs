using System;
using Microsoft.Xna.Framework;

namespace NoNameButtonGame.Helpers;

public static class MoveHelper
{
    public static void MoveTowards(IMoveable move, IMoveable to, float distance)
    {
        var movePosition = move.GetPosition();
        move.Move(movePosition + GetDirection(move, to) * distance);
    }

    public static Vector2 GetDirection(IMoveable move, IMoveable to)
    {
        var movePosition = move.GetPosition();
        var moveSize = move.GetSize();

        var moveFrom = movePosition + moveSize / 2;

        var toPosition = to.GetPosition();
        var toSize = to.GetSize();

        var moveTo = toPosition + toSize / 2;

        return Vector2.Normalize(moveTo - moveFrom);
    }

    public static void RotateTowards(IRotateable rotate, IMoveable towards)
    {
        rotate.Rotation = GetAngle(rotate, towards);
    }

    public static float GetAngle(IRotateable origin, IMoveable destination)
    {
        var rotatePosition = origin.GetPosition();
        var rotateSize = origin.GetSize();

        var rotateCenter = rotatePosition + rotateSize / 2;

        var towardsPosition = destination.GetPosition();
        var towardsSize = destination.GetSize();

        var towardsCenter = towardsPosition + towardsSize / 2;
        var direction = towardsCenter - rotateCenter;
        return (float)Math.Atan2(direction.Y, direction.X);
    }

    /// <summary>
    /// to - from
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <returns></returns>
    public static Vector2 GetRelative(Vector2 from, Vector2 to)
        => to - from;

    public static Rectangle GetRectangle(this IMoveable moveable)
        => new Rectangle(moveable.GetPosition().ToPoint(), moveable.GetSize().ToPoint());
}
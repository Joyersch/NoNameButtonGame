using System;
using Microsoft.Xna.Framework;

namespace NoNameButtonGame.Helpers;

public static class Vector2Helper
{
    public static float GetAngle(Vector2 origin, Vector2 destination)
    {
        var direction = destination - origin;
        return (float)Math.Atan2(direction.Y, direction.X);
    }
}
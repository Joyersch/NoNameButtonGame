using Joyersch.Monogame;
using Microsoft.Xna.Framework;

namespace NoNameButtonGame;

public static class SpartialExtensions
{
    public static Rectangle GetRectangle(this ISpatial spatial)
        => new Rectangle(spatial.GetPosition().ToPoint(), spatial.GetSize().ToPoint());
}
using Joyersch.Monogame;
using Microsoft.Xna.Framework;

namespace NoNameButtonGame;

public class RectangleWrapper : IRectangle
{
    public Rectangle Rectangle { private set; get; }

    public RectangleWrapper(Rectangle rectangle)
    {
        Rectangle = rectangle;
    }
}
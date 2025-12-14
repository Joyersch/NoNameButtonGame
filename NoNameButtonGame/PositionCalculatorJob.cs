using Microsoft.Xna.Framework;

namespace NoNameButtonGame;

public abstract class PositionCalculatorJob
{
    public abstract Vector2 Execute(Rectangle area, Vector2 prior);
}
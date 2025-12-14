using Joyersch.Monogame;
using Microsoft.Xna.Framework;

namespace NoNameButtonGame;

public class OffsetSizePositionJob : PositionCalculatorJob
{
    private readonly IMoveable _moveable;
    private readonly float _percent;
    private readonly bool _onlyX;
    private readonly bool _onlyY;

    public OffsetSizePositionJob(IMoveable moveable, float percent, bool onlyX = false, bool onlyY = false)
    {
        _moveable = moveable;
        _percent = percent;
        _onlyX = onlyX;
        _onlyY = onlyY;
    }

    public override Vector2 Execute(Rectangle area, Vector2 prior)
    {
        var bySize = _moveable.GetSize() * _percent;

        if (_onlyX) bySize.Y = 0;
        if (_onlyY) bySize.X = 0;
        return prior + bySize;
    }
}
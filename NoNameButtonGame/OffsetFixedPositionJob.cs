using Microsoft.Xna.Framework;

namespace NoNameButtonGame;

public class OffsetFixedPositionJob : PositionCalculatorJob
{
    private readonly Vector2 _position;

    private bool _onlyX;

    private bool _onlyY;

    public OffsetFixedPositionJob(Vector2 position, bool onlyX = false, bool onlyY = false)
    {
        _onlyX = onlyX;
        _onlyY = onlyY;
        _position = position;
    }

    public override Vector2 Execute(Rectangle area, Vector2 prior)
    {
        var potential = _position;
        if (_onlyX) potential.Y = 0;
        if (_onlyY) potential.X = 0;
        return potential + prior;
    }
}
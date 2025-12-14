using Microsoft.Xna.Framework;
using NoNameButtonGame;

namespace Joyersch.Monogame;

public class FixedPositionJob : PositionCalculatorJob
{
    private readonly Vector2 _position;

    private bool _onlyX;

    private bool _onlyY;

    public FixedPositionJob(Vector2 position, bool onlyX = false, bool onlyY = false)
    {
        _onlyX = onlyX;
        _onlyY = onlyY;
        _position = position;
    }

    public override Vector2 Execute(Rectangle area, Vector2 prior)
    {
        if (_onlyX) return new Vector2(_position.X, prior.Y);
        if (_onlyY) return new Vector2(prior.X, _position.Y);
        return _position;
    }
}
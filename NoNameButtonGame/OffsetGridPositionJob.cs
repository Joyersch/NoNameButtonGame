using Microsoft.Xna.Framework;

namespace NoNameButtonGame;

public class OffsetGridPositionJob : PositionCalculatorJob
{
    private readonly float _grid;
    private readonly bool _onlyX;
    private readonly bool _onlyY;

    public OffsetGridPositionJob(float grid, bool onlyX = false, bool onlyY = false)
    {
        _grid = grid;
        _onlyX = onlyX;
        _onlyY = onlyY;
    }

    public override Vector2 Execute(Rectangle area, Vector2 prior)
    {
        var potential = prior + area.Size.ToVector2() * _grid;
        if (_onlyX) return new Vector2(potential.X, prior.Y);
        if (_onlyY) return new Vector2(prior.X, potential.Y);
        return potential;
    }
}
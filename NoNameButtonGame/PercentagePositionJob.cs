using Microsoft.Xna.Framework;

namespace NoNameButtonGame;

public class PercentagePositionJob : PositionCalculatorJob
{
    private readonly float _percent;
    private readonly bool _onlyX;
    private readonly bool _onlyY;

    public PercentagePositionJob(int on, int from, bool onlyX = false, bool onlyY = false) : this(on / (float)from,
        onlyX, onlyY)
    {
    }

    public PercentagePositionJob(float percent, bool onlyX = false, bool onlyY = false)
    {
        _percent = percent;
        _onlyX = onlyX;
        _onlyY = onlyY;
    }

    public override Vector2 Execute(Rectangle area, Vector2 prior)
    {
        var size = area.Size.ToVector2();
        var position = area.Location.ToVector2();
        var potentialPosition = position + size * _percent;

        if (_onlyX) return new Vector2(potentialPosition.X, prior.Y);
        if (_onlyY) return new Vector2(prior.X, potentialPosition.Y);
        return potentialPosition;
    }
}
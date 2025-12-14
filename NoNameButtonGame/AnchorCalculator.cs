using Microsoft.Xna.Framework;
using NoNameButtonGame;

namespace Joyersch.Monogame;

public sealed class AnchorCalculator : ICalculator
{
    private readonly IMoveable _main;
    private readonly IMoveable _sub;

    private Anchor _mainAnchor;
    private Anchor _subAnchor;

    private Vector2 _distance;
    private IScaleable _distanceScale;

    public enum Anchor
    {
        TopLeft,
        Top,
        TopRight,
        Left,
        Right,
        BottomLeft,
        Bottom,
        BottomRight,
    }

    public AnchorCalculator(IMoveable sub, IMoveable main)
    {
        _main = main;
        _sub = sub;
        _distance = Vector2.Zero;
    }

    public AnchorCalculator SetMainAnchor(Anchor anchor)
    {
        _mainAnchor = anchor;
        return this;
    }

    public AnchorCalculator SetSubAnchor(Anchor anchor)
    {
        _subAnchor = anchor;
        return this;
    }

    public AnchorCalculator SetDistanceX(float x)
    {
        _distance.X = x;
        return this;
    }

    public AnchorCalculator SetDistanceY(float y)
    {
        _distance.Y = y;
        return this;
    }

    public AnchorCalculator SetDistanceScale(IScaleable scale)
    {
        _distanceScale = scale;
        return this;
    }

    public AnchorCalculator SetDistance(Vector2 distance)
    {
        _distance = distance;
        return this;
    }

    public AnchorCalculator SetDistance(float distance)
        => SetDistance(new Vector2(distance, distance));

    public void Apply()
        => _sub.Move(Calculate());

    public Vector2 Calculate()
    {
        var mainPoint = GetPoint(_main, _mainAnchor);
        var subOffset = _sub.GetPosition() - GetPoint(_sub, _subAnchor);
        var direction = _mainAnchor switch
        {
            Anchor.TopLeft => new Vector2(-1, -1),
            Anchor.Top => new Vector2(0, -1),
            Anchor.TopRight => new Vector2(1, -1),
            Anchor.Left => new Vector2(-1, 0),
            Anchor.Right => new Vector2(1, 0),
            Anchor.BottomLeft => new Vector2(-1, 1),
            Anchor.Bottom => new Vector2(0, 1),
            Anchor.BottomRight => new Vector2(1, 1),
        };

        float scale = 1F;

        if (_distanceScale != null)
            scale = _distanceScale.Scale;
        subOffset += _distance * scale * direction;
        return mainPoint + subOffset;
    }

    private static Vector2 GetPoint(IMoveable sender, Anchor anchor)
    {
        var position = sender.GetPosition();
        var size = sender.GetSize();

        return anchor switch
        {
            Anchor.TopLeft => position,
            Anchor.Top => position + new Vector2(size.X / 2, 0),
            Anchor.TopRight => position + new Vector2(size.X, 0),
            Anchor.Left => position + new Vector2(0, size.Y / 2),
            Anchor.Right => position + new Vector2(size.X, size.Y / 2),
            Anchor.BottomLeft => position + new Vector2(0, size.Y),
            Anchor.Bottom => position + new Vector2(size.X / 2, size.Y),
            Anchor.BottomRight => position + size,
        };
    }
}
using Microsoft.Xna.Framework;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.Interfaces;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace NoNameButtonGame.LogicObjects;

public class PositionCalculator
{

    private IMoveable _moveable;
    private readonly Vector2 _areaPosition;
    private readonly Vector2 _areaSize;
    private Vector2 _calculatedPosition;

    public PositionCalculator(Rectangle area, IMoveable gameObject) :
        this(area.Location.ToVector2(), area.Size.ToVector2(), gameObject)
    {
    }

    public PositionCalculator(Vector2 areaPosition, Vector2 areaSize, IMoveable moveable)
    {
        _moveable = moveable;
        _calculatedPosition = areaPosition;
        _areaPosition = areaPosition;
        _areaSize = areaSize;
    }

    public PositionCalculator OnX(int on, int from)
    {
        _calculatedPosition.X = _areaPosition.X + _areaSize.X / from * on;
        return this;
    }
    
    public PositionCalculator OnX(float percent)
    {
        _calculatedPosition.X = _areaPosition.X + _areaSize.X * percent;
        return this;
    }
    
    public PositionCalculator OnY(int on, int from)
    {
        _calculatedPosition.Y = _areaPosition.Y + _areaSize.Y / from * on;
        return this;
    }
    
    public PositionCalculator OnY(float percent)
    {
        _calculatedPosition.Y = _areaPosition.Y + _areaSize.Y * percent;
        return this;
    }

    public PositionCalculator OnCenter()
    {
        _calculatedPosition = _areaPosition + _areaSize / 2;
        return this;
    }
    
    public PositionCalculator Centered()
    {
        _calculatedPosition -= _moveable.GetSize() / 2;
        return this;
    }
    
    public PositionCalculator BySize(float percent)
    {
        _calculatedPosition += _moveable.GetSize() * percent;
        return this;
    }
    
    public PositionCalculator BySizeX(float percent)
    {
        _calculatedPosition.X += (_moveable.GetSize() * percent).X;
        return this;
    }
    
    public PositionCalculator BySizeY(float percent)
    {
        _calculatedPosition.Y += (_moveable.GetSize() * percent).Y;
        return this;
    }

    public void Move()
        => _moveable.Move(_calculatedPosition);
}
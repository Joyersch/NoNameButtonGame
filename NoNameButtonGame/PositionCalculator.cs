using System.Collections.Generic;
using System.Linq;
using Joyersch.Monogame;
using Microsoft.Xna.Framework;

namespace NoNameButtonGame;

public sealed class PositionCalculator : ICalculator
{
    private readonly IRectangle _area;
    private IMoveable _moveable;

    private List<PositionCalculatorJob> _jobs;

    public PositionCalculator(IRectangle area, IMoveable moveable)
    {
        _area = area;
        _moveable = moveable;
        _jobs = [];
    }

    public PositionCalculator OnX(int on, int from)
    {
        _jobs.Add(new PercentagePositionJob(on, from, onlyX: true));
        return this;
    }

    public PositionCalculator OnX(float percent)
    {
        _jobs.Add(new PercentagePositionJob(percent, onlyX: true));
        return this;
    }

    public PositionCalculator OnY(int on, int from)
    {
        _jobs.Add(new PercentagePositionJob(on, from, onlyY: true));
        return this;
    }

    public PositionCalculator OnY(float percent)
    {
        _jobs.Add(new PercentagePositionJob(percent, onlyY: true));
        return this;
    }

    public PositionCalculator OnCenter()
    {
        _jobs.Add(new PercentagePositionJob(0.5F));
        return this;
    }

    public PositionCalculator OnPositionX(float x)
    {
        _jobs.Add(new FixedPositionJob(new Vector2(x, 0), onlyX: true));
        return this;
    }

    public PositionCalculator OnPositionY(float y)
    {
        _jobs.Add(new FixedPositionJob(new Vector2(0, y), onlyY: true));
        return this;
    }

    public PositionCalculator Centered()
    {
        _jobs.Add(new OffsetSizePositionJob(_moveable, -0.5F));
        return this;
    }

    public PositionCalculator BySize(float percent)
    {
        _jobs.Add(new OffsetSizePositionJob(_moveable, percent));
        return this;
    }

    public PositionCalculator BySizeX(float percent)
    {
        _jobs.Add(new OffsetSizePositionJob(_moveable, percent, onlyX: true));
        return this;
    }

    public PositionCalculator BySizeY(float percent)
    {
        _jobs.Add(new OffsetSizePositionJob(_moveable, percent, onlyY: true));
        return this;
    }

    public PositionCalculator ByGrid(float v)
    {
        _jobs.Add(new OffsetGridPositionJob(v));
        return this;
    }

    public PositionCalculator ByGrid(float x, float y)
    {
        _jobs.Add(new OffsetGridPositionJob(x, onlyX: true));
        _jobs.Add(new OffsetGridPositionJob(y, onlyY: true));
        return this;
    }

    public PositionCalculator ByGridX(float x)
    {
        _jobs.Add(new OffsetGridPositionJob(x, onlyX: true));
        return this;
    }

    public PositionCalculator ByGridY(float y)
    {
        _jobs.Add(new OffsetGridPositionJob(y, onlyY: true));
        return this;
    }

    public PositionCalculator WithX(float x)
    {
        _jobs.Add(new OffsetFixedPositionJob(new Vector2(x), onlyX: true));
        return this;
    }

    public PositionCalculator WithY(float y)
    {
        _jobs.Add(new OffsetFixedPositionJob(new Vector2(y), onlyY: true));
        return this;
    }

    public PositionCalculator With(float x, float y)
    {
        _jobs.Add(new OffsetFixedPositionJob(new Vector2(x, y)));
        return this;
    }

    public void Apply()
    {
        if (_moveable is null)
            return;

        _moveable.Move(Calculate());
    }

    public Vector2 Calculate()
    {
        Vector2 position = _moveable.GetPosition();
        return _jobs.Aggregate(position, (current, job) => job.Execute(_area.Rectangle, current));
    }
}
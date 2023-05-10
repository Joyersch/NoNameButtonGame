using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.Interfaces;

namespace NoNameButtonGame.LogicObjects;

public class OverTimeInvoker : IManageable
{
    private double _invokeTime;
    private double _currentTime;
    private bool _hasStarted;

    public event Action Trigger;

    public OverTimeInvoker(double invokeTime, bool start = true)
    {
        _invokeTime = invokeTime;
        _hasStarted = start;
    }

    public Rectangle Rectangle { get; } = Rectangle.Empty;

    public void Update(GameTime gameTime)
    {
        if (!_hasStarted)
            return;

        _currentTime += gameTime.ElapsedGameTime.TotalMilliseconds;
        while (_currentTime > _invokeTime)
        {
            _currentTime -= _invokeTime;
            Trigger?.Invoke();
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
    }

    public void DrawStatic(SpriteBatch spriteBatch)
    {
    }

    public void Start()
        => _hasStarted = true;
}
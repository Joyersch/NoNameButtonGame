using System;
using Joyersch.Monogame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NoNameButtonGame;

public sealed class OverTimeInvoker : IManageable
{
    private double _invokeTime;
    private double _currentTime;
    private bool _hasStarted;

    public event Action Trigger;

    public bool InvokeOnce { get; set; } = false;

    public bool HasStarted => _hasStarted;

    public double ExecutedTime => _currentTime;

    public OverTimeInvoker(double invokeTime, bool start = true)
    {
        _invokeTime = invokeTime;
        _hasStarted = start;
    }

    public Rectangle Rectangle { get; } = Rectangle.Empty;

    public void Update(GameTime gameTime)
    {
        if (!_hasStarted || _invokeTime == 0F /*If this is 0F, it will cause an infinite loop*/)
            return;
        
        _currentTime += gameTime.ElapsedGameTime.TotalMilliseconds;
        while (_currentTime > _invokeTime)
        {
            _currentTime -= _invokeTime;
            Trigger?.Invoke();
            if (InvokeOnce)
                Stop();
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

    public void Stop()
    {
        _hasStarted = false;
        _currentTime = 0D;
    }

    public void Reset()
    {
        Stop();
        Start();
    }
    
    public bool ChangeTime(float time)
    {
        if (_hasStarted)
            return false;
        _invokeTime = time;
        return true;
    }

    public void ForceChangeTime(float time)
    {
        _invokeTime = time;
    }
}
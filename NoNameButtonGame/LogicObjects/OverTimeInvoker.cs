using System;
using Microsoft.Xna.Framework;
using NoNameButtonGame.Interfaces;

namespace NoNameButtonGame.LogicObjects;

public class OverTimeInvoker : IManageable
{
    private double _invokeTime;
    private double _currentTime;

    public event Action Trigger;

    public OverTimeInvoker(double invokeTime)
    {
        _invokeTime = invokeTime;
    }
    public void Update(GameTime gameTime)
    {
        _currentTime += gameTime.ElapsedGameTime.TotalMilliseconds;
        while (_currentTime > _invokeTime)
        {
            _currentTime -= _invokeTime;
            Trigger?.Invoke();
        }
    }
}
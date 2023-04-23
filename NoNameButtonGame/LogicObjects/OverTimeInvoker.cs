using System;
using Microsoft.Xna.Framework;
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

    public void Start()
        => _hasStarted = true;
}
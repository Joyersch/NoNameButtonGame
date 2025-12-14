using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NoNameButtonGame.Ui;

public sealed class OverTimeMover : IManageable
{
    private IMoveable _moveable;
    private MoveMode _mode;
    private float _moveIn;
    private Vector2 _start;
    private Vector2 _destination;

    private float _currentMoveTime;
    
    public bool IsMoving { get; private set; }

    public Rectangle Rectangle { get; } = Rectangle.Empty;

    public event Action ArrivedOnDestination;

    public enum MoveMode
    {
        Lin,
        Sin
    }

    public OverTimeMover(IMoveable moveable, Vector2 moveTo, float moveInTime, MoveMode moveMode)
    {
        _moveable = moveable;
        _mode = moveMode;
        _moveIn = moveInTime;
        _destination = moveTo;
        _start = _moveable.GetPosition();
    }

    public void Update(GameTime gameTime)
    {
        if (!IsMoving)
            return;
        
        _currentMoveTime += (float) gameTime.ElapsedGameTime.TotalMilliseconds;

        // This has to be calculated every time, since destination can be changes from the outside
        var move = _destination - _start;
        
        var moveTo = _mode switch
        {
            MoveMode.Lin => move * _currentMoveTime / _moveIn,
            MoveMode.Sin => move * (float) Math.Sin(_currentMoveTime / _moveIn * (Math.PI / 2))
        };
        
        _moveable.Move(_start + moveTo);
        
        if (_currentMoveTime >= _moveIn)
        {
            _moveable.Move(_destination);
            _start = _moveable.GetPosition();
            IsMoving = false;
            ArrivedOnDestination?.Invoke();
        }
    }

    public void Start()
    {
        IsMoving = true;
        _start = _moveable.GetPosition();
        _currentMoveTime = 0;
    }

    public void Stop()
    {
        IsMoving = false;
        _currentMoveTime = 0;
    }

    public void ChangeDestination(Vector2 newDestination)
        => _destination = newDestination;

    public void ChangeTime(float newTime)
        => _moveIn = newTime;

    public void ChangeMode(MoveMode mode)
        => _mode = mode;

    public void Draw(SpriteBatch spriteBatch)
    {
        // Nothing to draw
    }
}
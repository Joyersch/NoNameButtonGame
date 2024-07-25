using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Logic;
using MonoUtils.Logic.Hitboxes;
using MonoUtils.Logic.Management;
using MonoUtils.Ui.Logic;
using MonoUtils.Ui;
using NoNameButtonGame.GameObjects.Glitch;

namespace NoNameButtonGame.LevelSystem.LevelContainer.HoldButtonChallenge;

public class Row : IManageable, IInteractable, IMoveable, IMouseActions, IPlayable
{
    private readonly Random _random;
    private GlitchBlockCollection _left;
    private OverTimeMover _leftMover;
    private GlitchBlockCollection _right;
    private OverTimeMover _rightMover;

    private static float _time = 4250F;

    public event Action<object> Leave;
    public event Action<object> Enter;
    public event Action<object> Click;

    public Rectangle Rectangle { get; private set; }

    private Vector2 _leftPosition;
    private Vector2 _rightPosition;

    private bool _isPlaying;
    private bool _leftArrived;
    private bool _rightArrived;

    public Row(float distance, Vector2 singleSize, float scale, Random random)
    {
        _random = random;
        _left = new GlitchBlockCollection(singleSize, scale);
        _left.Leave += o => Leave?.Invoke(o);
        _left.Enter += o => Enter?.Invoke(o);
        _left.Click += o => Click?.Invoke(o);

        _right = new GlitchBlockCollection(singleSize, scale);
        _right.Leave += o => Leave?.Invoke(o);
        _right.Enter += o => Enter?.Invoke(o);
        _right.Click += o => Click?.Invoke(o);
        _right.GetAnchor(_left)
            .SetMainAnchor(AnchorCalculator.Anchor.TopRight)
            .SetSubAnchor(AnchorCalculator.Anchor.TopLeft)
            .SetDistanceX(distance)
            .Move();
        Rectangle = Rectangle.Union(_left.Rectangle, _right.Rectangle);

        _leftMover = new OverTimeMover(_left, Vector2.Zero, _time, OverTimeMover.MoveMode.Sin);
        _rightMover = new OverTimeMover(_right, Vector2.One, _time, OverTimeMover.MoveMode.Sin);
    }

    public void Update(GameTime gameTime)
    {
        _leftMover.Update(gameTime);
        _rightMover.Update(gameTime);
        _left.Update(gameTime);
        _right.Update(gameTime);
    }


    public void Draw(SpriteBatch spriteBatch)
    {
        _left.Draw(spriteBatch);
        _right.Draw(spriteBatch);
    }

    public void UpdateInteraction(GameTime gameTime, IHitbox toCheck)
    {
        _left.UpdateInteraction(gameTime, toCheck);
        _right.UpdateInteraction(gameTime, toCheck);
    }

    public Vector2 GetPosition()
        => Rectangle.Location.ToVector2();

    public Vector2 GetSize()
        => Rectangle.Size.ToVector2();

    public void Move(Vector2 newPosition)
    {
        var offset = newPosition - GetPosition();
        _left.Move(_left.GetPosition() + offset);
        _right.Move(_right.GetPosition() + offset);
        Rectangle = Rectangle.Union(_left.Rectangle, _right.Rectangle);
    }

    public void Play()
    {
        if (_isPlaying)
            return;

        _leftPosition = _left.GetPosition();
        _rightPosition = _right.GetPosition();
        _leftArrived = false;
        _rightArrived = false;

        _isPlaying = true;
        switch (_random.Next(3))
        {
            case 0:
                _leftMover.ChangeDestination(_rightPosition - new Vector2(_left.GetSize().X, 0));
                _leftMover.ChangeMode(OverTimeMover.MoveMode.Sin);
                _leftMover.ChangeTime(_time /  2.5F);
                _leftMover.ArrivedOnDestination += ResetPositions;
                _leftMover.Start();
                break;
            case 1:
                _rightMover.ChangeDestination(_leftPosition + new Vector2(_right.GetSize().X, 0));
                _rightMover.ChangeMode(OverTimeMover.MoveMode.Sin);
                _rightMover.ChangeTime(_time /  2.5F);
                _rightMover.ArrivedOnDestination += ResetPositions;
                _rightMover.Start();
                break;
            case 2:
                _leftMover.ChangeDestination(_rightPosition - new Vector2(_left.GetSize().X  * 1.5F, 0));
                _leftMover.ChangeMode(OverTimeMover.MoveMode.Sin);
                _leftMover.ChangeTime(_time / 2);
                _leftMover.ArrivedOnDestination += delegate
                {
                    _leftArrived = true;
                    if (_rightArrived)
                        _isPlaying = false;
                };
                _leftMover.Start();

                _rightMover.ChangeDestination(_leftPosition + new Vector2(_right.GetSize().X * 1.5F, 0));
                _rightMover.ChangeMode(OverTimeMover.MoveMode.Sin);
                _rightMover.ChangeTime(_time / 2);
                _rightMover.ArrivedOnDestination += ResetPositions;
                _rightMover.ArrivedOnDestination += delegate
                {
                    _rightArrived = true;
                    if (_leftArrived)
                        _isPlaying = false;
                };
                _rightMover.Start();

                break;
        }
    }

    private void ResetPositions()
    {
        _leftArrived = false;
        _rightArrived = false;
        _leftMover.ChangeDestination(_leftPosition);
        _leftMover.ChangeTime(_time);
        _leftMover.ChangeMode(OverTimeMover.MoveMode.Lin);
        _leftMover.ArrivedOnDestination += delegate
        {
            _leftArrived = true;
            if (_rightArrived)
                _isPlaying = false;
        };
        _leftMover.Start();

        _rightMover.ChangeDestination(_rightPosition);
        _rightMover.ChangeTime(_time);
        _rightMover.ChangeMode(OverTimeMover.MoveMode.Lin);
        _rightMover.ArrivedOnDestination += delegate
        {
            _rightArrived = true;
            if (_leftArrived)
                _isPlaying = false;
        };
        _rightMover.Start();
    }
}
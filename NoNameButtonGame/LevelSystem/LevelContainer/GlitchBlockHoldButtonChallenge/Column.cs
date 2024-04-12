using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Logging;
using MonoUtils.Logic;
using MonoUtils.Logic.Hitboxes;
using MonoUtils.Logic.Management;
using MonoUtils.Ui.Logic;
using MonoUtils.Ui.Objects;
using NoNameButtonGame.GameObjects.Glitch;

namespace NoNameButtonGame.LevelSystem.LevelContainer.GlitchBlockHoldButtonChallenge;

public class Column : IManageable, IInteractable, IMoveable, IMouseActions, IPlayable
{
    private readonly Random _random;
    private GlitchBlockCollection _up;
    private GlitchBlockCollection _down;
    private OverTimeMover _upMover;
    private OverTimeMover _downMover;

    private static float _time = 2000F;

    public event Action<object> Leave;
    public event Action<object> Enter;
    public event Action<object> Click;

    public Rectangle Rectangle { get; private set; }

    private Vector2 _upPosition;
    private Vector2 _downPosition;

    private bool _isPlaying;
    private bool _upArrived;
    private bool _downArrived;

    public Column(float distance, Vector2 singleSize, float scale, Random random)
    {
        _random = random;
        _up = new GlitchBlockCollection(singleSize, scale);
        _up.Leave += o => Leave?.Invoke(o);
        _up.Enter += o => Enter?.Invoke(o);
        _up.Click += o => Click?.Invoke(o);

        _down = new GlitchBlockCollection(singleSize, scale);
        _down.Leave += o => Leave?.Invoke(o);
        _down.Enter += o => Enter?.Invoke(o);
        _down.Click += o => Click?.Invoke(o);
        _down.GetAnchor(_up)
            .SetMainAnchor(AnchorCalculator.Anchor.BottomRight)
            .SetSubAnchor(AnchorCalculator.Anchor.TopRight)
            .SetDistanceY(distance)
            .Move();
        Rectangle = Rectangle.Union(_up.Rectangle, _down.Rectangle);

        _upMover = new OverTimeMover(_up, Vector2.Zero, _time, OverTimeMover.MoveMode.Sin);
        _downMover = new OverTimeMover(_down, Vector2.One, _time, OverTimeMover.MoveMode.Sin);
    }

    public void Update(GameTime gameTime)
    {
        _upMover.Update(gameTime);
        _downMover.Update(gameTime);
        _up.Update(gameTime);
        _down.Update(gameTime);
    }


    public void Draw(SpriteBatch spriteBatch)
    {
        _up.Draw(spriteBatch);
        _down.Draw(spriteBatch);
    }

    public void UpdateInteraction(GameTime gameTime, IHitbox toCheck)
    {
        _up.UpdateInteraction(gameTime, toCheck);
        _down.UpdateInteraction(gameTime, toCheck);
    }

    public Vector2 GetPosition()
        => Rectangle.Location.ToVector2();

    public Vector2 GetSize()
        => Rectangle.Size.ToVector2();

    public void Move(Vector2 newPosition)
    {
        var offset = newPosition - GetPosition();
        _up.Move(_up.GetPosition() + offset);
        _down.Move(_down.GetPosition() + offset);
        Rectangle = Rectangle.Union(_up.Rectangle, _down.Rectangle);
    }

    public void Play()
    {
        if (_isPlaying)
            return;

        _upPosition = _up.GetPosition();
        _downPosition = _down.GetPosition();
        _upArrived = false;
        _downArrived = false;

        _isPlaying = true;
        switch (_random.Next(3))
        {
            case 0:
                _upMover.ChangeDestination(_downPosition - new Vector2(0, _up.GetSize().Y));
                _upMover.ChangeMode(OverTimeMover.MoveMode.Sin);
                _upMover.ChangeTime(_time / 2.5F);
                _upMover.ArrivedOnDestination += ResetPositions;
                _upMover.Start();
                break;
            case 1:
                _downMover.ChangeDestination(_upPosition + new Vector2(0, _down.GetSize().Y));
                _downMover.ChangeMode(OverTimeMover.MoveMode.Sin);
                _downMover.ChangeTime(_time /  2.5F);
                _downMover.ArrivedOnDestination += ResetPositions;
                _downMover.Start();
                break;
            case 2:
                _upMover.ChangeDestination(_downPosition - new Vector2(0, _up.GetSize().Y * 1.5F));
                _upMover.ChangeMode(OverTimeMover.MoveMode.Sin);
                _upMover.ChangeTime(_time / 4);
                _upMover.Start();
                _upMover.ArrivedOnDestination += delegate
                {
                    _upArrived = true;
                    if (_downArrived)
                        _isPlaying = false;
                };

                _downMover.ChangeDestination(_upPosition + new Vector2(0, _down.GetSize().Y * 1.5F));
                _downMover.ChangeMode(OverTimeMover.MoveMode.Sin);
                _downMover.ChangeTime(_time / 4);
                _downMover.ArrivedOnDestination += ResetPositions;
                _downMover.Start();
                _downMover.ArrivedOnDestination += delegate
                {
                    _downArrived = true;
                    if (_upArrived)
                        _isPlaying = false;
                };

                break;
        }
    }
    private void ResetPositions()
    {
        _upArrived = false;
        _downArrived = false;
        _upMover.ChangeDestination(_upPosition);
        _upMover.ChangeTime(_time);
        _upMover.ChangeMode(OverTimeMover.MoveMode.Lin);
        _upMover.Start();
        _upMover.ArrivedOnDestination += delegate
        {
            _upArrived = true;
            if (_downArrived)
                _isPlaying = false;
        };

        _downMover.ChangeDestination(_downPosition);
        _downMover.ChangeTime(_time);
        _downMover.ChangeMode(OverTimeMover.MoveMode.Lin);
        _downMover.Start();
        _downMover.ArrivedOnDestination += delegate
        {
            _downArrived = true;
            if (_upArrived)
                _isPlaying = false;
        };
    }
}
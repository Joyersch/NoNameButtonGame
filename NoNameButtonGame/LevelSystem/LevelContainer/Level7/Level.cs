using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Logging;
using MonoUtils.Logic;
using MonoUtils.Logic.Text;
using MonoUtils.Ui;
using MonoUtils.Ui.Logic;
using MonoUtils.Ui.Objects.Buttons;
using MonoUtils.Ui.Objects.TextSystem;
using NoNameButtonGame.Colors;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level7;

internal class Level : SampleLevel
{
    private readonly CameraAnchorGrid _anchorGrid;

    private readonly TextButton _button;
    private bool _started;
    private readonly Timer _timer;
    private readonly Timer _idleTimer;

    private readonly FollowerCollection _followeres;
    private readonly OverTimeInvoker _idleSpawnerInvoker;
    private readonly PulsatingRed _color;

    private readonly DelayedText _info;

    public Level(Display display, Vector2 window, Random random) : base(display, window, random)
    {
        var textComponent = TextProvider.GetText("Levels.Level7");

        Name = textComponent.GetValue("Name");

        _anchorGrid = new CameraAnchorGrid(Camera, Cursor, 666F, OverTimeMover.MoveMode.Sin);

        _timer = new Timer(37500D /*60 seconds*/);
        _timer.GetCalculator(Display.Screen)
            .OnX(0.005F)
            .OnY(0.01F)
            .Move();
        _timer.Trigger += Finish;

        _followeres = new FollowerCollection(Cursor, Camera);
        _followeres.Enter += Fail;
        AutoManaged.Add(_followeres);

        _info = new DelayedText("Oh No... There coming!", false)
        {
            DisplayDelay = 50
        };

        _button = new TextButton("Start");
        _button.GetCalculator(Camera.Rectangle)
            .OnCenter()
            .Centered()
            .Move();
        _button.Click += delegate
        {
            if (_started)
                return;

            _started = true;
            _timer.Start();
            _followeres.Start();
            _info.Start();
        };


        // Starts spam spawning blocks if the activated
        _idleSpawnerInvoker = new OverTimeInvoker(50F, false);
        _idleSpawnerInvoker.Trigger += delegate { _followeres.Spawn(); };

        _color = new PulsatingRed()
        {
            NoGradient = true,
        };
        AutoManaged.Add(_color);

        // activates idle spawner if 5 seconds pass
        _idleTimer = new Timer(5000F);
        _idleTimer.Start();
        _idleTimer.GetAnchor(_timer)
            .SetMainAnchor(AnchorCalculator.Anchor.BottomLeft)
            .SetSubAnchor(AnchorCalculator.Anchor.TopLeft)
            .SetDistanceY(_idleTimer.GetSize().Y / 2)
            .Move();

        _idleTimer.Trigger += delegate
        {
            Log.WriteInformation("Idle check confirm");
            _idleSpawnerInvoker.Start();
            _color.Increment = 10;
        };

        ColorListener.Add(_color, _idleTimer);

        // resets the idle spawner check if the camera is moved
        _anchorGrid.StoppedMoving += delegate
        {
            Log.WriteInformation("Idle check reset");
            _idleTimer.Reset();
            _idleSpawnerInvoker.Stop();
            _color.Increment = 1;
        };
    }

    public override void Update(GameTime gameTime)
    {
        if (_started)
        {
            _anchorGrid.Update(gameTime);
            _timer.Update(gameTime);
            _idleSpawnerInvoker.Update(gameTime);
            _idleTimer.Update(gameTime);
            _info.Update(gameTime);
        }
        else
        {
            _button.UpdateInteraction(gameTime, Cursor);
            _button.Update(gameTime);
        }

        base.Update(gameTime);
    }

    protected override void Draw(SpriteBatch spriteBatch)
    {
        if (!_started)
        {
            _button.Draw(spriteBatch);
        }
        else
        {
            _info.Draw(spriteBatch);
        }

        base.Draw(spriteBatch);
    }

    protected override void DrawStatic(SpriteBatch spriteBatch)
    {
        base.DrawStatic(spriteBatch);
        if (!_started)
            return;

        _timer.Draw(spriteBatch);
        _idleTimer.Draw(spriteBatch);
    }
}
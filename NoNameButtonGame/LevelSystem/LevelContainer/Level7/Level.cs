using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoUtils;
using MonoUtils.Helper;
using MonoUtils.Logging;
using MonoUtils.Logic;
using MonoUtils.Logic.Text;
using MonoUtils.Ui;
using MonoUtils.Ui.Logic;
using MonoUtils.Ui.Objects.Buttons;
using MonoUtils.Ui.Objects.TextSystem;
using NoNameButtonGame.Colors;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.GameObjects.Glitch;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level7;

internal class Level : SampleLevel
{
    private readonly CameraAnchorGrid _anchorGrid;

    private readonly TextButton<SampleButton> _button;
    private bool _started;
    private readonly Timer _timer;
    private readonly Timer _idleTimer;

    private readonly FollowerCollection _followeres;
    private readonly OverTimeInvoker _idleSpawnerInvoker;
    private readonly PulsatingRed _color;

    private readonly DelayedText _info;
    private bool _hasDisplayedInfo;

    private bool _startedInitialize;
    private readonly GlitchBlockCollection _initializer;
    private readonly OverTimeMover _initializerMover;
    private readonly Text _initializerIndicator;
    private bool _initializerIsOffscreen;
    private bool _initializerWasOnScreen;

    public Level(Display display, Vector2 window, Random random) : base(display, window, random)
    {
        var textComponent = TextProvider.GetText("Levels.Level7");

        Name = textComponent.GetValue("Name");

        _anchorGrid = new CameraAnchorGrid(Camera, Cursor, 666F, OverTimeMover.MoveMode.Sin);

        _timer = new Timer(37500D /*35,5 seconds*/, textComponent.GetValue("FinishPrefix"));
        _timer.GetCalculator(Display.Screen)
            .OnX(0.005F)
            .OnY(0.01F)
            .Move();
        _timer.Trigger += Finish;

        _followeres = new FollowerCollection(Cursor, Camera);
        _followeres.Enter += Fail;
        AutoManaged.Add(_followeres);

        _info = new DelayedText(textComponent.GetValue("StartMessage"), false)
        {
            DisplayDelay = 50
        };

        var size = new Vector2(GlitchBlock.ImageSize.X * 8, Camera.RealSize.Y);
        _initializer = new GlitchBlockCollection(size);
        _initializer.ChangeColor(GlitchBlock.Color);
        _initializer.GetCalculator(Camera.Rectangle)
            .OnCenter()
            .Centered()
            .ByGridX(-1F)
            .Move();
        _initializer.Enter += Fail;

        var position = _initializer.GetPosition() + new Vector2(Camera.RealSize.X * 2, 0);
        _initializerMover = new OverTimeMover(_initializer, position, 10000, OverTimeMover.MoveMode.Lin);
        AutoManaged.Add(_initializerMover);

        _button = new Button(textComponent.GetValue("StartButton"));
        _button.GetCalculator(Camera.Rectangle)
            .OnCenter()
            .Centered()
            .Move();
        _button.Click += delegate
        {
            _initializerMover.Start();
            _startedInitialize = true;
        };

        _initializerIndicator = new Text("[arrow]");
        _initializerIndicator.ChangeColor(GlitchBlock.Color);
        MoveHelper.RotateTowards(_initializerIndicator.Letters[0], _initializer);
        _initializerIndicator.Letters[0].Rotation += (float)(Math.PI / 4F); // 45°
        _initializerIndicator.GetCalculator(Camera.Rectangle)
            .OnCenter()
            .OnX(0.33F)
            .Centered()
            .Move();

        // Starts spam spawning blocks if the activated
        _idleSpawnerInvoker = new OverTimeInvoker(100F, false);
        _idleSpawnerInvoker.Trigger += delegate { _followeres.Spawn(); };

        _color = new PulsatingRed()
        {
            NoGradient = true,
        };
        AutoManaged.Add(_color);

        // activates idle spawner if 5 seconds pass
        _idleTimer = new Timer(5000F, textComponent.GetValue("IdlePrefix"));
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
            if (!_started)
            {
                _started = true;
                _timer.Start();
                _followeres.Start();
            }

            if (!_hasDisplayedInfo)
            {
                _info.GetCalculator(Camera.Rectangle)
                    .OnCenter()
                    .Centered()
                    .Move();
                _info.Start();
                _hasDisplayedInfo = true;
            }

            Log.WriteInformation("Idle check reset");
            _idleTimer.Reset();
            _idleSpawnerInvoker.Stop();
            _color.Increment = 1;
        };
    }

    public override void Update(GameTime gameTime)
    {
        if (_startedInitialize)
            _anchorGrid.Update(gameTime);
        else
        {
            _button.UpdateInteraction(gameTime, Cursor);
            _button.Update(gameTime);
        }

        if (_initializerMover.IsMoving)
        {
            _initializerIndicator.Move(Cursor.GetPosition());
            MoveHelper.MoveTowards(_initializerIndicator, _initializer, 16);
            MoveHelper.RotateTowards(_initializerIndicator.Letters[0], _initializer);
            _initializerIndicator.Letters[0].Rotation += (float)(Math.PI / 4F); // 45°
            _initializerIndicator.Update(gameTime);
        }

        Log.WriteLine(Cursor.GetPosition().ToString(), 3);
        if (_started)
        {
            _timer.Update(gameTime);
            _idleSpawnerInvoker.Update(gameTime);
            _idleTimer.Update(gameTime);
            _info.Update(gameTime);
        }
        else
        {
            _initializer.Update(gameTime);
            _initializer.UpdateInteraction(gameTime, Cursor);
        }

        base.Update(gameTime);

        if (!_initializerWasOnScreen && Camera.Rectangle.Intersects(_initializer.Rectangle))
            _initializerWasOnScreen = true;
        _initializerIsOffscreen = !Camera.Rectangle.Intersects(_initializer.Rectangle);

        if (((!_initializerMover.IsMoving && _startedInitialize) // Has arrived on destination
             || (_initializerIsOffscreen && _initializerWasOnScreen)) // Was on screen but is not anymore
            && !_started)
        {
            _started = true;
            _timer.Start();
            _followeres.Start();
        }
    }

    protected override void Draw(SpriteBatch spriteBatch)
    {
        if (_initializerMover.IsMoving && !_started)
            _initializerIndicator.Draw(spriteBatch);

        if (!_startedInitialize)
        {
            _button.Draw(spriteBatch);
        }

        if (_started)
        {
            _info.Draw(spriteBatch);
        }
        else
        {
            _initializer.Draw(spriteBatch);
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
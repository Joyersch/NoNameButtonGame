using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoUtils;
using MonoUtils.Logging;
using MonoUtils.Logic;
using MonoUtils.Logic.Threading;
using MonoUtils.Ui;
using MonoUtils.Ui.Objects;
using MonoUtils.Ui.Objects.TextSystem;
using NoNameButtonGame.LevelSystem.LevelContainer.Level4.Overworld;
using MonoUtils.Ui.Menu;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level4;

public class Level : SampleLevel
{
    private readonly GameWindow _gameWindow;
    private const string QuestsPath = "Levels.Level4.Quests";
    private Cursor _cursor;
    private Vector2 _savedPosition;
    private bool _isLooking;
    private Text _objective;
    private Guid _castle;

    private LoadingScreen _loadingScreen;
    private LazyUpdater _lazyUpdater;

    private State _state;
    private UpdateState _updateState;
    private ViewState _viewState;

    private ResourceManager _resourceManager;
    private UserInterface _userInterface;
    private readonly Dictionary<Guid, UserInterface> _userInterfaces;

    private enum State
    {
        Start,
        Moved,
        SeenCastle,
        EnteredCastle
    }

    private enum ViewState
    {
        Overworld,
        Ui
    }

    private enum UpdateState
    {
        Overworld,
        Interface
    }

    private readonly string[] _objectives = new[]
    {
        "Objective: Find the castle!",
        "Objective: Enter the castle!",
        "Objective: Help the royal!"
    };

    private DelayedText _infoMoveText;
    private OverworldCollection _overworld;

    public Level(Display display, Vector2 window, GameWindow gameWindow, Random random) : base(display, window, random)
    {
        _gameWindow = gameWindow;
        Name = "Level 4 - RPG";

        _objective = new Text(string.Empty, display.SimpleScale);

        _infoMoveText = new DelayedText("Use Right-click to move around", false)
        {
            StartAfter = 3000F,
            DisplayDelay = 75
        };
        _infoMoveText.Start();
        _infoMoveText.GetCalculator(Camera.Rectangle)
            .OnCenter()
            .OnY(3, 10)
            .Centered()
            .Move();

        _overworld = new OverworldCollection(Random, Camera, new Vector2(100, 100));

        //base.Camera.Zoom = 0.5F;

        _overworld.Interaction += OpenLocationUserInterface;

        // will be set when required
        _userInterface = null;
        _userInterfaces = new Dictionary<Guid, UserInterface>();

        _resourceManager = new ResourceManager(random, _overworld.VillageCount);

        _cursor = new Cursor();
        Actuator = _cursor;
        PositionListener.Add(Mouse, _cursor);

        string questions = Global.ReadFromResources(QuestsPath);

        _loadingScreen =
            new LoadingScreen(Vector2.Zero, Window, _overworld.GenerateRequired, Display.SimpleScale,
                _overworld.GenerateGoal);
        //_loadingScreen.ProgressEnabled = false;

        _lazyUpdater = new LazyUpdater();
        _lazyUpdater.SetFunc(_overworld.Generate);
    }

    private void OpenLocationUserInterface(ILocation obj)
    {
        var guid = obj.GetGuid();
        var name = obj.GetName();

        if (_state == State.SeenCastle && _castle == guid)
            _state++;

        if (!_userInterfaces.ContainsKey(guid))
        {
            _userInterface =
                new UserInterface(_resourceManager.GetTrades(guid), _resourceManager, _gameWindow, name, 1F);
            _userInterface.Exit += UserInterfaceOnExit;
            _userInterfaces.Add(guid, _userInterface);
        }
        else
            _userInterface = _userInterfaces[guid];

        _userInterface.GetCalculator(Camera.Rectangle)
            .OnCenter()
            .Centered()
            .Move();

        _viewState = ViewState.Ui;
    }

    private void UserInterfaceOnExit()
    {
        _userInterface = null;
        _viewState = ViewState.Overworld;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);

        if (!_overworld.HasFullyGenerated)
            return;

        _overworld.Draw(spriteBatch);
        _infoMoveText.Draw(spriteBatch);
        _userInterface?.Draw(spriteBatch);
        _cursor.Draw(spriteBatch);
    }

    public override void DrawStatic(SpriteBatch spriteBatch)
    {
        base.DrawStatic(spriteBatch);
        if (!_overworld.HasFullyGenerated)
        {
            _loadingScreen.Draw(spriteBatch);
            return;
        }

        if (_viewState == ViewState.Ui)
            return;

        if (_state > State.Start)
            _objective.Draw(spriteBatch);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (InputReaderKeyboard.CheckKey(Keys.F1, true))
            Camera.Zoom = 2F;
        if (InputReaderKeyboard.CheckKey(Keys.F2, true))
            Camera.Zoom = 0.1F;
        if (InputReaderKeyboard.CheckKey(Keys.F3, true))
            Camera.Zoom = 100F;

        if (!_overworld.HasFullyGenerated)
        {
            _lazyUpdater.Update(gameTime);
            _loadingScreen.SetCurrent(_overworld.GenerateCurrent);
            _loadingScreen.SetMax(_overworld.GenerateRequired);
            _loadingScreen.SetGoal(_overworld.GenerateGoal);
            _loadingScreen.Update(gameTime);
            //_overworld.Update(gameTime);
            return;
        }

        _cursor.Update(gameTime);

        if (_viewState != ViewState.Ui)
            _overworld.UpdateInteraction(gameTime, _cursor);

        _overworld.Update(gameTime);

        if (_viewState == ViewState.Ui)
        {
            _userInterface?.UpdateInteraction(gameTime, _cursor);
            _userInterface?.Update(gameTime);
            return;
        }

        if (InputReaderMouse.CheckKey(InputReaderMouse.MouseKeys.Right, false))
        {
            if (_state == State.Start)
                _state++;

            if (!_isLooking)
            {
                _savedPosition = Mouse.Position;
                _isLooking = true;
            }

            var difference = _savedPosition - Mouse.Position;

            Camera.Move(Camera.Position + Vector2.Floor(difference));
            var overlap = Rectangle.Intersect(Camera.Rectangle, _overworld.Rectangle);
            if (overlap != Camera.Rectangle)
            {
                Vector2 correction = Vector2.Zero;

                var left = overlap.Left - Camera.Rectangle.Left;
                var right = overlap.Right - Camera.Rectangle.Right;
                var top = overlap.Top - Camera.Rectangle.Top;
                var bottom = overlap.Bottom - Camera.Rectangle.Bottom;

                var width = Camera.Rectangle.Width - overlap.Width;
                var height = Camera.Rectangle.Height - overlap.Height;

                if (left != 0)
                    correction.X += width;

                if (right != 0)
                    correction.X -= width;

                if (top != 0)
                    correction.Y += height;

                if (bottom != 0)
                    correction.Y -= height;
                Camera.Move(Camera.Position + Vector2.Floor(correction));
            }
        }
        else
            _isLooking = false;

        _infoMoveText.Update(gameTime);

        if (_overworld.CastleOnScreen && _state == State.Moved)
            _state++;
    }
}
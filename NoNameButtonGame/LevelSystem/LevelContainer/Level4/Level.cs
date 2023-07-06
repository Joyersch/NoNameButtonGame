using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils;
using MonoUtils.Logging;
using MonoUtils.Logic;
using MonoUtils.Ui;
using MonoUtils.Ui.Objects;
using MonoUtils.Ui.Objects.TextSystem;
using NoNameButtonGame.LevelSystem.LevelContainer.Level4.Overworld;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level4;

public class Level : SampleLevel
{
    private const string QuestsPath = "Levels.Level4.Quests";
    private Cursor _cursor;
    private Vector2 _savedPosition;
    private bool _isLooking;
    private Text _objective;
    private Guid _castle;

    private State _state;
    private UpdateState _updateState;
    private ViewState _viewState;

    private ResourceManager _resourceManager;
    private UserInterface _userInterface;

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

    public Level(Display display, Vector2 window, Random random) : base(display, window, random)
    {
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

        _overworld = new OverworldCollection(Random, Camera);
        _overworld.GenerateTrees(100000);
        _overworld.GenerateVillages(40);
        _castle = _overworld.GenerateCastle();
        _overworld.Interaction += OpenLocationUserInterface;
        AutoManaged.Add(_overworld);

        // will be set when required
        _userInterface = null;

        _cursor = new Cursor();
        Actuator = _cursor;
        PositionListener.Add(Mouse, _cursor);

        string questions = Global.ReadFromResources(QuestsPath);
    }

    private void OpenLocationUserInterface(ILocation obj)
    {
        if (_state == State.SeenCastle && _castle == obj.GetGuid())
            _state++;

        _userInterface = new UserInterface(_resourceManager, obj.GetName(), 1F);
        _userInterface.Exit += UserInterfaceOnExit;
        _userInterface.GetCalculator(Camera.Rectangle)
            .OnCenter()
            .Centered()
            .Move();
        _viewState = ViewState.Ui;
        Log.Write(obj.GetName() ?? obj.GetType().Name);
    }

    private void UserInterfaceOnExit()
    {
        _userInterface = null;
        _viewState = ViewState.Overworld;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        _infoMoveText.Draw(spriteBatch);
        _userInterface?.Draw(spriteBatch);
        _cursor.Draw(spriteBatch);
    }

    public override void DrawStatic(SpriteBatch spriteBatch)
    {
        base.DrawStatic(spriteBatch);
        if (_viewState == ViewState.Ui)
            return;

        if (_state > State.Start)
            _objective.Draw(spriteBatch);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        _cursor.Update(gameTime);
        Log.WriteLine(Camera.Position.ToString(), 0);
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

            var offset = _savedPosition - Mouse.Position;

            var newPosition = Camera.Position + Vector2.Floor(offset);
            if (_overworld.Rectangle.Intersects(new Rectangle(newPosition.ToPoint(), new Point(1, 1))))
                Camera.Move(newPosition);
        }
        else
            _isLooking = false;

        _infoMoveText.Update(gameTime);

        if (_overworld.CastleOnScreen && _state == State.Moved)
            _state++;
    }
}
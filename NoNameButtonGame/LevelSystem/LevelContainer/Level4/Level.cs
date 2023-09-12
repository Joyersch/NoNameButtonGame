using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
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
    private readonly GameWindow _gameWindow;
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

        _overworld = new OverworldCollection(Random, Camera);
        int villageCount = 40;

        _overworld.GenerateVillages(villageCount);
        _castle = _overworld.GenerateCastle();
        _overworld.GenerateForests(100);
        //_overworld.GenerateTrees(1000);

        List<ConnectedGameObject> csgos = new List<ConnectedGameObject>();

        for (int x = 0; x < 7; x++)
        {
            for (int y = 0; y < 7; y++)
            {
                if (!(y == 3 || x == 3) || x <= 1 || x >= 5 || y <= 1 || y >= 5)
                    csgos.Add(new Forest(new Vector2(x * 32, y * 32), new Vector2(32, 32)){DrawColor = Color.Green});
            }
        }

        foreach (var csgo in csgos)
        {
            csgo.SetTextureLocation(csgos);
            AutoManaged.Add(csgo);
        }

        _overworld.Interaction += OpenLocationUserInterface;

        // will be set when required
        _userInterface = null;
        _userInterfaces = new Dictionary<Guid, UserInterface>();

        _resourceManager = new ResourceManager(random, villageCount);

        _cursor = new Cursor();
        Actuator = _cursor;
        PositionListener.Add(Mouse, _cursor);

        string questions = Global.ReadFromResources(QuestsPath);
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
        _overworld.Draw(spriteBatch);
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
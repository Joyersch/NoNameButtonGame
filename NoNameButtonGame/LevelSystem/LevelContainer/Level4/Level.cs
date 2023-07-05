using System;
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
    
    private enum State
    {
        Start,
        Moved,
        SeenCastle,
        EnteredCastle
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

        string questions = Global.ReadFromResources(QuestsPath);


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

        _cursor = new Cursor();
        Actuator = _cursor;
        AutoManaged.Add(_cursor);
        PositionListener.Add(Mouse, _cursor);
    }

    private void OpenLocationUserInterface(ILocation obj)
    {
        if (_state == State.SeenCastle && _castle == obj.GetGuid())
            _state++;
        
        Log.Write(obj.GetName() ?? obj.GetType().Name);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        _infoMoveText.Draw(spriteBatch);
    }

    public override void DrawStatic(SpriteBatch spriteBatch)
    {
        base.DrawStatic(spriteBatch);
        if (_state > State.Start)
            _objective.Draw(spriteBatch);
    }

    public override void Update(GameTime gameTime)
    {
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

        base.Update(gameTime);
        if (_overworld.CastleOnScreen && _state == State.Moved)
            _state++;
    }
}
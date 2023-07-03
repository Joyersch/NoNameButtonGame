using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils;
using MonoUtils.Logging;
using MonoUtils.Logic;
using MonoUtils.Logic.Hitboxes.Collision;
using MonoUtils.Ui;
using MonoUtils.Ui.Objects;
using MonoUtils.Ui.Objects.TextSystem;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level4;

public class Level : SampleLevel
{
    private Cursor _cursor;
    private Vector2 _savedPosition;
    private bool _isLooking;

    private DelayedText _infoMoveText;
    private OverworldCollection _overworld;

    public Level(Display display, Vector2 window, Random random) : base(display, window, random)
    {
        Name = "Level 4 - RPG";
        _infoMoveText = new DelayedText("Use Right-click to move around", false)
        {
            StartAfter = 10000F,
            DisplayDelay = 75
        };
        _infoMoveText.Start();
        _infoMoveText.GetCalculator(Camera.Rectangle)
            .OnCenter()
            .OnY(3, 10)
            .Centered()
            .Move();

        _overworld = new OverworldCollection();
        AutoManaged.Add(_overworld);

        _cursor = new Cursor();
        Actuator = _cursor;
        AutoManaged.Add(_cursor);
        PositionListener.Add(Mouse, _cursor);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        _infoMoveText.Draw(spriteBatch);
    }

    public override void Update(GameTime gameTime)
    {
        if (InputReaderMouse.CheckKey(InputReaderMouse.MouseKeys.Right, false))
        {
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
        Log.WriteLine(Camera.Position.ToString(), 0);
        
        base.Update(gameTime);
    }
}
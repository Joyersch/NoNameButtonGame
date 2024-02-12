using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Logging;
using MonoUtils.Logic;
using MonoUtils.Logic.Hitboxes;
using MonoUtils.Logic.Management;
using MonoUtils.Ui;
using MonoUtils.Ui.Logic.Listener;
using MonoUtils.Ui.Objects;
using MonoUtils.Ui.Objects.TextSystem;
using NoNameButtonGame.Colors;
using NoNameButtonGame.GameObjects.Glitch;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level7;

public class FollowerCollection : IManageable, IInteractable
{
    private readonly Cursor _cursor;
    private readonly Camera _camera;
    private readonly float _speed;
    private readonly List<GlitchBlockCollection> _blocks;
    private readonly List<Indicator> _indicator;

    private readonly PulsatingRed _indicatorColor;
    private readonly ColorListener _colorListener;

    private OverTimeInvoker _invoker;

    public event Action Enter;
    private bool _started;
    private bool _blockToFar;
    private float _distance;

    public FollowerCollection(Cursor cursor, Camera camera)
    {
        _cursor = cursor;
        _camera = camera;
        _speed = 250F;
        _blocks = new List<GlitchBlockCollection>();
        _indicator = new List<Indicator>();
        _invoker = new OverTimeInvoker(1000F, false);
        _invoker.Trigger += SpawnNewBlock;
        _distance = _camera.RealSize.Length() / 2;
        _indicatorColor = new PulsatingRed();
        _colorListener = new ColorListener();
    }

    public Rectangle Rectangle => _camera.Rectangle;

    public void Update(GameTime gameTime)
    {
        _blockToFar = true;
        for (var i = 0; i < _blocks.Count; i++)
        {
            var block = _blocks[i];
            _indicator[i].CanDraw = false;

            var blockCenter = block.Position + block.Size / 2;
            // determine vector towards the player
            var fromBlockDirection = _cursor.Position - blockCenter;

            var fromBlockDirectionNormalized = Vector2.Normalize(fromBlockDirection);

            var length = fromBlockDirection.Length();

            if (length <= _distance * 2)
            {
                _indicator[i].CanDraw = true;
                var fromCursorDirection = blockCenter - _cursor.Position;
                var text = _indicator[i].Text;
                var letter = text.Letters[0];

                // + 45 degrees as the texture is rotated -45 degrees
                letter.Rotation = (float)(Math.Atan2(fromCursorDirection.Y, fromCursorDirection.X) + Math.PI / 4F);

                fromCursorDirection.Normalize();
                fromCursorDirection *= 10;
                text.Move(_cursor.Position + fromCursorDirection);
            }

            // compare distance to player to size of camera
            if (length <= _distance * 0.8)
                _blockToFar = false;

            // move block towards the player
            if (_started)
                block.Move(block.Position + fromBlockDirectionNormalized * (_speed + _blocks.Count * 3) *
                    (gameTime.ElapsedGameTime.Milliseconds / 1000F));
            block.Update(gameTime);
        }

        foreach (var indicator in _indicator)
        {
            indicator.Text.Update(gameTime);
        }

        _invoker.Update(gameTime);
        _indicatorColor.Update(gameTime);
        _colorListener.Update(gameTime);
    }

    private void SpawnNewBlock()
    {
        if (_blockToFar && _started)
            Spawn();
    }

    public void UpdateInteraction(GameTime gameTime, IHitbox toCheck)
    {
        foreach (var block in _blocks)
        {
            block.UpdateInteraction(gameTime, toCheck);
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var indicator in _indicator.Where(indicator => indicator.CanDraw))
        {
            indicator.Text.Draw(spriteBatch);
        }

        foreach (var block in _blocks)
        {
            block.Draw(spriteBatch);
        }
    }

    public void Spawn()
    {
        var block = new GlitchBlockCollection(GlitchBlock.DefaultSize);
        block.ChangeColor(GlitchBlock.Color);

        var player = _cursor.Rectangle.Center.ToVector2();
        var camera = _camera.RealPosition + _camera.RealSize / 2;

        var difference = player - camera;
        difference.Normalize();

        block.Move(_cursor.Position + difference * _distance * 1.5F);
        block.Enter += delegate { Enter?.Invoke(); };
        _blocks.Add(block);

        var text = new Text("[arrow]");
        var letter = text.Letters[0];
        letter.Origin = new Vector2(2, 5);

        _colorListener.Add(_indicatorColor, text);
        var indicator = new Indicator
        {
            Text = text,
            CanDraw = false
        };
        _indicator.Add(indicator);
    }

    public void Start()
    {
        _started = true;
        _invoker.Start();
    }

    public void Stop()
    {
        _started = false;
        _invoker.Stop();
    }
}
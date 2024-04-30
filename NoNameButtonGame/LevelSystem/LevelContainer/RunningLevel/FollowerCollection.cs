using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Helper;
using MonoUtils.Logic;
using MonoUtils.Logic.Hitboxes;
using MonoUtils.Logic.Management;
using MonoUtils.Ui;
using MonoUtils.Ui.Logic.Listener;
using MonoUtils.Ui.Objects;
using MonoUtils.Ui.Objects.TextSystem;
using NoNameButtonGame.Colors;
using NoNameButtonGame.GameObjects.Glitch;

namespace NoNameButtonGame.LevelSystem.LevelContainer.RunningLevel;

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
    private bool _blockOnScreen;
    private float _distance;

    public FollowerCollection(Cursor cursor, Camera camera, float spawnTime, float speed)
    {
        _cursor = cursor;
        _camera = camera;
        _speed = speed;
        _blocks = new List<GlitchBlockCollection>();
        _indicator = new List<Indicator>();
        _invoker = new OverTimeInvoker(spawnTime, false);
        _invoker.Trigger += SpawnNewBlock;
        _distance = _camera.RealSize.Length() / 2;
        _indicatorColor = new PulsatingRed();
        _colorListener = new ColorListener();
    }

    public Rectangle Rectangle => _camera.Rectangle;

    public void Update(GameTime gameTime)
    {
        _blockToFar = true;
        _blockOnScreen = !_started;

        for (var i = 0; i < _blocks.Count; i++)
        {
            var block = _blocks[i];
            _indicator[i].CanDraw = false;

            var blockCenter = block.GetPosition() + block.GetSize() / 2;
            // determine vector towards the player
            var fromBlockDirection = _cursor.GetPosition() - blockCenter;

            var length = fromBlockDirection.Length();

            if (length <= _distance * 2)
            {
                _indicator[i].CanDraw = true;
                var text = _indicator[i].Text;

                var letter = text.Letters[0];
                letter.Origin = new Vector2(2.5F, 2.5F);
                MoveHelper.RotateTowards(letter, block);
                // + 45 degrees as the texture is rotated -45 degrees
                letter.Rotation += (float)(Math.PI / 4F);

                text.Move(_cursor.GetPosition());
                MoveHelper.MoveTowards(text, block, 16);
            }

            // compare distance to player to size of camera
            if (length <= _distance * 0.8)
                _blockToFar = false;

            // move block towards the player
            if (_started)
            {
                var distance = (_speed + _blocks.Count * 3) * (gameTime.ElapsedGameTime.Milliseconds / 1000F);
                if (!block.Rectangle.Intersects(_camera.Rectangle))
                    distance *= length / 200;
                else
                    _blockOnScreen = true;
                MoveHelper.MoveTowards(block, _cursor, distance);
            }

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
        if ((_blockToFar || !_blockOnScreen) && _started)
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
        var block = new GlitchBlockCollection(GlitchBlock.ImageSize * 4);
        block.ChangeColor(GlitchBlock.Color);
        block.InRectangle(_camera.Rectangle)
            .OnCenter()
            .Centered()
            .Move();

        // minus distance moves away from the player
        MoveHelper.MoveTowards(block, _cursor, _distance * 1.5F);
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
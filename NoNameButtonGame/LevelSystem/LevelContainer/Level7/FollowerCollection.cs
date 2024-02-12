using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Logging;
using MonoUtils.Logic;
using MonoUtils.Logic.Hitboxes;
using MonoUtils.Logic.Management;
using MonoUtils.Ui;
using MonoUtils.Ui.Objects;
using NoNameButtonGame.GameObjects.Glitch;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level7;

public class FollowerCollection : IManageable, IInteractable
{
    private readonly Cursor _player;
    private readonly Camera _camera;
    private readonly float _speed;
    private readonly List<GlitchBlockCollection> _blocks;

    private OverTimeInvoker _invoker;

    public event Action Enter;
    private bool _started;
    private bool _blockToFar;
    private float _distance;

    public FollowerCollection(Cursor player, Camera camera)
    {
        _player = player;
        _camera = camera;
        _speed = 250F;
        _blocks = new List<GlitchBlockCollection>();
        _invoker = new OverTimeInvoker(1000F, false);
        _invoker.Trigger += SpawnNewBlock;
        _distance = _camera.RealSize.Length() / 2;
    }

    public Rectangle Rectangle => _camera.Rectangle;

    public void Update(GameTime gameTime)
    {
        _blockToFar = true;
        foreach (var block in _blocks)
        {
            // determine vector towards the player
            var direction = _player.Position - block.Position - block.Size / 2;

            // compare distance to player to size of camera
            if (direction.Length() <= _distance * 0.8)
                _blockToFar = false;
            direction.Normalize();

            // move block towards the player
            if (_started)
                block.Move(block.Position + direction * (_speed + _blocks.Count * 3) *
                    (gameTime.ElapsedGameTime.Milliseconds / 1000F));
            block.Update(gameTime);
        }

        _invoker.Update(gameTime);
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
        foreach (var block in _blocks)
        {
            block.Draw(spriteBatch);
        }
    }

    public void Spawn()
    {
        var block = new GlitchBlockCollection(GlitchBlock.DefaultSize);
        block.ChangeColor(GlitchBlock.Color);

        var player = _player.Rectangle.Center.ToVector2();
        var camera = _camera.RealPosition + _camera.RealSize / 2;

        var difference = player - camera;
        difference.Normalize();

        block.Move(_player.Position + difference * _distance * 1.5F);
        block.Enter += delegate { Enter?.Invoke(); };
        _blocks.Add(block);
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
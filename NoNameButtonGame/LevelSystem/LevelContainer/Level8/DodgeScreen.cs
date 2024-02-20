using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils;
using MonoUtils.Helper;
using MonoUtils.Logging;
using MonoUtils.Logic;
using MonoUtils.Logic.Hitboxes;
using MonoUtils.Logic.Management;
using MonoUtils.Ui;
using MonoUtils.Ui.Objects.Buttons;
using NoNameButtonGame.GameObjects.Glitch;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level8;

public class DodgeScreen : IManageable, IInteractable, IMoveable
{
    private readonly Camera _camera;
    private Vector2 _position;
    public Rectangle Rectangle { get; }

    private readonly List<GlitchBlockCollection> _blocks;
    private readonly List<GlitchBlockCollection> _walls;
    private readonly SampleObject _debug;

    public DodgeScreen(Camera camera)
    {
        _camera = camera;
        Rectangle = camera.Rectangle;
        _position = camera.RealPosition;
        _blocks = new List<GlitchBlockCollection>();
        _walls = new List<GlitchBlockCollection>();


        var wallSize = new Vector2(GlitchBlock.ImageSize.X * 6F, _camera.RealSize.Y);
        Log.WriteInformation($"wall_Y={wallSize.Y}");
        var left = new GlitchBlockCollection(wallSize);

        left.GetCalculator(camera.Rectangle)
            .Move();
        _walls.Add(left);

        var right = new GlitchBlockCollection(wallSize);

        right.GetCalculator(camera.Rectangle)
            .OnX(1F)
            .BySizeX(-1F)
            .Move();
        _walls.Add(right);

        _debug = new SampleObject(Vector2.Zero, wallSize);
        _debug.GetCalculator(camera.Rectangle)
            .BySizeX(1F)
            .Move();
    }

    public void ChangePattern(int[] pattern)
    {
    }


    public void Update(GameTime gameTime)
    {
        foreach (var wall in _walls)
        {
           wall.Update(gameTime);
        }
        _debug.Update(gameTime);
    }

    public void UpdateInteraction(GameTime gameTime, IHitbox toCheck)
    {
        foreach (var wall in _walls)
        {
            wall.UpdateInteraction(gameTime, toCheck);
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var wall in _walls)
        {
            wall.Draw(spriteBatch);
        }
        _debug.Draw(spriteBatch);
    }

    public Vector2 GetPosition()
        => _position;

    public Vector2 GetSize()
        => _camera.RealSize;

    public void Move(Vector2 newPosition)
    {
        var relative = MoveHelper.GetRelative(_position, newPosition);
        foreach (var wall in _walls)
        {
            wall.Move(wall.GetPosition() + relative);
        }

        _position = newPosition;
    }
}
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using MonoUtils;
using MonoUtils.Logic;
using MonoUtils.Logic.Hitboxes;
using MonoUtils.Logic.Management;
using MonoUtils.Ui.Objects;
using MonoUtils.Ui.Color;
using MonoUtils.Ui.Logic;

namespace NoNameButtonGame.GameObjects.Glitch;

internal class GlitchBlockCollection : GameObject, IMouseActions, IMoveable, IColorable, IInteractable
{
    private readonly List<GlitchBlock> _glitchBlocksGrid;

    public event Action<object> Leave;
    public event Action<object> Enter;
    public event Action<object> Click;

    private MouseActionsMat _mouseActionsMat;

    public GlitchBlockCollection(Vector2 size) : this(Vector2.Zero, size, GlitchBlock.DefaultSize)
    {
    }

    public GlitchBlockCollection(Vector2 position, Vector2 size) : this(position, size, GlitchBlock.DefaultSize)
    {
    }

    public GlitchBlockCollection(Vector2 position, Vector2 size, float singleScale) : this(position, size,
        GlitchBlock.DefaultSize * singleScale)
    {
    }

    public GlitchBlockCollection(Vector2 position, Vector2 size, Vector2 singleSize) : base(position, size,
        DefaultTexture, DefaultMapping)
    {
        var grid = size / singleSize;
        var gridEdge = new Vector2(size.X % singleSize.X, size.Y % singleSize.Y);

        grid.Floor();
        grid += Vector2.Ceiling(gridEdge / singleSize);

        _glitchBlocksGrid = new();
        for (int y = 0; y < grid.Y; y++)
        {
            for (int x = 0; x < grid.X; x++)
            {
                var newSize = singleSize;
                if (gridEdge.X > 0 && x + 1 == (int)grid.X)
                    newSize.X = gridEdge.X;
                if (gridEdge.Y > 0 && y + 1 == (int)grid.Y)
                    newSize.Y = gridEdge.Y;

                var block = new GlitchBlock(
                    new Vector2(position.X + x * singleSize.X, position.Y + y * singleSize.Y),
                    newSize,
                    new Rectangle(Vector2.Zero.ToPoint(), (newSize / 2).ToPoint()));
                _glitchBlocksGrid.Add(block);
            }
        }

        _mouseActionsMat = new MouseActionsMat(this);
        _mouseActionsMat.Leave += delegate { Leave?.Invoke(this); };
        _mouseActionsMat.Enter += delegate { Enter?.Invoke(this); };
        _mouseActionsMat.Click += delegate { Click?.Invoke(this); };
    }


    public override void Update(GameTime gameTime)
    {
        for (int i = 0; i < _glitchBlocksGrid.Count; i++)
        {
            _glitchBlocksGrid[i].Update(gameTime);
        }

        base.Update(gameTime);
    }

    public void UpdateInteraction(GameTime gameTime, IHitbox toCheck)
    {
        for (int i = 0; i < _glitchBlocksGrid.Count; i++)
        {
            _glitchBlocksGrid[i].UpdateInteraction(gameTime, toCheck);
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        for (int i = 0; i < _glitchBlocksGrid.Count; i++)
        {
            _glitchBlocksGrid[i].Draw(spriteBatch);
        }
    }

    protected override void CalculateHitboxes()
    {
        if (_glitchBlocksGrid is null)
            return;
        Hitboxes = _glitchBlocksGrid.SelectMany(block => block.Hitbox).ToArray();
    }

    public Vector2 GetPosition()
        => Position;

    public Vector2 GetSize()
        => Size;

    public void Move(Vector2 newPosition)
    {
        var offset = newPosition - Position;
        Position += offset;
        foreach (var block in _glitchBlocksGrid)
        {
            block.Move(block.Position + offset);
        }
    }

    public void ChangeColor(Color[] input)
    {
        foreach (var glitchBlock in _glitchBlocksGrid)
        {
            glitchBlock.ChangeColor(input);
        }
    }

    public int ColorLength()
        => 1;
}
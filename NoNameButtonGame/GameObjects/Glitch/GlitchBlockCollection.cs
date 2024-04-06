using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using MonoUtils;
using MonoUtils.Helper;
using MonoUtils.Logic;
using MonoUtils.Logic.Hitboxes;
using MonoUtils.Logic.Management;
using MonoUtils.Ui.Objects;
using MonoUtils.Ui.Color;
using MonoUtils.Ui.Logic;

namespace NoNameButtonGame.GameObjects.Glitch;

internal class GlitchBlockCollection : IHitbox, IManageable, ILayerable, IMouseActions, IMoveable, IColorable,
    IInteractable
{
    private Vector2 _position;
    private Vector2 _size;
    private Vector2 _scale;
    private Color _color;

    private readonly MouseActionsMat _mouseActionsMat;

    private readonly List<GlitchBlock> _glitchBlocksGrid;

    public event Action<object> Leave;
    public event Action<object> Enter;
    public event Action<object> Click;

    private Rectangle[] _hitboxes;
    public Rectangle[] Hitbox => _hitboxes;

    public Rectangle Rectangle { get; private set; }

    public float Layer { get; set; }

    public GlitchBlockCollection(Vector2 size) : this(Vector2.Zero, size, GlitchBlock.ImageSize * 4)
    {
    }

    public GlitchBlockCollection(Vector2 position, Vector2 size) : this(position, size, GlitchBlock.ImageSize * 4)
    {
    }

    public GlitchBlockCollection(Vector2 size, float singleScale) : this(Vector2.Zero, size,
        GlitchBlock.ImageSize * singleScale)
    {
    }

    public GlitchBlockCollection(Vector2 position, Vector2 size, float singleScale) : this(position, size,
        GlitchBlock.ImageSize * singleScale)
    {
    }

    public GlitchBlockCollection(Vector2 position, Vector2 size, Vector2 singleSize)
    {
        _position = position;
        _size = size;
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
                    newSize, singleSize);
                block.Leave += delegate { Leave?.Invoke(this); };
                block.Enter += delegate { Enter?.Invoke(this); };
                block.Click += delegate { Click?.Invoke(this); };
                _glitchBlocksGrid.Add(block);
            }
        }
    }

    public void Update(GameTime gameTime)
    {
        for (int i = 0; i < _glitchBlocksGrid.Count; i++)
        {
            _glitchBlocksGrid[i].Update(gameTime);
        }
    }

    public void UpdateInteraction(GameTime gameTime, IHitbox toCheck)
    {
        for (int i = 0; i < _glitchBlocksGrid.Count; i++)
        {
            _glitchBlocksGrid[i].UpdateInteraction(gameTime, toCheck);
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        for (int i = 0; i < _glitchBlocksGrid.Count; i++)
        {
            _glitchBlocksGrid[i].Draw(spriteBatch);
        }
    }

    public void Move(Vector2 newPosition)
    {
        var offset = newPosition - _position;
        _position += offset;
        foreach (var block in _glitchBlocksGrid)
        {
            block.Move(block.GetPosition() + offset);
        }

        Rectangle = this.GetRectangle();
        _hitboxes = _glitchBlocksGrid.SelectMany(block => block.Hitbox).ToArray();
    }

    public void ChangeColor(Color[] input)
    {
        foreach (var glitchBlock in _glitchBlocksGrid)
        {
            glitchBlock.ChangeColor(input);
        }
    }

    public void ChangeColor(Color input)
    {
        foreach (var glitchBlock in _glitchBlocksGrid)
        {
            glitchBlock.ChangeColor(new[] { input });
        }
    }

    public int ColorLength()
        => 1;

    public Color[] GetColor()
        => (Color[])_glitchBlocksGrid.Select(g => g.GetColor()[0]);


    public Vector2 GetPosition()
        => _position;

    public Vector2 GetSize()
        => _size;
}
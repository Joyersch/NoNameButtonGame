using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.Interfaces;
using System.Linq;

namespace NoNameButtonGame.GameObjects;

internal class GlitchBlockCollection : GameObject, IMouseActions, IMoveable, IColorable
{
    private readonly GlitchBlock[] _glitchBlocksGrid;
    private bool _hover;

    public event Action<object> Leave;
    public event Action<object> Enter;
    public event Action<object> Click;

    private Color _oldDrawColor;

    public GlitchBlockCollection(Vector2 position, Vector2 size) : this(position, size, GlitchBlock.DefaultSize)
    {
    }

    public GlitchBlockCollection(Vector2 position, Vector2 size, float singleScale) : this(position, size,
        GlitchBlock.DefaultSize * singleScale)
    {
    }

    public GlitchBlockCollection(Vector2 position, Vector2 size, Vector2 singleSize) : base(position, size, DefaultTexture, DefaultMapping)
    {
        var grid = size / singleSize;
        var gridEdge = new Vector2(size.X % singleSize.X, size.Y % singleSize.Y);

        grid.Floor();
        grid += Vector2.Ceiling(gridEdge / singleSize);


        _glitchBlocksGrid = new GlitchBlock[(int) grid.X * (int) grid.Y];
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
                    new Vector2(position.X + x * singleSize.X, position.Y + y * singleSize.Y), newSize);
                _glitchBlocksGrid[y * (int) grid.X + x] = block;
            }
        }
    }


    public void Update(GameTime gameTime, Rectangle mousePosition)
    {
        for (int i = 0; i < _glitchBlocksGrid.Length; i++)
        {
            _glitchBlocksGrid[i].Update(gameTime, mousePosition);
        }

        if (HitboxCheck(mousePosition))
        {
            if (!_hover)
                Enter?.Invoke(this);
            _hover = true;
        }
        else if (_hover)
        {
            Leave?.Invoke(this);
            _hover = false;
        }
        base.Update(gameTime);
        if (DrawColor != _oldDrawColor)
        {
            for (int i = 0; i < _glitchBlocksGrid.Length; i++)
            {
                _glitchBlocksGrid[i].DrawColor = DrawColor;
            }
        }

        _oldDrawColor = DrawColor;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        for (int i = 0; i < _glitchBlocksGrid.Length; i++)
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
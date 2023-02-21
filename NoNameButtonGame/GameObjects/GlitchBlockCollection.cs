using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.Interfaces;
using System.Linq;

namespace NoNameButtonGame.GameObjects;

internal class GlitchBlockCollection : GameObject, IMouseActions, IMoveable, IColorable
{
    private readonly GlitchBlock[] glitchBlocksGrid;
    private bool hover;

    public event Action<object> LeaveEventHandler;
    public event Action<object> EnterEventHandler;
    public event Action<object> ClickEventHandler;

    private Color oldDrawColor;

    public GlitchBlockCollection(Vector2 position, Vector2 size) : this(position, size, GlitchBlock.DefaultSize)
    {
    }

    public GlitchBlockCollection(Vector2 position, Vector2 size, float singleScale) : this(position, size,
        GlitchBlock.DefaultSize * singleScale)
    {
    }

    public GlitchBlockCollection(Vector2 position, Vector2 size, Vector2 singleSize) : base(position, size)
    {
        var grid = size / singleSize;
        var gridEdge = new Vector2(size.X % singleSize.X, size.Y % singleSize.Y);

        grid.Floor();
        grid += Vector2.Ceiling(gridEdge / singleSize);


        glitchBlocksGrid = new GlitchBlock[(int) grid.X * (int) grid.Y];
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
                glitchBlocksGrid[y * (int) grid.X + x] = block;
            }
        }
        
        CalculateHitboxes();
    }

    public override void Initialize()
    {
        textureHitboxMapping = Globals.Textures.GetMappingFromCache<GlitchBlock>();
    }


    public void Update(GameTime gameTime, Rectangle mousePosition)
    {
        for (int i = 0; i < glitchBlocksGrid.Length; i++)
        {
            glitchBlocksGrid[i].Update(gameTime, mousePosition);
        }

        if (HitboxCheck(mousePosition))
        {
            if (!hover)
                EnterEventHandler?.Invoke(this);
            hover = true;
        }
        else if (hover)
        {
            LeaveEventHandler?.Invoke(this);
            hover = false;
        }
        base.Update(gameTime);
        if (DrawColor != oldDrawColor)
        {
            for (int i = 0; i < glitchBlocksGrid.Length; i++)
            {
                glitchBlocksGrid[i].DrawColor = DrawColor;
            }
        }

        oldDrawColor = DrawColor;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        for (int i = 0; i < glitchBlocksGrid.Length; i++)
        {
            glitchBlocksGrid[i].Draw(spriteBatch);
        }
    }

    protected override void CalculateHitboxes()
    {
        if (glitchBlocksGrid is null)
            return;
        hitboxes = glitchBlocksGrid.SelectMany(block => block.Hitbox).ToArray();
    }

    public Vector2 GetPosition()
        => Position;

    public bool Move(Vector2 newPosition)
    {
        var offset = newPosition - Position;
        Position += offset;
        foreach (var block in glitchBlocksGrid)
        {
            block.Move(block.Position + offset);
        }

        return true;
    }

    public void ChangeColor(Color[] input)
    {
        foreach (var glitchBlock in glitchBlocksGrid)
        {
            glitchBlock.ChangeColor(input);
        }
    }

    public int ColorLength()
        => 1;
}
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NoNameButtonGame.Interfaces;
using NoNameButtonGame.Cache;
using System.Collections.Generic;
using System.Linq;

namespace NoNameButtonGame.GameObjects;

internal class GlitchBlockCollection : GameObject, IMouseActions, IMoveable, IColorable
{
    private GlitchBlock[] glitchBlocksGrid;
    private Rectangle[] ingameHitbox;
    public Rectangle[] Hitbox => ingameHitbox;

    private bool hover;

    public event Action<object> LeaveEventHandler;
    public event Action<object> EnterEventHandler;
    public event Action<object> ClickEventHandler;

    private Color OldDrawColor;

    public GlitchBlockCollection(Vector2 position, Vector2 canvas) : this(position, canvas, GlitchBlock.DefaultSize)
    {
    }

    public GlitchBlockCollection(Vector2 position, Vector2 canvas, float singleScale) : this(position, canvas,
        GlitchBlock.DefaultSize * singleScale)
    {
    }

    public GlitchBlockCollection(Vector2 position, Vector2 canvas, Vector2 singleSize) : base(position, canvas)
    {
        var grid = canvas / singleSize;
        var gridEdge = new Vector2(canvas.X % singleSize.X, canvas.Y % singleSize.Y);

        grid.Floor();
        grid += Vector2.Ceiling(gridEdge / singleSize);


        glitchBlocksGrid = new GlitchBlock[(int) grid.X * (int) grid.Y];
        for (int i = 0; i < grid.Y; i++)
        {
            for (int a = 0; a < grid.X; a++)
            {
                var block = new GlitchBlock(
                    new Vector2(position.X + a * singleSize.X, position.Y + i * singleSize.Y)
                    , a < grid.X || i < grid.Y ? singleSize : gridEdge);
                glitchBlocksGrid[i * ((int) grid.X) + a] = block;
            }
        }

        _hitboxes = glitchBlocksGrid.SelectMany(block => block.Hitbox).ToArray();
    }

    public override void Initialize()
    {
        _textureHitboxMapping = Globals.Textures.GetMappingFromCache<GlitchBlock>();
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
        if (DrawColor != OldDrawColor)
        {
            for (int i = 0; i < glitchBlocksGrid.Length; i++)
            {
                glitchBlocksGrid[i].DrawColor = DrawColor;
            }
        }

        OldDrawColor = DrawColor;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        for (int i = 0; i < glitchBlocksGrid.Length; i++)
        {
            glitchBlocksGrid[i].Draw(spriteBatch);
        }
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
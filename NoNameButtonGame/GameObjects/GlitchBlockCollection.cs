using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NoNameButtonGame.Interfaces;
using NoNameButtonGame.Cache;
using System.Collections.Generic;

namespace NoNameButtonGame.GameObjects;

internal class GlitchBlockCollection : GameObject, IMouseActions, IMoveable, IColorable
{
    private GlitchBlock[] glitchBlocksGrid;
    private Rectangle[] ingameHitbox;
    public Rectangle[] Hitbox => ingameHitbox;

    public event Action<object> LeaveEventHandler;
    public event Action<object> EnterEventHandler;
    public event Action<object> ClickEventHandler;

    private Color OldDrawColor;

    public GlitchBlockCollection(Vector2 position, Vector2 size) : base(position, size)
    {
        // Note:
        // Even though I've done a rework in 2023 I will not touch this stuff.
        // It works and it is to complicated to redo without breaking something.
        
        Vector2 grid = size / 32;


        Vector2 gridedge = new Vector2(size.X % 32, size.Y % 32);
        grid = new Vector2((float) (int) grid.X, (float) (int) grid.Y);

        if (gridedge.X == 0 && gridedge.Y == 0)
        {
            glitchBlocksGrid = new GlitchBlock[((int) grid.X) * ((int) grid.Y)];
            for (int i = 0; i < grid.Y; i++)
            {
                for (int a = 0; a < grid.X; a++)
                {
                    glitchBlocksGrid[i * ((int) grid.X) + a] =
                        new GlitchBlock(new Vector2(position.X + a * 32, position.Y + i * 32), new Vector2(32, 32));
                    glitchBlocksGrid[i * ((int) grid.X) + a].EnterEventHandler += CallEnter;
                }
            }
        }
        else
        {
            int gx = 0, gy = 0;
            if (gridedge.X != 0)
                gx = (int) grid.X + 1;
            else
                gx = (int) grid.X;
            if (gridedge.Y != 0)
                gy = (int) grid.Y + 1;
            else
                gy = (int) grid.Y;
            glitchBlocksGrid = new GlitchBlock[gx * gy];
            for (int i = 0; i < gy; i++)
            {
                for (int a = 0; a < gx; a++)
                {
                    Vector2 CSize = new Vector2(gridedge.X, gridedge.Y);
                    if (a < grid.X)
                        CSize.X = 32;
                    if (i < grid.Y)
                        CSize.Y = 32;
                    glitchBlocksGrid[i * gx + a] =
                        new GlitchBlock(new Vector2(position.X + a * 32, position.Y + i * 32), CSize);

                    glitchBlocksGrid[i * gx + a].EnterEventHandler += CallEnter;
                }
            }
        }

        List<Rectangle> Hitboxes = new List<Rectangle>();
        for (int i = 0; i < glitchBlocksGrid.Length; i++)
        {
            for (int a = 0; a < glitchBlocksGrid[i].Hitbox.Length; a++)
            {
                Hitboxes.Add(glitchBlocksGrid[i].Hitbox[a]);
            }
        }

        _hitboxes = Hitboxes.ToArray();
    }

    public override void Initialize()
    {
        _textureHitboxMapping = Globals.Textures.GetMappingFromCache<GlitchBlock>();
    }

    private void CallEnter(object sender)
        => EnterEventHandler?.Invoke(sender);

    public void Update(GameTime gt, Rectangle MousePos)
    {
        for (int i = 0; i < glitchBlocksGrid.Length; i++)
        {
            glitchBlocksGrid[i].Update(gt, MousePos);
        }

        if (Rectangle.Intersects(MousePos))
            EnterEventHandler?.Invoke(this);
        base.Update(gt);
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
        Position += newPosition;
        for (int i = 0; i < glitchBlocksGrid.Length; i++)
        {
            glitchBlocksGrid[i].Position += newPosition;
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
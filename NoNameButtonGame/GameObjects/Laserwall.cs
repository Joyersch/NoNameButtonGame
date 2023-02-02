using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NoNameButtonGame.Interfaces;
using NoNameButtonGame.Hitboxes;
using System.Collections.Generic;

namespace NoNameButtonGame.GameObjects;

class Laserwall : GameObject, IMouseActions, IMoveable
{
    DontTouch[] dontTouchGrid;
    Rectangle[] ingameHitbox;
    public Rectangle[] Hitbox => ingameHitbox;

    public event Action<object> LeaveEventHandler;
    public event Action<object> EnterEventHandler;
    public event Action<object> ClickEventHandler;

    Color OldDrawColor;

    public Laserwall(Vector2 position, Vector2 size) : base(position, size)
    {
        // Note:
        // Even though I've done a rework in 2023 I will not touch this stuff.
        // It works and it is to complicated to redo without breaking something.
        
        Vector2 grid = size / 32;


        Vector2 gridedge = new Vector2(size.X % 32, size.Y % 32);
        grid = new Vector2((float) (int) grid.X, (float) (int) grid.Y);

        if (gridedge.X == 0 && gridedge.Y == 0)
        {
            dontTouchGrid = new DontTouch[((int) grid.X) * ((int) grid.Y)];
            for (int i = 0; i < grid.Y; i++)
            {
                for (int a = 0; a < grid.X; a++)
                {
                    dontTouchGrid[i * ((int) grid.X) + a] =
                        new DontTouch(new Vector2(position.X + a * 32, position.Y + i * 32), new Vector2(32, 32));
                    dontTouchGrid[i * ((int) grid.X) + a].EnterEventHandler += CallEnter;
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
            dontTouchGrid = new DontTouch[gx * gy];
            for (int i = 0; i < gy; i++)
            {
                for (int a = 0; a < gx; a++)
                {
                    Vector2 CSize = new Vector2(gridedge.X, gridedge.Y);
                    if (a < grid.X)
                        CSize.X = 32;
                    if (i < grid.Y)
                        CSize.Y = 32;
                    dontTouchGrid[i * gx + a] =
                        new DontTouch(new Vector2(position.X + a * 32, position.Y + i * 32), CSize);

                    dontTouchGrid[i * gx + a].EnterEventHandler += CallEnter;
                }
            }
        }

        List<Rectangle> Hitboxes = new List<Rectangle>();
        for (int i = 0; i < dontTouchGrid.Length; i++)
        {
            for (int a = 0; a < dontTouchGrid[i].Hitbox.Length; a++)
            {
                Hitboxes.Add(dontTouchGrid[i].Hitbox[a]);
            }
        }

        _hitboxes = Hitboxes.ToArray();
    }

    public override void Initialize()
    {
        _textureHitboxMapping = Mapping.GetMappingFromCache<DontTouch>();
    }

    private void CallEnter(object sender)
        => EnterEventHandler?.Invoke(sender);

    public void Update(GameTime gt, Rectangle MousePos)
    {
        for (int i = 0; i < dontTouchGrid.Length; i++)
        {
            dontTouchGrid[i].Update(gt, MousePos);
        }

        if (rectangle.Intersects(MousePos))
            EnterEventHandler?.Invoke(this);
        base.Update(gt);
        if (DrawColor != OldDrawColor)
        {
            for (int i = 0; i < dontTouchGrid.Length; i++)
            {
                dontTouchGrid[i].DrawColor = DrawColor;
            }
        }

        OldDrawColor = DrawColor;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        for (int i = 0; i < dontTouchGrid.Length; i++)
        {
            dontTouchGrid[i].Draw(spriteBatch);
        }
    }

    public Vector2 GetPosition()
        => Position;

    public bool Move(Vector2 Direction)
    {
        Position += Direction;
        for (int i = 0; i < dontTouchGrid.Length; i++)
        {
            dontTouchGrid[i].Position += Direction;
        }
        return true;
    }
}
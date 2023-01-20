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

    public event EventHandler LeaveEventHandler;
    public event EventHandler EnterEventHandler;
    public event EventHandler ClickEventHandler;

    Color OldDrawColor;

    public Laserwall(Vector2 Pos, Vector2 Size): base(Pos, Size)
    {
        Vector2 grid = Size / 32;


        Vector2 gridedge = new Vector2(Size.X % 32, Size.Y % 32);
        grid = new Vector2((float) (int) grid.X, (float) (int) grid.Y);

        if (gridedge.X == 0 && gridedge.Y == 0)
        {
            dontTouchGrid = new DontTouch[((int) grid.X) * ((int) grid.Y)];
            for (int i = 0; i < grid.Y; i++)
            {
                for (int a = 0; a < grid.X; a++)
                {
                    dontTouchGrid[i * ((int) grid.X) + a] =
                        new DontTouch(new Vector2(Pos.X + a * 32, Pos.Y + i * 32), new Vector2(32, 32));
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
                        new DontTouch(new Vector2(Pos.X + a * 32, Pos.Y + i * 32), CSize);

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

    private void CallEnter(object sender, EventArgs e)
    {
        EnterEventHandler?.Invoke(sender, e);
    }

    public void Update(GameTime gt, Rectangle MousePos)
    {
        for (int i = 0; i < dontTouchGrid.Length; i++)
        {
            dontTouchGrid[i].Update(gt, MousePos);
        }

        if (rectangle.Intersects(MousePos))
            EnterEventHandler?.Invoke(this, EventArgs.Empty);
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

    public bool Move(Vector2 Direction)
    {
        try
        {
            Position += Direction;
            for (int i = 0; i < dontTouchGrid.Length; i++)
            {
                dontTouchGrid[i].Position += Direction;
            }
        }
        catch
        {
            return false;
        }

        return true;
    }
}
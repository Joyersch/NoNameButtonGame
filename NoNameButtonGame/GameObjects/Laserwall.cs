﻿using System;
using Raigy.Obj;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NoNameButtonGame.Interfaces;
using NoNameButtonGame.BeforeMaths;
using System.Collections.Generic;
namespace NoNameButtonGame.GameObjects
{
    class Laserwall : GameObject, IHitbox, IMouseActions, IMoveable
    {
        DontTouch[] dontTouchGrid;
        Rectangle[] ingameHitbox;
        public Rectangle[] Hitbox => ingameHitbox;

        public event EventHandler Leave;
        public event EventHandler Enter;
        public event EventHandler Click;

        Color OldDrawColor;

        public Laserwall(Vector2 Pos, Vector2 Size, THBox box) {
            this.Size = Size;
            this.Position = Pos;
            Vector2 grid = Size / 32;


            Vector2 gridedge = new Vector2(Size.X % 32, Size.Y % 32);
            grid = new Vector2((float)(int)grid.X, (float)(int)grid.Y);

            if (gridedge.X == 0 && gridedge.Y == 0) {
                dontTouchGrid = new DontTouch[((int)grid.X) * ((int)grid.Y)];
                for (int i = 0; i < grid.Y; i++) {
                    for (int a = 0; a < grid.X; a++) {
                        dontTouchGrid[i * ((int)grid.X) + a] = new DontTouch(new Vector2(Pos.X + a * 32, Pos.Y + i * 32), new Vector2(32, 32), box);
                        dontTouchGrid[i * ((int)grid.X) + a].Enter += CallEnter;
                    }
                }
            } else {
                int gx = 0, gy = 0;
                if (gridedge.X != 0)
                    gx = (int)grid.X + 1;
                else
                    gx = (int)grid.X;
                if (gridedge.Y != 0)
                    gy = (int)grid.Y + 1;
                else
                    gy = (int)grid.Y;
                dontTouchGrid = new DontTouch[gx * gy];
                for (int i = 0; i < gy; i++) {
                    for (int a = 0; a < gx; a++) {
                        Vector2 CSize = new Vector2(gridedge.X, gridedge.Y);
                        if (a < grid.X)
                            CSize.X = 32;
                        if (i < grid.Y)
                            CSize.Y = 32;
                        dontTouchGrid[i * gx + a] = new DontTouch(new Vector2(Pos.X + a * 32, Pos.Y + i * 32), CSize, box);

                        dontTouchGrid[i * gx + a].Enter += CallEnter;
                    }
                }
            }

            List<Rectangle> Hitboxes = new List<Rectangle>();
            for (int i = 0; i < dontTouchGrid.Length; i++) {
                for (int a = 0; a < dontTouchGrid[i].Hitbox.Length; a++) {
                    Hitboxes.Add(dontTouchGrid[i].Hitbox[a]);
                }
            }
            ingameHitbox = Hitboxes.ToArray();
        }

        private void CallEnter(object sender, EventArgs e) {
            Enter?.Invoke(sender, e);
        }
        
        public void Update(GameTime gt, Rectangle MousePos) {
            for (int i = 0; i < dontTouchGrid.Length; i++) {
                dontTouchGrid[i].Update(gt, MousePos);
            }
            if (rec.Intersects(MousePos))
                Enter?.Invoke(this, new EventArgs());
            base.Update(gt);
            if (DrawColor != OldDrawColor) {
                for (int i = 0; i < dontTouchGrid.Length; i++) {
                    dontTouchGrid[i].DrawColor = DrawColor;
                }
            }
            OldDrawColor = DrawColor;
        }
        
        public override void Draw(SpriteBatch sp) {
            for (int i = 0; i < dontTouchGrid.Length; i++) {
                dontTouchGrid[i].Draw(sp);
            }
        }

        public bool Move(Vector2 Direction) {
            try { 
                Position += Direction;
                for (int i = 0; i < dontTouchGrid.Length; i++) {
                    dontTouchGrid[i].Position += Direction;
                }
            } catch { return false; }
            return true;
        }
    }
}

﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Raigy.Obj;
using NoNameButtonGame.BeforeMaths;
using NoNameButtonGame.Interfaces;
using Microsoft.Xna.Framework.Input;

namespace NoNameButtonGame.GameObjects
{
    class DontTouch: GameObject, IHitbox, IMouseActions
    {
        Rectangle[] frameHitbox;
        Rectangle[] ingameHitbox;
        Vector2 Scale;
        int FramePos = 0;
        int FrameMax = 0;
        int FrameSpeed = 180;
        public Rectangle[] Hitbox {
            get => ingameHitbox;
        }
        float GT;

        public event EventHandler Enter;
        public event EventHandler Leave;
        public event EventHandler Click;

        public DontTouch(Vector2 Pos, Vector2 Size, THBox box) {
            base.Size = Size;
           
            Position = Pos;
            FrameSize = box.Imagesize;
            frameHitbox = box.Hitbox;
            
            
            if (Size.X % 32 != 0 || Size.Y % 32 != 0) {
                FrameSize = FrameSize / 32 * Size;
                for (int i = 0; i < frameHitbox.Length; i++) {
                    frameHitbox[i].Size = FrameSize.ToPoint();
                }
            }

            Texture = box.Texture;
            FrameMax = box.Aniframes;
            Scale = new Vector2(Size.X / FrameSize.X, Size.Y / FrameSize.Y);
            ImageLocation = new Rectangle(0,0, (int)box.Imagesize.X, (int)box.Imagesize.Y);
            ingameHitbox = new Rectangle[frameHitbox.Length];
            DrawColor = Color.White;

            for (int i = 0; i < box.Hitbox.Length; i++) {
                ingameHitbox[i] = new Rectangle((int)(Position.X + (box.Hitbox[i].X * Scale.X)), (int)(Position.Y + (box.Hitbox[i].Y * Scale.Y)), (int)(box.Hitbox[i].Width * Scale.X), (int)(box.Hitbox[i].Height * Scale.Y));
            }
        }

        public bool HitboxCheck(Rectangle rec) {
            for (int i = 0; i < Hitbox.Length; i++) {
                if (Hitbox[i].Intersects(rec)) {
                    return true;
                }
            }
            return false;
        }
        private void UpdateHitbox() {
            Scale = new Vector2(Size.X / FrameSize.X, Size.Y / FrameSize.Y);
            for (int i = 0; i < frameHitbox.Length; i++) {
                ingameHitbox[i] = new Rectangle((int)(Position.X + (frameHitbox[i].X * Scale.X)), (int)(Position.Y + (frameHitbox[i].Y * Scale.Y)), (int)(frameHitbox[i].Width * Scale.X), (int)(frameHitbox[i].Height * Scale.Y));
            }
        }

        public override void Draw(SpriteBatch sp) {
            base.Draw(sp);
        }

        public void Update(GameTime gt, Rectangle MousePos) {
            MouseState mouseState = Mouse.GetState();
            GT += (float)gt.ElapsedGameTime.TotalMilliseconds;
            while(GT > FrameSpeed) {
                GT -= FrameSpeed;
                FramePos++;
                if (FramePos == FrameMax) FramePos = 0;
                ImageLocation = new Rectangle(0, FramePos * (int)FrameSize.X, (int)FrameSize.X, (int)FrameSize.Y);
            }
            if (HitboxCheck(MousePos)) {
                Enter(this, new EventArgs());
            }

            UpdateHitbox();
            base.Update(gt);
        }
    }
}

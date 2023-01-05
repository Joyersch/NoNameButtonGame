using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Raigy.Obj;
using Microsoft.Xna.Framework.Input;
using NoNameButtonGame.Interfaces;
using NoNameButtonGame.BeforeMaths;
using Raigy.Input;
using NoNameButtonGame.Text;

namespace NoNameButtonGame.GameObjects
{
    class LockButton : GameObject, IMouseActions, IHitbox
    {
        public event EventHandler Leave;
        public event EventHandler Enter;
        public event EventHandler Click;
        bool Hover;
        Rectangle[] frameHitbox;
        Rectangle[] ingameHitbox;
        Vector2 Scale;

        public bool Locked;

        TextBuilder textContainer;
        public Rectangle[] Hitbox {
            get => ingameHitbox;
        }

        public LockButton(Vector2 Pos, Vector2 Size, THBox box, bool Startstate) {
            base.Size = Size;
            Position = Pos;
            ImageLocation = new Rectangle((int)box.Imagesize.X, 0, (int)box.Imagesize.X, (int)box.Imagesize.Y);
            FrameSize = box.Imagesize;
            frameHitbox = new Rectangle[box.Hitbox.Length];
            Texture = box.Texture;
            Scale = new Vector2(Size.X / FrameSize.X, Size.Y / FrameSize.Y);
            frameHitbox = box.Hitbox;
            DrawColor = Color.White;
            textContainer = new TextBuilder("test", new Vector2(float.MinValue, float.MinValue), new Vector2(16, 16), null, 0);

            ingameHitbox = new Rectangle[frameHitbox.Length];
            for (int i = 0; i < box.Hitbox.Length; i++) {
                ingameHitbox[i] = new Rectangle((int)(Position.X + (box.Hitbox[i].X * Scale.X)), (int)(Position.Y + (box.Hitbox[i].Y * Scale.Y)), (int)(box.Hitbox[i].Width * Scale.X), (int)(box.Hitbox[i].Height * Scale.Y));
            }
            Locked = Startstate;
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
        public void Update(GameTime gt, Rectangle MousePos) {
            MouseState mouseState = Mouse.GetState();
            if (HitboxCheck(MousePos)) {
                if (!Hover) {
                    Hover = true;
                    if (Enter != null)
                        Enter(this, new EventArgs());
                }
                if (InputReaderMouse.CheckKey(InputReaderMouse.MouseKeys.Left, true)) {
                   if (!Locked)
                        Click(this, new EventArgs());
                } else {
                    //HoldTime -= gt.ElapsedGameTime.TotalMilliseconds / 2;
                }
            } else {
                if (Hover)
                    if (Leave != null)
                        Leave(this, new EventArgs());
                Hover = false;


            }

            if (Hover || Locked) {
                ImageLocation = new Rectangle((int)FrameSize.X, 0, (int)FrameSize.X, (int)FrameSize.Y);
            } else {
                ImageLocation = new Rectangle(0, 0, (int)FrameSize.X, (int)FrameSize.Y);
            }
            UpdateHitbox();
            textContainer.ChangeText(Locked? "Locked" : "Unlocked");

            textContainer.Position = rec.Center.ToVector2() - textContainer.rec.Size.ToVector2() / 2;
            textContainer.Position.Y -= 32;
            textContainer.Update(gt);
            Update(gt);
        }

        public override void Draw(SpriteBatch sp) {
            base.Draw(sp);
            textContainer.Draw(sp);
        }
    }
}

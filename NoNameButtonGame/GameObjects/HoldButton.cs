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
    class HoldButton : GameObject, IMouseActions, IHitbox
    {
        public event EventHandler Leave;
        public event EventHandler Enter;
        public event EventHandler Click;

        bool Hover;
        float HoldTime = 0F;
        public float EndHoldTime = 10000F;

        Rectangle[] frameHitbox;
        Rectangle[] ingameHitbox;
        Vector2 Scale;

        TextBuilder textContainer;
        public Rectangle[] Hitbox {
            get => ingameHitbox;
        }

        public HoldButton(Vector2 Position, Vector2 Size, THBox thBox) {
            base.Size = Size;
            base.Position = Position;
            DrawColor = Color.White;
            ImageLocation = new Rectangle((int)thBox.Imagesize.X, 0, (int)thBox.Imagesize.X, (int)thBox.Imagesize.Y);
            FrameSize = thBox.Imagesize;
            frameHitbox = new Rectangle[thBox.Hitbox.Length];
            Texture = thBox.Texture;
            Scale = new Vector2(Size.X / FrameSize.X, Size.Y / FrameSize.Y);
            frameHitbox = thBox.Hitbox;
            textContainer = new TextBuilder("test", new Vector2(float.MinValue, float.MinValue), new Vector2(16, 16), null, 0);


            ingameHitbox = new Rectangle[frameHitbox.Length];
            for (int i = 0; i < thBox.Hitbox.Length; i++) {
                ingameHitbox[i] = new Rectangle((int)(base.Position.X + (thBox.Hitbox[i].X * Scale.X)), (int)(base.Position.Y + (thBox.Hitbox[i].Y * Scale.Y)), (int)(thBox.Hitbox[i].Width * Scale.X), (int)(thBox.Hitbox[i].Height * Scale.Y));
            }

        }

        public bool HitboxCheck(Rectangle compareTo) {
            for (int i = 0; i < Hitbox.Length; i++) {
                if (Hitbox[i].Intersects(compareTo)) {
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
                if (InputReaderMouse.CheckKey(InputReaderMouse.MouseKeys.Left, false)) {
                    HoldTime += (float)gt.ElapsedGameTime.TotalMilliseconds;
                    if (HoldTime > EndHoldTime) {
                        if (InputReaderMouse.CheckKey(InputReaderMouse.MouseKeys.Left, true)) {
                            Click(this, new EventArgs());
                            EndHoldTime = 0;
                        }

                    }

                } else {

                    HoldTime -= (float)gt.ElapsedGameTime.TotalMilliseconds / 2;
                }
            } else {
                if (Hover)
                    if (Leave != null)
                        Leave(this, new EventArgs());
                Hover = false;
                HoldTime -= (float)gt.ElapsedGameTime.TotalMilliseconds / 2;

            }
            if (HoldTime > EndHoldTime) {
                HoldTime = EndHoldTime;
            }
            if (HoldTime < 0) {
                HoldTime = 0;
            }
            if (Hover) {
                ImageLocation = new Rectangle((int)FrameSize.X, 0, (int)FrameSize.X, (int)FrameSize.Y);
            } else {
                ImageLocation = new Rectangle(0, 0, (int)FrameSize.X, (int)FrameSize.Y);
            }
            UpdateHitbox();
            textContainer.ChangeText((((EndHoldTime - HoldTime) / 1000).ToString("0.0") + "s").Replace(',', '.'));

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

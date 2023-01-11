using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NoNameButtonGame.Interfaces;
using NoNameButtonGame.Hitboxes;
using Joyersch.Input;
using NoNameButtonGame.Text;

namespace NoNameButtonGame.GameObjects
{
    class StateButton : GameObject, IMouseActions, IHitbox
    {
        public event EventHandler Leave;
        public event EventHandler Enter;
        public event EventHandler Click;
        bool Hover;
        Rectangle[] frameHitbox;
        Rectangle[] ingameHitbox;
        Vector2 Scale;
        int ammoutStates;
        public int States { get { return ammoutStates; } set { ammoutStates = value; CurrentState = ammoutStates; } }
        public int CurrentStates { get { return CurrentState; } }
        int CurrentState;

        TextBuilder textContainer;
        public Rectangle[] Hitbox {
            get => ingameHitbox;
        }
        public StateButton(Vector2 Pos, Vector2 Size, HitboxMap box, int States) {
            base.Size = Size;
            Position = Pos;
            ImageLocation = new Rectangle((int)box.ImageSize.X, 0, (int)box.ImageSize.X, (int)box.ImageSize.Y);
            FrameSize = box.ImageSize;
            frameHitbox = new Rectangle[box.Hitboxes.Length];
            Texture = box.Texture;
            Scale = new Vector2(Size.X / FrameSize.X, Size.Y / FrameSize.Y);
            frameHitbox = box.Hitboxes;
            textContainer = new TextBuilder("test", new Vector2(float.MinValue, float.MinValue), new Vector2(16, 16), null, 0);

            ingameHitbox = new Rectangle[frameHitbox.Length];
            for (int i = 0; i < box.Hitboxes.Length; i++) {
                ingameHitbox[i] = new Rectangle((int)(Position.X + (box.Hitboxes[i].X * Scale.X)), (int)(Position.Y + (box.Hitboxes[i].Y * Scale.Y)), (int)(box.Hitboxes[i].Width * Scale.X), (int)(box.Hitboxes[i].Height * Scale.Y));
            }
            CurrentState = States;
            ammoutStates = States;
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
                        Enter(this, new());
                }
                if (InputReaderMouse.CheckKey(InputReaderMouse.MouseKeys.Left, true)) {
                    CurrentState--;
                    if (CurrentState <= 0)
                        Click(this, new ());
                } else {
                    //HoldTime -= gt.ElapsedGameTime.TotalMilliseconds / 2;
                }
            } else {
                if (Hover)
                    if (Leave != null)
                        Leave(this, new ());
                Hover = false;
                

            }

            if (Hover) {
                ImageLocation = new Rectangle((int)FrameSize.X, 0, (int)FrameSize.X, (int)FrameSize.Y);
            } else {
                ImageLocation = new Rectangle(0, 0, (int)FrameSize.X, (int)FrameSize.Y);
            }
            UpdateHitbox();
            textContainer.ChangeText(CurrentState.ToString());

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

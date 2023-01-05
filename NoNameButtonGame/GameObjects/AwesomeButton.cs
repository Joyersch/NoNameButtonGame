using System;
using Microsoft.Xna.Framework;
using Raigy.Obj;
using Microsoft.Xna.Framework.Input;
using NoNameButtonGame.Interfaces;
using NoNameButtonGame.BeforeMaths;
using Raigy.Input;

namespace NoNameButtonGame.GameObjects
{
    class AwesomeButton : GameObject, IMouseActions, IHitbox, IMoveable
    {
        public event EventHandler Leave;
        public event EventHandler Enter;
        public event EventHandler Click;
        bool Hover;

        readonly Rectangle[] textureHitbox;
        readonly Rectangle[] ingameHitbox;
        Vector2 Scale;

        public string Name { get; set; }

        public Rectangle[] Hitbox {
            get => ingameHitbox;
        }


        public AwesomeButton(Vector2 Position, Vector2 Size, THBox thBox) {
            base.Size = Size;
            base.Position = Position;
            DrawColor = Color.White;
            ImageLocation = new Rectangle((int)thBox.Imagesize.X, 0, (int)thBox.Imagesize.X, (int)thBox.Imagesize.Y);
            FrameSize = thBox.Imagesize;
            textureHitbox = new Rectangle[thBox.Hitbox.Length];
            Texture = thBox.Texture;
            Scale = new Vector2(Size.X / FrameSize.X, Size.Y / FrameSize.Y);
            textureHitbox = thBox.Hitbox;
            ingameHitbox = new Rectangle[textureHitbox.Length];
            for (int i = 0; i < thBox.Hitbox.Length; i++) {
                ingameHitbox[i] = new Rectangle((int)(base.Position.X + (thBox.Hitbox[i].X * Scale.X)), (int)(base.Position.Y + (thBox.Hitbox[i].Y * Scale.Y)), (int)(thBox.Hitbox[i].Width * Scale.X), (int)(thBox.Hitbox[i].Height * Scale.Y));
            }
        }

        public void Update(GameTime gameTime, Rectangle mousePos) {
            MouseState mouseState = Mouse.GetState();
            if (HitboxCheck(mousePos)) {
                if (!Hover) {
                    Hover = true;
                    Enter?.Invoke(this, new EventArgs());
                }
                if (InputReaderMouse.CheckKey(InputReaderMouse.MouseKeys.Left, true)) {
                    Click?.Invoke(this, new EventArgs());
                }
            } else {
                if (Hover)
                    Leave?.Invoke(this, new EventArgs());
                Hover = false;
            }

            if (Hover) {
                ImageLocation = new Rectangle((int)FrameSize.X, 0, (int)FrameSize.X, (int)FrameSize.Y);
            } else {
                ImageLocation = new Rectangle(0, 0, (int)FrameSize.X, (int)FrameSize.Y);
            }

            UpdateHitbox();

            Update(gameTime);
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
            for (int i = 0; i < textureHitbox.Length; i++) {
                ingameHitbox[i] = new Rectangle((int)(Position.X + (textureHitbox[i].X * Scale.X)), (int)(Position.Y + (textureHitbox[i].Y * Scale.Y)), (int)(textureHitbox[i].Width * Scale.X), (int)(textureHitbox[i].Height * Scale.Y));
            }
        }
      

        public bool Move(Vector2 Direction) {
            try {
                Position += Direction;
                return true;
            } catch { return false; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NoNameButtonGame.Interfaces;
using NoNameButtonGame.Hitboxes;
using NoNameButtonGame.Input;
using NoNameButtonGame.Text;

namespace NoNameButtonGame.GameObjects
{
    class TextButton : GameObject, IMouseActions, IHitbox
    {
        public event EventHandler Leave;
        public event EventHandler Enter;
        public event EventHandler Click;
        bool Hover;
        Rectangle[] textureHitbox;
        Rectangle[] IngameHitbox;
        Vector2 Scale;
        public string Name;

        TextBuilder textContainer;

        public TextBuilder Text
        {
            get { return textContainer; }
        }

        public Rectangle[] Hitbox
        {
            get => IngameHitbox;
        }

        public TextButton(Vector2 Pos, Vector2 Size, TextureHitboxMapping box, string Name, string Text,
            Vector2 TextSize)
        {
            base.Size = Size;
            Position = Pos;
            DrawColor = Color.White;
            ImageLocation = new Rectangle((int) box.ImageSize.X, 0, (int) box.ImageSize.X, (int) box.ImageSize.Y);
            FrameSize = box.ImageSize;
            textureHitbox = new Rectangle[box.Hitboxes.Length];
            Texture = box.Texture;
            Scale = new Vector2(Size.X / FrameSize.X, Size.Y / FrameSize.Y);
            textureHitbox = box.Hitboxes;
            textContainer = new TextBuilder(Text, Position, TextSize, null, 0);
            textContainer.ChangeText(Text);
            this.Name = Name;
            IngameHitbox = new Rectangle[textureHitbox.Length];
            for (int i = 0; i < box.Hitboxes.Length; i++)
            {
                IngameHitbox[i] = new Rectangle((int) (Position.X + (box.Hitboxes[i].X * Scale.X)),
                    (int) (Position.Y + (box.Hitboxes[i].Y * Scale.Y)), (int) (box.Hitboxes[i].Width * Scale.X),
                    (int) (box.Hitboxes[i].Height * Scale.Y));
            }
        }

        public bool HitboxCheck(Rectangle rec)
        {
            for (int i = 0; i < Hitbox.Length; i++)
            {
                if (Hitbox[i].Intersects(rec))
                {
                    return true;
                }
            }

            return false;
        }

        private void UpdateHitbox()
        {
            Scale = new Vector2(Size.X / FrameSize.X, Size.Y / FrameSize.Y);
            for (int i = 0; i < textureHitbox.Length; i++)
            {
                IngameHitbox[i] = new Rectangle((int) (Position.X + (textureHitbox[i].X * Scale.X)),
                    (int) (Position.Y + (textureHitbox[i].Y * Scale.Y)), (int) (textureHitbox[i].Width * Scale.X),
                    (int) (textureHitbox[i].Height * Scale.Y));
            }
        }

        public void Update(GameTime gt, Rectangle MousePos)
        {
            MouseState mouseState = Mouse.GetState();
            if (HitboxCheck(MousePos))
            {
                if (!Hover)
                {
                    Hover = true;
                    Enter?.Invoke(this, new EventArgs());
                }

                if (InputReaderMouse.CheckKey(InputReaderMouse.MouseKeys.Left, true))
                {
                    Click(this, new EventArgs());
                }
                else
                {
                    //HoldTime -= gt.ElapsedGameTime.TotalMilliseconds / 2;
                }
            }
            else
            {
                if (Hover)
                    Leave?.Invoke(this, new EventArgs());
                Hover = false;
            }

            if (Hover)
            {
                ImageLocation = new Rectangle((int) FrameSize.X, 0, (int) FrameSize.X, (int) FrameSize.Y);
            }
            else
            {
                ImageLocation = new Rectangle(0, 0, (int) FrameSize.X, (int) FrameSize.Y);
            }

            UpdateHitbox();
            textContainer.ChangePosition(rec.Center.ToVector2() - textContainer.rec.Size.ToVector2() / 2);
            textContainer.Update(gt);
            Update(gt);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            textContainer.Draw(spriteBatch);
        }
    }
}
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

namespace NoNameButtonGame.GameObjects;

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

    public Rectangle[] Hitbox
    {
        get => ingameHitbox;
    }

    public HoldButton(Vector2 Position, Vector2 Size, TextureHitboxMapping thBox)
    {
        base.Size = Size;
        base.Position = Position;
        DrawColor = Color.White;
        ImageLocation = new Rectangle((int) thBox.ImageSize.X, 0, (int) thBox.ImageSize.X, (int) thBox.ImageSize.Y);
        FrameSize = thBox.ImageSize;
        frameHitbox = new Rectangle[thBox.Hitboxes.Length];
        Texture = thBox.Texture;
        Scale = new Vector2(Size.X / FrameSize.X, Size.Y / FrameSize.Y);
        frameHitbox = thBox.Hitboxes;
        textContainer = new TextBuilder("test", new Vector2(float.MinValue, float.MinValue), new Vector2(16, 16), null,
            0);


        ingameHitbox = new Rectangle[frameHitbox.Length];
        for (int i = 0; i < thBox.Hitboxes.Length; i++)
        {
            ingameHitbox[i] = new Rectangle((int) (base.Position.X + (thBox.Hitboxes[i].X * Scale.X)),
                (int) (base.Position.Y + (thBox.Hitboxes[i].Y * Scale.Y)), (int) (thBox.Hitboxes[i].Width * Scale.X),
                (int) (thBox.Hitboxes[i].Height * Scale.Y));
        }
    }

    public bool HitboxCheck(Rectangle compareTo)
    {
        for (int i = 0; i < Hitbox.Length; i++)
        {
            if (Hitbox[i].Intersects(compareTo))
            {
                return true;
            }
        }

        return false;
    }

    private void UpdateHitbox()
    {
        Scale = new Vector2(Size.X / FrameSize.X, Size.Y / FrameSize.Y);
        for (int i = 0; i < frameHitbox.Length; i++)
        {
            ingameHitbox[i] = new Rectangle((int) (Position.X + (frameHitbox[i].X * Scale.X)),
                (int) (Position.Y + (frameHitbox[i].Y * Scale.Y)), (int) (frameHitbox[i].Width * Scale.X),
                (int) (frameHitbox[i].Height * Scale.Y));
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
                if (Enter != null)
                    Enter(this, new());
            }

            if (InputReaderMouse.CheckKey(InputReaderMouse.MouseKeys.Left, false))
            {
                HoldTime += (float) gt.ElapsedGameTime.TotalMilliseconds;
                if (HoldTime > EndHoldTime)
                {
                    if (InputReaderMouse.CheckKey(InputReaderMouse.MouseKeys.Left, true))
                    {
                        Click(this, new());
                        EndHoldTime = 0;
                    }
                }
            }
            else
            {
                HoldTime -= (float) gt.ElapsedGameTime.TotalMilliseconds / 2;
            }
        }
        else
        {
            if (Hover)
                if (Leave != null)
                    Leave(this, new());
            Hover = false;
            HoldTime -= (float) gt.ElapsedGameTime.TotalMilliseconds / 2;
        }

        if (HoldTime > EndHoldTime)
        {
            HoldTime = EndHoldTime;
        }

        if (HoldTime < 0)
        {
            HoldTime = 0;
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
        textContainer.ChangeText((((EndHoldTime - HoldTime) / 1000).ToString("0.0") + "s").Replace(',', '.'));

        textContainer.Position = rec.Center.ToVector2() - textContainer.rec.Size.ToVector2() / 2;
        textContainer.Position.Y -= 32;
        textContainer.Update(gt);
        Update(gt);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        textContainer.Draw(spriteBatch);
    }
}
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NoNameButtonGame.Interfaces;
using Joyersch.Input;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.Hitboxes;

namespace NoNameButtonGame.GameObjects;

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

    public Rectangle[] Hitbox => ingameHitbox;


    public AwesomeButton(Vector2 Position, Vector2 Size, HitboxMap thBox)
    {
        base.Size = Size;
        base.Position = Position;
        DrawColor = Color.White;
        ImageLocation = new Rectangle((int) thBox.ImageSize.X, 0, (int) thBox.ImageSize.X, (int) thBox.ImageSize.Y);
        FrameSize = thBox.ImageSize;
        textureHitbox = new Rectangle[thBox.Hitboxes.Length];
        Texture = thBox.Texture;
        Scale = new Vector2(Size.X / FrameSize.X, Size.Y / FrameSize.Y);
        textureHitbox = thBox.Hitboxes;
        ingameHitbox = new Rectangle[textureHitbox.Length];
        for (int i = 0; i < thBox.Hitboxes.Length; i++)
        {
            ingameHitbox[i] = new Rectangle((int) (base.Position.X + (thBox.Hitboxes[i].X * Scale.X)),
                (int) (base.Position.Y + (thBox.Hitboxes[i].Y * Scale.Y)), (int) (thBox.Hitboxes[i].Width * Scale.X),
                (int) (thBox.Hitboxes[i].Height * Scale.Y));
        }
    }

    public void Update(GameTime gameTime, Rectangle mousePos)
    {
        MouseState mouseState = Mouse.GetState();
        if (HitboxCheck(mousePos))
        {
            if (!Hover)
            {
                Hover = true;
                Enter?.Invoke(this, new());
            }

            if (InputReaderMouse.CheckKey(InputReaderMouse.MouseKeys.Left, true))
            {
                Click?.Invoke(this, new());
            }
        }
        else
        {
            if (Hover)
                Leave?.Invoke(this, new());
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

        Update(gameTime);
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
        for (int i = 0; i < textureHitbox.Length; i++)
        {
            ingameHitbox[i] = new Rectangle((int) (Position.X + (textureHitbox[i].X * Scale.X)),
                (int) (Position.Y + (textureHitbox[i].Y * Scale.Y)), (int) (textureHitbox[i].Width * Scale.X),
                (int) (textureHitbox[i].Height * Scale.Y));
        }
    }


    public bool Move(Vector2 Direction)
    {
        try
        {
            Position += Direction;
            return true;
        }
        catch
        {
            return false;
        }
    }
}
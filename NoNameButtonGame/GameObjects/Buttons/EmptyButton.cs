using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NoNameButtonGame.Interfaces;
using NoNameButtonGame.Input;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.Hitboxes;

namespace NoNameButtonGame.GameObjects.Buttons;

public class EmptyButton : GameObject, IMouseActions, IHitbox, IMoveable
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

    protected TextureHitboxMapping _textureHitboxMapping;


    public EmptyButton(Vector2 position, Vector2 size)
    {
        base.Size = size;
        base.Position = position;
        DrawColor = Color.White;
        Initialize();
        ImageLocation = new Rectangle((int) _textureHitboxMapping.ImageSize.X, 0,
            (int) _textureHitboxMapping.ImageSize.X, (int) _textureHitboxMapping.ImageSize.Y);
        FrameSize = _textureHitboxMapping.ImageSize;
        textureHitbox = new Rectangle[_textureHitboxMapping.Hitboxes.Length];
        Texture = _textureHitboxMapping.Texture;
        Scale = new Vector2(size.X / FrameSize.X, size.Y / FrameSize.Y);
        textureHitbox = _textureHitboxMapping.Hitboxes;
        ingameHitbox = new Rectangle[textureHitbox.Length];
        for (int i = 0; i < _textureHitboxMapping.Hitboxes.Length; i++)
        {
            ingameHitbox[i] = new Rectangle((int) (base.Position.X + (_textureHitboxMapping.Hitboxes[i].X * Scale.X)),
                (int) (base.Position.Y + (_textureHitboxMapping.Hitboxes[i].Y * Scale.Y)),
                (int) (_textureHitboxMapping.Hitboxes[i].Width * Scale.X),
                (int) (_textureHitboxMapping.Hitboxes[i].Height * Scale.Y));
        }
    }

    public virtual void Initialize()
    {
        _textureHitboxMapping = Mapping.GetMappingFromCache<EmptyButton>();
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
using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NoNameButtonGame.Interfaces;
using NoNameButtonGame.Input;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.Hitboxes;

namespace NoNameButtonGame.GameObjects.Buttons;

public class EmptyButton : GameObject, IMouseActions, IHitbox, IMoveable
{
    public event EventHandler LeaveEventHandler;
    public event EventHandler EnterEventHandler;
    public event EventHandler ClickEventHandler;
    private bool _hover;

    public Rectangle[] Hitbox { get; }
    private Vector2 _scale;

    protected TextureHitboxMapping _textureHitboxMapping;


    public EmptyButton(Vector2 position, Vector2 size)
    {
        base.Size = size;
        base.Position = position;
        DrawColor = Color.White;
        Initialize();
        ImageLocation = new Rectangle(
            (int) _textureHitboxMapping.ImageSize.X
            , 0
            , (int) _textureHitboxMapping.ImageSize.X
            , (int) _textureHitboxMapping.ImageSize.Y);
        FrameSize = _textureHitboxMapping.ImageSize;
        Texture = _textureHitboxMapping.Texture;
        Hitbox = new Rectangle[_textureHitboxMapping.Hitboxes.Length];
        CalculateHitboxes();
    }

    public virtual void Initialize()
    {
        _textureHitboxMapping = Mapping.GetMappingFromCache<EmptyButton>();
    }

    public void Update(GameTime gameTime, Rectangle mousePos)
    {
        MouseState mouseState = Mouse.GetState();

        bool hover = HitboxCheck(mousePos);
        if (hover)
        {
            if (!_hover)
                EnterEventHandler?.Invoke(this, EventArgs.Empty);

            if (InputReaderMouse.CheckKey(InputReaderMouse.MouseKeys.Left, true))
                ClickEventHandler?.Invoke(this, EventArgs.Empty);
        }
        else if (_hover)
            LeaveEventHandler?.Invoke(this, EventArgs.Empty);

        _hover = hover;
        ImageLocation = new Rectangle(_hover ? (int) FrameSize.X : 0, 0, (int) FrameSize.X, (int) FrameSize.Y);

        CalculateHitboxes();

        Update(gameTime);
    }

    public bool HitboxCheck(Rectangle compareTo)
        => Hitbox.Any(h => h.Intersects(compareTo));

    private void CalculateHitboxes()
    {
        _scale = new Vector2(Size.X / FrameSize.X, Size.Y / FrameSize.Y);
        var hitboxes = _textureHitboxMapping.Hitboxes;

        for (int i = 0; i < hitboxes.Length; i++)
        {
            Hitbox[i] = CalculateInGameHitbox(hitboxes[i]);
        }
    }

    private Rectangle CalculateInGameHitbox(Rectangle hitbox)
        => new((int) (Position.X + hitbox.X * _scale.X)
            , (int) (Position.Y + hitbox.Y * _scale.Y)
            , (int) (hitbox.Width * _scale.X)
            , (int) (hitbox.Height * _scale.Y));


    public bool Move(Vector2 Direction)
    {
        Position += Direction;
        return true;
    }
}
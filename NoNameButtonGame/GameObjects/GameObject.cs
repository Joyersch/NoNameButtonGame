using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.Xna.Framework.Content;
using NoNameButtonGame.Cache;
using NoNameButtonGame.Interfaces;

namespace NoNameButtonGame.GameObjects;

public class GameObject : IHitbox
{
    public Texture2D Texture;
    public Vector2 Position;
    public Vector2 Canvas;
    public Vector2 FrameSize;
    public Rectangle ImageLocation;
    public Color DrawColor;
    public Rectangle Rectangle;

    protected TextureHitboxMapping _textureHitboxMapping;
    protected Rectangle[] _hitboxes;
    protected Vector2 _scale;

    public Vector2 Scale => _scale;

    public Rectangle[] Hitbox => _hitboxes;

    public static Vector2 DefaultSize => new Vector2(0, 0);

    public GameObject(Vector2 position) : this(position, DefaultSize)
    {
    }

    public GameObject(Vector2 position, float scale) : this(position, DefaultSize * scale)
    {
    }

    public GameObject(Vector2 position, Vector2 canvas)
    {
        Canvas = canvas;
        Position = position;
        DrawColor = Color.White;
        Rectangle = new Rectangle(Position.ToPoint(), Canvas.ToPoint());
        Initialize();
        ImageLocation = new Rectangle(
            (int) _textureHitboxMapping.ImageSize.X
            , 0
            , (int) _textureHitboxMapping.ImageSize.X
            , (int) _textureHitboxMapping.ImageSize.Y);
        FrameSize = _textureHitboxMapping.ImageSize;
        Texture = _textureHitboxMapping.Texture;
        _hitboxes = new Rectangle[_textureHitboxMapping.Hitboxes.Length];
        CalculateHitboxes();
        Rectangle = new Rectangle(Position.ToPoint(), Canvas.ToPoint());
    }

    public virtual void Initialize()
    {
        throw new Exception();
    }

    public virtual void Update(GameTime gameTime)
    {
        CalculateHitboxes();
        UpdateRectangle();
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        if (ImageLocation == new Rectangle(0, 0, 0, 0))
            spriteBatch.Draw(Texture, Rectangle, DrawColor);
        else
            spriteBatch.Draw(Texture, Rectangle, ImageLocation, DrawColor);
    }

    protected virtual void UpdateRectangle()
        => Rectangle = new Rectangle(Position.ToPoint(), Canvas.ToPoint());

    public bool HitboxCheck(Rectangle compareTo)
        => Hitbox.Any(h => h.Intersects(compareTo));

    protected void CalculateHitboxes()
    {
        _scale = new Vector2(Canvas.X / FrameSize.X, Canvas.Y / FrameSize.Y);
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
}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using NoNameButtonGame.Cache;
using NoNameButtonGame.Interfaces;

namespace NoNameButtonGame.GameObjects;

public class GameObject : IHitbox
{
    private readonly Texture2D texture;
    public Vector2 Position;
    public Vector2 Size;
    protected Vector2 FrameSize;
    protected Rectangle ImageLocation;
    public Color DrawColor;
    public Rectangle Rectangle;

    protected TextureHitboxMapping textureHitboxMapping;
    protected Rectangle[] hitboxes;
    protected Vector2 scaleToTexture;

    public Vector2 ScaleToTexture => scaleToTexture;

    public Rectangle[] Hitbox => hitboxes;

    public static Vector2 DefaultSize => new Vector2(0, 0);

    public GameObject(Vector2 position) : this(position, DefaultSize)
    {
    }

    public GameObject(Vector2 position, float scale) : this(position, DefaultSize * scale)
    {
    }

    public GameObject(Vector2 position, Vector2 size)
    {
        Size = size;
        Position = position;
        DrawColor = Color.White;
        Rectangle = new Rectangle(Position.ToPoint(), Size.ToPoint());
        Initialize();
        ImageLocation = new Rectangle(
            0
            , 0
            , (int) textureHitboxMapping.ImageSize.X
            , (int) textureHitboxMapping.ImageSize.Y);
        FrameSize = textureHitboxMapping.ImageSize;
        texture = textureHitboxMapping.Texture;
        hitboxes = new Rectangle[textureHitboxMapping.Hitboxes.Length];
        CalculateHitboxes();
        Rectangle = new Rectangle(Position.ToPoint(), Size.ToPoint());
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
            spriteBatch.Draw(texture, Rectangle, DrawColor);
        else
            spriteBatch.Draw(texture, Rectangle, ImageLocation, DrawColor);
    }

    protected virtual void UpdateRectangle()
        => Rectangle = new Rectangle(Position.ToPoint(), Size.ToPoint());

    public bool HitboxCheck(Rectangle compareTo)
        => Hitbox.Any(h => h.Intersects(compareTo));

    protected virtual void CalculateHitboxes()
    {
        scaleToTexture = Size / textureHitboxMapping.ImageSize;
        var textureHitboxes = textureHitboxMapping.Hitboxes;

        for (int i = 0; i < textureHitboxes.Length; i++)
        {
            hitboxes[i] = CalculateInGameHitbox(textureHitboxes[i]);
        }
    }

    private Rectangle CalculateInGameHitbox(Rectangle hitbox)
        => new((int) (Position.X + hitbox.X * scaleToTexture.X)
            , (int) (Position.Y + hitbox.Y * scaleToTexture.Y)
            , (int) (hitbox.Width * scaleToTexture.X)
            , (int) (hitbox.Height * scaleToTexture.Y));
}
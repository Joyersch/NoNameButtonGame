using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using NoNameButtonGame.Cache;
using NoNameButtonGame.Interfaces;

namespace NoNameButtonGame.GameObjects;

public class GameObject : IHitbox
{
    protected readonly Texture2D texture;
    public Vector2 Position;
    public Vector2 Size;
    protected Vector2 FrameSize;
    protected Rectangle ImageLocation;
    public Color DrawColor;
    public Rectangle Rectangle;

    protected TextureHitboxMapping textureHitboxMapping;
    protected Rectangle[] hitboxes;
    protected Vector2 scaleToTexture;

    public Rectangle[] Hitbox => hitboxes;

    public static Vector2 DefaultSize => new Vector2(0, 0);
    public static Texture2D DefaultTexture;

    public static TextureHitboxMapping DefaultMapping => new TextureHitboxMapping()
    {
        ImageSize = new Vector2(16, 16),
        Hitboxes = new[]
        {
            new Rectangle(0, 0, 16, 16)
        }
    };

    public GameObject(Vector2 position) : this(position, 1)
    {
    }

    public GameObject(Vector2 position, float scale) : this(position, DefaultSize * scale, DefaultTexture,
        DefaultMapping)
    {
    }

    public GameObject(Vector2 position, Vector2 size, Texture2D texture, TextureHitboxMapping mapping)
    {
        Size = size;
        Position = position;
        DrawColor = Color.White;
        this.texture = texture;
        this.textureHitboxMapping = mapping;
        ImageLocation = new Rectangle(
            0
            , 0
            , (int) textureHitboxMapping.ImageSize.X
            , (int) textureHitboxMapping.ImageSize.Y);
        FrameSize = textureHitboxMapping.ImageSize;
        hitboxes = new Rectangle[textureHitboxMapping.Hitboxes.Length];
        Rectangle = new Rectangle(Position.ToPoint(), Size.ToPoint());
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
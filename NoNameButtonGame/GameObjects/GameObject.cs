using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using NoNameButtonGame.GameObjects.Texture;
using NoNameButtonGame.Interfaces;

namespace NoNameButtonGame.GameObjects;

public class GameObject : IHitbox
{
    protected readonly Texture2D Texture;
    public Vector2 Position;
    public Vector2 Size;
    protected Vector2 FrameSize;
    protected Rectangle ImageLocation;
    public Color DrawColor;
    public Rectangle Rectangle;

    protected TextureHitboxMapping TextureHitboxMapping;
    protected Rectangle[] Hitboxes;
    protected Vector2 ScaleToTexture;

    public Rectangle[] Hitbox => Hitboxes;

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
        this.Texture = texture;
        this.TextureHitboxMapping = mapping;
        ImageLocation = new Rectangle(
            0
            , 0
            , (int) TextureHitboxMapping.ImageSize.X
            , (int) TextureHitboxMapping.ImageSize.Y);
        FrameSize = TextureHitboxMapping.ImageSize;
        Hitboxes = new Rectangle[TextureHitboxMapping.Hitboxes.Length];
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
            spriteBatch.Draw(Texture, Rectangle, DrawColor);
        else
            spriteBatch.Draw(Texture, Rectangle, ImageLocation, DrawColor);
    }

    protected virtual void UpdateRectangle()
        => Rectangle = new Rectangle(Position.ToPoint(), Size.ToPoint());

    public bool HitboxCheck(Rectangle compareTo)
        => Hitbox.Any(h => h.Intersects(compareTo));

    protected virtual void CalculateHitboxes()
    {
        ScaleToTexture = Size / TextureHitboxMapping.ImageSize;
        var textureHitboxes = TextureHitboxMapping.Hitboxes;

        for (int i = 0; i < textureHitboxes.Length; i++)
        {
            Hitboxes[i] = CalculateInGameHitbox(textureHitboxes[i]);
        }
    }

    private Rectangle CalculateInGameHitbox(Rectangle hitbox)
        => new((int) (Position.X + hitbox.X * ScaleToTexture.X)
            , (int) (Position.Y + hitbox.Y * ScaleToTexture.Y)
            , (int) (hitbox.Width * ScaleToTexture.X)
            , (int) (hitbox.Height * ScaleToTexture.Y));
}
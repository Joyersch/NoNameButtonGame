using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.Cache;
using NoNameButtonGame.GameObjects.Texture;

namespace NoNameButtonGame.GameObjects;

internal class Cursor : GameObject
{
    public new static Vector2 DefaultSize => DefaultMapping.ImageSize;
    public new static Texture2D DefaultTexture;

    public new static TextureHitboxMapping DefaultMapping => new TextureHitboxMapping()
    {
        ImageSize = new Vector2(7, 10),
        Hitboxes = new[]
        {
            new Rectangle(0, 0, 1, 1)
        }
    };

    public Cursor(Vector2 position) : this(position, DefaultSize)
    {
    }

    public Cursor(Vector2 position, float scale) : this(position, DefaultSize * scale)
    {
    }

    public Cursor(Vector2 position, Vector2 size) : base(position, size, DefaultTexture, DefaultMapping)
    {
        ImageLocation = Rectangle.Empty;
    }
    
}
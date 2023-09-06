using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level4.Overworld;

public class Forest : ConnectedGameObject
{
    public Forest() : this(Vector2.Zero)
    {
    }

    public Forest(Vector2 position) : this(position, 1F)
    {
    }

    public Forest(Vector2 position, float scale) : this(position, scale * DefaultSize)
    {
    }

    public Forest(Vector2 position, Vector2 size) : this(position, size, DefaultTexture, DefaultMapping)
    {
    }

    public Forest(Vector2 position, Vector2 size, Texture2D texture, TextureHitboxMapping mapping) : base(position, size, texture, mapping)
    {
    }

    public new static Vector2 DefaultSize => DefaultMapping.ImageSize;

    public new static Texture2D DefaultTexture;

    public new static TextureHitboxMapping DefaultMapping => new TextureHitboxMapping()
    {
        ImageSize = new Vector2(32, 32),
        Hitboxes = new[]
        {
            new Rectangle(0, 0, 32, 32)
        }
    };
    
    
}
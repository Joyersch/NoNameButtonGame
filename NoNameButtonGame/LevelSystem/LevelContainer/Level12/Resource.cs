using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level12;

public class Resource : GameObject
{
    public new static Vector2 DefaultSize => DefaultMapping.ImageSize;
    public new static Texture2D DefaultTexture;

    public new static TextureHitboxMapping DefaultMapping => new TextureHitboxMapping()
    {
        ImageSize = new Vector2(8, 8),
        Hitboxes = new[]
        {
            new Rectangle(0, 0, 8, 8)
        }
    };

    public enum Type
    {
        Carrot,
        Potato,
        Stone,
        Ruby,
        Bread,
        Stick,
        Sapphire,
        Amethyst
    }

    private readonly Type _type;
    
    public Resource(Vector2 position, Type type) : this(position, 1, type)
    {
    }
    
    public Resource(Vector2 position, float scale, Type type) : this(position, DefaultSize * scale, type)
    {
    }

    public Resource(Vector2 position, Vector2 size, Type type) : this(position, size, type, DefaultTexture,
        DefaultMapping)
    {
    }

    public Resource(Vector2 position, Vector2 size, Type type, Texture2D texture, TextureHitboxMapping mapping) : base(
        position, size, texture, mapping)
    {
        _type = type;
        SetType(type);
    }
    
    private void SetType(Type type)
        => ImageLocation = type switch
        {
            Type.Carrot => new Rectangle(0, 0, 8, 8),
            Type.Potato => new Rectangle(8, 0, 8, 8),
            Type.Stone => new Rectangle(16, 0, 8, 8),
            Type.Ruby => new Rectangle(24, 0, 8, 8),
            Type.Bread => new Rectangle(0, 8, 8, 8),
            Type.Stick => new Rectangle(8, 8, 8, 8),
            Type.Sapphire => new Rectangle(16, 8, 8, 8),
            Type.Amethyst => new Rectangle(24, 8, 8, 8)
        };
}
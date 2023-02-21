using Microsoft.Xna.Framework;

namespace NoNameButtonGame.GameObjects;

internal class Cursor : GameObject
{
    public new static Vector2 DefaultSize => new Vector2(7, 10);
    
    public Cursor(Vector2 position) : this(position, DefaultSize)
    {
    }
    
    public Cursor(Vector2 position, float scale) : this(position, DefaultSize * scale)
    {
    }
    
    public Cursor(Vector2 position, Vector2 size) : base(position, size)
    {
        ImageLocation = Rectangle.Empty;
    }

    public override void Initialize()
    {
       textureHitboxMapping = Globals.Textures.GetMappingFromCache<Cursor>();
    }
}
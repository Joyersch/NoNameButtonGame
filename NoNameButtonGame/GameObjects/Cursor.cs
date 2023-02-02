using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.Hitboxes;
using NoNameButtonGame.Interfaces;

namespace NoNameButtonGame.GameObjects;

class Cursor : GameObject
{

    public Cursor(Vector2 position) : this(position, DefaultSize)
    {
    }
    
    public Cursor(Vector2 position, Vector2 size) : base(position, size)
    {
        ImageLocation = Rectangle.Empty;
    }

    public override void Initialize()
    {
       _textureHitboxMapping = Mapping.GetMappingFromCache<Cursor>();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    public static Vector2 DefaultSize => new Vector2(7, 10);
}
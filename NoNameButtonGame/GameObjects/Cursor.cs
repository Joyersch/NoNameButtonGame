using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.Cache;
using NoNameButtonGame.Interfaces;

namespace NoNameButtonGame.GameObjects;

internal class Cursor : GameObject
{
    public static Vector2 DefaultSize => new Vector2(7, 10);
    
    public Cursor(Vector2 position) : this(position, DefaultSize)
    {
    }
    
    public Cursor(Vector2 position, float scale) : this(position, DefaultSize * scale)
    {
    }
    
    public Cursor(Vector2 position, Vector2 canvas) : base(position, canvas)
    {
        ImageLocation = Rectangle.Empty;
    }

    public override void Initialize()
    {
       _textureHitboxMapping = Globals.Textures.GetMappingFromCache<Cursor>();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }
}
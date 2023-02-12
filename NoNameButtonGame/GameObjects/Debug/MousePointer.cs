using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.Cache;

namespace NoNameButtonGame.GameObjects.Debug;

public class MousePointer : GameObject
{
    private Vector2 _mousePositionOffset;
    public Color DrawColor => Color.White;
    private bool draw = false;

    public MousePointer() : this(Vector2.Zero, Vector2.Zero)
    {
    }
    
    public MousePointer(Vector2 position, Vector2 size) : this(position, size, false)
    {
    }

    public MousePointer(Vector2 position, Vector2 size, bool draw) : base(position, size)
    {
        this.draw = draw;
    }

    public override void Initialize()
    {
        _textureHitboxMapping = Globals.Textures.GetMappingFromCache<MousePointer>();
    }

    public void Update(GameTime gameTime, Vector2 mousePosition)
    {
        Position = mousePosition;
        base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (!draw)
            return;

        spriteBatch.Draw(_textureHitboxMapping.Texture, new Rectangle((int) Position.X - 3, (int) Position.Y - 3, 6, 6),
            DrawColor);
        base.Draw(spriteBatch);
    }
}
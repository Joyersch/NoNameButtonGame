using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.Hitboxes;

namespace NoNameButtonGame.GameObjects;

public class MousePointer : GameObject
{
    public static TextureHitboxMapping TextureHitboxMapping;
    private Vector2 _mousePositionOffset;
    public Color DrawColor => Color.White;

    public override void Update(GameTime gameTime)
    {
        MouseState mouse = Mouse.GetState();
        _mousePositionOffset = mouse.Position.ToVector2() - new Vector2(3, 3);
    }
    
    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(TextureHitboxMapping.Texture, new Rectangle((int)_mousePositionOffset.X, (int)_mousePositionOffset.Y, 6, 6), DrawColor);
    }
}
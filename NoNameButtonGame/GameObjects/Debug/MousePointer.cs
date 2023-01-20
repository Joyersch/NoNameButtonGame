using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.Hitboxes;

namespace NoNameButtonGame.GameObjects.Debug;

public class MousePointer : GameObject
{
    private Vector2 _mousePositionOffset;
    public Color DrawColor => Color.White;

    public MousePointer()
    {
        var textureMapping = Mapping.GetMappingFromCache<MousePointer>();
        Texture = textureMapping.Texture;
    }
    public override void Update(GameTime gameTime)
    {
        
        MouseState mouse = Mouse.GetState();
        _mousePositionOffset = mouse.Position.ToVector2() - new Vector2(3, 3);
    }
    
    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, new Rectangle((int)_mousePositionOffset.X, (int)_mousePositionOffset.Y, 6, 6), DrawColor);
    }
}
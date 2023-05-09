using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NoNameButtonGame.Interfaces;

public interface IManageable
{
    public void Update(GameTime gameTime);

    public void Draw(SpriteBatch spriteBatch);
    public void DrawStatic(SpriteBatch spriteBatch);
}
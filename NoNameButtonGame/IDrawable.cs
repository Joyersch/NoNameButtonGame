using Microsoft.Xna.Framework.Graphics;

namespace NoNameButtonGame;

public interface IDrawable : IRectangle
{
    public void Draw(SpriteBatch spriteBatch);
}
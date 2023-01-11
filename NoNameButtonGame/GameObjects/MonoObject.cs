using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace NoNameButtonGame.GameObjects
{
    public abstract class MonoObject
    {
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}

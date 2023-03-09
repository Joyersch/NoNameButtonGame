using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.GameObjects.Texture;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level6;

public class Shop : GameObject
{
    public new static Vector2 DefaultSize => Display.Display.Size;
    

    public Shop(Vector2 position) : base(position, DefaultSize, DefaultTexture, DefaultMapping)
    {
    }
}
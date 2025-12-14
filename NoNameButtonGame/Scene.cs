using Joyersch.Monogame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NoNameButtonGame;

public class Scene
{
    public readonly GraphicsDevice GraphicsDevice;
    public Camera Camera { get; private set; }
    public Display Display { get; private set; }

    public Scene(GraphicsDevice graphicsDevice, Vector2 screenSize, float defaultZoom)
    {
        GraphicsDevice = graphicsDevice;
        Display = new Display(graphicsDevice, screenSize);
        Camera = new Camera(Display, defaultZoom);
    }

    public void Update(GameTime gameTime)
    {
        Display.Update();
        Camera.Update(gameTime);
    }
}
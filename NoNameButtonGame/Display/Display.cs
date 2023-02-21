using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace NoNameButtonGame.Display;

public class Display
{
    public readonly RenderTarget2D Target;
    public readonly float DefaultWidth = 1280F;
    public readonly float DefaultHeight = 720F;

    public Rectangle Window;
    
    //SHOUTOUT: https://youtu.be/yUSB_wAVtE8
    private Rectangle backbufferBounds;
    private float backbufferAspectRatio;
    private float screenAspectRatio;
    
    private GraphicsDevice device;
    public Display(GraphicsDevice device)
    {
        this.device = device;
        Target = new RenderTarget2D(this.device, (int) DefaultWidth, (int) DefaultHeight);
    }
    
    public void Update(GameTime gameTime)
    {
        //SHOUTOUT: https://youtu.be/yUSB_wAVtE8
        backbufferBounds = device.PresentationParameters.Bounds;
        backbufferAspectRatio = (float) backbufferBounds.Width / backbufferBounds.Height;
        screenAspectRatio = (float) Target.Width / Target.Height;
        
        float x, y, w, h; 
        x = 0f;
        y = 0f;
        w = backbufferBounds.Width;
        h = backbufferBounds.Height;
        if (backbufferAspectRatio > screenAspectRatio)
        {
            w = h * screenAspectRatio;
            x = (backbufferBounds.Width - w) / 2f;
        }
        else if (backbufferAspectRatio < screenAspectRatio)
        {
            h = w / screenAspectRatio;
            y = (backbufferBounds.Height - h) / 2f;
        }
        Window = new Rectangle((int) x, (int) y, (int) w, (int) h);
    }
}
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
    private Rectangle BackbufferBounds;
    private float backbufferAspectRatio;
    private float screenAspectRatio;
    
    private GraphicsDevice device;
    public Display(GraphicsDevice device)
    {
        this.device = this.device;
        Target = new RenderTarget2D(this.device, (int) DefaultWidth, (int) DefaultHeight);
    }
    
    public void Update(GameTime gameTime)
    {
        //SHOUTOUT: https://youtu.be/yUSB_wAVtE8
        BackbufferBounds = device.PresentationParameters.Bounds;
        backbufferAspectRatio = (float) BackbufferBounds.Width / BackbufferBounds.Height;
        screenAspectRatio = (float) Target.Width / Target.Height;
        
        float x, y, w, h; 
        x = 0f;
        y = 0f;
        w = BackbufferBounds.Width;
        h = BackbufferBounds.Height;
        if (backbufferAspectRatio > screenAspectRatio)
        {
            w = h * screenAspectRatio;
            x = (BackbufferBounds.Width - w) / 2f;
        }
        else if (backbufferAspectRatio < screenAspectRatio)
        {
            h = w / screenAspectRatio;
            y = (BackbufferBounds.Height - h) / 2f;
        }
        Window = new Rectangle((int) x, (int) y, (int) w, (int) h);
    }
}
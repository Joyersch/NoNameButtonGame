using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace NoNameButtonGame.Display;

public class Display
{
    public readonly RenderTarget2D target2D;
    public readonly float defaultWidth = 1280F;
    public readonly float defaultHeight = 720F;

    public Rectangle Window;
    
    //SHOUTOUT: https://youtu.be/yUSB_wAVtE8
    private Rectangle BackbufferBounds;
    private float backbufferAspectRatio;
    private float ScreenAspectRatio;
    
    private GraphicsDevice gpu;
    public Display(GraphicsDevice gpu)
    {
        this.gpu = gpu;
        target2D = new RenderTarget2D(gpu, (int) defaultWidth, (int) defaultHeight);
    }
    
    public void Update(GameTime gameTime)
    {
        //SHOUTOUT: https://youtu.be/yUSB_wAVtE8
        BackbufferBounds = gpu.PresentationParameters.Bounds;
        backbufferAspectRatio = (float) BackbufferBounds.Width / BackbufferBounds.Height;
        ScreenAspectRatio = (float) target2D.Width / target2D.Height;
        
        float rx, ry, rw, rh; 
        rx = 0f;
        ry = 0f;
        rw = BackbufferBounds.Width;
        rh = BackbufferBounds.Height;
        if (backbufferAspectRatio > ScreenAspectRatio)
        {
            rw = rh * ScreenAspectRatio;
            rx = (BackbufferBounds.Width - rw) / 2f;
        }
        else if (backbufferAspectRatio < ScreenAspectRatio)
        {
            rh = rw / ScreenAspectRatio;
            ry = (BackbufferBounds.Height - rh) / 2f;
        }
        Window = new Rectangle((int) rx, (int) ry, (int) rw, (int) rh);
    }
}
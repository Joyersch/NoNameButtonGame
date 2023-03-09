using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace NoNameButtonGame.Display;

public class Display
{
    public readonly RenderTarget2D Target;
    public static readonly float Width = 1280F;
    public static readonly float Height = 720F;

    public static Vector2 Size => new Vector2(Width, Height);

    public Rectangle Window;
    
    //SHOUTOUT: https://youtu.be/yUSB_wAVtE8
    private Rectangle _backbufferBounds;
    private float _backbufferAspectRatio;
    private float _screenAspectRatio;
    
    private readonly GraphicsDevice _device;
    public Display(GraphicsDevice device)
    {
        this._device = device;
        Target = new RenderTarget2D(this._device, (int) Width, (int) Height);
    }
    
    public void Update(GameTime gameTime)
    {
        //SHOUTOUT: https://youtu.be/yUSB_wAVtE8
        _backbufferBounds = _device.PresentationParameters.Bounds;
        _backbufferAspectRatio = (float) _backbufferBounds.Width / _backbufferBounds.Height;
        _screenAspectRatio = (float) Target.Width / Target.Height;

        var x = 0f;
        var y = 0f;
        float w = _backbufferBounds.Width;
        float h = _backbufferBounds.Height;
        if (_backbufferAspectRatio > _screenAspectRatio)
        {
            w = h * _screenAspectRatio;
            x = (_backbufferBounds.Width - w) / 2f;
        }
        else if (_backbufferAspectRatio < _screenAspectRatio)
        {
            h = w / _screenAspectRatio;
            y = (_backbufferBounds.Height - h) / 2f;
        }
        Window = new Rectangle((int) x, (int) y, (int) w, (int) h);
    }
}
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace NoNameButtonGame.Display;

public class Display
{
    public readonly RenderTarget2D Target;
    public static readonly float Width = 1280F;
    public static readonly float Height = 720F;

    public static Vector2 Size => new Vector2(Width, Height);

    public Vector2 Bounds;

    public Vector2 Scale => Bounds / Size;

    // In theory Scale.X and Scale.Y should always be the same for this game
    public float SimpleScale => Scale.X;

    public Rectangle Window { get; private set; }

    public Rectangle Screen { get; private set; }

    //SHOUTOUT: https://youtu.be/yUSB_wAVtE8

    private readonly GraphicsDevice _device;

    public Display(GraphicsDevice device)
    {
        _device = device;
        Screen = _device.PresentationParameters.Bounds;
        Bounds = _device.PresentationParameters.Bounds.Size.ToVector2();
        Target = new RenderTarget2D(this._device, (int) Width, (int) Height);
    }

    public void Update(GameTime gameTime)
    {
        Screen = _device.PresentationParameters.Bounds;
        Bounds = _device.PresentationParameters.Bounds.Size.ToVector2();
        //SHOUTOUT: https://youtu.be/yUSB_wAVtE8
        var backbufferAspectRatio = Bounds.X / Bounds.Y;
        var screenAspectRatio = (float) Target.Width / Target.Height;

        var x = 0f;
        var y = 0f;
        float w = Screen.Width;
        float h = Screen.Height;
        if (backbufferAspectRatio > screenAspectRatio)
        {
            w = h * screenAspectRatio;
            x = (Screen.Width - w) / 2f;
        }
        else if (backbufferAspectRatio < screenAspectRatio)
        {
            h = w / screenAspectRatio;
            y = (Screen.Height - h) / 2f;
        }

        Window = new Rectangle((int) x, (int) y, (int) w, (int) h);
    }
}
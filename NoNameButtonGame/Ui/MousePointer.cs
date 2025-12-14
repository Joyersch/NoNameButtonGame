using Joyersch.Monogame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NoNameButtonGame.Ui;

public sealed class MousePointer : IManageable, IMoveable
{
    public Vector2 Position { get; private set; }
    public Vector2 Size { get; private set; } = new Vector2(4, 4);
    public static bool CanDraw;
    private readonly Texture2D _texture;
    private readonly Camera _camera;
    private readonly Display _display;

    private Vector2 _canvasPosition;
    private Vector2 _realPosition;
    private Vector2 _canvasCenter;
    private Vector2 _window;
    private Vector2 _screenScale;
    private Vector2 _windowCenter;

    public bool UseRelative { get; set; }
    public float Speed { get; set; } = 1F;

    public static Texture2D Texture;

    public MousePointer(Scene scene)
    {
        _camera = scene.Camera;
        _display = scene.Display;
        UpdateWindow(scene.Display.Size);
    }

    public Rectangle Rectangle => Rectangle.Empty;

    public void Update(GameTime gameTime)
    {
        // Mouse position on the actual screen
        _realPosition = Microsoft.Xna.Framework.Input.Mouse.GetState().Position.ToVector2();
        var realCenter = _windowCenter;

        var cameraOffset = _display.Size / _camera.Zoom / 2;
        var appliedOffset = _camera.Position - cameraOffset;
        var scale = _screenScale * _camera.Zoom;

        _canvasPosition = _realPosition / scale + appliedOffset;
        _canvasCenter = realCenter / scale + appliedOffset;

        if (UseRelative)
        {
            SetMousePositionToCenter();
        }
        else
            _canvasCenter = Position;

        Position -= (_canvasCenter - _canvasPosition) * Speed * (_display.Scale / 2);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (!CanDraw)
            return;

        spriteBatch.Draw(Texture, new Rectangle((int)_realPosition.X - 6, (int)_realPosition.Y - 6, 12, 12),
            Microsoft.Xna.Framework.Color.Red);
    }

    public void DrawIndicator(SpriteBatch spriteBatch)
    {
        if (!CanDraw)
            return;

        spriteBatch.Draw(_texture, _canvasCenter, Global.Color);
    }

    public void UpdateWindow(Vector2 window)
    {
        _window = window;
        _windowCenter = _window / 2;
        _screenScale = _window / _display.Size;
    }

    private void SetMousePositionToCenter()
        => Microsoft.Xna.Framework.Input.Mouse.SetPosition((int)_windowCenter.X, (int)_windowCenter.Y);

    public void SetMousePointerPositionToCenter()
    {
        SetMousePositionToCenter();
        Position = _canvasCenter;
    }

    public Vector2 GetPosition()
        => Position;

    public Vector2 GetSize()
        => Size;

    public void Move(Vector2 newPosition)
        => Position = newPosition;
}
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoUtils.Logic;
using MonoUtils.Objects;
using MonoUtils.Ui;

namespace NoNameButtonGame.GameObjects.Debug;

public class MousePointer : GameObject, IMoveable
{
    private readonly bool _draw;
    private readonly Camera _camera;

    private Vector2 _cursorPosition;
    private Vector2 _drawPosition;
    private Vector2 _window;
    private Point _windowCenter;

    public bool UseRelative { get; set; } = false;
    public float Speed { get; set; } = 1F;

    public new static Texture2D DefaultTexture;

    public new static TextureHitboxMapping DefaultMapping => new TextureHitboxMapping()
    {
        ImageSize = new Vector2(6, 6),
        Hitboxes = new[]
        {
            new Rectangle(0, 0, 6, 6)
        }
    };

    public MousePointer(Vector2 window, Camera camera, bool draw) : this(window, camera, draw, Vector2.Zero,
        DefaultSize)
    {
    }

    public MousePointer(Vector2 window, Camera camera, bool draw, Vector2 position, Vector2 size) : base(position, size,
        DefaultTexture,
        DefaultMapping)
    {
        _window = window;
        _windowCenter = new Point((int) _window.X / 2, (int) _window.Y / 2);
        _camera = camera;
        _draw = draw;
    }

    public void Update(GameTime gameTime)
    {
        _drawPosition = Microsoft.Xna.Framework.Input.Mouse.GetState().Position.ToVector2();
        var screenScale = new Vector2(_window.X / Display.CustomWidth,
            _window.Y / Display.CustomHeight);
        var offset = Display.Size / _camera.Zoom / 2;
        _cursorPosition = _drawPosition / screenScale / _camera.Zoom + _camera.Position - offset;
        var _centerPosition = _windowCenter.ToVector2() / screenScale / _camera.Zoom + _camera.Position - offset;
        if (!UseRelative)
            Position = _cursorPosition;
        else
        {
            SetMousePositionToCenter();
            Position -= (_centerPosition - _cursorPosition) * Speed;
        }

        base.Update(gameTime);
    }

    protected override void GeneralDraw(SpriteBatch spriteBatch)
    {
        if (!_draw)
            return;

        spriteBatch.Draw(Texture, new Rectangle((int) _drawPosition.X - 3, (int) _drawPosition.Y - 3, 6, 6),
            DrawColor);
        base.GeneralDraw(spriteBatch);
    }

    public void UpdateWindow(Vector2 window)
        => _window = window;

    public Vector2 GetPosition()
        => Position;

    public Vector2 GetSize()
        => Size;

    public void Move(Vector2 newPosition)
        => Position = newPosition;

    private void SetMousePositionToCenter()
        => Microsoft.Xna.Framework.Input.Mouse.SetPosition(_windowCenter.X, _windowCenter.Y);

    public void SetMousePointerPositionToCenter()
    {
        SetMousePositionToCenter();
        if (UseRelative)
            Position = _cursorPosition;
    }
}
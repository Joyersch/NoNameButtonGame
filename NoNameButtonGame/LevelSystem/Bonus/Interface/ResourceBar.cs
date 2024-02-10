using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils;
using MonoUtils.Logging;
using MonoUtils.Logic;
using MonoUtils.Logic.Management;
using MonoUtils.Ui.Objects.TextSystem;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Bonus.Interface;

public class ResourceBar : IManageable, IMoveable
{
    private readonly ResourceManager _resourceManager;
    private readonly Vector2 _size;
    private readonly Vector2 _singleSize;
    private readonly ResourcePair[] _resources;
    private Rectangle _rectangle;
    private Vector2 _position;


    public Vector2 Position => _position;
    public Rectangle Rectangle => _rectangle;

    private GameObject _gameObject;

    public ResourceBar(ResourceManager resourceManager, Vector2 position, float scale)
    {
        _resourceManager = resourceManager;
        _position = position;
        _singleSize = new Vector2((Resource.DefaultSize.X) * scale, Resource.DefaultSize.Y);
        _size = new Vector2(_singleSize.X * 8, _singleSize.Y);
        
        _rectangle = new Rectangle(position.ToPoint(), _size.ToPoint());
        _resources = new ResourcePair[8];
        for (int i = 0; i < 8; i++)
        {
            _resources[i] = new ResourcePair((Resource.Type) i, position, _singleSize, scale);
            _resources[i].GetCalculator(position, _singleSize)
                .OnCenter()
                .ByGridX(i)
                .Centered()
                .Move();
        }
    }

    public void Update(GameTime gameTime)
    {
        for (int i = 0; i < 8; i++)
        {
            _resources[i].SetNumber(_resourceManager.GetUserValue((Resource.Type) i));
            _resources[i].GetCalculator(_position, _singleSize)
                .OnCenter()
                .ByGridX(i)
                .Centered()
                .Move();
            _resources[i].Update(gameTime);
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        for (int i = 0; i < 8; i++)
            _resources[i].Draw(spriteBatch);
    }

    public Vector2 GetPosition()
        => _position;

    public Vector2 GetSize()
        => _size;

    public void Move(Vector2 newPosition)
    {
        var offset = newPosition - _position;
        for (int i = 0; i < 8; i++)
            _resources[i].Move(_resources[i].GetPosition() + offset);
        _rectangle.X = (int) newPosition.X;
        _rectangle.Y = (int) newPosition.Y;
        _position = newPosition;
    }
}
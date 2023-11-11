using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Logging;
using MonoUtils.Logic;
using MonoUtils.Logic.Management;
using MonoUtils.Ui.Objects.TextSystem;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level12.Interface;

public class ResourcePair : IManageable, IMoveable
{
    private Vector2 _position;
    private readonly Vector2 _areaSize;
    private readonly Resource _resource;
    private readonly Text _text;
    private int _number;
    public Rectangle Rectangle { get; private set; }

    public ResourcePair(Resource.Type resourceType, Vector2 areaSize, float scale) : this(resourceType, Vector2.Zero,
        areaSize, scale)
    {
    }

    public ResourcePair(Resource.Type resourceType, Vector2 position, Vector2 areaSize, float scale)
    {
        _position = position;
        _areaSize = areaSize;
        _resource = new Resource(position, scale * 2, resourceType);
        _text = new Text($"{_number}", scale);
        Rectangle = new Rectangle(_position.ToPoint(), areaSize.ToPoint());

        _resource
            .GetCalculator(Rectangle)
            .OnX(0.33F)
            .OnY(0.5F)
            .Centered()
            .Move();

        _text
            .GetCalculator(Rectangle)
            .OnX(0.66F)
            .OnY(0.5F)
            .Centered()
            .Move();
    }

    public void SetNumber(int number)
        => _number = number;


    public void Update(GameTime gameTime)
    {
        _resource
            .GetCalculator(Rectangle)
            .OnX(0.33F)
            .OnY(0.5F)
            .Centered()
            .Move();

        _text.ChangeText($"{_number}");
        _text
            .GetCalculator(Rectangle)
            .OnX(0.66F)
            .OnY(0.5F)
            .Centered()
            .Move();

        _resource.Update(gameTime);
        _text.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _resource.Draw(spriteBatch);
        _text.Draw(spriteBatch);
    }

    public Vector2 GetPosition()
        => _position;

    public Vector2 GetSize()
        => Rectangle.Size.ToVector2();

    public void Move(Vector2 newPosition)
    {
        var offset = newPosition - _position;
        _text.Move(_text.Position + offset);
        _resource.Move(_resource.Position + offset);

        _position = newPosition;
        Rectangle = new Rectangle(_position.ToPoint(), _areaSize.ToPoint());
    }
}
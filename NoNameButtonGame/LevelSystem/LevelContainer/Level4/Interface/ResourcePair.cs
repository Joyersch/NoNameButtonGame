using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Logic;
using MonoUtils.Logic.Management;
using MonoUtils.Ui.Objects.TextSystem;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level4.Interface;

public class ResourcePair : IManageable, IMoveable
{
    private Vector2 _position;
    private readonly Resource _resource;
    private readonly Text _text;
    private int _number;
    public Rectangle Rectangle => Rectangle.Union(_resource.Rectangle, _text.Rectangle);

    public ResourcePair(Resource.Type resourceType, Vector2 position, float scale)
    {
        _position = position;
        _resource = new Resource(position, scale, resourceType);
        _text = new Text($"{_number}", scale);
        _resource
            .GetCalculator(Rectangle)
            .OnX(0.25F)
            .OnY(0.5F)
            .Centered()
            .Move();

        _text
            .GetCalculator(Rectangle)
            .OnX(0.75F)
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
            .OnX(0.25F)
            .OnY(0.5F)
            .Centered()
            .Move();

        _text.ChangeText($"{_number}");
        _text
            .GetCalculator(Rectangle)
            .OnX(0.75F)
            .OnY(0.5F)
            .Centered()
            .Move();

        _resource.Update(gameTime);
        _text.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _resource.Draw(spriteBatch);
    }

    public Vector2 GetPosition()
        => Rectangle.Location.ToVector2();

    public Vector2 GetSize()
        => Rectangle.Size.ToVector2();

    public void Move(Vector2 newPosition)
    {
        var offset = newPosition - _position;
        _text.Move(_text.Position + offset);
        _resource.Move(_text.Position + offset);
        _position = newPosition;
    }
}
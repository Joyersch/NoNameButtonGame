using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils;
using MonoUtils.Logging;
using MonoUtils.Logic;
using MonoUtils.Logic.Hitboxes;
using MonoUtils.Ui.Objects.Buttons;
using MonoUtils.Ui.Objects.TextSystem;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level4;

public class UserInterface : GameObject, IInteractable
{
    private readonly ResourceManager _manager;
    private readonly Text _text;
    private readonly SquareTextButton _close;

    public event Action Exit;

    public new static Vector2 DefaultSize => DefaultMapping.ImageSize * 5;
    public new static Texture2D DefaultTexture;

    public new static TextureHitboxMapping DefaultMapping => new TextureHitboxMapping()
    {
        ImageSize = new Vector2(128, 72),
        Hitboxes = new[]
        {
            new Rectangle(0, 0, 128, 72)
        }
    };

    public UserInterface(ResourceManager manager, string name, float scale) : this(manager, name, scale, Vector2.Zero,
        DefaultSize * scale)
    {
    }

    public UserInterface(ResourceManager manager, string name, float scale, Vector2 position, Vector2 size) : base(
        position, size, DefaultTexture, DefaultMapping)
    {
        DrawColor = new Color(75, 75, 75);
        _manager = manager;
        _text = new Text(name, scale * 2);
        _text.GetCalculator(Rectangle)
            .OnX(0.05F)
            .OnY(0.1F)
            .Move();
        _close = new SquareTextButton(Letter.ReverseParse(Letter.Character.Crossout).ToString(), scale * 0.5F);
        _close.GetCalculator(Rectangle)
            .OnX(0.9575F)
            .OnY(0.0325F)
            .Move();
        _close.Click += _ => Exit?.Invoke();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        _close.Update(gameTime);
        _text.Update(gameTime);
        Log.WriteLine(_close.Position.ToString(), 2);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        _close.Draw(spriteBatch);
        _text.Draw(spriteBatch);
    }

    public override void Move(Vector2 newPosition)
    {
        var offset = newPosition - Position;
        base.Move(newPosition);
        _close.Move(_close.Position + offset);
        _text.Move(_text.Position + offset);
    }

    public void UpdateInteraction(GameTime gameTime, IHitbox toCheck)
    {
        _close.UpdateInteraction(gameTime, toCheck);
    }
}
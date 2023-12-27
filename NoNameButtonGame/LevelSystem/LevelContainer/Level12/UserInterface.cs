using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils;
using MonoUtils.Logging;
using MonoUtils.Logic;
using MonoUtils.Logic.Hitboxes;
using MonoUtils.Ui.Objects.Buttons;
using MonoUtils.Ui.Objects.Console;
using MonoUtils.Ui.Objects.TextSystem;
using NoNameButtonGame.LevelSystem.LevelContainer.Level12.Interface;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level12;

public class UserInterface : GameObject, IInteractable
{
    private readonly List<TradeBar> _trades;
    private readonly ResourceManager _manager;
    private readonly ResourceBar _resourceBar;
    private readonly Text _text;
    private readonly SquareTextButton _close;
    private readonly DevConsole _input;
    private static CommandProcessor _commandProcessor;
    private bool _isInputActive;
    private bool _isConsoleHover;

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

    public UserInterface(IEnumerable<ResourceTrade> trades, ResourceManager manager, GameWindow window, string name,
        float scale) : this(trades, manager, window, name, scale, Vector2.Zero, DefaultSize * scale)
    {
    }

    public UserInterface(IEnumerable<ResourceTrade> trades, ResourceManager manager, GameWindow window, string name,
        float scale, Vector2 position, Vector2 size) : base(position, size, DefaultTexture, DefaultMapping)
    {
        var resourceTrades = trades.ToList();
        _trades = new List<TradeBar>();
        int c = 0;
        foreach (var trade in resourceTrades)
        {
            var tradeBar = new TradeBar(trade, position, scale);
            tradeBar.GetCalculator(position, size)
                .OnX(0.55F)
                .OnY(0.1F + 0.1F * c++)
                .Move();
            _trades.Add(tradeBar);
        }

        _manager = manager;
        DrawColor = new Color(75, 75, 75);

        if (_commandProcessor is null)
        {
            _commandProcessor = new CommandProcessor();
            _commandProcessor.Commands.Add(("help", "Shows all commands!",
                new MonoUtils.Ui.Objects.Console.Commands.HelpCommand()));
        }

        _text = new Text(name, scale * 2);
        _text.GetCalculator(Rectangle)
            .OnX(0.04F)
            .OnY(0.1F)
            .Move();
        _close = new SquareTextButton("[crossout]", scale * 0.5F);
        _close.GetCalculator(Rectangle)
            .OnX(0.9575F)
            .OnY(0.0325F)
            .Move();
        _close.Click += _ => Exit?.Invoke();

        _input = new DevConsole(_commandProcessor, Vector2.Zero, scale * 0.5F);
        // ToDo: add hook for window.TextInput for _input.TextInput event handle method
        _input.DrawColor = Color.Transparent;
        _input.GetCalculator(Rectangle)
            .OnX(0.035F)
            .OnY(0.2275F)
            .Move();

        _resourceBar = new ResourceBar(_manager, position, scale * 1.25F);
        _resourceBar.GetCalculator(Rectangle)
            .OnCenter()
            .OnY(0.7F)
            .Centered()
            .Move();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        _close.Update(gameTime);
        _text.Update(gameTime);

        if (InputReaderMouse.CheckKey(InputReaderMouse.MouseKeys.Left, true))
            _isInputActive = _isConsoleHover;

        /*
         ToDo: handle user input registration from window
          if (_isInputActive)
            _input.ActivateInput();
        else
            _input.DeactivateInput();
             */
        _input.Update(gameTime);

        _resourceBar.GetCalculator(Rectangle)
            .OnCenter()
            .OnY(0.85F)
            .Centered()
            .Move();

        _resourceBar.Update(gameTime);

        foreach (var trade in _trades)
            trade.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        _close.Draw(spriteBatch);
        _text.Draw(spriteBatch);
        _input.Draw(spriteBatch);
        _resourceBar.Draw(spriteBatch);
        foreach (var trade in _trades)
            trade.Draw(spriteBatch);
    }

    public override void Move(Vector2 newPosition)
    {
        var offset = newPosition - Position;
        _close.Move(_close.Position + offset);
        _text.Move(_text.Position + offset);
        _input.Move(_input.Position + offset);
        _resourceBar.Move(_resourceBar.Position + offset);
        foreach (var trade in _trades)
            trade.Move(trade.GetPosition() + offset);
        base.Move(newPosition);
    }

    public void UpdateInteraction(GameTime gameTime, IHitbox toCheck)
    {
        _isConsoleHover = toCheck.Hitbox.Any(h => _input.Hitbox.Any(hh => hh.Intersects(h)));
        foreach (var trade in _trades)
            trade.UpdateInteraction(gameTime, toCheck);
        _close.UpdateInteraction(gameTime, toCheck);
    }
}
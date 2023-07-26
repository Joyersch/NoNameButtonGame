using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Logic;
using MonoUtils.Logic.Hitboxes;
using MonoUtils.Logic.Management;
using MonoUtils.Ui.Objects.Buttons;
using MonoUtils.Ui.Objects.Buttons.AddOn;
using MonoUtils.Ui.Objects.TextSystem;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level4.Interface;

public class TradeBar : IManageable, IInteractable, IMoveable
{
    private readonly ResourceTrade _trade;
    private Vector2 _position;
    private Vector2 _size = new Vector2(240, 40);

    private LockButtonAddon _button;
    private Text _equals;
    private ResourcePair[] _from;
    private ResourcePair[] _to;

    public TradeBar(ResourceTrade trade, Vector2 position, float scale)
    {
        _trade = trade;
        _position = position;
        _button = new LockButtonAddon(new ButtonAddonAdapter(new MiniTextButton("Buy", scale * 0.75F)), scale * 0.75F);
        _button.Callback += ButtonClicked;

        var fromItems = trade.FromItem.ToArray();
        var toItems = trade.ToItems.ToArray();

        var fromCount = fromItems.Length;
        var toCount = toItems.Length;

        _from = new ResourcePair[fromCount];
        _to = new ResourcePair[toCount];

        int from = 0, to = 0;


        foreach (var fromItem in fromItems)
        {
            var pair = new ResourcePair(fromItem.Item,
                new Vector2(Text.DefaultLetterSize.X * 2 + 8, Text.DefaultLetterSize.Y), scale);
            pair.SetNumber(fromItem.Amount);
            pair.GetCalculator(position, _size)
                .OnCenter()
                .OnX(0.04F + 0.12F * from)
                .Centered()
                .Move();
            _from[from++] = pair;
        }


        foreach (var toItem in toItems)
        {
            var pair = new ResourcePair(toItem.Item,
                new Vector2(Text.DefaultLetterSize.X * 2 + 8, Text.DefaultLetterSize.Y), scale);
            pair.SetNumber(toItem.Amount);
            pair.GetCalculator(position, _size)
                .OnCenter()
                .OnX(0.59F + 0.12F * to)
                .Centered()
                .Move();
            _to[to++] = pair;
        }

        _button.GetCalculator(position, _size)
            .OnCenter()
            .OnX(1F)
            .Centered()
            .Move();

        _equals = new Text(Letter.ReverseParse(Letter.Character.Right).ToString());
        _equals.GetCalculator(position, _size)
            .OnCenter()
            .OnX(0.5F)
            .Centered()
            .Move();
    }

    private void ButtonClicked(object sender, IButtonAddon.CallState state)
    {
        if (state != IButtonAddon.CallState.Click)
            return;

        _trade.Trade();
    }

    public Rectangle Rectangle => new Rectangle(_position.ToPoint(), _size.ToPoint());

    public void Update(GameTime gameTime)
    {
        if (_trade.CanTrade())
            _button.Unlock();
        else
            _button.Lock();
        _button.Update(gameTime);
        _equals.Update(gameTime);

        foreach (var from in _from)
            from.Update(gameTime);

        foreach (var to in _to)
            to.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _button.Draw(spriteBatch);
        _equals.Draw(spriteBatch);

        foreach (var from in _from)
            from.Draw(spriteBatch);

        foreach (var to in _to)
            to.Draw(spriteBatch);
    }

    public void UpdateInteraction(GameTime gameTime, IHitbox toCheck)
    {
        _button.UpdateInteraction(gameTime, toCheck);
    }

    public Vector2 GetPosition()
        => _position;

    public Vector2 GetSize()
        => _size;

    public void Move(Vector2 newPosition)
    {
        var offset = newPosition - _position;
        _equals.Move(_equals.Position + offset);
        _button.Move(_button.Position + offset);

        foreach (var from in _from)
            from.Move(from.GetPosition() + offset);

        foreach (var to in _to)
            to.Move(to.GetPosition() + offset);

        _position = newPosition;
    }
}
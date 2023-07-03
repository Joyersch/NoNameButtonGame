using System;
using System.Net.WebSockets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Logic.Hitboxes;
using MonoUtils.Logic.Management;
using NoNameButtonGame.LevelSystem.LevelContainer.Level4.Overworld.Quest;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level4.Overworld;

public class Questline : IManageable, IInteractable
{
    private readonly Rectangle _area;
    private readonly Random _random;
    private readonly QuestIndicator _indicator;
    private readonly QuestItem _item;

    private Color _drawColor;
    public Color DrawColor
    {
        get => _drawColor;
        set
        {
            _drawColor = value;
            _indicator.DrawColor = value;
            _item.DrawColor = value;
        }
    }

    public Questline(Vector2 position, Rectangle area, Random random)
    {
        _area = area;
        _random = random;
        _indicator = new QuestIndicator(position, 1F);
        _item = new QuestItem(
            new Vector2(
                _random.Next(_area.X, _area.Y)
                , _random.Next(_area.Width, area.Height))
            , 1F);
    }

    public Rectangle Rectangle { get; }

    public void Update(GameTime gameTime)
    {
        _indicator.Update(gameTime);
        _item.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _indicator.Draw(spriteBatch);
        _item.Draw(spriteBatch);
    }

    public void UpdateInteraction(GameTime gameTime, IHitbox toCheck)
    {
        _item.UpdateInteraction(gameTime, toCheck);
    }
}
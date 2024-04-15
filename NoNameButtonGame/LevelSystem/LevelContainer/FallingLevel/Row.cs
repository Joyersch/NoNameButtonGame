using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Logic;
using MonoUtils.Logic.Hitboxes;
using MonoUtils.Logic.Management;
using MonoUtils.Ui.Objects;
using NoNameButtonGame.GameObjects.Glitch;

namespace NoNameButtonGame.LevelSystem.LevelContainer.FallingLevel;

public class Row : IMoveable, IManageable, IInteractable, IMouseActions
{
    private Vector2 _position;
    private Vector2 _size;

    private GlitchBlockCollection[] _blocks;

    public Rectangle Rectangle => new Rectangle(_position.ToPoint(), _size.ToPoint());

    private int _disabled = -1;

    public event Action<object> Leave;
    public event Action<object> Enter;
    public event Action<object> Click;

    public Row(Vector2 position, Vector2 singleSize, float singleScale, int frame)
    {
        _blocks = new GlitchBlockCollection[10];
        _blocks[0] = new GlitchBlockCollection(position, singleSize, singleScale, frame);

        _blocks[0].Click += o => Click?.Invoke(o);
        var lastblock = _blocks[0];
        for (int i = 1; i < 10; i++)
        {
            var block = new GlitchBlockCollection(singleSize, singleScale, frame);
            block.GetAnchor(lastblock)
                .SetMainAnchor(AnchorCalculator.Anchor.TopRight)
                .SetSubAnchor(AnchorCalculator.Anchor.TopLeft)
                .Move();

            block.Enter += o => Enter?.Invoke(o);
            block.Click += o => Click?.Invoke(o);
            block.Leave += o => Leave?.Invoke(o);
            lastblock = block;
            _blocks[i] = block;
        }

        _size = new Vector2(singleSize.X * 10, singleSize.Y);
    }

    public void Update(GameTime gameTime)
    {
        for (int i = 0; i < 10; i++)
        {
            if (i != _disabled)
                _blocks[i].Update(gameTime);
        }
    }

    public void UpdateInteraction(GameTime gameTime, IHitbox toCheck)
    {
        for (int i = 0; i < 10; i++)
        {
            if (i != _disabled)
                _blocks[i].UpdateInteraction(gameTime, toCheck);
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        for (int i = 0; i < 10; i++)
        {
            if (i != _disabled)
                _blocks[i].Draw(spriteBatch);
        }

    }

    public Vector2 GetPosition()
        => _position;

    public Vector2 GetSize()
        => _size;

    public void Move(Vector2 newPosition)
    {
        var offset = newPosition - _position;
        for (int i = 0; i < 10; i++)
            _blocks[i].Move(_blocks[i].GetPosition() + offset);
        _position = newPosition;
    }

    public void SetPath(int id)
        => _disabled = id;
}